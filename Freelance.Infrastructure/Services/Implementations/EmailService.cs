using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Freelance.Infrastructure.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task Notify(AnnouncementOffer offer)
        {
            var client = GetClient();

            var myMessage = new SendGridMessage();
            myMessage.AddTo(new EmailAddress("plokarzbartlomiej@gmail.com"));
            myMessage.SetFrom(new EmailAddress("Bachelors@degree.com", "Freelance"));
            myMessage.SetSubject("New offer");
            myMessage.AddContent(MimeType.Html, "<h1>hALO</h1>");
            try
            {
                var response = await client.SendEmailAsync(myMessage);
            }
            catch (Exception err)
            {
                Trace.TraceError("Failed to create Web transport: ." + err.Message);
                await Task.FromResult(0);
            }
        }

        public async Task SendNotification(Announcement announcement)
        {
            var client = GetClient();

            var myMessage = new SendGridMessage();
            myMessage.AddTo(new EmailAddress("plokarzbartlomiej@gmail.com"));
            myMessage.SetFrom(new EmailAddress("Bachelors@degree.com", "Freelance"));
            myMessage.SetSubject("Twoje ogłoszenie wkrótce wygaśnie");
            myMessage.AddContent(MimeType.Html, );
            try
            {
                var response = await client.SendEmailAsync(myMessage);
            }
            catch (Exception err)
            {
                Trace.TraceError("Failed to create Web transport: ." + err.Message);
                await Task.FromResult(0);
            }
        }

        public Task SendNotification(Job job)
        {
            throw new NotImplementedException();
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

        private SendGridClient GetClient()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            return client;
        }
    }
}
