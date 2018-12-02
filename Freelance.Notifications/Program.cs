using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Freelance.Notifications
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var myMessage = new SendGridMessage();
            myMessage.AddTo(new EmailAddress("plokarzbartlomiej@gmail.com"));
            myMessage.SetFrom(new EmailAddress("Bachelors@degree.com", "Freelance"));
            myMessage.SetSubject("Twoje ogłoszenie wkrótce wygaśnie");

            var actionLink = String.Format("freelance.azurewebsites.net/jobs/details/{0}/activate", 1);
            myMessage.AddContent(MimeType.Html, String.Format("<div style=\"text-align:center\">Twoje zlecenie wkrótce wygaśnie.</div>\r\n<div style=\"text-align:center\">Aby temu zapobiec przejdź pod adres:</div>\r\n<div style=\"text-align:center\"><a href=\"{0}\">{0}</a></div>", actionLink));
            try
            {
                client.SendEmailAsync(myMessage).Wait();
            }
            catch (Exception err)
            {
                Trace.TraceError("Failed to create Web transport: ." + err.Message);
            }
        }
    }
}
