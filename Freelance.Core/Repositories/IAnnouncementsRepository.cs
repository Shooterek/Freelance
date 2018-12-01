using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Core.Repositories
{
    public interface IAnnouncementsRepository
    {
        Task<RepositoryActionResult<ICollection<Announcement>>> GetAllAsync();
        Task<RepositoryActionResult<Announcement>> GetByIdAsync(int id);
        Task<RepositoryActionResult<ICollection<Announcement>>> GetByServiceTypeAsync(ServiceType serviceType);
        Task<RepositoryActionResult<Announcement>> UpdateAsync(Announcement entity);
        Task<RepositoryActionResult<Announcement>> RemoveAsync(int id);
        Task<RepositoryActionResult<Announcement>> AddAsync(Announcement entity);
        Task<RepositoryActionResult<AnnouncementOffer>> AddOfferAsync(AnnouncementOffer entity);
        Task<RepositoryActionResult<ICollection<AnnouncementOffer>>> GetPublishedOffersAsync(string userId);
        Task<RepositoryActionResult<ICollection<AnnouncementOffer>>> GetReceivedOffersAsync(string userId);
        Task<RepositoryActionResult<AnnouncementOffer>> RemoveOfferAsync(int id);

        Task<RepositoryActionResult<AnnouncementOffer>> AcceptOfferAsync(int offerId, string userId);
        Task<RepositoryActionResult<bool>> DeclineOfferAsync(int offerId, string userId);
        Task<RepositoryActionResult<AnnouncementOffer>> EndOfferAsync(int id, string userId);
        Task<List<Announcement>> GetOldAnnouncementsAsync();
    }
}