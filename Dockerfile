# Stage 1: Build Rust binary
FROM rust:latest AS rust-builder
WORKDIR /app
COPY . .
RUN cargo build --release --bin medal
RUN strip target/release/medal

# Stage 2: Build Go binary
FROM golang:1.22 AS go-builder
WORKDIR /app
COPY go.mod main.go ./
RUN go mod tidy
RUN CGO_ENABLED=0 GOOS=linux go build -o server .

# Stage 3: Runtime
FROM debian:12-slim
COPY --from=rust-builder /app/target/release/medal /bin/medal
COPY --from=go-builder /app/server /bin/server
ENTRYPOINT ["/bin/server"]
