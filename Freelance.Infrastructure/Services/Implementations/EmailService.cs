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
using Freelance.Infrastructure.ViewModels.Announcements;
using Freelance.Infrastructure.ViewModels.Jobs;
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
            myMessage.AddTo(new EmailAddress(offer.Announcement.Advertiser.Email));
            myMessage.SetFrom(new EmailAddress("freelance@aspnetapplication.com", "Freelance"));
            myMessage.SetSubject("Nowa oferta");

            var actionLink = String.Format("freelance.azurewebsites.net/announcements/details/{0}", offer.AnnouncementId);
            myMessage.AddContent(MimeType.Html,
                String.Format("<div>Dostałeś nową ofertę do ogłoszenia: '{0}'.</div>\r\n</br>\r\n<div><a href=\"{1}\">Przejdź do ogłoszenia</a></div>",
                    offer.Announcement.Title, actionLink));
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

        public async Task SendNotification(AnnouncementViewModel announcement)
        {
            var client = GetClient();

            var myMessage = new SendGridMessage();
            myMessage.AddTo(new EmailAddress(announcement.Advertiser.Email));
            myMessage.SetFrom(new EmailAddress("freelance@aspnetapplication.com", "Freelance"));
            myMessage.SetSubject("Twoje ogłoszenie wkrótce wygaśnie");

            var actionLink = String.Format("freelance.azurewebsites.net/announcements/{0}/activate", announcement.AnnouncementId);
            myMessage.AddContent(MimeType.Html, 
                String.Format("<div>Twoje ogłoszenie '{0}' wkrótce wygaśnie.</div>\r\n</br>\r\n<div><a href=\"{1}\">Kliknij aby ponownie aktywować</a></div>",
                announcement.Title, actionLink));
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

        public async Task SendNotification(JobViewModel job)
        {
            var client = GetClient();

            var myMessage = new SendGridMessage();
            myMessage.AddTo(new EmailAddress(job.Employer.Email));
            myMessage.SetFrom(new EmailAddress("freelance@aspnetapplication.com", "Freelance"));
            myMessage.SetSubject("Twoje zlecenie wkrótce wygaśnie");

            var actionLink = String.Format("freelance.azurewebsites.net/jobs/{0}/activate", job.JobId);
            myMessage.AddContent(MimeType.Html,
                String.Format("<div>Twoje zlecenie '{0}' wkrótce wygaśnie.</div>\r\n</br>\r\n<div><a href=\"{1}\">Kliknij aby ponownie aktywować</a></div>",
                    job.Title, actionLink));
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
            var client = GetClient();

            var myMessage = new SendGridMessage();
            myMessage.AddTo(new EmailAddress(offer.Job.Employer.Email));
            myMessage.SetFrom(new EmailAddress("freelance@aspnetapplication.com", "Freelance"));
            myMessage.SetSubject("Nowa oferta");

            var actionLink = String.Format("freelance.azurewebsites.net/jobs/details/{0}", offer.JobId);
            myMessage.AddContent(MimeType.Html,
                String.Format("<div>Dostałeś nową ofertę do zlecenia: '{0}'.</div>\r\n</br>\r\n<div><a href=\"{1}\">Przejdź do zlecenia</a></div>",
                    offer.Job.Title, actionLink));
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

        private SendGridClient GetClient()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            return client;
        }
    }
}
