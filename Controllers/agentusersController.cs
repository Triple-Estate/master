using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using G8Cozy.Models;
using G8CozyMVC.Data;

namespace G8CozyMVC.Controllers
{
    public class agentusersController : Controller
    {
        private readonly G8CozyMVCContext _context;
        private string ApiKey;
        public agentusersController(G8CozyMVCContext context, IConfiguration configuration)
        {
            ApiKey = configuration.GetValue<string>("AppSettings:ApiKey");
            _context = context;
        }

        // GET: agentusers
        public async Task<IActionResult> Index()
        {
            return View(await _context.agentuser.ToListAsync());
        }

        public async Task<IActionResult> Accept(int id, int userid)
        {
            var agentuser = await _context.agentuser.FindAsync(id);
            if (agentuser != null)
            {
                notification mto2 = new notification
                {
                    UserId = agentuser.UserId,
                    Msg = $"you have got Appointment with Id{agentuser.AgentId} at {agentuser.Data}"
                };
                _context.notification.Add(mto2);
                agentuser.status = 1;
                _context.Update(agentuser);

                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Reject(int id)
        {
            var agentuser = await _context.agentuser.FindAsync(id);
            if (agentuser != null)
            {
                notification mto2 = new notification
                {
                    UserId = agentuser.UserId,
                    Msg = $"sorry your Appointment has been rejected by id{agentuser.AgentId}"
                };
                _context.notification.Add(mto2);
                await _context.SaveChangesAsync();

                _context.agentuser.Remove(agentuser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        

        /*
        // GET: agentusers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agentuser = await _context.agentuser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agentuser == null)
            {
                return NotFound();
            }

            return View(agentuser);
        }

        // GET: agentusers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: agentusers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AgentId,UserId,Data")] agentuser agentuser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agentuser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(agentuser);
        }

        // GET: agentusers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agentuser = await _context.agentuser.FindAsync(id);
            if (agentuser == null)
            {
                return NotFound();
            }
            return View(agentuser);
        }

        // POST: agentusers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AgentId,UserId,Data")] agentuser agentuser)
        {
            if (id != agentuser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agentuser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!agentuserExists(agentuser.Id))
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
            return View(agentuser);
        }

        // GET: agentusers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agentuser = await _context.agentuser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agentuser == null)
            {
                return NotFound();
            }

            return View(agentuser);
        }

        // POST: agentusers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agentuser = await _context.agentuser.FindAsync(id);
            if (agentuser != null)
            {
                _context.agentuser.Remove(agentuser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool agentuserExists(int id)
        {
            return _context.agentuser.Any(e => e.Id == id);
        }
    }
}
