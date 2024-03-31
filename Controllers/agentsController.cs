using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using G8Cozy.Models;
using G8CozyMVC.Data;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;

namespace G8CozyMVC.Controllers
{
    public class agentsController : Controller
    {
        private readonly G8CozyMVCContext _context;
        private readonly ILogger<usersController> _logger;
        private string ApiKey;
        public agentsController(G8CozyMVCContext context, IConfiguration configuration, ILogger<usersController> logger)
        {
            ApiKey = configuration.GetValue<string>("AppSettings:ApiKey");
            _context = context;
            _logger = logger;
        }

        // GET: agents
        public async Task<IActionResult> Index()
        {
            List<agent> data = await _context.agent.ToListAsync();

            // Create an instance of 'G8Cozy.Models.editabled' and set its properties
            var editabledModel = new G8Cozy.Models.useagent
            {
                agent = data
                // Set other properties if needed
            };

            return View(editabledModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> userage([Bind("Id,agent,agentuser,UserId,AgentId,Data,Phone")] useagent usa)
        {
            if (ModelState.IsValid)
            {
                var editabledModel = new agentuser
                {
                    AgentId = usa.AgentId,
                    UserId = usa.UserId,
                    Data = usa.Data,
                    // Set other properties if needed
                };

                _context.agentuser.Add(editabledModel);

                // notification
                notification mto = new notification
                {
                    UserId = usa.UserId,
                    Msg = $"wait until Id{usa.AgentId} 's decision" // $"you have Appointment with Id{usa.AgentId} at {usa.Data} you can call +{usa.Phone} him/her for details"
                };
                _context.notification.Add(mto);

                notification mto2 = new notification
                {
                    UserId = usa.AgentId,
                    Msg = $"you have got Appointment with Id{usa.UserId} at {usa.Data} check out calendar"
                };
                _context.notification.Add(mto2);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            _logger.LogInformation("canceled useragent!");
            return View(usa);
        }


        // GET: agents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _context.agent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        /*
        // GET: agents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: agents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,Description,Xp,Type,Image")] agent agent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(agent);
        }
        */
        // GET: agents/Edit/5
        public async Task<IActionResult> Edit(int? uid, string apikey)
        {
            if (uid == null)
            {
                return NotFound();
            }

            var agent = await _context.agent.FindAsync(uid);
            if (agent == null || agent.APIKEY != apikey)
            {
                return NotFound();
            }

            return View(agent);
        }

        // POST: agents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Phone,Description,Xp,Type,Image,APIKEY")] agent agent)
        {
            if (id != agent.Id)
            {
                return NotFound();
            }

            var existingAgent = await _context.agent.FindAsync(id);
            if (existingAgent == null || ModelState.IsValid && existingAgent.APIKEY == agent.APIKEY)
            {
                try
                {
                    // Update the properties of the existingAgent with values from agent
                    _context.Entry(existingAgent).CurrentValues.SetValues(agent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!agentExists(agent.Id))
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

            return View(agent);
        }



        /*
        // GET: agents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _context.agent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        // POST: agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agent = await _context.agent.FindAsync(id);
            if (agent != null)
            {
                _context.agent.Remove(agent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool agentExists(int id)
        {
            return _context.agent.Any(e => e.Id == id);
        }
    }
}
