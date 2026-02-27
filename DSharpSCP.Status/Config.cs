using System.ComponentModel;

namespace DSharpSCP.Status
{
    public class Config
    {
        [Description("Empty status:")]
        public string EmptyStatus { get; set; } = "Nobody on server!";
        
        [Description("Server Status:")]
        public string ServerStatus { get; set; } = "{players}/{max} on server.";
        
        [Description("Idle on empty?")]
        public bool Idle { get; set; } = true;
        
        [Description("Is debug enabled?")]
        public bool Debug { get; set; } = false;
    }
}