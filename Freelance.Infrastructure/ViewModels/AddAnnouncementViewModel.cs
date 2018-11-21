using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels
{
    public class AddAnnouncementViewModel
    {
        public HttpPostedFileBase[] Photos { get; set; }
        public List<SelectListItem> ServiceTypes { get; set; }
        public AnnouncementViewModel Announcement { get; set; }
    }
}
