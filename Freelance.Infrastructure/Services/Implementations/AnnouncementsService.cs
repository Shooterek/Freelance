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
    public class AnnouncementsService : IAnnouncementsService
    {
        private IAnnouncementsRepository _announcementRepository;
        private IEmailService _emailService;

        public AnnouncementsService(IAnnouncementsRepository announcementRepository, IEmailService emailService)
        {
            _announcementRepository = announcementRepository;
            _emailService = emailService;
        }

        public async Task<AnnouncementsListViewModel> GetAnnouncementsAsync(int page, int amount, decimal minWage, decimal maxWage, string[] availability, string localization)
        {
            var result = await _announcementRepository.GetAllAsync();

            Availability availableDays = (Availability) 0;
            if (availability != null)
            {
                availableDays = availability.ConvertToFlagEnum<Availability>();
            }

            var entities = result.Entity.Where(a => (a.Localization == localization || localization == null)
                                                    && a.ExpectedHourlyWage > minWage && a.ExpectedHourlyWage < maxWage).ToList();

            if (availability != null)
            {
                availableDays = availability.ConvertToFlagEnum<Availability>();
                entities = entities.Where(a => (a.Availability & availableDays) > 0).ToList();
            }


            var totalItems = entities.Count;
            var pagingInfo = new PagingInfo(page, amount, totalItems);

            var filter = new AnnouncementFilter {Availability = availableDays, Localization = localization};

            var announcements = entities.Skip((page - 1) * amount).Take(amount).ToList();

            return new AnnouncementsListViewModel() {Announcements = announcements, PagingInfo = pagingInfo, Filter = filter};
        }

        public async Task<AnnouncementsListViewModel> GetAnnouncementsByServiceTypeAsync(ServiceType serviceType, int page, int amount)
        {
            var result = await _announcementRepository.GetAllAsync();

            var entitiesByServiceType = result.Entity.Where(a => a.ServiceTypeId == serviceType.ServiceTypeId).ToList();

            var totalItems = result.Entity.Count;
            var pagingInfo = new PagingInfo(page, amount, totalItems);

            var entities = entitiesByServiceType.Skip((page - 1) * amount).Take(amount).ToList();
            return new AnnouncementsListViewModel() {Announcements = entities, PagingInfo = pagingInfo};
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

        public async Task<Announcement> GetAnnouncementByIdAsync(int announcementId)
        {
            var result = await _announcementRepository.GetByIdAsync(announcementId);

            return result.Entity;
        }

        public async Task<Announcement> AddAnnouncementAsync(Announcement announcement)
        {
            var result = await _announcementRepository.AddAsync(announcement);

            if (result.Status == RepositoryStatus.Created)
            {
                return result.Entity;
            }

            return null;
        }

        public async Task UpdateAnnouncementAsync(Announcement announcement)
        {
            var result = await _announcementRepository.UpdateAsync(announcement);
        }

        public async Task RemoveAnnouncementAsync(int announcementId)
        {
            var result = await _announcementRepository.RemoveAsync(announcementId);
        }

        public async Task<ICollection<AnnouncementOffer>> GetPublishedOffersAsync(string userId)
        {
            var result = await _announcementRepository.GetPublishedOffersAsync(userId);

            return result.Entity;
        }

        public async Task<ICollection<AnnouncementOffer>> GetReceivedOffersAsync(string userId)
        {
            var result = await _announcementRepository.GetReceivedOffersAsync(userId);

            return result.Entity;
        }

        public async Task<AnnouncementOffer> AddOfferAsync(AnnouncementOffer offer)
        {
            var result = await _announcementRepository.AddOfferAsync(offer);

            if (result.Status == RepositoryStatus.Created)
            {
                _emailService.Notify(result.Entity);
                return result.Entity;
            }

            return null;
        }

        public async Task RemoveOfferAsync(int id)
        {
            var result = await _announcementRepository.RemoveOfferAsync(id);
        }

        public async Task AcceptOfferAsync(int offerId, string userId)
        {
            var result = await _announcementRepository.AcceptOfferAsync(offerId, userId);
        }

        public async Task DeclineOfferAsync(int offerId, string userId)
        {
            var result = await _announcementRepository.DeclineOfferAsync(offerId, userId);
        }
    }
}
