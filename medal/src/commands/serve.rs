use axum::{
    Router,
    body::Bytes,
    extract::Query,
    routing::{get, post},
};
use serde::Deserialize;
use tokio::net::TcpListener;
use tracing::info;

use crate::commands::decompile_no_io;

pub async fn serve(port: u16, luau: bool, lua51: bool) -> Result<(), std::io::Error> {
    // Build our application with a route
    let mut app = Router::new().route("/", get(ok));

    if luau {
        app = app.route("/luau/decompile", post(decompile_luau));
    }

    if lua51 {
        app = app.route("/lua51/decompile", post(decompile_lua51));
    }

    // Run the web server
    let listener = TcpListener::bind(format!("0.0.0.0:{port}")).await?;
    info!("🚀 Listening on {}", listener.local_addr()?);
    axum::serve(listener, app).await
}

async fn ok() -> &'static str {
    "yep web-server is on"
}

#[inline]
pub const fn default_encode_key() -> u8 {
    203
}

#[derive(Deserialize)]
struct EncodeKey {
    #[serde(default = "default_encode_key")]
    encode_key: u8,
}

async fn decompile_luau(query: Query<EncodeKey>, body: Bytes) -> String {
    decompile_no_io(body, query.encode_key, false)
}

async fn decompile_lua51(body: Bytes) -> String {
    decompile_no_io(body, default_encode_key(), true)
}
