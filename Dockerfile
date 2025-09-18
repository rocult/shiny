FROM rust:latest AS builder
WORKDIR /app
COPY . .
RUN cargo build --release --bin web-server
RUN strip target/release/web-server

FROM debian:12-slim AS runtime
COPY --from=builder /app/target/release/web-server /bin/web-server
ENTRYPOINT ["/bin/web-server"]