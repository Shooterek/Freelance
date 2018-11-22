using System.Collections.Generic;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.ViewModels;
using Freelance.Infrastructure.ViewModels.Announcements;

namespace Freelance.Infrastructure.Services.Interfaces
{
    public interface IAnnouncementsService
    {
        Task<AnnouncementsListViewModel> GetAnnouncementsAsync(int page, int amount, decimal minWage, decimal maxWage, string[] availability,
            string localization, int? serviceTypeId, string sort);
        Task<AnnouncementViewModel> GetAnnouncementByIdAsync(int announcementId);
        Task<AnnouncementViewModel> AddAnnouncementAsync(AnnouncementViewModel announcement);
        Task UpdateAnnouncementAsync(AnnouncementViewModel announcement);
        Task RemoveAnnouncementAsync(int announcementId);
        Task<ICollection<AnnouncementOfferViewModel>> GetPublishedOffersAsync(string userId);
        Task<ICollection<AnnouncementOfferViewModel>> GetReceivedOffersAsync(string userId);
        Task<AnnouncementOfferViewModel> AddOfferAsync(AnnouncementOfferViewModel offer);
        Task RemoveOfferAsync(int id);
        Task AcceptOfferAsync(int offerId, string userId);
        Task DeclineOfferAsync(int offerId, string userId);
        Task EndOfferAsync(int id, string userId);
    }
}