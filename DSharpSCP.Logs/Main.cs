using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Discord.Rest;
using Discord.WebSocket;
using InventorySystem.Items.ThrowableProjectiles;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;

namespace DSharpSCP.Logs;

public class Main : Plugin<Config>
{
    public override void Enable()
    {
        Instance = this;
        PlayerEvents.Joined += EventHandlers.PlayerEvents.Joined;
        PlayerEvents.Banned += EventHandlers.PlayerEvents.Banned;
        PlayerEvents.Left += EventHandlers.PlayerEvents.Left;
        PlayerEvents.Muted += EventHandlers.PlayerEvents.Muted;
        PlayerEvents.Death += EventHandlers.PlayerEvents.Death;
        PlayerEvents.Cuffed += EventHandlers.PlayerEvents.Cuffed;
        PlayerEvents.ReportedPlayer += EventHandlers.PlayerEvents.ReportedPlayer;
        ServerEvents.CommandExecuted += EventHandlers.ServerEvents.CommandExecuted;
        ServerEvents.RoundStarted += EventHandlers.ServerEvents.RoundStarted;
        ServerEvents.RoundEnded += EventHandlers.ServerEvents.RoundEnded;
        ServerEvents.RoundRestarted += EventHandlers.ServerEvents.RoundRestarted;
        PlayerEvents.Hurt += EventHandlers.PlayerEvents.Hurt;
        PlayerEvents.ThrewProjectile += EventHandlers.PlayerEvents.ThrewProjectile;
        PlayerEvents.ChangedRole += EventHandlers.PlayerEvents.ChangedRole;
    }

    public override void Disable()
    {
        PlayerEvents.Joined -= EventHandlers.PlayerEvents.Joined;
        PlayerEvents.Banned -= EventHandlers.PlayerEvents.Banned;
        PlayerEvents.Left -= EventHandlers.PlayerEvents.Left;
        PlayerEvents.Muted -= EventHandlers.PlayerEvents.Muted;
        PlayerEvents.Death -= EventHandlers.PlayerEvents.Death;
        PlayerEvents.Cuffed -= EventHandlers.PlayerEvents.Cuffed;
        PlayerEvents.ReportedPlayer -= EventHandlers.PlayerEvents.ReportedPlayer;
        ServerEvents.CommandExecuted -= EventHandlers.ServerEvents.CommandExecuted;
        ServerEvents.RoundStarted -= EventHandlers.ServerEvents.RoundStarted;
        ServerEvents.RoundEnded -= EventHandlers.ServerEvents.RoundEnded;
        ServerEvents.RoundRestarted -= EventHandlers.ServerEvents.RoundRestarted;
        PlayerEvents.Hurt -= EventHandlers.PlayerEvents.Hurt;
        PlayerEvents.ThrewProjectile -= EventHandlers.PlayerEvents.ThrewProjectile;
        PlayerEvents.ChangedRole -= EventHandlers.PlayerEvents.ChangedRole;
        Instance = null;
        lastLogMessages.Clear();
    }

    public static Dictionary<SocketChannel, RestUserMessage> lastLogMessages = new();
    
    public static Main Instance { get; private set; }
    
    public override string Name => "DSharpSCP.Logs";

    public override string Description => "SCP:SL Logs module for DSharpSCP.";

    public override string Author => "liginda";

    public override Version Version => new(1,0,0);

    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);
}