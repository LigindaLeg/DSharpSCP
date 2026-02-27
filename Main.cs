using System;
using System.Threading.Tasks;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using Discord;
using Discord.WebSocket;
using LabApi.Features.Console;

namespace DSharpSCP;

public class Main : Plugin<Config>
{
    public override void Enable()
    {
        Instance = this;
        _ = Task.Run(StartBotAsync);
        DefaultGuild = Config.DiscordGuildID;
    }

    public override void Disable()
    {
        if (Client != null)
        {
            _ = Client.StopAsync();
            _ = Client.LogoutAsync();
        }
        DefaultGuild = 0;
        Instance = null;
    }

    private async Task StartBotAsync()
    {
        try
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents =
                    GatewayIntents.Guilds |
                    GatewayIntents.GuildMessages |
                    GatewayIntents.MessageContent
            };

            Client = new DiscordSocketClient(config);

            if (Config.Debug)
            {
                Client.Log += LogAsync;
            }
            Client.Ready += OnReady;

            string token = this.Config.DiscordBotToken;

            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();
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
