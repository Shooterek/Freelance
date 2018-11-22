using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.ViewModels.Announcements;
using Freelance.Infrastructure.ViewModels.Jobs;

namespace Freelance.Infrastructure.ViewModels
{
    public class UserOffersListViewModel
    {
        public UserOffersListViewModel(List<AnnouncementOfferViewModel> receivedAnnouncementOffers, List<AnnouncementOfferViewModel> publishedAnnouncementOffers,
            List<JobOfferViewModel> receivedJobOffers, List<JobOfferViewModel> publishedJobOffers)
        {
            ReceivedAnnouncementOffers = receivedAnnouncementOffers;
            PublishedAnnouncementOffers = publishedAnnouncementOffers;
            ReceivedJobOffers = receivedJobOffers;
            PublishedJobOffers = publishedJobOffers;
        }

        public List<AnnouncementOfferViewModel> ReceivedAnnouncementOffers{ get; set; }
        public List<AnnouncementOfferViewModel> PublishedAnnouncementOffers{ get; set; }
        public List<JobOfferViewModel> ReceivedJobOffers { get; set; }
        public List<JobOfferViewModel> PublishedJobOffers { get; set; }
    }
}
