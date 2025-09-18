use ast::{LocalRw, Reduce, SideEffects, Traverse, UnaryOperation};

use itertools::Itertools;
use petgraph::{
    algo::dominators::Dominators,
    stable_graph::{EdgeIndex, NodeIndex},
    visit::{DfsPostOrder, EdgeRef},
    Direction,
};
use rustc_hash::FxHashMap;
use tuple::Map;

use crate::{
    block::{BlockEdge, BranchType},
    function::Function,
};

#[derive(Debug)]
pub enum PatternOperator {
    And,
    Or,
}

impl From<PatternOperator> for ast::BinaryOperation {
    fn from(val: PatternOperator) -> Self {
        match val {
            PatternOperator::And => ast::BinaryOperation::And,
            PatternOperator::Or => ast::BinaryOperation::Or,
        }
    }
}

#[derive(Debug)]
pub struct ConditionalAssignmentPattern {
    assigner: NodeIndex,
    next: NodeIndex,
    tested_local: ast::RcLocal,
    assigned_local: ast::RcLocal,
    assigned_value: ast::RValue,
    parameter: ast::RcLocal,
    operator: PatternOperator,
}

type ConditionalSequenceConfiguration = (bool, bool);

#[derive(Debug)]
pub struct ConditionalSequencePattern {
    first_node: NodeIndex,
    second_node: NodeIndex,
    short_circuit: NodeIndex,
    assign: bool,
    inverted: bool,
    final_condition: ast::RValue,
}

#[derive(Debug)]
pub struct GenericForNextPattern {
    body_node: NodeIndex,
    res_locals: Vec<ast::RcLocal>,
    // generator is not necessarily a local in Luau
    // it is commonly something like: `generator or __get_builtin("iter")`
    generator: ast::RValue,
    state: ast::RcLocal,
    internal_control: ast::RcLocal,
}

fn simplify_condition(function: &mut Function, node: NodeIndex) -> bool {
    let block = function.block_mut(node).unwrap();
    if let Some(if_stat) = block.last_mut().and_then(|s| s.as_if_mut()) {
        if let Some(unary) = if_stat.condition.as_unary()
            && unary.operation == UnaryOperation::Not
        {
            if_stat.condition = *unary.value.clone();
            let (then_edge, else_edge) = function.conditional_edges(node).unwrap().map(|e| e.id());
            let (then_edge, else_edge) = function.graph_mut().index_twice_mut(then_edge, else_edge);
            then_edge.branch_type = BranchType::Else;
            else_edge.branch_type = BranchType::Then;
            return true;
        } else if let Some(binary) = if_stat.condition.as_binary() {
            if binary.left.as_literal().is_some() && binary.right.as_literal().is_none() {
                if_stat.condition = ast::Binary::new(
                    *binary.right.clone(),
                    *binary.left.clone(),
                    match binary.operation {
                        ast::BinaryOperation::Equal => ast::BinaryOperation::Equal,
                        ast::BinaryOperation::NotEqual => ast::BinaryOperation::NotEqual,
                        ast::BinaryOperation::LessThan => ast::BinaryOperation::GreaterThan,
                        ast::BinaryOperation::LessThanOrEqual => {
                            ast::BinaryOperation::GreaterThanOrEqual
                        }
                        ast::BinaryOperation::GreaterThan => ast::BinaryOperation::LessThan,
                        ast::BinaryOperation::GreaterThanOrEqual => {
                            ast::BinaryOperation::LessThanOrEqual
                        }
                        _ => return false,
                    },
                )
                .into();
                return true;
            }
        }
    }
    false
}

fn single_assign(block: &ast::Block) -> Option<&ast::Assign> {
    if block.len() == 1
        && let Some(assign) = block.last().unwrap().as_assign()
        && assign.left.len() == 1
    {
        Some(assign)
    } else {
        None
    }
}

