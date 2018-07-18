using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels
{
    public class AddAnnouncementViewModel
    {
        public List<ServiceType> ServiceTypes { get; set; }
        public Announcement Announcement { get; set; }
    }
}
