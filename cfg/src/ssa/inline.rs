use crate::function::Function;
use ast::{LocalRw, Reduce, SideEffects, Traverse};
use indexmap::IndexMap;
use itertools::{Either, Itertools};
use petgraph::visit::EdgeRef;
use rustc_hash::{FxHashMap, FxHashSet};

struct TraverseSelf<'a, T: Traverse>(&'a mut T);

impl<'a> Traverse for TraverseSelf<'a, ast::RValue> {
    fn rvalues_mut(&mut self) -> Vec<&mut ast::RValue> {
        vec![self.0]
    }

    fn rvalues(&self) -> Vec<&ast::RValue> {
        vec![self.0]
    }
}

struct Inliner<'a> {
    function: &'a mut Function,
    local_to_group: &'a FxHashMap<ast::RcLocal, usize>,
    upvalue_to_group: &'a IndexMap<ast::RcLocal, ast::RcLocal>,
    local_usages: &'a mut FxHashMap<ast::RcLocal, usize>,
}

impl<'a> Inliner<'a> {
    fn new(
        function: &'a mut Function,
        local_to_group: &'a FxHashMap<ast::RcLocal, usize>,
        upvalue_to_group: &'a IndexMap<ast::RcLocal, ast::RcLocal>,
        local_usages: &'a mut FxHashMap<ast::RcLocal, usize>,
    ) -> Self {
        Self {
            function,
            local_to_group,
            upvalue_to_group,
            local_usages,
        }
    }

    fn try_inline(
        traversible: &mut impl Traverse,
        read: &ast::RcLocal,
        new_rvalue: &mut Option<ast::RValue>,
        new_rvalue_has_side_effects: bool,
    ) -> bool {
        traversible
            .traverse_values(&mut |p, v| {
                match p {
                    ast::PreOrPost::Pre => {
                        if let Either::Right(rvalue) = v {
                            match rvalue {
                                ast::RValue::Binary(ast::Binary {
                                    left,
                                    right,
                                    operation,
                                }) if operation.is_comparator()
                                    && left.has_side_effects()
                                    && let box ast::RValue::Local(local) = &right
                                    && local == read =>
                                {
                                    *right = std::mem::replace(
                                        left,
                                        Box::new(new_rvalue.take().unwrap()),
                                    );
                                    *operation = match *operation {
                                        // TODO: __eq metamethod?
                                        ast::BinaryOperation::Equal => ast::BinaryOperation::Equal,
                                        ast::BinaryOperation::NotEqual => {
                                            ast::BinaryOperation::NotEqual
                                        }
                                        ast::BinaryOperation::LessThanOrEqual => {
                                            ast::BinaryOperation::GreaterThanOrEqual
                                        }
                                        ast::BinaryOperation::GreaterThanOrEqual => {
                                            ast::BinaryOperation::LessThanOrEqual
                                        }
                                        ast::BinaryOperation::LessThan => {
                                            ast::BinaryOperation::GreaterThan
                                        }
                                        ast::BinaryOperation::GreaterThan => {
                                            ast::BinaryOperation::LessThan
                                        }
                                        _ => unreachable!(),
                                    };
                                    return Some(true);
                                }
                                _ => {}
                            }
                        }
                    }
                    ast::PreOrPost::Post => {
                        if let Either::Right(rvalue) = v {
                            match rvalue {
                                ast::RValue::Local(local) if local == read => {
                                    *rvalue = new_rvalue.take().unwrap();
                                    // success!
                                    return Some(true);
                                }
                                _ => {}
                            }
                            if new_rvalue_has_side_effects && rvalue.has_side_effects() {
                                // failure :(
                                return Some(false);
                            }
                        }
                    }
                }
                // keep searching
                None
            })
            .unwrap_or(false)
    }

