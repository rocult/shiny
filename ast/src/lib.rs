#![feature(box_patterns)]

use derive_more::From;
use enum_as_inner::EnumAsInner;
use enum_dispatch::enum_dispatch;
use formatter::Formatter;
use itertools::Either;

use std::{
    fmt,
    ops::{Deref, DerefMut},
};

mod assign;
mod binary;
mod r#break;
mod call;
mod close;
mod closure;
mod r#continue;
mod r#for;
pub mod formatter;
mod global;
mod goto;
mod r#if;
mod index;
mod literal;
mod local;
//mod name_gen;
pub mod local_declarations;
pub mod name_locals;
mod repeat;
pub mod replace_locals;
mod r#return;
mod set_list;
mod side_effects;
mod table;
mod traverse;
pub mod type_system;
mod unary;
mod vararg;
mod r#while;

pub use assign::*;
pub use binary::*;
pub use call::*;
pub use close::*;
pub use closure::*;
pub use global::*;
pub use goto::*;
pub use index::*;
pub use literal::*;
pub use local::*;
pub use r#break::*;
pub use r#continue::*;
pub use r#for::*;
pub use r#if::*;
pub use r#return::*;
pub use r#while::*;
pub use repeat::*;
pub use set_list::*;
pub use side_effects::*;
pub use table::*;
pub use traverse::*;
use type_system::{Type, TypeSystem};
pub use unary::*;
pub use vararg::*;

pub trait Reduce {
    fn reduce(self) -> RValue;
    fn reduce_condition(self) -> RValue;
}

#[enum_dispatch(LocalRw, SideEffects, Traverse)]
#[derive(Debug, Clone, PartialEq, EnumAsInner)]
pub enum Select {
    VarArg(VarArg),
    Call(Call),
    MethodCall(MethodCall),
}

impl fmt::Display for Select {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        match self {
            Select::VarArg(var_arg) => write!(f, "{}", var_arg),
            Select::Call(call) => write!(f, "{}", call),
            Select::MethodCall(method_call) => write!(f, "{}", method_call),
        }
    }
}

#[enum_dispatch(LocalRw, SideEffects, Traverse)]
#[derive(Debug, Clone, PartialEq, EnumAsInner)]
pub enum RValue {
    Local(RcLocal),
    Global(Global),
    Call(Call),
    MethodCall(MethodCall),
    VarArg(VarArg),
    Table(Table),
    Literal(Literal),
    Index(Index),
    Unary(Unary),
    Binary(Binary),
    Closure(Closure),
    Select(Select),
}

impl type_system::Infer for RValue {
    fn infer<'a: 'b, 'b>(&'a mut self, system: &mut TypeSystem<'b>) -> Type {
        match self {
            RValue::Local(local) => local.infer(system),
            RValue::Global(_) => Type::Any,
            RValue::Call(_) => Type::Any,
            //RValue::Table(table) => table.infer(system),
            RValue::Literal(literal) => literal.infer(system),
            RValue::Index(_) => Type::Any,
            RValue::Unary(_) => Type::Any,
            RValue::Binary(_) => Type::Any,
            RValue::Closure(closure) => closure.infer(system),
            _ => Type::VarArg,
        }
    }
}

impl<'a: 'b, 'b> Reduce for RValue {
    fn reduce(self) -> RValue {
        match self {
            Self::Unary(unary) => unary.reduce(),
            Self::Binary(binary) => binary.reduce(),
            Self::Literal(literal) => literal.reduce(),
            Self::Table(table) => table.reduce(),
            Self::Closure(closure) => closure.reduce(),
            other => other,
        }
    }

    fn reduce_condition(self) -> RValue {
        match self {
            Self::Unary(unary) => unary.reduce_condition(),
            Self::Binary(binary) => binary.reduce_condition(),
            Self::Literal(literal) => literal.reduce_condition(),
            Self::Table(table) => table.reduce_condition(),
            Self::Closure(closure) => closure.reduce_condition(),
            other => other,
        }
    }
}

impl RValue {
    pub fn precedence(&self) -> usize {
        match self {
            Self::Binary(binary) => binary.precedence(),
            Self::Unary(unary) => unary.precedence(),
            RValue::Literal(Literal::Number(n)) if n.is_finite() && n.is_sign_negative() => {
                return 7;
            }
            _ => 9,
        }
    }

    pub fn into_lvalue(self) -> Option<LValue> {
        match self {
            Self::Local(local) => Some(LValue::Local(local)),
            Self::Global(global) => Some(LValue::Global(global)),
            Self::Index(index) => Some(LValue::Index(index)),
            _ => None,
        }
    }
}

impl fmt::Display for RValue {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        match self {
            RValue::Local(local) => write!(f, "{}", local),
            RValue::Global(global) => write!(f, "{}", global),
            RValue::Literal(literal) => write!(f, "{}", literal),
            RValue::Call(call) => write!(f, "{}", call),
            RValue::MethodCall(method_call) => write!(f, "{}", method_call),
            RValue::VarArg(var_arg) => write!(f, "{}", var_arg),
            RValue::Table(table) => write!(f, "{}", table),
            RValue::Index(index) => write!(f, "{}", index),
            RValue::Unary(unary) => write!(f, "{}", unary),
            RValue::Binary(binary) => write!(f, "{}", binary),
            RValue::Closure(closure) => write!(f, "{}", closure),
            RValue::Select(select) => write!(f, "{}", select),
        }
    }
}

