FROM rust:latest AS builder
WORKDIR /app
COPY . .
RUN cargo build --release --bin medal
RUN strip target/release/medal

FROM debian:12-slim AS runtime
COPY --from=builder /app/target/release/medal /bin/medal
ENTRYPOINT ["/bin/medal"]