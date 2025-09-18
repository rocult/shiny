use std::{env, io};

use axum::{
    Router,
    body::Bytes,
    routing::{get, post},
};
use base64::prelude::*;
use tokio::net::TcpListener;
use tracing::info;

#[tokio::main]
async fn main() -> Result<(), io::Error> {
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

    // Parse port from CLI args
    let port: u16 = env::args()
        .nth(1)
        .unwrap_or_else(|| "3000".to_string())
        .parse()
        .unwrap_or(8080);
    let bind_addr = format!("0.0.0.0:{}", port);

    // Build our application with a route
    let app = Router::new()
        .route("/decompile", post(decompile))
        .route("/", get(ok));

    // Run the web server
    let listener = TcpListener::bind(&bind_addr).await?;
    info!("🚀 Listening on {}", listener.local_addr()?);
    axum::serve(listener, app).await
}

async fn ok() -> &'static str {
    "yep web-server is on"
}

async fn decompile(body: Bytes) -> String {
    let mut bytecode = Vec::new();
    if BASE64_STANDARD.decode_slice(&body, &mut bytecode).is_err() {
        bytecode = body.into();
    }
    luau_lifter::decompile_bytecode(&bytecode, 203)
}
