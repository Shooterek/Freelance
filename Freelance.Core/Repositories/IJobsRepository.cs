﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Core.Repositories
{
    public interface IJobsRepository
    {
        Task<RepositoryActionResult<ICollection<Job>>> GetAllAsync();
        Task<RepositoryActionResult<Job>> GetByIdAsync(int id);
        Task<RepositoryActionResult<ICollection<Job>>> GetByServiceTypeAsync(ServiceType serviceType);
        Task<RepositoryActionResult<Job>> UpdateAsync(Job entity);
        Task<RepositoryActionResult<Job>> RemoveAsync(int id);
        Task<RepositoryActionResult<Job>> AddAsync(Job entity);
        Task<RepositoryActionResult<JobOffer>> AddOfferAsync(JobOffer entity);
        Task<RepositoryActionResult<ICollection<JobOffer>>> GetReceivedOffersAsync(string userId);
        Task<RepositoryActionResult<ICollection<JobOffer>>> GetPublishedOffersAsync(string userId);
        Task<RepositoryActionResult<JobOffer>> RemoveOfferAsync(int id);
        Task<RepositoryActionResult<bool>> DeclineOfferAsync(int offerId, string userId);
        Task<RepositoryActionResult<bool>> AcceptOfferAsync(int offerId, string userId);
    }
}
