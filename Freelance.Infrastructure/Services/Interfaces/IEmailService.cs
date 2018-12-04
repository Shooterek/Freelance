using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.ViewModels.Announcements;
using Freelance.Infrastructure.ViewModels.Jobs;

namespace Freelance.Infrastructure.Services.Interfaces
{
    public interface IEmailService
    {
        Task Notify(JobOffer offer);
        Task Notify(AnnouncementOffer offer);
        Task SendNotification(AnnouncementViewModel announcement);
        Task SendNotification(JobViewModel job);
    }
}
