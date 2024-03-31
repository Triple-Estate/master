using System.ComponentModel.DataAnnotations;

namespace G8Cozy.Models
{
    public class agent
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public string Description { get; set; }
        public int Xp { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public string APIKEY { get; set; }
    }
    public class useagent
    {
        public int Id { get; set; }
        public int Phone { get; set; }
        public IEnumerable<G8Cozy.Models.agent>? agent { get; set; }
        public string AgentId { get; set; }
        public string UserId { get; set; }
        public DateTime Data { get; set; }
    }
}
