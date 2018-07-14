using System.Collections.Generic;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.ViewModels;

namespace Freelance.Infrastructure.Services.Interfaces
{
    public interface IAnnouncementsService
    {
        Task<AnnouncementsListViewModel> GetAnnouncementsAsync(int page, int amount);
        Task<AnnouncementsListViewModel> GetAnnouncementsByUserIdAsync(string userId, int page, int amount);
        Task<Announcement> GetAnnouncementById(int announcementId);
        Task UpdateAnnouncementAsync(Announcement announcement);
        Task RemoveAnnouncementAsync(int announcementId);
    }
}