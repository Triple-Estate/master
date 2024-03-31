using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace G8Cozy.Models
{
    public class user
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool type { get; set; }
        public string? Code { get; set; }

    }

    public class code
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class admin
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class loguser
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        
        [BindRequired]
        public string Password { get; set; }
    }

    public class editabled
    {
        public IEnumerable<G8Cozy.Models.home>? home { get; set; }
        public IEnumerable<G8Cozy.Models.agent>? agent { get; set; }
        public IEnumerable<G8Cozy.Models.agentuser>? agentuser { get; set; }
        public string? Accept { get; set; }
        public string? Reject { get; set; }

        [BindRequired]
        public string Password { get; set; }
    }



}
