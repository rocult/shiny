use clap::Parser;

use crate::commands::{Cli, decompile, serve};

mod commands;

#[tokio::main]
async fn main() -> Result<(), std::io::Error> {
    // Setup the logger
    let subscriber = tracing_subscriber::fmt()
        .compact()
        .with_file(true)
        .with_line_number(true)
        .with_thread_ids(true)
        .with_target(false)
        .finish();
    tracing::subscriber::set_global_default(subscriber)
        .expect("failed to set global tracing subscriber");

    // Run the requested command
    let cli = Cli::parse();
    match cli.command {
        commands::Commands::Decompile {
            input,
            output,
            encode_key,
            lua51,
        } => decompile(&input, &output, encode_key, lua51)?,
        commands::Commands::Serve { port, luau, lua51 } => serve(port, luau, lua51).await?,
    }

    // Done
    Ok(())
}
