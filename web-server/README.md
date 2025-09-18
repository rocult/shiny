# web-server

A web server written in Rust for Medal's Luau decompiler.

## Script

```lua
getgenv().decompile = function(script_instance)
    local bytecode = getscriptbytecode(script_instance)
    local encoded = crypt.base64.encode(bytecode)
    return request(
        {
            Url = "http://localhost:3000/decompile",
            Method = "POST",
            Body = encoded
        }
    ).Body
end

loadstring(game:HttpGet("https://raw.githubusercontent.com/luau/SynSaveInstance/main/saveinstance.luau"))()({
  mode = "scripts",
  NilInstances = true,
})
```
