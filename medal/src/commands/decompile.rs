use std::path::Path;

use base64::prelude::*;

pub fn decompile_no_io<T>(bytecode: T, encode_key: u8, lua51: bool) -> String
where
    T: Into<Vec<u8>>,
{
    let mut bytecode = bytecode.into();
    if let Ok(decoded) = BASE64_STANDARD.decode(&bytecode) {
        bytecode = decoded;
    }

    if lua51 {
        lua51_lifter::decompile_bytecode(&bytecode)
    } else {
        luau_lifter::decompile_bytecode(&bytecode, encode_key)
    }
}

pub fn decompile(
    input: &Path,
    output: &Path,
    encode_key: u8,
    lua51: bool,
) -> Result<(), std::io::Error> {
    let bytecode = std::fs::read(input)?;
    let out = decompile_no_io(bytecode, encode_key, lua51);
    std::fs::write(output, out)
}
