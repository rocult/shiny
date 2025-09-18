use std::path::PathBuf;

use clap::{Parser, Subcommand};

mod decompile;
pub use decompile::*;
mod serve;
pub use serve::*;

#[derive(Debug, Subcommand)]
pub enum Commands {
    /// Decompile a single bytecode file, either raw or base64 encoded
    Decompile {
        /// The path to the bytecode
        #[arg(short, long)]
        input: PathBuf,

        /// The path to output the bytecode
        #[arg(short, long)]
        output: PathBuf,

        /// If the supplied bytecode is Luau, what encode key to use (shuffles instructions)
        #[arg(short, long, default_value_t = default_encode_key())]
        encode_key: u8,

        /// Indicate the bytecode is Lua 5.1, assumes Luau by default
        #[arg(short, long, default_value_t = false)]
        lua51: bool,
    },

    /// Start a simple web server to handle decompiling.
    Serve {
        /// The port the web server should listen on
        #[arg(short, long, default_value_t = 3000)]
        port: u16,

        /// Whether to allow decompiling Luau bytecode
        #[arg(short, long, default_value_t = true)]
        luau: bool,

        /// Whether to allow decompiling Lua 5.1 bytecode
        #[arg(short, long, default_value_t = false)]
        lua51: bool,
    },
}

#[derive(Debug, Parser)]
#[command(version, about, long_about = None)]
pub struct Cli {
    #[command(subcommand)]
    pub command: Commands,
}
