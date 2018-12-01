using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using Freelance.Core.Models;
using Freelance.Infrastructure.Services.Interfaces;
using Quartz;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Freelance.ScheduledJobs.Jobs
{
    public class ReminderJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var service = DependencyResolver.Current.GetService<IEmailService>();

            await service.Notify(new AnnouncementOffer());
            await service.SendNotification(new Announcement() {AnnouncementId = 1});
        }
    }
}