#[enum_dispatch(SideEffects, Traverse)]
#[derive(Debug, Clone, PartialEq, EnumAsInner)]
pub enum LValue {
    Local(RcLocal),
    Global(Global),
    Index(Index),
}

impl LocalRw for LValue {
    fn values_read(&self) -> Vec<&RcLocal> {
        match self {
            LValue::Local(_) => Vec::new(),
            LValue::Global(global) => global.values_read(),
            LValue::Index(index) => index.values_read(),
        }
    }

    fn values_read_mut(&mut self) -> Vec<&mut RcLocal> {
        match self {
            LValue::Local(_) => Vec::new(),
            LValue::Global(global) => global.values_read_mut(),
            LValue::Index(index) => index.values_read_mut(),
        }
    }

    fn values_written(&self) -> Vec<&RcLocal> {
        match self {
            LValue::Local(local) => vec![local],
            LValue::Global(global) => global.values_written(),
            LValue::Index(index) => index.values_written(),
        }
    }

    fn values_written_mut(&mut self) -> Vec<&mut RcLocal> {
        match self {
            LValue::Local(local) => vec![local],
            LValue::Global(global) => global.values_written_mut(),
            LValue::Index(index) => index.values_written_mut(),
        }
    }
}

impl fmt::Display for LValue {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        match self {
            LValue::Local(local) => write!(f, "{}", local),
            LValue::Global(global) => write!(f, "{}", global),
            LValue::Index(index) => write!(f, "{}", index),
        }
    }
}

#[derive(Debug, PartialEq, Eq, Clone)]
pub struct Comment {
    pub text: String,
}

impl Comment {
    pub fn new(text: String) -> Self {
        Self { text }
    }
}

impl Traverse for Comment {}

impl SideEffects for Comment {}

impl LocalRw for Comment {}

#[enum_dispatch(LocalRw, SideEffects, Traverse)]
#[derive(Debug, Clone, PartialEq, EnumAsInner)]
pub enum Statement {
    Empty(Empty),
    Call(Call),
    MethodCall(MethodCall),
    Assign(Assign),
    If(If),
    Goto(Goto),
    Label(Label),
    While(While),
    Repeat(Repeat),
    NumForInit(NumForInit),
    NumForNext(NumForNext),
    NumericFor(NumericFor),
    GenericForInit(GenericForInit),
    GenericForNext(GenericForNext),
    GenericFor(GenericFor),
    Return(Return),
    Continue(Continue),
    Break(Break),
    Close(Close),
    SetList(SetList),
    Comment(Comment),
}

#[derive(Debug, PartialEq, Eq, Clone)]
pub struct Empty {}

impl SideEffects for Empty {}

impl LocalRw for Empty {}

impl Traverse for Empty {}

impl fmt::Display for Empty {
    fn fmt(&self, _: &mut fmt::Formatter) -> fmt::Result {
        Ok(())
    }
}

impl fmt::Display for Comment {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        write!(f, "-- {}", self.text)
    }
}

impl fmt::Display for Statement {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        match self {
            // TODO: STYLE: order in same order as `Statement` enum
            Statement::Call(call) => write!(f, "{}", call),
            Statement::MethodCall(method_call) => write!(f, "{}", method_call),
            Statement::Assign(assign) => write!(f, "{}", assign),
            // TODO: STYLE: replace all `if_` with `r#if`, etc
            Statement::If(if_) => write!(f, "{}", if_),
            Statement::Goto(goto) => write!(f, "{}", goto),
            Statement::Label(label) => write!(f, "{}", label),
            Statement::While(while_) => write!(f, "{}", while_),
            Statement::Repeat(repeat) => write!(f, "{}", repeat),
            Statement::NumForInit(num_for_init) => write!(f, "{}", num_for_init),
            Statement::NumForNext(num_for_next) => write!(f, "{}", num_for_next),
            Statement::NumericFor(numeric_for) => write!(f, "{}", numeric_for),
            Statement::GenericForInit(generic_for_init) => write!(f, "{}", generic_for_init),
            Statement::GenericForNext(generic_for_next) => write!(f, "{}", generic_for_next),
            Statement::GenericFor(generic_for) => write!(f, "{}", generic_for),
            Statement::Return(return_) => write!(f, "{}", return_),
            Statement::Continue(continue_) => write!(f, "{}", continue_),
            Statement::Break(break_) => write!(f, "{}", break_),
            Statement::Comment(comment) => write!(f, "{}", comment),
            Statement::SetList(setlist) => write!(f, "{}", setlist),
            Statement::Close(close) => write!(f, "{}", close),
            Statement::Empty(empty) => write!(f, "{}", empty),
        }
    }
}

#[derive(Debug, PartialEq, Clone, Default, From)]
pub struct Block(pub Vec<Statement>);

// rust-analyzer doesnt like derive_more :/
impl Deref for Block {
    type Target = Vec<Statement>;

    fn deref(&self) -> &Self::Target {
        &self.0
    }
}

impl DerefMut for Block {
    fn deref_mut(&mut self) -> &mut Self::Target {
        &mut self.0
    }
}

impl fmt::Display for Block {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        Formatter::format(self, f, Default::default())
    }
}
