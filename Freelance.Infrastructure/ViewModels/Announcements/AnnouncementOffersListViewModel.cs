using System.Collections.Generic;

namespace Freelance.Infrastructure.ViewModels.Announcements
{
    public class AnnouncementOffersListViewModel
    {
        public ICollection<AnnouncementOfferViewModel> Offers{ get; set; }
        public bool ShowAll { get; set; }
    }
}
