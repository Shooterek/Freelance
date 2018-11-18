using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels
{
    public class AnnouncementOffersListViewModel
    {
        public IEnumerable<AnnouncementOffer> Offers{ get; set; }
        public bool ShowAll { get; set; }
    }
}