fn match_conditional_sequence(
    function: &Function,
    node: NodeIndex,
) -> Option<ConditionalSequencePattern> {
    // TODO: check if len() == 1?
    let block = function.block(node).unwrap();
    if let Some(r#if) = block.last().and_then(|s| s.as_if()) {
        let first_condition = r#if.condition.clone();
        let test_pattern = |second_conditional, other, other_args: FxHashMap<_, _>| {
            let second_conditional_successors = function.edges(second_conditional).collect_vec();
            let second_block = function.block(second_conditional).unwrap();
            if let Some(second_conditional_if) = second_block.last().and_then(|s| s.as_if()) {
                if second_conditional_successors.len() == 2
                    && let Ok(edge_to_other) = second_conditional_successors
                        .iter()
                        .filter(|&s| {
                            s.target() == other
                                && s.weight()
                                    .arguments
                                    .iter()
                                    .all(|(p, _)| other_args.contains_key(p))
                        })
                        .exactly_one()
                {
                    if second_block.len() == 2 {
                        if let ast::Statement::Assign(assign) = &second_block[0] {
                            // TODO: make sure this variable isnt used anywhere but this block
                            // and the args passed to other.
                            let values_written = assign.values_written();
                            if values_written.len() == 1
                                && second_conditional_if.condition
                                    == values_written[0].clone().into()
                            {
                                let valid = if other_args.len() == 1
                                    && let Ok((_, ast::RValue::Local(local))) =
                                        edge_to_other.weight().arguments.iter().exactly_one()
                                    && local == values_written[0]
                                {
                                    true
                                } else {
                                    other_args.is_empty()
                                };
                                if valid {
                                    assert!(assign.right.len() == 1);
                                    return Some((assign.right[0].clone(), true));
                                }
                            }
                        }
                        return None;
                    } else if second_block.len() == 1
                        && edge_to_other
                            .weight()
                            .arguments
                            .iter()
                            .all(|(k, v)| other_args.get(k).is_some_and(|rv| rv == v))
                    {
                        return Some((second_conditional_if.condition.clone(), false));
                    }
                }
            }
            None
        };
        let first_terminator = function.conditional_edges(node).unwrap();
        let (then_edge, else_edge) = first_terminator;
        if function.predecessor_blocks(then_edge.target()).count() == 1
            && then_edge.weight().arguments.is_empty()
            && let else_args = else_edge
                .weight()
                .arguments
                .iter()
                .cloned()
                .collect::<FxHashMap<_, _>>()
            && let Some((second_condition, assign)) =
                test_pattern(then_edge.target(), else_edge.target(), else_args)
        {
            let second_terminator = function.conditional_edges(then_edge.target()).unwrap();
            if second_terminator.0.target() == else_edge.target() {
                Some(ConditionalSequencePattern {
                    first_node: node,
                    second_node: then_edge.target(),
                    short_circuit: else_edge.target(),
                    assign,
                    inverted: true,
                    final_condition: ast::Binary::new(
                        ast::Unary::new(first_condition, ast::UnaryOperation::Not).into(),
                        second_condition,
                        ast::BinaryOperation::Or,
                    )
                    .into(),
                })
            } else {
                Some(ConditionalSequencePattern {
                    first_node: node,
                    second_node: then_edge.target(),
                    short_circuit: else_edge.target(),
                    assign,
                    inverted: false,
                    final_condition: ast::Binary::new(
                        first_condition,
                        second_condition,
                        ast::BinaryOperation::And,
                    )
                    .into(),
                })
            }
        } else if function.predecessor_blocks(else_edge.target()).count() == 1
            && else_edge.weight().arguments.is_empty()
            && let then_args = then_edge
                .weight()
                .arguments
                .iter()
                .cloned()
                .collect::<FxHashMap<_, _>>()
            && let Some((second_condition, assign)) =
                test_pattern(else_edge.target(), then_edge.target(), then_args)
        {
            let second_terminator = function.conditional_edges(else_edge.target()).unwrap();
            if first_terminator.0.target() == second_terminator.0.target() {
                Some(ConditionalSequencePattern {
                    first_node: node,
                    second_node: else_edge.target(),
                    short_circuit: then_edge.target(),
                    assign,
                    inverted: false,
                    final_condition: ast::Binary::new(
                        first_condition,
                        second_condition,
                        ast::BinaryOperation::Or,
                    )
                    .into(),
                })
            } else {
                Some(ConditionalSequencePattern {
                    first_node: node,
                    second_node: else_edge.target(),
                    short_circuit: then_edge.target(),
                    assign,
                    inverted: true,
                    final_condition: ast::Binary::new(
                        ast::Unary::new(first_condition, ast::UnaryOperation::Not).into(),
                        second_condition,
                        ast::BinaryOperation::And,
                    )
                    .into(),
                })
            }
        } else {
            None
        }
    } else {
        None
    }
}

