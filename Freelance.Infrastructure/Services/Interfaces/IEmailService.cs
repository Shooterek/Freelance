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
        void Notify(JobOffer offer);
        void Notify(AnnouncementOffer offer);
    }
}
