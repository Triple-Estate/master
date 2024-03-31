using System.ComponentModel;

namespace G8Cozy.Models
{
    public class agentuser
    {
        public int Id { get; set; } 
        public string AgentId { get; set; }    
        public string UserId { get; set; }
        public int status { get; set; }
        public DateTime Data { get; set; }
    }
}
