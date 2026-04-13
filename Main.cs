using System;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Loader.Features.Plugins;

namespace DSharpSCP;

public class Main : Plugin<Config>
{
    public static Main Instance { get; private set; }

    public DiscordSocketClient Client { get; private set; }

    public ulong DefaultGuild { get; private set; }

    public override string Name => "DSharpSCP";

    public override string Description => "Discord Bot into SCP:SL";

    public override string Author => "liginda";

    public override Version Version => new(2, 0, 0);

    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

    public override void Enable()
    {
        Instance = this;

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        ServicePointManager.Expect100Continue = false;

        DefaultGuild = this.Config.DiscordGuildID;

        if (this.Config.Debug)
        {
            Logger.Debug("DSharpSCP enabled.");
            Logger.Debug($"Default guild id set to {DefaultGuild}");
        }

        _ = Task.Run(StartBotAsync).ContinueWith(t =>
        {
            if (t.Exception != null)
                Logger.Error($"DSharpSCP startup task failed: {t.Exception}");
        }, TaskContinuationOptions.OnlyOnFaulted);
    }

    public override void Disable()
    {
        if (this.Config.Debug)
            Logger.Debug("DSharpSCP disabled.");

        var client = Client;
        Client = null;
        DefaultGuild = 0;

        if (client != null)
        {
            client.Ready -= OnReady;
            client.Log -= LogAsync;

            try
            {
                client.StopAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to stop Discord client: {ex}");
            }

            try
            {
                client.LogoutAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to log out Discord client: {ex}");
            }

            client.Dispose();
        }

        Instance = null;
    }

    private async Task StartBotAsync()
    {
        var instance = Instance;
        if (instance == null)
            return;

        var debug = instance.Config.Debug;
        var token = instance.Config.DiscordBotToken?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(token) || string.Equals(token, "token here", StringComparison.OrdinalIgnoreCase))
        {
            Logger.Error("Discord bot token is not configured.");
            return;
        }

        try
        {
            if (debug)
                Logger.Debug("Setting up Discord bot configuration...");

            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent
            };

            var client = new DiscordSocketClient(config);
            client.Ready += OnReady;
            client.Log += LogAsync;

            Client = client;

            if (debug)
                Logger.Debug("Logging into Discord...");

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
        }
        catch (Exception ex)
        {
            Logger.Error($"Discord bot failed to start: {ex}");
        }
    }

    private Task LogAsync(LogMessage msg)
    {
        if (Instance?.Config.Debug == true)
            Logger.Debug($"[Discord] {msg}");

        return Task.CompletedTask;
    }

    private Task OnReady()
    {
        var client = Client;
        if (client != null)
            Logger.Info($"Бот запущен как {client.CurrentUser}");

        return Task.CompletedTask;
    }
}
