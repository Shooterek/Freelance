using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.Utils;

namespace Freelance.Infrastructure.ViewModels
{
    public class AnnouncementsListViewModel
    {
        public IList<Announcement> Announcements { get; set; }

        public Announcement Filter { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
