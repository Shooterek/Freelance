using System.Collections.Generic;

namespace Freelance.Infrastructure.ViewModels.Announcements
{
    public class AnnouncementOffersListViewModel
    {
        public ICollection<AnnouncementOfferViewModel> Offers{ get; set; }
        public bool ShowAll { get; set; }
        public bool IsAuthor { get; set; }
        public string CurrentUserId { get; set; }
    }
}
