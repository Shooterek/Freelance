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
        Task<RepositoryActionResult<Announcement>> UpdateAsync(Announcement entity);
        Task<RepositoryActionResult<Announcement>> RemoveAsync(int id);
        Task<RepositoryActionResult<Announcement>> AddAsync(Announcement entity);
        Task<RepositoryActionResult<AnnouncementOffer>> AddOfferAsync(AnnouncementOffer entity);
        Task<RepositoryActionResult<AnnouncementOffer>> AcceptOfferAsync(AnnouncementOffer offer);
        Task<RepositoryActionResult<AnnouncementOffer>> DeclineOfferAsync(AnnouncementOffer offer);
        Task<RepositoryActionResult<AnnouncementOffer>> EndOfferAsync(AnnouncementOffer offer);
        Task<List<Announcement>> GetOldAnnouncementsAsync();
        Task<RepositoryActionResult<AnnouncementOffer>> GetAnnouncementOfferAsync(int offerId);
    }
}