    // TODO: dont clone rvalues
    // TODO: REFACTOR: move to ssa module?
    // TODO: inline into block arguments
    fn inline_rvalues(self) {
        let node_indices = self.function.graph().node_indices().collect::<Vec<_>>();
        for node in node_indices {
            let block = self.function.block_mut(node).unwrap();

            // TODO: rename values_read to locals_read
            let mut stat_to_values_read = Vec::with_capacity(block.len());
            for stat in &block.0 {
                stat_to_values_read.push(
                    stat.values_read()
                        .into_iter()
                        .filter(|&l| {
                            self.local_usages[l] == 1 && !self.upvalue_to_group.contains_key(l)
                        })
                        .cloned()
                        .map(Some)
                        .collect_vec(),
                );
            }

            // visit all statements that read at least one local with only one usage,
            // this is the statement we will inline into
            // then seek backwards from the previous statement to the start of the block
            // until we find a statement that assigns to a single-use local that
            // is used in the statement we are inlining into.
            // TODO: push multiple use local assignments forward to their first use
            let mut index = 0;
            'w: while index < block.len() {
                let mut groups_written = FxHashSet::default();
                let mut allow_side_effects = true;
                for stat_index in (0..index).rev() {
                    let mut values_read = stat_to_values_read[index]
                        .iter_mut()
                        .filter(|l| l.is_some())
                        .peekable();
                    if values_read.peek().is_none() {
                        index += 1;
                        continue 'w;
                    }
                    // we cant inline across upvalue writes because an inlining candidate with side effects,
                    // for ex. a non-local function call, might access the upvalue
                    for value_written in block[stat_index].values_written() {
                        if self.upvalue_to_group.contains_key(value_written) {
                            // TODO: set allow_side_effects to false instead
                            allow_side_effects = false;
                        }
                    }

                    /*
                    -- we dont want to inline `tostring(a)` into `print(b)`
                    local print = print
                    local a = 1
                    while true do
                        local b = tostring(a)
                        a = 1
                        print(b)
                    end
                    */
                    if block[stat_index]
                        .values_read()
                        .into_iter()
                        .filter_map(|l| self.local_to_group.get(l))
                        .any(|g| groups_written.contains(g))
                    {
                        continue;
                    }

                    if let ast::Statement::Assign(assign) = &block[stat_index]
                        && let Ok(new_rvalue) = assign.right.iter().exactly_one()
                    {
                        let new_rvalue_has_side_effects = new_rvalue.has_side_effects()
                            || new_rvalue
                                .values_read()
                                .iter()
                                .any(|v| self.upvalue_to_group.contains_key(*v));
                        if !new_rvalue_has_side_effects || allow_side_effects {
                            if let Ok(ast::LValue::Local(local)) = &assign.left.iter().exactly_one()
                                && let Some(read) = stat_to_values_read[index]
                                    .iter_mut()
                                    .find(|l| l.as_ref() == Some(local))
                            {
                                let mut new_rvalue = Some(
                                    block[stat_index]
                                        .as_assign_mut()
                                        .unwrap()
                                        .right
                                        .pop()
                                        .unwrap(),
                                );
                                if Self::try_inline(
                                    &mut block[index],
                                    read.as_ref().unwrap(),
                                    &mut new_rvalue,
                                    new_rvalue_has_side_effects,
                                ) {
                                    assert!(new_rvalue.is_none());

                                    // TODO: PERF: this is probably inefficient
                                    for rvalue in block[index].rvalues_mut() {
                                        *rvalue =
                                            std::mem::replace(rvalue, ast::Literal::Nil.into())
                                                .reduce();
                                    }

                                    // TODO: PERF: remove `local_usages[l] == 1` filter in stat_to_values_read
                                    // and use stat_to_values_read here
                                    for local in block[stat_index].values_read() {
                                        let local_usage_count =
                                            self.local_usages.get_mut(local).unwrap();
                                        *local_usage_count = local_usage_count.saturating_sub(1);
                                    }
                                    // we dont need to update local usages because tracking usages for a local
                                    // with no declarations serves no purpose
                                    block[stat_index] = ast::Empty {}.into();
                                    *read = None;
                                    continue 'w;
                                } else {
                                    block[stat_index]
                                        .as_assign_mut()
                                        .unwrap()
                                        .right
                                        .push(new_rvalue.unwrap());
                                }
                            } else if let Some(generic_for_init) =
                                block[index].as_generic_for_init()
                                && generic_for_init
                                    .0
                                    .right
                                    .iter()
                                    .rev()
                                    .map_while(|r| r.as_local())
                                    .eq_by(assign.left.iter().rev(), |a, b| Some(a) == b.as_local())
                                && assign.left.iter().all(|l| {
                                    l.as_local().is_some_and(|l| {
                                        stat_to_values_read[index]
                                            .iter_mut()
                                            .any(|r| r.as_ref() == Some(l))
                                    })
                                })
                            {
                                let start_index =
                                    generic_for_init.0.right.len() - assign.left.len();
                                let has_leading_side_effects = || {
                                    let mut leading_side_effects = false;
                                    for expr in generic_for_init.0.right.iter().take(start_index) {
                                        if expr.has_side_effects() {
                                            leading_side_effects = true;
                                            break;
                                        }
                                    }
                                    leading_side_effects
                                };

                                if !new_rvalue_has_side_effects || !has_leading_side_effects() {
                                    let new_rvalue = block[stat_index]
                                        .as_assign_mut()
                                        .unwrap()
                                        .right
                                        .pop()
                                        .unwrap();

                                    let generic_for_init =
                                        block[index].as_generic_for_init_mut().unwrap();
                                    let old_locals = generic_for_init
                                        .0
                                        .right
                                        .drain(start_index..)
                                        .map(|r| r.as_local().unwrap().clone())
                                        .collect_vec();
                                    generic_for_init.0.right.push(new_rvalue);

                                    // TODO: PERF: remove `local_usages[l] == 1` filter in stat_to_values_read
                                    // and use stat_to_values_read here
                                    for local in block[stat_index].values_read() {
                                        let local_usage_count =
                                            self.local_usages.get_mut(local).unwrap();
                                        *local_usage_count = local_usage_count.saturating_sub(1);
                                    }
                                    // we dont need to update local usages because tracking usages for a local
                                    // with no declarations serves no purpose
                                    block[stat_index] = ast::Empty {}.into();
                                    for old_local in old_locals {
                                        *stat_to_values_read[index]
                                            .iter_mut()
                                            .find(|l| l.as_ref() == Some(&old_local))
                                            .unwrap() = None;
                                    }
                                    continue 'w;
                                }
                            }
                        }
                    }
                    groups_written.extend(
                        block[stat_index]
                            .values_written()
                            .into_iter()
                            .filter_map(|l| self.local_to_group.get(l))
                            .cloned(),
                    );
                    allow_side_effects &= !block[stat_index].has_side_effects();
                }
                index += 1;
            }
            // we cant inline anything with side effects or anything that depends on other params
            // because block params are executed in parallel.
            for edge in self.function.edges(node).map(|e| e.id()).collect_vec() {
                // TODO: rename values_read to locals_read
                let mut arg_to_values_read = self
                    .function
                    .graph()
                    .edge_weight(edge)
                    .unwrap()
                    .arguments
                    .iter()
                    .map(|(_, a)| {
                        a.values_read()
                            .into_iter()
                            .filter(|&l| {
                                self.local_usages[l] == 1 && !self.upvalue_to_group.contains_key(l)
                            })
                            .cloned()
                            .map(Some)
                            .collect_vec()
                    })
                    .collect_vec();

                let mut index = 0;
                'w: while index < arg_to_values_read.len() {
                    let mut groups_written = FxHashSet::default();
                    for stat_index in (0..self.function.block(node).unwrap().len()).rev() {
                        let mut values_read = arg_to_values_read[index]
                            .iter_mut()
                            .filter(|l| l.is_some())
                            .peekable();
                        if values_read.peek().is_none() {
                            index += 1;
                            continue 'w;
                        }
                        let block = self.function.block_mut(node).unwrap();
                        // we cant inline across upvalue writes because an inlining candidate with side effects,
                        // for ex. a non-local function call, might access the upvalue
                        for value_written in block[stat_index].values_written() {
                            if self.upvalue_to_group.contains_key(value_written) {
                                // TODO: set allow_side_effects to false instead
                                index += 1;
                                continue 'w;
                            }
                        }

                        /*
                        -- we dont want to inline `tostring(a)` into `print(b)`
                        local print = print
                        local a = 1
                        while true do
                            local b = tostring(a)
                            a = 1
                            print(b)
                        end
                        */
                        if block[stat_index]
                            .values_read()
                            .into_iter()
                            .filter_map(|l| self.local_to_group.get(l))
                            .any(|g| groups_written.contains(g))
                        {
                            continue;
                        }

                        if let ast::Statement::Assign(assign) = &block[stat_index]
                            && let Ok(new_rvalue) = assign.right.iter().exactly_one()
                        {
                            let new_rvalue_has_side_effects = new_rvalue.has_side_effects()
                                || new_rvalue
                                    .values_read()
                                    .iter()
                                    .any(|v| self.upvalue_to_group.contains_key(*v));
                            if !new_rvalue_has_side_effects
                                && let Ok(ast::LValue::Local(local)) =
                                    &assign.left.iter().exactly_one()
                                && let Some(read) = arg_to_values_read[index]
                                    .iter_mut()
                                    .find(|l| l.as_ref() == Some(local))
                            {
                                let mut new_rvalue = Some(
                                    block[stat_index]
                                        .as_assign_mut()
                                        .unwrap()
                                        .right
                                        .pop()
                                        .unwrap(),
                                );
                                if Self::try_inline(
                                    &mut TraverseSelf(
                                        &mut self
                                            .function
                                            .graph_mut()
                                            .edge_weight_mut(edge)
                                            .unwrap()
                                            .arguments[index]
                                            .1,
                                    ),
                                    read.as_ref().unwrap(),
                                    &mut new_rvalue,
                                    new_rvalue_has_side_effects,
                                ) {
                                    assert!(new_rvalue.is_none());
                                    let block = self.function.block_mut(node).unwrap();

                                    // TODO: PERF: remove `local_usages[l] == 1` filter in stat_to_values_read
                                    // and use stat_to_values_read here
                                    for local in block[stat_index].values_read() {
                                        let local_usage_count =
                                            self.local_usages.get_mut(local).unwrap();
                                        *local_usage_count = local_usage_count.saturating_sub(1);
                                    }
                                    // we dont need to update local usages because tracking usages for a local
                                    // with no declarations serves no purpose

                                    block[stat_index] = ast::Empty {}.into();
                                    *read = None;
                                    continue 'w;
                                } else {
                                    let block = self.function.block_mut(node).unwrap();

                                    block[stat_index]
                                        .as_assign_mut()
                                        .unwrap()
                                        .right
                                        .push(new_rvalue.unwrap());
                                }
                            }
                        }
                        let block = self.function.block(node).unwrap();

                        groups_written.extend(
                            block[stat_index]
                                .values_written()
                                .into_iter()
                                .filter_map(|l| self.local_to_group.get(l))
                                .cloned(),
                        );
                    }
                    index += 1;
                }
            }
        }
    }
}

