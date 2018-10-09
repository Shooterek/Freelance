﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Core.Repositories;

namespace Freelance.Infrastructure.Repositories
{
    public class JobsRepository : IJobsRepository
    {
        private ApplicationDbContext _context;

        public JobsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryActionResult<ICollection<Job>>> GetAllAsync()
        {
            var jobs = await _context.Jobs.ToListAsync();

            return new RepositoryActionResult<ICollection<Job>>(jobs, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<Job>> GetByIdAsync(int id)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(a => a.JobId == id);

            if (job == null)
            {
                return new RepositoryActionResult<Job>(null, RepositoryStatus.NotFound);
            }
            return new RepositoryActionResult<Job>(job, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<ICollection<Job>>> GetByServiceTypeAsync(ServiceType serviceType)
        {
            var jobs = await _context.Jobs.Where(a => a.ServiceTypeId == serviceType.ServiceTypeId).ToListAsync();

            return new RepositoryActionResult<ICollection<Job>>(jobs, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<Job>> UpdateAsync(Job entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<Job>(entity, RepositoryStatus.Updated);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<Job>(entity, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<Job>> RemoveAsync(int id)
        {
            try
            {
                var job = await _context.Jobs.FirstOrDefaultAsync(a => a.JobId == id);

                if (job == null)
                {
                    return new RepositoryActionResult<Job>(null, RepositoryStatus.NotFound);
                }

                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<Job>(job, RepositoryStatus.Deleted);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<Job>(null, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<Job>> AddAsync(Job entity)
        {
            try
            {
                var job = _context.Jobs.Add(entity);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<Job>(job, RepositoryStatus.Created);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<Job>(entity, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<JobOffer>> AddOfferAsync(JobOffer entity)
        {
            try
            {
                var job = _context.JobOffers.Add(entity);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<JobOffer>(entity, RepositoryStatus.Created);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<JobOffer>(entity, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<ICollection<JobOffer>>> GetOffersAsync(string userId)
        {
            var offers = await _context.JobOffers.Where(o => o.Job.EmployerId == userId).ToListAsync();
        
            return new RepositoryActionResult<ICollection<JobOffer>>(offers, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<JobOffer>> RemoveOfferAsync(int id)
        {
            try
            {
                var offer = await _context.JobOffers.FirstOrDefaultAsync(a => a.JobOfferId== id);

                if (offer == null)
                {
                    return new RepositoryActionResult<JobOffer>(null, RepositoryStatus.NotFound);
                }

                _context.JobOffers.Remove(offer);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<JobOffer>(offer, RepositoryStatus.Deleted);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<JobOffer>(null, RepositoryStatus.Error);
            }
        }
    }
}
