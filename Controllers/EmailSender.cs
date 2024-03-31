using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace G8CozyMVC.Controllers
{
    public interface IEmailSender
    {
        string SendEmail(string to);
    }

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;
        private readonly string _smtpServer;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        Random random;
        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _logger = logger;
            _configuration = configuration;
            var appSettingsSection = configuration.GetSection("AppSettings:smtpS");
            _smtpServer = appSettingsSection.GetValue<string>("SmtpServer");
            _smtpUsername = appSettingsSection.GetValue<string>("SmtpUsername");
            _smtpPassword = configuration.GetValue<string>("SmtpPassword");
            random = new Random();

        }


        public string SendEmail(string to)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_smtpServer, int.Parse("587")))
                {

                    int randomNumber = random.Next(101);
                    smtpClient.UseDefaultCredentials = false; // Ensure we don't use default credentials
                    smtpClient.Credentials = new NetworkCredential(_smtpUsername, "zzbcijcaotgojntp");
                    smtpClient.EnableSsl = true; // Enable SSL/TLS

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpUsername),
                        To = { new MailAddress(to) },
                        Subject = "from tripleEstate",
                        Body = $"{randomNumber}"
                    };
                    _logger.LogInformation($"ooooooooooooooooooooooo {to}, {_smtpPassword}, {_smtpServer} {_smtpUsername}  {mailMessage} ");


                    smtpClient.Send(mailMessage);
                    return $"{randomNumber}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                return "0";
            }
        }
    }
}
