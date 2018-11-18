﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Services.Interfaces;
using Freelance.Infrastructure.Utils;
using Freelance.Infrastructure.ViewModels;
using WebGrease.Css.Extensions;

namespace Freelance.Infrastructure.Services.Implementations
{
    public class AnnouncementsService : IAnnouncementsService
    {
        private IAnnouncementsRepository _announcementRepository;
        private IServiceTypesService _serviceTypesService;
        private IEmailService _emailService;

        public AnnouncementsService(IAnnouncementsRepository announcementRepository, IEmailService emailService, IServiceTypesService serviceTypesService)
        {
            _announcementRepository = announcementRepository;
            _emailService = emailService;
            _serviceTypesService = serviceTypesService;
        }

        public async Task<AnnouncementsListViewModel> GetAnnouncementsAsync(int page, int amount, decimal minWage, decimal maxWage,
            string[] availability, string localization, int? serviceTypeId, string sortOrder)
        {
            var result = await _announcementRepository.GetAllAsync();

            Availability availableDays = (Availability) 0;
            if (availability != null)
            {
                availableDays = availability.ConvertToFlagEnum<Availability>();
            }

            var entities = result.Entity.Where(a => (a.Localization == localization || localization == null)
                                                    && a.ExpectedHourlyWage > minWage && a.ExpectedHourlyWage < maxWage
                                                    && (serviceTypeId == null || a.ServiceTypeId == serviceTypeId)).ToList();

            if (availability != null)
            {
                availableDays = availability.ConvertToFlagEnum<Availability>();
                entities = entities.Where(a => (a.Availability & availableDays) > 0).ToList();
            }

            switch (sortOrder)
            {
                case null:
                case Constants.SortDateDescending:
                    entities = entities.OrderByDescending(a => a.PublicationDate).ToList();
                    sortOrder = sortOrder == null ? null : Constants.SortDateDescending;
                    break;
                    
                case Constants.SortWageDescending:
                    entities = entities.OrderByDescending(a => a.ExpectedHourlyWage).ToList();
                    sortOrder = Constants.SortWageDescending;
                    break;

                case Constants.SortWageAscending:
                    entities = entities.OrderBy(a => a.ExpectedHourlyWage).ToList();
                    sortOrder = Constants.SortWageAscending;
                    break;

                case Constants.SortDateAscending:
                    entities = entities.OrderBy(a => a.PublicationDate).ToList();
                    sortOrder = Constants.SortDateAscending;
                    break;
            }

            var totalItems = entities.Count;
            var pagingInfo = new PagingInfo(page, amount, totalItems);

            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();
            var servicesList = new List<SelectListItem> {new SelectListItem() {Value = "", Text = "", Selected = serviceTypeId == null} };

            serviceTypes.ForEach(s => servicesList.Add(new SelectListItem() { Value = s.ServiceTypeId.ToString(), Text = s.Name,
                Selected = serviceTypeId != null && s.ServiceTypeId == serviceTypeId.Value}));

            var filter = new AnnouncementFilter {Availability = availableDays, Localization = localization,
                ServiceTypes = servicesList, ServiceTypeId = serviceTypeId, SortOrder = sortOrder};

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
                await _emailService.Notify(result.Entity);
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

        public async Task EndOfferAsync(int id, string userId)
        {
            var result = await _announcementRepository.EndOfferAsync(id, userId);
        }
    }
}
