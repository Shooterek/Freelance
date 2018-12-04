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
            var emailService = DependencyResolver.Current.GetService<IEmailService>();
            var announcementsService = DependencyResolver.Current.GetService<IAnnouncementsService>();
            var jobsService = DependencyResolver.Current.GetService<IJobsService>();

            var jobs = await jobsService.GetOldJobsAsync();
            jobs.ForEach(async job =>
            {
                var t1 = emailService.SendNotification(job);
                job.WasNotified = true;
                var t2 = jobsService.UpdateJobAsync(job);

                await t1;
                await t2;
            });

            var announcements = await announcementsService.GetOldAnnouncementsAsync();
            announcements.ForEach(async announcement =>
            {
                var t1 = emailService.SendNotification(announcement);
                announcement.WasNotified = true;
                var t2 = announcementsService.UpdateAnnouncementAsync(announcement);

                await t1;
                await t2;
            });
        }
    }
}