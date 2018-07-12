using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Services.Interfaces;
using Freelance.Infrastructure.Utils;
using Freelance.Infrastructure.ViewModels;

namespace Freelance.Infrastructure.Services.Implementations
{
    public class AnnouncementService : IAnnouncementService
    {
        private IAnnouncementRepository _announcementRepository;

        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<AnnouncementsListViewModel> GetAnnouncementsAsync(int page, int amount)
        {
            var result = await _announcementRepository.GetAllAsync();

            var totalItems = result.Entity.Count;
            var pagingInfo = new PagingInfo(page, amount, totalItems);

            var announcements = result.Entity.Skip((page - 1) * amount).Take(amount).ToList();

            return new AnnouncementsListViewModel() {Announcements = announcements, PagingInfo = pagingInfo};
        }

        public async Task<AnnouncementsListViewModel> GetAnnouncementsByUserIdAsync(string userId, int page, int amount)
        {
            var result = await _announcementRepository.GetAllAsync();

            var usersAnnouncements = result.Entity.Where(a => a.AdvertiserId == userId).ToList();

            var totalItems = usersAnnouncements.Count;
            var pagingInfo = new PagingInfo(page, amount, totalItems);

            var announcements = usersAnnouncements.Skip((page - 1) * amount).Take(amount).ToList();

            return new AnnouncementsListViewModel() { Announcements = announcements, PagingInfo = pagingInfo };
        }

        public async Task<Announcement> GetAnnouncementById(int announcementId)
        {
            var result = await _announcementRepository.GetByIdAsync(announcementId);

            return result.Entity;
        }

        public async Task UpdateAnnouncementAsync(Announcement announcement)
        {
            var result = await _announcementRepository.UpdateAsync(announcement);
        }

        public async Task RemoveAnnouncementAsync(int announcementId)
        {
            var result = await _announcementRepository.RemoveAsync(announcementId);
        }
    }
}
