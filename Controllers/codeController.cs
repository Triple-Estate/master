/*using G8Cozy.Models;
using G8CozyMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.HttpResults;
using NuGet.Packaging.Signing;
using Microsoft.AspNetCore.Http;

namespace G8CozyMVC.Controllers
{
    public class codeController
    {
        private string verN;
        private string verM;
        private string ver;

        private readonly G8CozyMVCContext _context;
        private string ApiKey;

        private string Username;
        private string Password;
        private readonly ILogger<usersController> _logger;
        private readonly IEmailSender _emailSender;
        public codeController(G8CozyMVCContext context, IConfiguration configuration, ILogger<usersController> logger, IEmailSender emailsender)
        {
            var appSettingsSection = configuration.GetSection("AppSettings:Admin");
            Username = appSettingsSection.GetValue<string>("Username");
            Password = appSettingsSection.GetValue<string>("Password");
            ApiKey = configuration.GetValue<string>("AppSettings:ApiKey");
            _context = context;
            _logger = logger;
            _emailSender = emailsender;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> forgetPassword([Bind("Email,Password,Code")] code model)
        {

            if (!string.IsNullOrEmpty(model.Email))
            {
                _logger.LogInformation($"Email is being sent with SMTP protocol: {model.Email}");
                ver = _emailSender.SendEmail(model.Email);
                verM = model.Email;
                verN = model.Code;
                // Redirect or return appropriate response after sending email
                return RedirectToAction("~/Views/code/PasswordResetConfirmation");
            }
            else
            {
                // Handle invalid model state or missing email
                return View("~/Views/users/Create.cshtml", model);
            }
        }

        // GET: users/PasswordResetConfirmation
        /*public async Task<IActionResult> PasswordResetConfirmation()
        {
            return View();
        }
        */
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordResetConfirmation([Bind("Code")] code model)
        {
            string Vercode = (string)TempData["ver"];
            string Vermail = (string)TempData["verM"];
            string VerPasswordNEW = (string)TempData["verN"];

            if (!string.IsNullOrEmpty(model.Code) && !string.IsNullOrEmpty(Vercode) && !string.IsNullOrEmpty(Vermail) && !string.IsNullOrEmpty(VerPasswordNEW))
            {
                _logger.LogInformation($"Verocde {Vercode},  model.code {model.Code}");

                // TODO fix the xss and brutforce attack i did'nt know how to make it secure!!! 1);;if(1
                if (Vercode == model.Code)
                {
                    var existingUser = await _context.user.FirstOrDefaultAsync(u => u.Email == Vermail);
                    if (existingUser != null)
                    {
                        // existingUser.Password = VerPasswordNEW;
                        //userN= new _context.user(existingUser.Id, )
                        _logger.LogInformation($"USER FOUND id:{existingUser.Id} vermail:{Vermail} Vercode:{Vercode} VerPasswordNEW:{VerPasswordNEW}");


                        //_context.Update(userN);
                        //await _context.SaveChangesAsync();
                    }
                    var userModel = new G8Cozy.Models.user();
                }
            }

            // Return the view with the original model if verification fails
            return View("~/Views/users/Create.cshtml", model);
        }
        */
        /*
    }
}
*/