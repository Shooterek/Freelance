using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels
{
    public class AddJobViewModel
    {
        public List<SelectListItem> ServiceTypes { get; set; }
        public Job Job { get; set; }
    }
}
