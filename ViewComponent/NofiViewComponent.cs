using G8CozyMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Add this for IConfiguration

namespace G8CozyMVC.ViewComponents // Correct the namespace to use ViewComponents
{
    public class NofiViewComponent : ViewComponent
    {
        private readonly G8CozyMVCContext _context;
        private string ApiKey;

        public NofiViewComponent(G8CozyMVCContext context, IConfiguration configuration)
        {
            ApiKey = configuration.GetValue<string>("AppSettings:ApiKey");
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Assuming you have a DbSet<Notification> in your _context
            var notifications = await _context.notification.ToListAsync();

            // You can pass the notifications to the view as a model
            return View(notifications);
        }
    }
}
