using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.Services.Interfaces;

namespace Freelance.Infrastructure.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task Notify(AnnouncementOffer offer)
        {
            var message = new MailMessage();
            message.To.Add("plokarzbartlomiej@gmail.com");
            message.Subject = "New Offer";
            message.IsBodyHtml = true;
            message.From = new MailAddress(ConfigurationManager.AppSettings["email-address"]);
            message.Body = "<h1>hALO</h1>";
            using (var smtp = new SmtpClient())
            {
                var credentials = new NetworkCredential
                {
                    UserName = ConfigurationManager.AppSettings["email-address"],
                    Password = ConfigurationManager.AppSettings["email-password"]
                };
                smtp.Credentials = credentials;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }

        public async Task Notify(JobOffer offer)
        {
            var message = new MailMessage();
            message.To.Add(offer.Offerer.Email);
            message.Subject = "New Offer";
            message.IsBodyHtml = true;
            message.Body = "<h1>hALO</h1>";

            using (var smtp = new SmtpClient())
            {
                var credentials = new NetworkCredential
                {
                    UserName = ConfigurationManager.AppSettings["email-address"],
                    Password = ConfigurationManager.AppSettings["email-password"]
                };
                smtp.Credentials = credentials;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }
    }
}
