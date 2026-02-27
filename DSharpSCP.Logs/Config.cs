using System.ComponentModel;

namespace DSharpSCP.Logs
{
    public class Config
    {
        [Description("Joined Log Text")] public string JoinedLog { get; set; } = "Player {player} ({steamid}) joined the server";
        [Description("Left Log Text")] public string LeftLog { get; set; } = "Player {player} ({steamid}) left the server";
        [Description("Ban Log Message")] public Types.BanLog BanLog { get; set; } = new();
        [Description("For Admin (with SteamID) Ban Log Message")] public Types.ForAdminBanLog ForAdminBanLog { get; set; } = new();
        [Description("Mute Log Message")] public Types.MuteLog MuteLog { get; set; } = new();
        [Description("For Admin (with SteamID) Mute Log Message")] public Types.ForAdminMuteLog ForAdminMuteLog { get; set; } = new();
        [Description("Kill Log Text")] public string KillLog { get; set; } = "Player {player} ({playerid}) killled player {target} ({targetid}) with {reason}";
        [Description("Cuff Log Text")] public string CuffLog { get; set; } = "Player {target} ({targetid}) has been cuffed by {player} ({playerid})";
        [Description("Report Log Message")] public Types.ReportLog ReportLog { get; set; } = new();
        [Description("For Admin (with SteamID) Report Log Message")] public Types.ForAdminReportLog ForAdminReportLog { get; set; } = new();
        [Description("Command Log Text")] public string CommandLog { get; set; } = "Player {player} ({playerid}) issued command `{command}`";
        [Description("Round Started Log Text")] public string RoundStartedLog { get; set; } = "Round Started!";
        [Description("Round Ended Log Text")] public string RoundEndedLog { get; set; } = "Round Ended!";
        [Description("Round Restarted Log Text")] public string RoundRestartedLog { get; set; } = "Round Restarted!";
        [Description("Hurt Log Text")] public string HurtLog { get; set; } = "Player {attacker} ({attackerid}) hurt player {player} ({playerid}) with {damage}";
        [Description("Grenade Log Text")] public string GrenadeLog { get; set; } = "Player {player} ({playerid}) threw grenade {grenade}";
        [Description("Change Role Log Text")] public string ChangeRoleLog { get; set; } = "Player {player} ({playerid}) changed role from {oldRole} to {newRole}";
        
        [Description("Joined Log Channel Id")] public ulong JoinedLogChannelId {get; set;} = 0;
        [Description("Left Log Channel Id")] public ulong LeftLogChannelId {get; set;} = 0;
        [Description("Ban Log Channel Id")] public ulong BanLogChannel { get; set; } = 0;
        [Description("For Admin Ban Log Channel Id")] public ulong ForAdminBanLogChannel { get; set; } = 0;
        [Description("Mute Log Channel Id")] public ulong MuteLogChannel { get; set; } = 0;
        [Description("For Admin Mute Log Channel Id")] public ulong ForAdminMuteLogChannel { get; set; } = 0;
        [Description("Kill Log Channel Id")] public ulong KillLogChannel { get; set; } = 0;
        [Description("Cuff Log Channel Id")] public ulong CuffLogChannel { get; set; } = 0;
        [Description("Report Log Channel Id")] public ulong ReportLogChannel { get; set; } = 0;
        [Description("For Admin Report Log Channel Id")] public ulong ForAdminReportLogChannel { get; set; } = 0;
        [Description("Command Log Channel Id")] public ulong CommandLogChannel { get; set; } = 0;
        [Description("Round Started Log Channel Id")] public ulong RoundStartedLogChannel { get; set; } = 0;
        [Description("Round Ended Log Channel Id")] public ulong RoundEndedLogChannel { get; set; } = 0;
        [Description("Round Restarted Log Channel Id")] public ulong RoundRestartedLogChannel { get; set; } = 0;
        [Description("Hurt Log Channel Id")] public ulong HurtLogChannel { get; set; } = 0;
        [Description("Grenade Log Channel Id")] public ulong GrenadeLogChannel { get; set; } = 0;
        [Description("Change Role Log Channel Id")] public ulong ChangeRoleLogChannel { get; set; } = 0;
    }
}