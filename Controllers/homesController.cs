using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using G8Cozy.Models;
using G8CozyMVC.Data;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Microsoft.AspNetCore.Http;

namespace G8CozyMVC.Controllers
{
    public class homesController : Controller
    {
        private readonly G8CozyMVCContext _context;
        private string ApiKey;
        private readonly ILogger<usersController> _logger;

        public homesController(G8CozyMVCContext context, IConfiguration configuration, ILogger<usersController> logger)
        {
            ApiKey = configuration.GetValue<string>("AppSettings:ApiKey");
            _context = context;
            _logger = logger;
        }

        // GET: homes
        public async Task<IActionResult> Index()
        {
            return View(await _context.home.ToListAsync());
        }
        
        // GET: homes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var home = await _context.home
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }

            return View(home);
        }
        

        // GET: homes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: homes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create([Bind("Id,Description,Price,YearBuilt,SquareFeet,Keywords,HomeType,Room,Bath,Image,IsSell,HavePool,HaveOnSiteParking,HavePark")] home home)
        {

            _logger.LogInformation($"sorry the you have dont good day");
            if (ModelState.IsValid)
            {
                if(home.Keywords == HttpContext.Session.GetString("UserPw"))
                {
                    _context.Add(home);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogInformation($"sorry the password {home.Keywords}  doesnt match");
                }
            }
            return View(home);
        }

        /*

        // GET: homes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var home = await _context.home.FindAsync(id);
            if (home == null)
            {
                return NotFound();
            }
            return View(home);
        }

        // POST: homes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Price,YearBuilt,SquareFeet,Keywords,HomeType,Room,Bath,Image,IsSell,HavePool,HaveOnSiteParking,HavePark")] home home)
        {
            if (id != home.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(home);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!homeExists(home.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(home);
        }
        // GET: homes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var home = await _context.home
                .FirstOrDefaultAsync(m => m.Id == id);
            if (home == null)
            {
                return NotFound();
            }

            return View(home);
        }

        // POST: homes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var home = await _context.home.FindAsync(id);
            if (home != null)
            {
                _context.home.Remove(home);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool homeExists(int id)
        {
            return _context.home.Any(e => e.Id == id);
        }
    }
}
