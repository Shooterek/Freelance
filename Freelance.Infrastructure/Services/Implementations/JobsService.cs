using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Services.Interfaces;
using Freelance.Infrastructure.Utils;
using Freelance.Infrastructure.ViewModels;

namespace Freelance.Infrastructure.Services.Implementations
{
    public class JobsService : IJobsService
    {
        private IJobsRepository _jobRepository;

        public JobsService(IJobsRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<JobsListViewModel> GetJobsAsync(int page, int amount)
        {
            var result = await _jobRepository.GetAllAsync();

            var totalItems = result.Entity.Count;
            var pagingInfo = new PagingInfo(page, amount, totalItems);

            var jobs = result.Entity.Skip((page - 1) * amount).Take(amount).ToList();

            return new JobsListViewModel() { Jobs = jobs, PagingInfo = pagingInfo };
        }

        public async Task<JobsListViewModel> GetJobsByServiceTypeAsync(ServiceType serviceType, int page, int amount)
        {
            var result = await _jobRepository.GetAllAsync();

            var entitiesByServiceType = result.Entity.Where(a => a.ServiceTypeId == serviceType.ServiceTypeId).ToList();

            var totalItems = result.Entity.Count;
            var pagingInfo = new PagingInfo(page, amount, totalItems);

            var entities = entitiesByServiceType.Skip((page - 1) * amount).Take(amount).ToList();
            return new JobsListViewModel() { Jobs = entities, PagingInfo = pagingInfo };
        }

        public async Task<JobsListViewModel> GetJobsByUserIdAsync(string userId, int page, int amount)
        {
            var result = await _jobRepository.GetAllAsync();

            var usersJobs = result.Entity.Where(a => a.EmployerId == userId).ToList();

            var totalItems = usersJobs.Count;
            var pagingInfo = new PagingInfo(page, amount, totalItems);

            var jobs = usersJobs.Skip((page - 1) * amount).Take(amount).ToList();

            return new JobsListViewModel() { Jobs = jobs, PagingInfo = pagingInfo };
        }

        public async Task<Job> GetJobByIdAsync(int jobId)
        {
            var result = await _jobRepository.GetByIdAsync(jobId);

            return result.Entity;
        }

        public async Task<Job> AddJobAsync(Job job)
        {
            var result = await _jobRepository.AddAsync(job);

            if (result.Status == RepositoryStatus.Created)
            {
                return result.Entity;
            }

            return null;
        }

        public async Task UpdateJobAsync(Job job)
        {
            var result = await _jobRepository.UpdateAsync(job);
        }

        public async Task RemoveJobAsync(int jobId)
        {
            var result = await _jobRepository.RemoveAsync(jobId);
        }

        public async Task<ICollection<JobOffer>> GetOffersAsync(string userId)
        {
            var result = await _jobRepository.GetOffersAsync(userId);

            return result.Entity;
        }

        public async Task<JobOffer> AddOfferAsync(JobOffer offer)
        {
            var result = await _jobRepository.AddOfferAsync(offer);

            if (result.Status == RepositoryStatus.Created)
            {
                return result.Entity;
            }

            return null;
        }

        public async Task RemoveOfferAsync(int id)
        {
            var result = await _jobRepository.RemoveAsync(id);
        }
    }
}