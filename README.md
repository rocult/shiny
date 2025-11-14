# Shiny

A blazingly fast Lua(u) decompiler, forked from [medal](https://github.com/shrimp-nz/medal).

## Usage

```bash
Usage: medal.exe <COMMAND>

Commands:
  decompile  Decompile a single bytecode file, either raw or base64 encoded
  serve      Start a simple web server to handle decompiling
  help       Print this message or the help of the given subcommand(s)

Options:
  -h, --help     Print help
  -V, --version  Print version
```

## Script

When using `medal serve`, you can use the decompiler directly inside of your executor.
These scripts assume you are self-hosting the decompiler.
If you are using some sort of API, make sure to replace `http://localhost:3000` or `http://10.0.2.2:3000` with the correct API base.

### Windows

```lua
getgenv().decompile = function(script_instance)
  local bytecode = getscriptbytecode(script_instance)
  local encoded = crypt.base64encode(bytecode)
  return request(
    {
      Url = "http://localhost:3000/luau/decompile",
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
      Url = "http://10.0.2.2:3000/luau/decompile",
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

## License

The [original license](https://github.com/shrimp-nz/medal/blob/main/LICENSE.txt) for medal is retained, but the license for this fork has been changed to stop people from profiting from open source work for free.

## Credits

All credits to the medal project goes to in honor and memory of

- Jujhar Singh (KowalskiFX)
- Mathias Pedersen (Customality)

Keep the Singh and Pedersen family in your guys' prayers, we love you both.
