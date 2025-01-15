using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Core.Repositries;

namespace WebApplication1.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task sendEmailAsync(string to, string subject, string body)
        {
            
               var smtpClient = new SmtpClient(configuration["EmailSetting:SmtpServer"])
                {
                    Port = int.Parse(configuration["EmailSetting:port"]),
                   Credentials = new NetworkCredential(configuration["EmailSetting:Username"], configuration["EmailSetting:Password"]),
                   EnableSsl = true
                };
           

                var MailMessage = new MailMessage()
                {
                    From = new MailAddress("PlantCare <plantcare.hti@gmail.com>"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                   

                };
            
                MailMessage.To.Add(new MailAddress(to));
                await smtpClient.SendMailAsync(MailMessage);

            

        }
    }
}
