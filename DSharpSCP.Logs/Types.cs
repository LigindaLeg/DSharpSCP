namespace DSharpSCP.Logs;

public class Types
{
    public interface V2Log
    {
        string Header { get; set; }
        bool Separator { get; set; }
        string Log { get; set; }
    }
    public class BanLog : V2Log
    {
        public string Header { get; set; } = "Player Banned!";
        public bool Separator { get; set; } = true;
        public string Log { get; set; } = "**Player:** {player}\n**Reason:** {reason}\n**Admin:** {admin}\n**Duration:** {durationDays} d. {durationHours} h. {durationMinutes} m.\n**Expires:** {expires}";
    }
    
    public class ForAdminBanLog : V2Log
    {
        public string Header { get; set; } = "Ban Log!";
        public bool Separator { get; set; } = true;
        public string Log { get; set; } = "**Player:** {player} ({playerid})\n**Reason:** {reason}\n**Admin:** {admin} ({adminid})\n**Duration:** {durationDays} d. {durationHours} h. {durationMinutes} m.\n**Expires:** {expires}";
    }

    public class MuteLog : V2Log
    {
        public string Header { get; set; } = "Player Muted!";
        public bool Separator { get; set; } = true;
        public string Log { get; set; } = "**Player:** {player}\n**Admin:** {admin}";
    }

    public class ForAdminMuteLog : V2Log
    {
        public string Header { get; set; } = "Mute Log!";
        public bool Separator { get; set; } = true;
        public string Log { get; set; } = "**Player:** {player} ({playerid})\n**Admin:** {admin} ({adminid})";
    }
    
    public class ReportLog : V2Log
    {
        public string Header { get; set; } = "New Report!";
        public bool Separator { get; set; } = true;
        public string Log { get; set; } = "**From:** {player}\n**To:** {target}\n**Reason:** {reason}";
    }

    public class ForAdminReportLog : V2Log
    {
        public string Header { get; set; } = "New Report!";
        public bool Separator { get; set; } = true;
        public string Log { get; set; } = "**Player:** {player} ({playerid})\n**To:** {target} ({targetid})\n****Reason:** {reason}";
    }
}