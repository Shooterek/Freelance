using System.Collections.Generic;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.ViewModels;
using Freelance.Infrastructure.ViewModels.Announcements;

namespace Freelance.Infrastructure.Services.Interfaces
{
    public interface IAnnouncementsService
    {
        Task<AnnouncementsListViewModel> GetAnnouncementsAsync(int page, int amount, int minWage, int maxWage, string[] availability,
            string localization, int? serviceTypeId, string sort);
        Task<AnnouncementViewModel> GetAnnouncementByIdAsync(int announcementId);
        Task<AnnouncementViewModel> AddAnnouncementAsync(AnnouncementViewModel announcement);
        Task RemoveAnnouncementAsync(int announcementId);
        Task<AnnouncementOfferViewModel> AddOfferAsync(AnnouncementOfferViewModel offer);
        Task<AnnouncementOfferViewModel> AcceptOfferAsync(int offerId, string userId);
        Task DeclineOfferAsync(int offerId, string userId);
        Task<AnnouncementOfferViewModel> EndOfferAsync(int id, string userId);
        Task<List<AnnouncementViewModel>> GetOldAnnouncementsAsync();
        Task<AnnouncementViewModel> ActivateAnnouncementAsync(int id, string userId);
        Task<AnnouncementViewModel> UpdateAnnouncementAsync(AnnouncementViewModel announcement);
    }
}