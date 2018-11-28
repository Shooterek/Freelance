using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.Services.Interfaces
{
    public interface IEmailService
    {
        Task Notify(JobOffer offer);
        Task Notify(AnnouncementOffer offer);
        Task SendNotification(Announcement announcement);
        Task SendNotification(Job job);
    }
}