pub fn structure_conditionals(function: &mut Function) -> bool {
    let mut did_structure = false;
    // TODO: does this need to be in dfs post order?
    let mut dfs = DfsPostOrder::new(function.graph(), function.entry().unwrap());
    while let Some(node) = dfs.next(function.graph()) {
        if simplify_condition(function, node) {
            did_structure = true;
        }
        if structure_bool_conditional(function, node) {
            did_structure = true;
        }

        if let Some(pattern) = match_conditional_sequence(function, node)
            // TODO: can we continue?
            && &Some(pattern.second_node) != function.entry()
        {
            let second_to_sc_edges = function
                .edges(pattern.second_node)
                .filter(|e| e.target() == pattern.short_circuit)
                .collect::<Vec<_>>();
            assert!(second_to_sc_edges.len() == 1);
            let second_to_sc_args = second_to_sc_edges[0].weight().arguments.clone();
            let first_to_sc_edges = function
                .edges(pattern.first_node)
                .filter(|e| e.target() == pattern.short_circuit)
                .collect::<Vec<_>>();
            assert!(first_to_sc_edges.len() == 1);
            let first_to_sc_edge = first_to_sc_edges[0].id();
            for arg in &mut function
                .graph_mut()
                .edge_weight_mut(first_to_sc_edge)
                .unwrap()
                .arguments
            {
                if let Some(new_arg) = second_to_sc_args.iter().find(|(k, _)| k == &arg.0) {
                    *arg = new_arg.clone();
                }
            }

            let second_terminator = function.conditional_edges(pattern.second_node).unwrap();
            let other_edge = if second_terminator.0.target() == pattern.short_circuit {
                second_terminator.1
            } else {
                second_terminator.0
            };
            let other_edge = other_edge.id();
            assert!(skip_over_node(function, pattern.first_node, other_edge));

            let mut removed_block = function.remove_block(pattern.second_node).unwrap();
            let first_node = pattern.first_node;
            if pattern.assign {
                let assign = removed_block.first_mut().unwrap().as_assign_mut().unwrap();
                assign.right = vec![pattern.final_condition.reduce()];
            } else {
                let removed_if = removed_block.last_mut().unwrap().as_if_mut().unwrap();
                removed_if.condition = pattern.final_condition.reduce_condition();
            }
            if pattern.inverted {
                let removed_if = removed_block.last_mut().unwrap().as_if_mut().unwrap();
                // TODO: unnecessary clone?
                removed_if.condition =
                    ast::Unary::new(removed_if.condition.clone(), UnaryOperation::Not)
                        .reduce_condition();
            }
            let first_block = function.block_mut(first_node).unwrap();
            first_block.pop();
            first_block.extend(removed_block.0);
            did_structure = true;
        }

        did_structure |= try_remove_unnecessary_condition(function, node);
    }

    did_structure
}

// TODO: REFACTOR: move to ast
// None = unknown
fn is_truthy(rvalue: ast::RValue) -> Option<bool> {
    match rvalue.reduce_condition() {
        // __len has to return number, but __unm can return any value
        ast::RValue::Unary(ast::Unary {
            operation: ast::UnaryOperation::Length,
            ..
        }) => Some(true),
        ast::RValue::Literal(
            ast::Literal::Boolean(true) | ast::Literal::Number(_) | ast::Literal::String(_),
        )
        | ast::RValue::Table(_)
        | ast::RValue::Closure(_) => Some(true),
        ast::RValue::Literal(ast::Literal::Nil | ast::Literal::Boolean(_)) => Some(false),
        _ => None,
    }
}

