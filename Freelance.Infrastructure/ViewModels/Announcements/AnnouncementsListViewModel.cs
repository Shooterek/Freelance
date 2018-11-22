using System.Collections.Generic;
using Freelance.Infrastructure.Utils;

namespace Freelance.Infrastructure.ViewModels.Announcements
{
    public class AnnouncementsListViewModel
    {
        public IList<AnnouncementViewModel> Announcements { get; set; }

        public AnnouncementFilter Filter { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
