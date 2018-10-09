using System.Collections.Generic;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.ViewModels;

namespace Freelance.Infrastructure.Services.Interfaces
{
    public interface IAnnouncementsService
    {
        Task<AnnouncementsListViewModel> GetAnnouncementsAsync(int page, int amount);
        Task<AnnouncementsListViewModel> GetAnnouncementsByServiceTypeAsync(ServiceType serviceType, int page, int amount);
        Task<AnnouncementsListViewModel> GetAnnouncementsByUserIdAsync(string userId, int page, int amount);
        Task<Announcement> GetAnnouncementByIdAsync(int announcementId);
        Task<Announcement> AddAnnouncementAsync(Announcement announcement);
        Task UpdateAnnouncementAsync(Announcement announcement);
        Task RemoveAnnouncementAsync(int announcementId);
        Task<ICollection<AnnouncementOffer>> GetOffersAsync(string userId);
        Task<AnnouncementOffer> AddOfferAsync(AnnouncementOffer offer);
        Task RemoveOfferAsync(int id);
    }
}