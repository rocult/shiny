use super::list::parse_list;
use nom::{
    number::complete::{le_f32, le_f64, le_i32, le_u32, le_u8},
    IResult,
};
use nom_leb128::leb128_usize;

const CONSTANT_NIL: u8 = 0;
const CONSTANT_BOOLEAN: u8 = 1;
const CONSTANT_NUMBER: u8 = 2;
const CONSTANT_STRING: u8 = 3;
const CONSTANT_IMPORT: u8 = 4;
const CONSTANT_TABLE: u8 = 5;
const CONSTANT_CLOSURE: u8 = 6;
const CONSTANT_VECTOR: u8 = 7;
const CONSTANT_TABLE_WITH_CONSTANTS: u8 = 8;
const CONSTANT_INTEGER: u8 = 9;

#[derive(Debug)]
pub enum Constant {
    Nil,
    Boolean(bool),
    Number(f64),
    String(usize),
    Import(usize),
    Table(Vec<usize>),
    Closure(usize),
    Vector(f32, f32, f32, f32),
    TableWithConstants(Vec<(usize, i32)>),
    Integer(i64),
}

fn leb128_u64(input: &[u8]) -> IResult<&[u8], u64> {
    let mut result: u64 = 0;
    let mut shift: u32 = 0;
    let mut i = 0;
    loop {
        let byte = input[i];
        i += 1;
        result |= ((byte & 0x7f) as u64) << shift;
        if byte & 0x80 == 0 {
            break;
        }
        shift += 7;
    }
    Ok((&input[i..], result))
}

impl Constant {
    pub(crate) fn parse(input: &[u8]) -> IResult<&[u8], Self> {
        let (input, tag) = le_u8(input)?;
        match tag {
            CONSTANT_NIL => Ok((input, Constant::Nil)),
            CONSTANT_BOOLEAN => {
                let (input, value) = le_u8(input)?;
                Ok((input, Constant::Boolean(value != 0u8)))
            }
            CONSTANT_NUMBER => {
                let (input, value) = le_f64(input)?;
                Ok((input, Constant::Number(value)))
            }
            CONSTANT_STRING => {
                let (input, string_index) = leb128_usize(input)?;
                Ok((input, Constant::String(string_index)))
            }
            CONSTANT_IMPORT => {
                let (input, import_index) = le_u32(input)?;
                Ok((input, Constant::Import(import_index as usize)))
            }
            CONSTANT_TABLE => {
                let (input, keys) = parse_list(input, leb128_usize)?;
                Ok((input, Constant::Table(keys)))
            }
            CONSTANT_CLOSURE => {
                let (input, f_id) = leb128_usize(input)?;
                Ok((input, Constant::Closure(f_id)))
            }
            CONSTANT_VECTOR => {
                let (input, x) = le_f32(input)?;
                let (input, y) = le_f32(input)?;
                let (input, z) = le_f32(input)?;
                let (input, w) = le_f32(input)?;
                Ok((input, Constant::Vector(x, y, z, w)))
            }
            CONSTANT_TABLE_WITH_CONSTANTS => {
                let (mut input, key_count) = leb128_usize(input)?;
                let mut entries = Vec::with_capacity(key_count);
                for _ in 0..key_count {
                    let (rest, key_idx) = leb128_usize(input)?;
                    let (rest, value_idx) = le_i32(rest)?;
                    entries.push((key_idx, value_idx));
                    input = rest;
                }
                Ok((input, Constant::TableWithConstants(entries)))
            }
            CONSTANT_INTEGER => {
                let (input, sign_flag) = le_u8(input)?;
                let (input, magnitude) = leb128_u64(input)?;
                let value = if sign_flag != 0 {
                    (!magnitude).wrapping_add(1) as i64
                } else {
                    magnitude as i64
                };
                Ok((input, Constant::Integer(value)))
            }
            _ => panic!("{}", tag),
        }
    }
}
