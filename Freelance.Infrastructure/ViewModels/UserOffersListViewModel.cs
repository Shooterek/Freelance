using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels
{
    public class UserOffersListViewModel
    {
        public UserOffersListViewModel(List<AnnouncementOffer> receivedAnnouncementOffers, List<AnnouncementOffer> publishedAnnouncementOffers,
            List<JobOffer> receivedJobOffers, List<JobOffer> publishedJobOffers)
        {
            ReceivedAnnouncementOffers = receivedAnnouncementOffers;
            PublishedAnnouncementOffers = publishedAnnouncementOffers;
            ReceivedJobOffers = receivedJobOffers;
            PublishedJobOffers = publishedJobOffers;
        }

        public List<AnnouncementOffer> ReceivedAnnouncementOffers{ get; set; }
        public List<AnnouncementOffer> PublishedAnnouncementOffers{ get; set; }
        public List<JobOffer> ReceivedJobOffers { get; set; }
        public List<JobOffer> PublishedJobOffers { get; set; }
    }
}
