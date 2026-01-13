using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Builder;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using MoonsecDeobfuscator.Deobfuscation;
using MoonsecDeobfuscator.Bytecode.Models;

namespace GalacticBytecodeBot
{
    public class Program
    {
        private DiscordSocketClient _client = null!;
        private InteractionService _interactions = null!;
        private IServiceProvider _services = null!;

        public static async Task Main(string[] args)
        {
            // Load environment variables
            try { DotNetEnv.Env.Load(); } catch { Console.WriteLine("No .env file found, using environment variables."); }
            
            // Start health check server for Render
            _ = StartHealthCheckServer();

            await new Program().RunAsync();
        }

        private static async Task StartHealthCheckServer()
        {
            var portStr = Environment.GetEnvironmentVariable("PORT") ?? "3000";
            var builder = WebApplication.CreateBuilder();
            
            // FIXED: Use UseSetting instead of UseUrls for .NET 9.0
            builder.WebHost.UseSetting("urls", $"http://0.0.0.0:{portStr}");
            
            var app = builder.Build();
            app.MapGet("/", () => "MoonSec Bot is running.");
            
            Console.WriteLine($"🌐 Health check listening on port {portStr}");
            await app.RunAsync();
        }

        public async Task RunAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.DirectMessages,
                AlwaysDownloadUsers = true
            });

            _interactions = new InteractionService(_client.Rest);

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_interactions)
                .AddSingleton<DeobfuscationService>()
                .BuildServiceProvider();

            _client.Log += msg => { Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {msg}"); return Task.CompletedTask; };
            _client.Ready += ReadyAsync;
            _client.InteractionCreated += HandleInteractionAsync;

            var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrEmpty(token))
                throw new Exception("DISCORD_TOKEN missing in environment variables");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task ReadyAsync()
        {
            await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _interactions.RegisterCommandsGloballyAsync(true);

            await _client.SetStatusAsync(UserStatus.Online);
            await _client.SetActivityAsync(new Game("🌙 MoonSec → Medal Pipeline"));
            Console.WriteLine($"✅ Bot connected as {_client.CurrentUser}");
            
            if (!File.Exists("/app/medal"))
                Console.WriteLine("⚠️ WARNING: Medal not found at /app/medal");
            else
                Console.WriteLine("✅ Medal found at /app/medal");
        }

        private async Task HandleInteractionAsync(SocketInteraction interaction)
        {
            var context = new SocketInteractionContext(_client, interaction);
            await _interactions.ExecuteCommandAsync(context, _services);
        }
    }

    public class DeobfuscationModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly DeobfuscationService _service;

        public DeobfuscationModule(DeobfuscationService service)
        {
            _service = service;
        }

        [SlashCommand("deobfuscate", "Deobfuscates a MoonSec-protected Lua file")]
        public async Task DeobfuscateCommand(
            [Summary("file", "Lua or text file")] IAttachment file)
        {
            await DeferAsync();

            if (!file.Filename.EndsWith(".lua") && !file.Filename.EndsWith(".txt") && 
                !file.Filename.EndsWith(".luau"))
            {
                await FollowupAsync("❌ Only `.lua`, `.luau` or `.txt` files are allowed.");
                return;
            }

            try
            {
                using var http = new HttpClient();
                var bytes = await http.GetByteArrayAsync(file.Url);
                var sourceCode = Encoding.UTF8.GetString(bytes);

                // Step 1: Deobfuscate and create bytecode
                await FollowupAsync("🔄 **Step 1/3:** Deobfuscating with MoonSec...");
                var bytecode = _service.GenerateBytecode(sourceCode);
                
                var tempBytecode = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.luac");
                await File.WriteAllBytesAsync(tempBytecode, bytecode);

                // Step 2: Decompile with Medal
                await ModifyOriginalResponseAsync(msg => msg.Content = "🔄 **Step 2/3:** Decompiling with Medal...");
                var decompiled = await _service.DecompileWithMedal(tempBytecode);

                try { File.Delete(tempBytecode); } catch { }

                if (string.IsNullOrWhiteSpace(decompiled))
                {
                    await ModifyOriginalResponseAsync(msg => msg.Content = "❌ Medal failed to decompile.");
                    return;
                }

                // Step 3: Send result - FIXED: Build after setting description
                await ModifyOriginalResponseAsync(msg => msg.Content = "✅ **Step 3/3:** Sending result...");

                var embedBuilder = new EmbedBuilder()
                    .WithTitle("✅ Deobfuscation Complete")
                    .WithColor(Color.Green)
                    .WithFooter($"Processed by {Context.User.Username}");

                if (decompiled.Length > 2000)
                {
                    // Send as file
                    await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(decompiled));
                    await FollowupWithFileAsync(stream, "decompiled.lua", 
                        text: $"{Context.User.Mention} here is your decompiled code:", 
                        embed: embedBuilder.Build());
                }
                else
                {
                    // Send in embed
                    embedBuilder.WithDescription($"```lua\n{decompiled}\n```");
                    await FollowupAsync($"{Context.User.Mention}", embed: embedBuilder.Build());
                }

                await DeleteOriginalResponseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex}");
                await FollowupAsync($"❌ Processing failed: `{ex.Message}`");
            }
        }
    }

    public class DeobfuscationService
    {
        public byte[] GenerateBytecode(string sourceCode)
        {
            var deob = new Deobfuscator();
            var result = deob.Deobfuscate(sourceCode);
            
            using var ms = new MemoryStream();
            var serializer = new MoonsecDeobfuscator.Deobfuscation.Bytecode.Serializer(ms);
            serializer.Serialize(result);
            return ms.ToArray();
        }

        public async Task<string> DecompileWithMedal(string bytecodePath)
        {
            var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/app/medal",
                    Arguments = $"\"{bytecodePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();
            
            var exitTask = process.WaitForExitAsync();
            var timeoutTask = Task.Delay(TimeSpan.FromMinutes(3), cts.Token);
            
            if (await Task.WhenAny(exitTask, timeoutTask) == timeoutTask)
            {
                process.Kill();
                throw new TimeoutException("Medal timed out after 3 minutes");
            }

            if (process.ExitCode != 0)
                throw new Exception($"Medal failed: {error}");

            return output;
        }
    }
}
