using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.Services.Interfaces;
using Quartz;

namespace Freelance.ScheduledJobs.Jobs
{
    public class ReminderJob : IJob
    {
        private IAnnouncementsService _announcementsService;
        private IJobsService _jobsService;
        private readonly IEmailService _emailService;

        public ReminderJob(IAnnouncementsService announcementsService, IJobsService jobsService, IEmailService emailService)
        {
            _announcementsService = announcementsService;
            _jobsService = jobsService;
            _emailService = emailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _emailService.Notify(new AnnouncementOffer());
        }
    }
}