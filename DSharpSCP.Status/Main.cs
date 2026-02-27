using System;
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
    public override void Enable()
    {
        ins = this;
        PlayerEvents.Joined += Joined;
        PlayerEvents.Left += Left;
        Timing.CallDelayed(1f, delegate()
        {
            _ = UpdateDiscordStatus();
        });
    }

    public override void Disable()
    {
        PlayerEvents.Joined -= Joined;
        PlayerEvents.Left -= Left;
        ins = null;
    }

    public static Main ins;
    
    public override string Name => "DSharpSCP.Status";

    public override string Description => "Status Module for DSharpSCP";

    public override string Author => "liginda";

    public override Version Version => new(1,0,0);

    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

    private static void Joined(PlayerJoinedEventArgs e)
    {
        if (e.Player == null || e.Player.IsDummy || e.Player.IsHost)
            return;
        _ = UpdateDiscordStatus();
    }
    private static void Left(PlayerLeftEventArgs e)
    {
        if (e.Player == null || e.Player.IsDummy || e.Player.IsHost)
            return;
        _ = UpdateDiscordStatus();
    }
    private static async Task UpdateDiscordStatus()
    {
        try
        {
            if (DSharpSCP.Main.Instance.Client != null)
            {
                if (Player.ConnectionsCount == 0)
                {
                    if (ins.Config.Idle)
                    {
                        await DSharpSCP.Main.Instance.Client.SetStatusAsync(UserStatus.Idle);
                    }
                    else
                    {
                        await DSharpSCP.Main.Instance.Client.SetStatusAsync(UserStatus.Online);
                    }
                    await DSharpSCP.Main.Instance.Client.SetCustomStatusAsync(ins.Config.EmptyStatus);
                }
                else
                {
                    if (DSharpSCP.Main.Instance.Client.Status != UserStatus.Online)
                    {
                        await DSharpSCP.Main.Instance.Client.SetStatusAsync(UserStatus.Online);
                    }
                    await DSharpSCP.Main.Instance.Client.SetCustomStatusAsync(ins.Config.ServerStatus.Replace("{players}", Player.ConnectionsCount.ToString()).Replace("{max}", Server.MaxPlayers.ToString()));
                }
                
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"Discord status update failed: {ex}");
        }
    }
}