pub fn inline(
    function: &mut Function,
    local_to_group: &FxHashMap<ast::RcLocal, usize>,
    upvalue_to_group: &IndexMap<ast::RcLocal, ast::RcLocal>,
) {
    let mut local_usages = FxHashMap::default();
    for node in function.graph().node_indices() {
        for read in function.values_read(node) {
            *local_usages.entry(read.clone()).or_insert(0usize) += 1;
        }
    }

    let mut changed = true;
    while changed {
        changed = false;
        Inliner::new(
            function,
            local_to_group,
            upvalue_to_group,
            &mut local_usages,
        )
        .inline_rvalues();

        // remove unused locals
        for block in function.blocks_mut() {
            for stat_index in 0..block.len() {
                if let ast::Statement::Assign(assign) = &block[stat_index]
                    && assign.left.len() == 1
                    && assign.right.len() == 1
                    && let ast::LValue::Local(local) = &assign.left[0]
                {
                    let rvalue = &assign.right[0];
                    let has_side_effects = rvalue.has_side_effects();
                    // TODO: REFACTOR: is_some_and
                    if !upvalue_to_group.contains_key(local)
                        && local_usages.get(local).map_or(true, |&u| u == 0)
                    {
                        if has_side_effects {
                            // TODO: PERF: dont clone
                            let new_stat = match rvalue {
                                ast::RValue::Call(call)
                                | ast::RValue::Select(ast::Select::Call(call)) => {
                                    Some(call.clone().into())
                                }
                                ast::RValue::MethodCall(method_call)
                                | ast::RValue::Select(ast::Select::MethodCall(method_call)) => {
                                    Some(method_call.clone().into())
                                }
                                _ => None,
                            };
                            if let Some(new_stat) = new_stat {
                                block[stat_index] = new_stat;
                                changed = true;
                            }
                        } else {
                            block[stat_index] = ast::Empty {}.into();
                            changed = true;
                        }
                    }
                }
            }
        }

        for block in function.blocks_mut() {
            // we check block.ast.len() elsewhere and do `i - ` here and elsewhere so we need to get rid of empty statements
            // TODO: fix ^
            block.retain(|s| s.as_empty().is_none());

            // `t = {} t.a = 1` -> `t = { a = 1 }`
            let mut i = 0;
            while i < block.len() {
                if let ast::Statement::Assign(assign) = &block[i]
                    && assign.left.len() == 1
                    && assign.right.len() == 1
                    && assign.right[0].as_table().is_some()
                    && let ast::LValue::Local(object_local) = &assign.left[0]
                {
                    let table_index = i;
                    let object_local = object_local.clone();
                    i += 1;
                    while i < block.len()
                        && let ast::Statement::Assign(field_assign) = &block[i]
                        && field_assign.left.len() == 1
                        && field_assign.right.len() == 1
                        && let ast::LValue::Index(ast::Index {
                            left: box ast::RValue::Local(local),
                            ..
                        }) = &field_assign.left[0]
                        && local == &object_local
                    {
                        let right = &field_assign.right[0];
                        if right.as_closure().is_none()
                            && right.values_read().contains(&&object_local)
                        {
                            break;
                        }

                        let field_assign = std::mem::replace(&mut block[i], ast::Empty {}.into())
                            .into_assign()
                            .unwrap();
                        block[table_index].as_assign_mut().unwrap().right[0]
                            .as_table_mut()
                            .unwrap()
                            .0
                            .push((
                                Some(Box::into_inner(
                                    field_assign
                                        .left
                                        .into_iter()
                                        .next()
                                        .unwrap()
                                        .into_index()
                                        .unwrap()
                                        .right,
                                )),
                                field_assign.right.into_iter().next().unwrap(),
                            ));
                        changed = true;
                        i += 1;
                    }
                } else {
                    i += 1;
                }
            }

            // if the first statement is a set_list, we cant inline it anyway
            for i in 1..block.len() {
                if let ast::Statement::SetList(set_list) = &block[i] {
                    let object_local = set_list.object_local.clone();
                    if let Some(assign) = block[i - 1].as_assign_mut()
                        && assign.left == [object_local.into()]
                    {
                        let set_list = std::mem::replace(&mut block[i], ast::Empty {}.into())
                            .into_set_list()
                            .unwrap();
                        *local_usages.get_mut(&set_list.object_local).unwrap() -= 1;
                        let assign = block.get_mut(i - 1).unwrap().as_assign_mut().unwrap();
                        let table = assign.right[0].as_table_mut().unwrap();
                        assert!(
                            table.0.iter().filter(|(k, _)| k.is_none()).count()
                                == set_list.index - 1
                        );
                        for value in set_list.values {
                            table.0.push((None, value));
                        }
                        // table already has tail?
                        // TODO: REFACTOR: is_some_and
                        assert!(!table.0.last().map_or(false, |(k, v)| k.is_none()
                            && matches!(
                                v,
                                ast::RValue::VarArg(_)
                                    | ast::RValue::Call(_)
                                    | ast::RValue::MethodCall(_)
                            )));
                        if let Some(tail) = set_list.tail {
                            table.0.push((None, tail));
                        }
                        changed = true;
                    }
                    // todo: only inline in changed blocks
                    //cfg::dot::render_to(function, &mut std::io::stdout());
                    //break 'outer;
                }
            }
        }
    }
    // we check block.ast.len() elsewhere and do `i - ` here and elsewhere so we need to get rid of empty statements
    // TODO: fix ^
    for block in function.blocks_mut() {
        block.retain(|s| s.as_empty().is_none());
    }
}
