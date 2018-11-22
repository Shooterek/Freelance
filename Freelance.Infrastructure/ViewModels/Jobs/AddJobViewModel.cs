using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels.Jobs
{
    public class AddJobViewModel
    {
        public HttpPostedFileBase[] Photos { get; set; }
        public List<SelectListItem> ServiceTypes { get; set; }
        public JobViewModel Job { get; set; }
    }
}
