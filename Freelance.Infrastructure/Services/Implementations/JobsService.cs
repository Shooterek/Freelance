﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Services.Interfaces;
using Freelance.Infrastructure.Utils;
using Freelance.Infrastructure.ViewModels;
using Freelance.Infrastructure.ViewModels.Jobs;
using WebGrease.Css.Extensions;

namespace Freelance.Infrastructure.Services.Implementations
{
    public class JobsService : IJobsService
    {
        private IJobsRepository _jobsRepository;
        private IServiceTypesService _serviceTypesService;
        private IMapper _mapper;

        public JobsService(IJobsRepository jobsRepository, IServiceTypesService serviceTypesService, IMapper mapper)
        {
            _jobsRepository = jobsRepository;
            _serviceTypesService = serviceTypesService;
            _mapper = mapper;
        }

        public async Task<JobsListViewModel> GetJobsAsync(int page, int amount, decimal minWage, decimal maxWage, string[] availability,
            string localization, int? serviceTypeId, string sort)
        {
            var result = await _jobsRepository.GetAllAsync();

            Availability availableDays = (Availability)0;
            if (availability != null)
            {
                availableDays = availability.ConvertToFlagEnum<Availability>();
            }

            var entities = result.Entity.Where(a => (a.Localization == localization || localization == null)
                                                    && (a.MinimumWage > minWage || a.MaximumWage < maxWage)
                                                    && (serviceTypeId == null || a.ServiceTypeId == serviceTypeId)).ToList();

            if (availability != null)
            {
                availableDays = availability.ConvertToFlagEnum<Availability>();
                entities = entities.Where(a => (a.Availability & availableDays) > 0).ToList();
            }

            switch (sort)
            {
                case null:
                case Constants.SortDateDescending:
                    entities = entities.OrderByDescending(a => a.PublicationDate).ToList();
                    sort = sort == null ? null : Constants.SortDateDescending;
                    break;

                case Constants.SortMinWageDescending:
                    entities = entities.OrderByDescending(a => a.MinimumWage).ToList();
                    sort = Constants.SortMinWageDescending;
                    break;

                case Constants.SortMaxWageDescending:
                    entities = entities.OrderByDescending(a => a.MaximumWage).ToList();
                    sort = Constants.SortMaxWageDescending;
                    break;

                case Constants.SortMinWageAscending:
                    entities = entities.OrderBy(a => a.MinimumWage).ToList();
                    sort = Constants.SortMinWageAscending;
                    break;

                case Constants.SortMaxWageAscending:
                    entities = entities.OrderBy(a => a.MaximumWage).ToList();
                    sort = Constants.SortMaxWageAscending;
                    break;

                case Constants.SortDateAscending:
                    entities = entities.OrderBy(a => a.PublicationDate).ToList();
                    sort = Constants.SortDateAscending;
                    break;
            }

            var totalItems = entities.Count;
            var pagingInfo = new PagingInfo(page, amount, totalItems);

            var serviceTypes = await _serviceTypesService.GetServiceTypesAsync();
            var servicesList = new List<SelectListItem> { new SelectListItem() { Value = "", Text = "", Selected = serviceTypeId == null } };

            serviceTypes.ForEach(s => servicesList.Add(new SelectListItem()
            {
                Value = s.ServiceTypeId.ToString(),
                Text = s.Name,
                Selected = serviceTypeId != null && s.ServiceTypeId == serviceTypeId.Value
            }));

            var filter = new JobFilter
            {
                Availability = availableDays,
                Localization = localization,
                ServiceTypes = servicesList,
                ServiceTypeId = serviceTypeId,
                SortOrder = sort
            };

            var announcements = entities.Skip((page - 1) * amount).Take(amount).ToList();

            return new JobsListViewModel()
            {
                Jobs = _mapper.Map<ICollection<JobViewModel>>(announcements),
                PagingInfo = pagingInfo,
                Filter = filter
            };
        }

        public async Task<JobViewModel> GetJobByIdAsync(int jobId)
        {
            var result = await _jobsRepository.GetByIdAsync(jobId);

            return _mapper.Map<JobViewModel>(result.Entity);
        }

        public async Task<JobViewModel> AddJobAsync(JobViewModel job)
        {
            var result = await _jobsRepository.AddAsync(_mapper.Map<Job>(job));

            if (result.Status == RepositoryStatus.Created)
            {
                return _mapper.Map<JobViewModel>(result.Entity);
            }

            return null;
        }

        public async Task UpdateJobAsync(JobViewModel job)
        {
            var result = await _jobsRepository.UpdateAsync(_mapper.Map<Job>(job));
        }

        public async Task RemoveJobAsync(int jobId)
        {
            var result = await _jobsRepository.RemoveAsync(jobId);
        }

        public async Task<ICollection<JobOfferViewModel>> GetReceivedOffersAsync(string userId)
        {
            var result = await _jobsRepository.GetReceivedOffersAsync(userId);

            return _mapper.Map<List<JobOfferViewModel>>(result.Entity);
        }

        public async Task<ICollection<JobOfferViewModel>> GetPublishedOffersAsync(string userId)
        {
            var result = await _jobsRepository.GetPublishedOffersAsync(userId);

            return _mapper.Map<ICollection<JobOfferViewModel>>(result.Entity);
        }

        public async Task<JobOfferViewModel> AddOfferAsync(JobOfferViewModel offer)
        {
            var result = await _jobsRepository.AddOfferAsync(_mapper.Map<JobOffer>(offer));

            if (result.Status == RepositoryStatus.Created)
            {
                return _mapper.Map<JobOfferViewModel>(result.Entity);
            }

            return null;
        }

        public async Task RemoveOfferAsync(int id)
        {
            var result = await _jobsRepository.RemoveAsync(id);
        }

        public async Task DeclineOfferAsync(int id, string userId)
        {
            var result = await _jobsRepository.DeclineOfferAsync(id, userId);
        }

        public async Task AcceptOfferAsync(int id, string userId)
        {
            var result = await _jobsRepository.AcceptOfferAsync(id, userId);
        }
    }
}