using System;
using System.Net;
using System.Threading.Tasks;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using Discord;
using Discord.WebSocket;
using LabApi.Features.Console;
using MEC;

namespace DSharpSCP;

public class Main : Plugin<Config>
{
    public override void Enable()
    {
        Instance = this;
        if (Instance.Config.Debug)
        {
            Logger.Debug("DSharpSCP enabled!");
        }
        // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        // ServicePointManager.Expect100Continue = false;
        if (Instance.Config.Debug)
        {
            Logger.Debug($"Default guild id setted to {Config.DiscordGuildID}");
        }
        DefaultGuild = Config.DiscordGuildID;
        if (Instance.Config.Debug)
        {
            Logger.Debug("Setuping bot...");
        }

        _ = Task.Run(StartBotAsync).ContinueWith(t =>
        {
            if (t.Exception != null)
                Logger.Error(t.Exception);
        });
    }

    public override void Disable()
    {
        if (Instance.Config.Debug)
        {
            Logger.Debug("DSharpSCP disabled!");
        }
        if (Client != null)
        {
            _ = Client.StopAsync();
            _ = Client.LogoutAsync();
        }
        DefaultGuild = 0;
        if (Instance.Config.Debug)
        {
            Logger.Debug("Default guild id setted to 0");
        }
        Instance = null;
    }

    private async Task StartBotAsync()
    {
        if (Instance.Config.Debug)
        {
            Logger.Debug("StartBotAsync started!");
        }
        try
        {
            if (Instance.Config.Debug)
            {
                Logger.Debug("Setuping bot configuration...");
            }
            var config = new DiscordSocketConfig
            {
                GatewayIntents =
                    GatewayIntents.Guilds |
                    GatewayIntents.GuildMessages |
                    GatewayIntents.MessageContent
            };
            if (Instance.Config.Debug)
            {
                Logger.Debug("Creating bot client...");
            }
            Client = new DiscordSocketClient(config);
            if (Instance.Config.Debug)
            {
                Logger.Debug("Registering events...");
            }
            if (Config.Debug)
            {
                if (Instance.Config.Debug)
                {
                    Logger.Debug("Debug is enabled!");
                }
                Client.Log += LogAsync;
            }
            Client.Ready += OnReady;
            if (Instance.Config.Debug)
            {
                Logger.Debug("Setting token...");
            }
            string token = this.Config.DiscordBotToken;
            try
            {
                if (Instance.Config.Debug)
                {
                    Logger.Debug("Logging in...");
                }
                await Client.LoginAsync(TokenType.Bot, token);
                if (Instance.Config.Debug)
                {
                    Logger.Debug("Starting bot...");
                }
                await Client.StartAsync();
            }
            catch (Exception e)
            {
                Logger.Error("Error on starting bot: " + e);
            }
            
        }
        catch (Exception ex)
        {
            Logger.Error($"Discord bot failed to start: {ex}");
        }
    }

    
    public DiscordSocketClient Client;
    public ulong DefaultGuild = 0;
    public static Main Instance;
    public override string Name => "DSharpSCP";

    public override string Description => "Discord Bot into SCP:SL";

    public override string Author => "liginda";

    public override Version Version => new(1,0,0);

    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);
    
    private Task LogAsync(LogMessage msg)
    {
        Logger.Debug($"[Discord] {msg}");
        return Task.CompletedTask;
    }

    private Task OnReady()
    {
        Logger.Info($"Бот запущен как {Client.CurrentUser}");
        return Task.CompletedTask;
    }
}