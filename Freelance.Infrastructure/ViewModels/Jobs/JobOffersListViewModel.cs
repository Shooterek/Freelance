using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Infrastructure.ViewModels.Announcements;

namespace Freelance.Infrastructure.ViewModels.Jobs
{
    public class JobOffersListViewModel
    {
        public ICollection<JobOfferViewModel> Offers { get; set; }
        public bool ShowAll { get; set; }
        public bool IsAuthor { get; set; }
        public string CurrentUserId { get; set; }
    }
}
