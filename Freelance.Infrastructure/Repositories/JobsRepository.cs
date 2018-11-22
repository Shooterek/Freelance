using System;
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
            var job = await _context.Jobs
                .Include(j => j.Offers)
                .Include(j => j.Offers.Select(o => o.Offerer))
                .Include(j => j.Employer)
                .Include(j => j.Employer.ReceivedOpinions)
                .Include(j => j.Employer.Photo)
                .FirstOrDefaultAsync(j => j.JobId == id);

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

                if (entity.Photos.Count > 0)
                {
                    foreach (var photo in entity.Photos)
                    {
                        photo.JobId = job.JobId;
                    }

                    var photos = _context.Photos.AddRange(entity.Photos);
                    job.Photos = photos.ToList();
                }
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

        public async Task<RepositoryActionResult<ICollection<JobOffer>>> GetReceivedOffersAsync(string userId)
        {
            var offers = await _context.JobOffers.Where(o => o.Job.EmployerId == userId).ToListAsync();
        
            return new RepositoryActionResult<ICollection<JobOffer>>(offers, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<ICollection<JobOffer>>> GetPublishedOffersAsync(string userId)
        {
            var offers = await _context.JobOffers.Where(o => o.OffererId == userId).ToListAsync();

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

        public async Task<RepositoryActionResult<bool>> DeclineOfferAsync(int offerId, string userId)
        {
            try
            {
                var offer = await _context.JobOffers.Include(o => o.Job).FirstOrDefaultAsync(a => a.JobOfferId == offerId);

                if (offer == null)
                {
                    return new RepositoryActionResult<bool>(false, RepositoryStatus.NotFound);
                }

                if (offer.Job.EmployerId != userId)
                {
                    return new RepositoryActionResult<bool>(false, RepositoryStatus.BadRequest);
                }

                _context.JobOffers.Remove(offer);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<bool>(true, RepositoryStatus.Deleted);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<bool>(true, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<bool>> AcceptOfferAsync(int offerId, string userId)
        {
            try
            {
                var offer = await _context.JobOffers.Include(o => o.Job).FirstOrDefaultAsync(a => a.JobOfferId == offerId);

                if (offer == null)
                {
                    return new RepositoryActionResult<bool>(false, RepositoryStatus.NotFound);
                }

                if (offer.Job.EmployerId != userId)
                {
                    return new RepositoryActionResult<bool>(false, RepositoryStatus.BadRequest);
                }

                offer.IsAccepted = true;
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<bool>(true, RepositoryStatus.Ok);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<bool>(true, RepositoryStatus.Error);
            }
        }
    }
}