// TODO: STYLE: rename
fn make_bool_conditional(
    function: &mut Function,
    node: NodeIndex,
    mut then_value: ast::RValue,
    mut else_value: ast::RValue,
) -> Option<ast::RValue> {
    let block = function.block_mut(node).unwrap();
    let r#if = block.last_mut().unwrap().as_if_mut().unwrap();
    if let ast::RValue::Literal(ast::Literal::Boolean(then_value)) = then_value
        && let ast::RValue::Literal(ast::Literal::Boolean(else_value)) = else_value
        && then_value != else_value
    {
        let cond = ast::Unary::new(
            std::mem::replace(&mut r#if.condition, ast::Literal::Nil.into()),
            ast::UnaryOperation::Not,
        );
        let cond = if then_value {
            ast::Unary::new(cond.into(), ast::UnaryOperation::Not)
        } else {
            cond
        };
        Some(cond.reduce())
    } else {
        // TODO: `v0 and v1 and v2`, v0, v1 and v2 are truthy, but only v2 is treated as such
        let then_truthy = match is_truthy(then_value.clone()) {
            Some(truthy) => truthy,
            None if !then_value.has_side_effects() => {
                let value = match &r#if.condition {
                    ast::RValue::Binary(ast::Binary {
                        right: box value,
                        operation: ast::BinaryOperation::And,
                        ..
                    }) => value,
                    value => value,
                };
                !value.has_side_effects() && *value == then_value
            }
            None => false,
        };
        // TODO: if condition is `and not else_value` or `not else_value` then truthy?
        let else_truthy = is_truthy(else_value.clone()).is_some_and(|v| v);
        let cond = if !then_truthy && !else_truthy {
            return None;
        } else if !then_truthy {
            std::mem::swap(&mut then_value, &mut else_value);
            ast::Unary::new(
                std::mem::replace(&mut r#if.condition, ast::Literal::Nil.into()),
                ast::UnaryOperation::Not,
            )
            .reduce_condition()
        } else if !else_truthy {
            std::mem::replace(&mut r#if.condition, ast::Literal::Nil.into()).reduce_condition()
        } else {
            let cond =
                std::mem::replace(&mut r#if.condition, ast::Literal::Nil.into()).reduce_condition();
            if let ast::RValue::Unary(ast::Unary {
                box value,
                operation: ast::UnaryOperation::Not,
            }) = cond
            {
                std::mem::swap(&mut then_value, &mut else_value);
                value
            } else {
                cond
            }
        };

        Some(
            ast::Binary::new(
                ast::Binary::new(cond, then_value, ast::BinaryOperation::And).into(),
                else_value,
                ast::BinaryOperation::Or,
            )
            .reduce(),
        )
    }
}

// TODO: `return if g then true else false` in luau?
// local a; if g then a = true else a = false end; return a -> return g and true or false
// local a; if g then a = false else a = true end; return a -> return not g
// local a; if g == 1 then a = true else a = false end; return a -> return g == 1
fn structure_bool_conditional(function: &mut Function, node: NodeIndex) -> bool {
    let match_triangle = |assigner, next, next_args: FxHashMap<ast::RcLocal, ast::RValue>| {
        if let Some(edge_to_next) = function.unconditional_edge(assigner)
            && edge_to_next.target() == next
            && edge_to_next.weight().arguments.iter().all(|(p, _)| next_args.contains_key(p))
            && let Some(assign) = single_assign(function.block(assigner).unwrap())
            // TODO: allow multiple unused (excl. first) locals in left
            && assign.left.len() == 1 && assign.right.len() == 1
            && let ast::LValue::Local(assigned_local) = &assign.left[0]
            && next_args.len() == 1 && let Ok((param, ast::RValue::Local(arg))) = edge_to_next.weight().arguments.iter().exactly_one()
            && arg == assigned_local
        {
            // TODO: make sure assigned_local is only used in the assigner and it's params to next
            // TODO: unnecessary clone
            Some((param, assign.right[0].clone(), next_args[param].clone()))
        } else {
            None
        }
    };

    if let Some(ast::Statement::If(_)) = function.block(node).unwrap().last() {
        let (then_edge, else_edge) = function.conditional_edges(node).unwrap();
        if then_edge.target() == else_edge.target() {
            if let Ok((res_local, then_value, else_value)) = then_edge
                .weight()
                .arguments
                .iter()
                .filter_map(|(p, a)| {
                    else_edge
                        .weight()
                        .arguments
                        .iter()
                        .find(|(p1, _)| p == p1)
                        .map(|(_, a1)| (p, a, a1))
                })
                .exactly_one()
            {
                let (then_edge, else_edge) = (then_edge.id(), else_edge.id());
                // TODO: unnecessary clones
                let res_local = res_local.clone();
                let then_value = then_value.clone();
                let else_value = else_value.clone();

                if let Some(res) = make_bool_conditional(function, node, then_value, else_value) {
                    function
                        .graph_mut()
                        .edge_weight_mut(then_edge)
                        .unwrap()
                        .arguments[0]
                        .1 = res_local.clone().into();
                    function
                        .graph_mut()
                        .edge_weight_mut(else_edge)
                        .unwrap()
                        .arguments[0]
                        .1 = res_local.clone().into();
                    let block = function.block_mut(node).unwrap();
                    let r#if = block.last_mut().unwrap().as_if_mut().unwrap();
                    r#if.condition = res_local.clone().into();
                    let pos = block.len() - 1;
                    block.insert(
                        pos,
                        ast::Assign::new(vec![res_local.into()], vec![res]).into(),
                    );
                    true
                } else {
                    false
                }
            } else {
                false
            }
        } else if then_edge.weight().arguments.is_empty()
            && function
                .predecessor_blocks(then_edge.target())
                .exactly_one()
                .is_ok()
            && let else_args = else_edge
                .weight()
                .arguments
                .iter()
                .cloned()
                .collect::<FxHashMap<_, _>>()
            && let Some((res_local, then_value, else_value)) =
                match_triangle(then_edge.target(), else_edge.target(), else_args)
        {
            let then_block = then_edge.target();
            let (then_edge, else_edge) = (
                function.unconditional_edge(then_block).unwrap().id(),
                else_edge.id(),
            );
            let res_local = res_local.clone();
            if let Some(res) = make_bool_conditional(function, node, then_value, else_value) {
                function
                    .graph_mut()
                    .edge_weight_mut(then_edge)
                    .unwrap()
                    .arguments[0]
                    .1 = res_local.clone().into();
                function
                    .graph_mut()
                    .edge_weight_mut(else_edge)
                    .unwrap()
                    .arguments[0]
                    .1 = res_local.clone().into();
                skip_over_node(function, node, then_edge);
                if function.predecessor_blocks(then_block).next().is_none() {
                    function.remove_block(then_block);
                }
                let block = function.block_mut(node).unwrap();
                let r#if = block.last_mut().unwrap().as_if_mut().unwrap();
                r#if.condition = res_local.clone().into();
                let pos = block.len() - 1;
                block.insert(
                    pos,
                    ast::Assign::new(vec![res_local.into()], vec![res]).into(),
                );
                true
            } else {
                false
            }
        } else if else_edge.weight().arguments.is_empty()
            && function
                .predecessor_blocks(else_edge.target())
                .exactly_one()
                .is_ok()
            && let then_args = then_edge
                .weight()
                .arguments
                .iter()
                .cloned()
                .collect::<FxHashMap<_, _>>()
            && let Some((res_local, else_value, then_value)) =
                match_triangle(else_edge.target(), then_edge.target(), then_args)
        {
            let else_block = else_edge.target();
            let (then_edge, else_edge) = (
                then_edge.id(),
                function.unconditional_edge(else_block).unwrap().id(),
            );
            let res_local = res_local.clone();
            if let Some(res) = make_bool_conditional(function, node, then_value, else_value) {
                function
                    .graph_mut()
                    .edge_weight_mut(then_edge)
                    .unwrap()
                    .arguments[0]
                    .1 = res_local.clone().into();
                function
                    .graph_mut()
                    .edge_weight_mut(else_edge)
                    .unwrap()
                    .arguments[0]
                    .1 = res_local.clone().into();
                skip_over_node(function, node, else_edge);
                if function.predecessor_blocks(else_block).next().is_none() {
                    function.remove_block(else_block);
                }
                let block = function.block_mut(node).unwrap();
                let r#if = block.last_mut().unwrap().as_if_mut().unwrap();
                r#if.condition = res_local.clone().into();
                let pos = block.len() - 1;
                block.insert(
                    pos,
                    ast::Assign::new(vec![res_local.into()], vec![res]).into(),
                );
                true
            } else {
                false
            }
        } else if function.predecessor_blocks(then_edge.target()).exactly_one().is_ok()
            && let Ok(then_next) = function.successor_blocks(then_edge.target()).exactly_one()
            && function.predecessor_blocks(else_edge.target()).exactly_one().is_ok()
            && let Ok(else_next) = function.successor_blocks(else_edge.target()).exactly_one()
            && then_next == else_next
            && let Some(then_assign) = single_assign(function.block(then_edge.target()).unwrap())
            // TODO: allow multiple unused (excl. first) locals in left
            && then_assign.left.len() == 1 && then_assign.right.len() == 1
            && let Some(else_assign) = single_assign(function.block(else_edge.target()).unwrap())
            // TODO: allow multiple unused (excl. first) locals in left
            && else_assign.left.len() == 1 && else_assign.right.len() == 1
            && let Ok((then_param, ast::RValue::Local(then_arg))) = then_edge.weight().arguments.iter().exactly_one()
            && let Ok((else_param, ast::RValue::Local(else_arg))) = else_edge.weight().arguments.iter().exactly_one()
            && then_param == else_param
            && then_assign.left[0].as_local() == Some(then_arg)
            && else_assign.left[0].as_local() == Some(else_arg)
        {
            // TODO: make sure then_arg and else_arg arent used outside their respective assigner blocks
            // and the arguments passed to next
            let res_local = then_param.clone();
            let then_value = then_assign.right[0].clone();
            let else_value = else_assign.right[0].clone();
            let then_block = then_edge.target();
            let else_block = else_edge.target();
            let (then_edge, else_edge) = (
                function.unconditional_edge(then_block).unwrap().id(),
                function.unconditional_edge(else_block).unwrap().id(),
            );
            if let Some(res) = make_bool_conditional(function, node, then_value, else_value) {
                function
                    .graph_mut()
                    .edge_weight_mut(then_edge)
                    .unwrap()
                    .arguments[0]
                    .1 = res_local.clone().into();
                function
                    .graph_mut()
                    .edge_weight_mut(else_edge)
                    .unwrap()
                    .arguments[0]
                    .1 = res_local.clone().into();
                skip_over_node(function, node, then_edge);
                if function.predecessor_blocks(then_block).next().is_none() {
                    function.remove_block(then_block);
                }
                skip_over_node(function, node, else_edge);
                if function.predecessor_blocks(else_block).next().is_none() {
                    function.remove_block(else_block);
                }
                let block = function.block_mut(node).unwrap();
                let r#if = block.last_mut().unwrap().as_if_mut().unwrap();
                r#if.condition = res_local.clone().into();
                let pos = block.len() - 1;
                block.insert(
                    pos,
                    ast::Assign::new(vec![res_local.into()], vec![res]).into(),
                );
                true
            } else {
                false
            }
        } else if let (then_target, else_target) = (then_edge.target(), else_edge.target())
            && function
                .predecessor_blocks(then_target)
                .exactly_one()
                .is_ok()
            && function
                .predecessor_blocks(else_target)
                .exactly_one()
                .is_ok()
            && function.successor_blocks(then_target).next().is_none()
            && function.successor_blocks(else_target).next().is_none()
            && let Ok(ast::Statement::Return(ast::Return {
                values: then_values,
            })) = function.block(then_target).unwrap().iter().exactly_one()
            && let Ok(then_value) = then_values.iter().exactly_one()
            && let Ok(ast::Statement::Return(ast::Return {
                values: else_values,
            })) = function.block(else_target).unwrap().iter().exactly_one()
            && let Ok(else_value) = else_values.iter().exactly_one()
        {
            // TODO: unnecessary clones
            let then_value = then_value.clone();
            let else_value = else_value.clone();

            if let Some(res) = make_bool_conditional(function, node, then_value, else_value) {
                function.remove_block(then_target);
                function.remove_block(else_target);
                let block = function.block_mut(node).unwrap();
                block.pop();
                block.push(ast::Return::new(vec![res]).into());
                true
            } else {
                false
            }
        } else {
            false
        }
    } else {
        false
    }
    //todo!();
}

fn match_method_call(call: &ast::Call) -> Option<(&ast::RValue, &str)> {
    // TODO: make sure `a:method with space()` doesnt happen
    if !call.arguments.is_empty()
        && !call.arguments[0].has_side_effects()
        && let Some(ast::Index {
            box left,
            right: box ast::RValue::Literal(ast::Literal::String(index)),
        }) = call.value.as_index()
        && left == &call.arguments[0]
    {
        if let Ok(index) = std::str::from_utf8(index) {
            Some((left, index))
        } else {
            None
        }
    } else {
        None
    }
}

// This code does not apply to Luau
pub fn structure_method_calls(function: &mut Function) -> bool {
    let mut did_structure = false;
    for block in function.blocks_mut() {
        for stat in &mut block.0 {
            if let ast::Statement::Call(call) = stat {
                if let Some((value, method)) = match_method_call(call) {
                    *stat = ast::MethodCall::new(
                        value.clone(),
                        method.to_string(),
                        call.arguments.drain(1..).collect(),
                    )
                    .into();
                    did_structure = true;
                }
            }
            stat.traverse_rvalues(&mut |rvalue| {
                if let ast::RValue::Call(call) = rvalue {
                    if let Some((value, method)) = match_method_call(call) {
                        *rvalue = ast::MethodCall::new(
                            value.clone(),
                            method.to_string(),
                            call.arguments.drain(1..).collect(),
                        )
                        .into();
                        did_structure = true;
                    }
                } else if let ast::RValue::Select(select) = rvalue {
                    if let ast::Select::Call(call) = select {
                        if let Some((value, method)) = match_method_call(call) {
                            *select = ast::MethodCall::new(
                                value.clone(),
                                method.to_string(),
                                call.arguments.drain(1..).collect(),
                            )
                            .into();
                            did_structure = true;
                        }
                    }
                }
            });
        }
    }
    did_structure
}

// TODO: STYLE: better argument names
// `before -> skip -> after` to `before -> after`.
// multiple `before -> skip` edges can exist.
// multiple `skip -> after` edges can exist, but we decide which to use based on
// the edge index
// updates edges
fn skip_over_node(
    function: &mut Function,
    before_node: NodeIndex,
    skip_to_after: EdgeIndex,
    // params from (skip_node, after_node)
    // parameters: &[(ast::RcLocal, ast::RValue)],
) -> bool {
    let (skip_node, after_node) = function.graph().edge_endpoints(skip_to_after).unwrap();
    let mut did_structure = false;
    let skip_to_after_args = function
        .graph()
        .edge_weight(skip_to_after)
        .unwrap()
        .arguments
        .clone();
    for edge in function
        .graph()
        .edges_directed(before_node, Direction::Outgoing)
        .filter(|e| e.target() == skip_node)
        .map(|e| e.id())
        .collect::<Vec<_>>()
    {
        let mut new_arguments = function
            .graph()
            .edge_weight(edge)
            .unwrap()
            .arguments
            .clone();
        new_arguments.extend(skip_to_after_args.iter().cloned());
        // TODO: eliminate duplicate arguments where possible

        // all arguments in edges to a block must have the same parameters
        // TODO: make arguments a map so order doesnt matter
        if !new_arguments
            .iter()
            .map(|(p, _)| p)
            .eq(skip_to_after_args.iter().map(|(p, _)| p))
        {
            continue;
        }

        let mut edge = function.graph_mut().remove_edge(edge).unwrap();
        edge.arguments = new_arguments.into_iter().collect();
        function.graph_mut().add_edge(before_node, after_node, edge);
        did_structure = true;
    }

    did_structure
}

fn try_remove_unnecessary_condition(function: &mut Function, node: NodeIndex) -> bool {
    let block = function.block(node).unwrap();
    if !block.is_empty()
        && block.last().unwrap().as_if().is_some()
        && let Some((then_edge, else_edge)) = function.conditional_edges(node)
        && then_edge.target() == else_edge.target()
        && then_edge.weight().arguments == else_edge.weight().arguments
    {
        let target = then_edge.target();
        // TODO: check if this works (+ restructuring/src/jump.rs)
        let cond = function
            .block_mut(node)
            .unwrap()
            .pop()
            .unwrap()
            .into_if()
            .unwrap()
            .condition;
        let new_stat = match cond {
            ast::RValue::Call(call) => Some(call.into()),
            ast::RValue::MethodCall(method_call) => Some(method_call.into()),
            cond if cond.has_side_effects() => Some(
                ast::Assign {
                    left: vec![ast::RcLocal::default().into()],
                    right: vec![cond],
                    prefix: true,
                    parallel: false,
                }
                .into(),
            ),
            _ => None,
        };
        function.block_mut(node).unwrap().extend(new_stat);
        let arguments = function
            .remove_edges(node)
            .into_iter()
            .next()
            .unwrap()
            .1
            .arguments;
        let mut new_edge = BlockEdge::new(BranchType::Unconditional);
        new_edge.arguments = arguments;
        function.set_edges(node, vec![(target, new_edge)]);
        true
    } else {
        false
    }
}

// TODO: same as in structurer
fn is_for_next(function: &Function, node: NodeIndex) -> bool {
    function
        .block(node)
        .unwrap()
        .first()
        .map(|s| {
            matches!(
                s,
                ast::Statement::GenericForNext(_) | ast::Statement::NumForNext(_)
            )
        })
        .unwrap_or(false)
}

// TODO: REFACTOR: same as match_jump in restructure, maybe can use some common code?
// TODO: STYLE: rename to merge_blocks or something
pub fn structure_jumps(function: &mut Function, dominators: &Dominators<NodeIndex>) -> bool {
    let mut did_structure = false;
    for node in function.graph().node_indices().collect_vec() {
        // we call function.remove_block, that might've resulted in node being removed
        if function.block(node).is_some()
            && let Some(jump) = function.unconditional_edge(node)
            && let jump_target = jump.target()
            && jump_target != node
            && !is_for_next(function, jump_target)
        {
            let jump_edge = jump.id();
            let block = function.block(node).unwrap();
            // TODO: block_is_no_op?
            if block.is_empty() {
                let mut remove = true;
                for pred in function.predecessor_blocks(node).collect_vec() {
                    let did = skip_over_node(function, pred, jump_edge)
                        | try_remove_unnecessary_condition(function, pred);
                    if did {
                        did_structure = true;
                    }
                    remove &= did;
                }
                if remove && function.entry() != &Some(node) {
                    function.remove_block(node);
                    continue;
                }
            }
            if function.predecessor_blocks(jump_target).count() == 1
                && dominators
                    .dominators(jump_target)
                    .map(|mut d| d.contains(&node))
                    .unwrap_or(false)
                // TODO: remove args or smthn idk
                && function.graph().edge_weight(jump_edge).unwrap().arguments.is_empty()
            {
                // assert!(function.graph().edge_weight(jump_edge).unwrap().arguments.is_empty());
                let edges = function.remove_edges(jump_target);
                let body = function.remove_block(jump_target).unwrap();
                if &Some(jump_target) == function.entry() {
                    function.set_entry(node);
                }
                function.block_mut(node).unwrap().extend(body.0);
                function.set_edges(node, edges);
                did_structure = true;
            }
        }
    }
    did_structure
}
