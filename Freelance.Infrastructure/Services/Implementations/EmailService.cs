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
            var message = new MailMessage();
            message.To.Add("plokarzbartlomiej@gmail.com");
            message.Subject = "New Offer";
            message.IsBodyHtml = true;
            message.Body = "<div>Otrzymałeś nową ofertę</div><div></div>";

            using (var smtp = new SmtpClient())
            {
                var credentials = new NetworkCredential
                {
                    UserName = "apikey",
                    Password = "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"
                };
                smtp.Credentials = credentials;
                smtp.Host = "smtp.sendgrid.net";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }

        public async Task SendNotification(Announcement announcement)
        {
            var client = GetClient();

            var myMessage = new SendGridMessage();
            myMessage.AddTo(new EmailAddress("plokarzbartlomiej@gmail.com"));
            myMessage.SetFrom(new EmailAddress("Bachelors@degree.com", "Freelance"));
            myMessage.SetSubject("Twoje ogłoszenie wkrótce wygaśnie");

            var actionLink = String.Format("freelance.azurewebsites.net/announcements/details/{0}/activate", announcement.AnnouncementId);
            myMessage.AddContent(MimeType.Html, String.Format("<div style=\"text-align:center\">Twoje ogłoszenie wkrótce wygaśnie.</div>\r\n<div style=\"text-align:center\">Aby temu zapobiec przejdź pod adres:</div>\r\n<div style=\"text-align:center\"><a href=\"{0}\">{0}</a></div>", actionLink));
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

        public async Task SendNotification(Job job)
        {
            var client = GetClient();

            var myMessage = new SendGridMessage();
            myMessage.AddTo(new EmailAddress("plokarzbartlomiej@gmail.com"));
            myMessage.SetFrom(new EmailAddress("Bachelors@degree.com", "Freelance"));
            myMessage.SetSubject("Twoje ogłoszenie wkrótce wygaśnie");

            var actionLink = String.Format("freelance.azurewebsites.net/jobs/details/{0}/activate", job.JobId);
            myMessage.AddContent(MimeType.Html, String.Format("<div style=\"text-align:center\">Twoje zlecenie wkrótce wygaśnie.</div>\r\n<div style=\"text-align:center\">Aby temu zapobiec przejdź pod adres:</div>\r\n<div style=\"text-align:center\"><a href=\"{0}\">{0}</a></div>", actionLink));
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
