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
using Microsoft.AspNetCore.Http.HttpResults;
using NuGet.Packaging.Signing;
using Microsoft.AspNetCore.Http;



namespace G8CozyMVC.Controllers
{
    public class usersController : Controller
    {
        private readonly G8CozyMVCContext _context;
        private string ApiKey;
        private string Username;
        private string Password;
        private readonly ILogger<usersController> _logger;
        private readonly IEmailSender _emailSender;
        public usersController(G8CozyMVCContext context, IConfiguration configuration, ILogger<usersController> logger, IEmailSender emailsender)
        {
            var appSettingsSection = configuration.GetSection("AppSettings:Admin");
            Username = appSettingsSection.GetValue<string>("Username");
            Password = appSettingsSection.GetValue<string>("Password");
            ApiKey = configuration.GetValue<string>("AppSettings:ApiKey");
            _context = context;
            _logger = logger;
            _emailSender = emailsender;
        }

        /*
        // GET: users
        public async Task<IActionResult> Index()
        {
            return View(await _context.user.ToListAsync());
        }

        // GET: users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.user
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        */
        // GET: users/Create
        public IActionResult Create()
        {
            Response.Cookies.Delete("MyApp.Session");
            return View();
        }

        // POST: users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,type")] user user)
        {
            _logger.LogInformation($"id{user.Id} name{user.Name} email{user.Email}  password{user.Password} user.typr{user.type}  MODEL= {ModelState.IsValid}");
            if (ModelState.IsValid) // TODO add here some pattern for unmatched texts (name's length must be longer than 3 character)
            {   


                var existingUser = await _context.user.FirstOrDefaultAsync(u => u.Name == user.Name || u.Email == user.Email);
                if (existingUser==null) {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    string guidString = Guid.NewGuid().ToString("N");
                    string apiKey = guidString.Substring(0, 32);

                    if (user.type)
                    {
                        _logger.LogInformation($"-------------- id:{user.Id} ");
                        agent agenter = new agent { APIKEY = apiKey, Id = user.Id, Name = user.Name, Phone = 0, Description = "hi, i'm agenter!", Xp = 0, Type = "all", Image = "" };
                        _context.agent.Add(agenter);
                    
                        _logger.LogInformation($"ISFOUND id:{agenter.APIKEY} ");
                        await _context.SaveChangesAsync();
                    }

                    HttpContext.Session.SetString("UserID", user.Id.ToString());
                    HttpContext.Session.SetString("UserAPI", apiKey);
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetString("UserPw", user.Password);
                    HttpContext.Session.SetString("UserType", user.type.ToString());

                    // add here nofitications find LATER TODO
                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    TempData["msg"] = "email or username is tooken please choose another";
                }
            }
            else { TempData["msg"] = "please check the results"; }
            // ViewData["err"] // TODO give error why the signup rejected! TODO add are you human verification!
            return View("~/Views/users/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLog([Bind("Name,Password")] admin ad)
        {
            if (ModelState.IsValid && ad.Name == Username && ad.Password == Password)
            {
                List<home> myhome = await _context.home.ToListAsync();
                List<agent> myagent = await _context.agent.ToListAsync();
                List<agentuser> myagentuser = await _context.agentuser.ToListAsync();
                // Create an instance of 'G8Cozy.Models.editabled' and set its properties
                var editabledModel = new G8Cozy.Models.editabled
                {
                    home = myhome,
                    agent = myagent,
                    agentuser = myagentuser,
                    // Set other properties if needed
                };

                return View("~/Views/AdminSecurityApi/index.cshtml", editabledModel);
            }

            return View("~/Views/users/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logme([Bind("Name,Email,Password")] loguser lu)
        {
            if (ModelState.IsValid && (!string.IsNullOrEmpty(lu.Name) || !string.IsNullOrEmpty(lu.Email)))
            {

                var existingUser = await _context.user.FirstOrDefaultAsync(u => u.Name == lu.Name || u.Email == lu.Email);
                if(existingUser != null)
                {
                    if (existingUser.Password == lu.Password)
                    {
                        _logger.LogInformation($"USER FOUND id:{existingUser.Id} name:{existingUser.Name} ");
                        HttpContext.Session.SetString("UserID", existingUser.Id.ToString());
                        var existingAgent = await _context.agent.FirstOrDefaultAsync(u => u.Id == existingUser.Id);
                        if (existingAgent != null)
                        {
                            HttpContext.Session.SetString("UserAPI", existingAgent.APIKEY);
                        }
                        HttpContext.Session.SetString("UserName", existingUser.Name);
                        HttpContext.Session.SetString("UserPw", existingUser.Password);
                        HttpContext.Session.SetString("UserType", existingUser.type.ToString());

                        // add here nofitications find LATER TODO
                        return View("~/Views/Home/Index.cshtml");
                    }
                }

            }
            return View("~/Views/users/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> forgetPassword([Bind("Email,Password,Code")] user model)
        {

            if (!string.IsNullOrEmpty(model.Email))
            {
                _logger.LogInformation($"Email is being sent with SMTP protocol: {model.Email}");
                TempData["ver"] = _emailSender.SendEmail(model.Email);
                TempData["verM"] = model.Email;
                TempData["verN"] = model.Code;
                // Redirect or return appropriate response after sending email
                return RedirectToAction("PasswordResetConfirmation");
            }
            else
            {
                // Handle invalid model state or missing email
                return View("~/Views/users/Create.cshtml", model);
            }
        }

        // GET: users/PasswordResetConfirmation
        public async Task<IActionResult> PasswordResetConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordResetConfirmation([Bind("Code")] user model)
        {
            string Vercode = (string)TempData["ver"];
            string Vermail = (string)TempData["verM"];
            string VerPasswordNEW = (string)TempData["verN"];

            if (!string.IsNullOrEmpty(model.Code) && !string.IsNullOrEmpty(Vercode) && !string.IsNullOrEmpty(Vermail)  && !string.IsNullOrEmpty(VerPasswordNEW))
            {
                _logger.LogInformation($"Verocde {Vercode},  model.code {model.Code}");
                
                // TODO fix the xss and brutforce attack i did'nt know how to make it secure!!! 1);;if(1
                if (Vercode == model.Code)
                {
                    var existingUser = await _context.user.FirstOrDefaultAsync(u => u.Email == Vermail);
                    if (existingUser != null)
                    {
                        existingUser.Password = VerPasswordNEW;
                        _logger.LogInformation($"USER FOUND id:{existingUser.Id} vermail:{Vermail} Vercode:{Vercode} VerPasswordNEW:{VerPasswordNEW}");


                        _context.Update(existingUser);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            // Return the view with the original model if verification fails
            return View("~/Views/users/Create.cshtml", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editabled([Bind("Accept,Reject,Password")] editabled lu)
        {
            if (Password == lu.Password) 
            {

                if (!string.IsNullOrEmpty(lu.Accept))
                {
                    string[] sp1 = lu.Accept.Split("``");
                    Array.Resize(ref sp1, sp1.Length - 1);
                    string[][] sp = new string[sp1.Length][];
                    if (sp != null)
                    {
                        // split
                        int index = 0;
                        foreach (string part in sp1)
                        {
                            string[] sp2 = part.Split("`");
                            _logger.LogInformation($"{sp2[0]} index {index}");
                            sp[index] = [sp2[0], sp2[1].Split(" ")[0], sp2[1].Split(" ")[1]];
                            index++;
                        }
                       
                        // public the selected homes
                        foreach (string[] uid in sp)
                        {
                            try
                             {
                                 var ho = await _context.home.FindAsync(int.Parse(uid[0]));
                                 if (ho != null)
                                 {
                                     ho.HaveOnSiteParking = true;

                                     _context.Update(ho);
                                 }
                             }
                             catch (Exception ex)
                             {
                                 _logger.LogInformation($"err {ex}");
                             }
                        }

                        // notification
                        foreach (string[] uid in sp)
                        {
                            notification mto = new notification
                            {
                                UserId = uid[1].ToString(),
                                Msg = $"Congriliation your home id{uid[1]}'s sale is Accepted, Thank You for shoosing us!"
                            };
                            _context.Add(mto);
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                if (!string.IsNullOrEmpty(lu.Reject))
                {
                    string[] sp1 = lu.Reject.Split("``");
                    Array.Resize(ref sp1, sp1.Length - 1);
                    string[][] sp = new string[sp1.Length][];
                    if (sp != null)
                    {
                        // split
                        int index = 0;
                        foreach (string part in sp1)
                        {
                            string[] sp2 = part.Split("`");
                            sp[index] = [sp2[0], sp2[1].Split(" ")[0], sp2[1].Split(" ")[1]];
                            _logger.LogInformation($"{sp[index][0]} {sp[index][1]} {sp[index][2]}");

                            index++;
                        }

                        // remove the selected homes
                        foreach (string[] uid in sp)
                        {
                            try
                            {
                                var ho = await _context.home.FindAsync(int.Parse(uid[0]));
                                if (ho != null)
                                {
                                    _logger.LogInformation($"removing {uid}");
                                    _context.Attach(ho);
                                    _context.home.Remove(ho);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogInformation($"err {ex}");
                            }
                        }

                        // notification
                        foreach (string[] uid in sp)
                        {
                            notification mto = new notification
                            {
                                UserId = uid[1].ToString(),
                                Msg = $"Sorry your home id{uid[1]}'s sale is Rejected, Please be more careful when setting home's Features Thanks!"
                            };
                            _context.Add(mto);
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                // add here nofitications TODO
                return View("~/Views/Home/Index.cshtml");
            }
            return View("~/Views/users/Create.cshtml");
        }




        /*
        // GET: users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.user.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,type")] user user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!userExists(user.Id))
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
            return View(user);
        }
        
        // GET: users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.user
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.user.FindAsync(id);
            if (user != null)
            {
                _context.user.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool userExists(int id)
        {
            return _context.user.Any(e => e.Id == id);
        }
    }
}
