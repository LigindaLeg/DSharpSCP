using System.ComponentModel;

namespace DSharpSCP;

public class Config
{
    [Description("Discord Bot Token")] 
    public string DiscordBotToken { get; set; } = "token here";
    
    [Description("Discord Guild ID")]
    public ulong DiscordGuildID { get; set; } = 0;
    
    [Description("Is debug enabled?")]
    public bool Debug { get; set; } = false;
}