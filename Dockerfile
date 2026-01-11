# Build stage
FROM rust:latest AS builder
WORKDIR /app

# Install nightly Rust
RUN rustup install nightly

# Copy source
COPY . .

# Build with nightly and strip binary
RUN cargo +nightly build --release --bin medal && \
    strip target/release/medal

# Runtime stage (minimal size)
FROM debian:12-slim AS runtime

# Install ca-certificates for HTTPS requests
RUN apt-get update && apt-get install -y ca-certificates && rm -rf /var/lib/apt/lists/*

# Copy binary from builder
COPY --from=builder /app/target/release/medal /usr/local/bin/medal

# Expose port
EXPOSE 3000

# Run the web server
ENTRYPOINT ["/usr/local/bin/medal", "serve", "--port", "3000"]
