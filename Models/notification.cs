using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace G8Cozy.Models
{
    public class notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Msg { get; set; }
    }
}
