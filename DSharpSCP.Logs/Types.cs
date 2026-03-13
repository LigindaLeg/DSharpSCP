namespace DSharpSCP.Logs;

public class Types
{
    public class V2Log()
    {
        public required string Header { get; set; }
        public required bool Separator { get; set; }
        public required string Log { get; set; }
    }

    public static class LogTemplates
    {
        public static V2Log Ban() => new()
        {
            Header = "Player Banned!",
            Separator = true,
            Log = "**Player:** {player}\n**Reason:** {reason}\n**Admin:** {admin}\n**Duration:** {durationDays} d. {durationHours} h. {durationMinutes} m.\n**Expires:** {expires}"
        };
        
        public static V2Log AdminBan() =>new()
        {
            Header = "Ban Log!",
            Separator = true,
            Log = "**Player:** {player} ({playerid})\n**Reason:** {reason}\n**Admin:** {admin} ({adminid})\n**Duration:** {durationDays} d. {durationHours} h. {durationMinutes} m.\n**Expires:** {expires}"
        };
        
        public static V2Log Mute() =>new()
        {
            Header = "Player Muted!",
            Separator = true,
            Log = "**Player:** {player}\n**Admin:** {admin}"
        };
        
        public static V2Log AdminMute() =>new()
        {
            Header = "Mute Log!",
            Separator = true,
            Log = "**Player:** {player} ({playerid})\n**Admin:** {admin} ({adminid})"
        };
        
        public static V2Log Report() =>new()
        {
            Header = "New Report!",
            Separator = true,
            Log = "**Player:** {player} ({playerid})\n**Admin:** {admin} ({adminid})"
        };
        
        public static V2Log AdminReport() =>new()
        {
            Header = "New Report!",
            Separator = true,
            Log = "**Player:** {player} ({playerid})\n**To:** {target} ({targetid})\n****Reason:** {reason}"
        };
    }
}