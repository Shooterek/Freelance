using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Freelance.Infrastructure.ViewModels.Announcements
{
    public class AddAnnouncementViewModel
    {
        public HttpPostedFileBase[] Photos { get; set; }
        public List<SelectListItem> ServiceTypes { get; set; }
        public AnnouncementViewModel Announcement { get; set; }
    }
}
