using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using MEC;

namespace DSharpSCP.Status;

public class Main : Plugin<Config>
{
    private static readonly SemaphoreSlim StatusLock = new(1, 1);

    public static Main Instance { get; private set; }

    public override string Name => "DSharpSCP.Status";

    public override string Description => "Status Module for DSharpSCP";

    public override string Author => "liginda";

    public override Version Version => new(2, 0, 0);

    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

    public override void Enable()
    {
        Instance = this;
        PlayerEvents.Joined += Joined;
        PlayerEvents.Left += Left;

        Timing.CallDelayed(1f, () => _ = UpdateDiscordStatus());
    }

    public override void Disable()
    {
        PlayerEvents.Joined -= Joined;
        PlayerEvents.Left -= Left;
        Instance = null;
    }

    private static void Joined(PlayerJoinedEventArgs e)
    {
        if (e.Player == null || e.Player.IsDummy || e.Player.IsNpc || e.Player.IsHost)
            return;

        _ = UpdateDiscordStatus();
    }

    private static void Left(PlayerLeftEventArgs e)
    {
        if (e.Player == null || e.Player.IsDummy || e.Player.IsNpc || e.Player.IsHost)
            return;

        _ = UpdateDiscordStatus();
    }

    private static async Task UpdateDiscordStatus()
    {
        var instance = Instance;
        var core = DSharpSCP.Main.Instance;
        var client = core?.Client;

        if (instance == null || core == null || client == null)
            return;

        await StatusLock.WaitAsync();
        try
        {
            if (Player.ConnectionsCount <= 0)
            {
                await client.SetStatusAsync(instance.Config.Idle ? UserStatus.Idle : UserStatus.Online);
                await client.SetCustomStatusAsync(instance.Config.EmptyStatus);
            }
            else
            {
                if (client.Status != UserStatus.Online)
                    await client.SetStatusAsync(UserStatus.Online);

                await client.SetCustomStatusAsync(
                    instance.Config.ServerStatus
                        .Replace("{players}", Player.ConnectionsCount.ToString())
                        .Replace("{max}", Server.MaxPlayers.ToString()));
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"Discord status update failed: {ex}");
        }
        finally
        {
            StatusLock.Release();
        }
    }
}
