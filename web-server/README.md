# web-server

A web server written in Rust for Medal's Luau decompiler.

## Script

### Windows

```lua
getgenv().decompile = function(script_instance)
    local bytecode = getscriptbytecode(script_instance)
    local encoded = crypt.base64encode(bytecode)
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

### Android Emulator

> [!NOTE]
>
> - If you have a proxy enabled in WiFi settings, add `10.0.2.2` to the exceptions or disable the proxy
> - If you are using a VPN, allow LAN connections

Run the web-server on your actual machine.

If you are using an emulator, `localhost` will not work since that will refer to the emulator's device.
Instead, we must use `10.0.2.2` instead.
To test if everything is working, on your actual machine [http://localhost:3000/] should load fine, and on the emulator's browser, [http://10.0.2.2:3000/] should load. If your emulator is giving you HTTP connection fail errors, then you can try [ngrok](https://ngrok.com/) instead.

```lua
getgenv().decompile = function(script_instance)
    local bytecode = getscriptbytecode(script_instance)
    local encoded = crypt.base64encode(bytecode)
    return request(
        {
            Url = "http://10.0.2.2:3000/decompile",
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
