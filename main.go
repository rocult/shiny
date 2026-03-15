package main

import (
	"fmt"
	"os"
	"os/exec"
	"strconv"

	"github.com/valyala/fasthttp"
)

func medalBin() string {
	if v := os.Getenv("MEDAL_BIN"); v != "" {
		return v
	}
	return "/bin/medal"
}

func runDecompile(body []byte, encodeKey uint8, lua51 bool) (string, error) {
	inFile, err := os.CreateTemp("", "medal-in-*")
	if err != nil {
		return "", err
	}
	defer os.Remove(inFile.Name())

	if _, err := inFile.Write(body); err != nil {
		inFile.Close()
		return "", err
	}
	inFile.Close()

	outFile, err := os.CreateTemp("", "medal-out-*")
	if err != nil {
		return "", err
	}
	outFile.Close()
	defer os.Remove(outFile.Name())

	args := []string{
		"decompile",
		"--input", inFile.Name(),
		"--output", outFile.Name(),
		"--encode-key", strconv.Itoa(int(encodeKey)),
	}
	if lua51 {
		args = append(args, "--lua51")
	}

	cmd := exec.Command(medalBin(), args...)
	if out, err := cmd.CombinedOutput(); err != nil {
		return "", fmt.Errorf("medal error: %w\n%s", err, out)
	}

	result, err := os.ReadFile(outFile.Name())
	if err != nil {
		return "", err
	}
	return string(result), nil
}

func handler(ctx *fasthttp.RequestCtx) {
	path := string(ctx.Path())
	method := string(ctx.Method())

	switch {
	case method == "GET" && path == "/":
		ctx.SetBodyString("yep web-server is on")

	case method == "POST" && path == "/luau/decompile":
		encodeKey := uint8(203)
		if v := ctx.QueryArgs().Peek("encode_key"); len(v) > 0 {
			if n, err := strconv.ParseUint(string(v), 10, 8); err == nil {
				encodeKey = uint8(n)
			}
		}
		result, err := runDecompile(ctx.PostBody(), encodeKey, false)
		if err != nil {
			ctx.SetStatusCode(fasthttp.StatusInternalServerError)
			ctx.SetBodyString(err.Error())
			return
		}
		ctx.SetBodyString(result)

	case method == "POST" && path == "/lua51/decompile":
		result, err := runDecompile(ctx.PostBody(), 203, true)
		if err != nil {
			ctx.SetStatusCode(fasthttp.StatusInternalServerError)
			ctx.SetBodyString(err.Error())
			return
		}
		ctx.SetBodyString(result)

	default:
		ctx.SetStatusCode(fasthttp.StatusNotFound)
	}
}

func main() {
	port := os.Getenv("PORT")
	if port == "" {
		port = "3000"
	}
	addr := "0.0.0.0:" + port
	fmt.Printf("Listening on %s\n", addr)
	if err := fasthttp.ListenAndServe(addr, handler); err != nil {
		fmt.Fprintf(os.Stderr, "server error: %v\n", err)
		os.Exit(1)
	}
}
