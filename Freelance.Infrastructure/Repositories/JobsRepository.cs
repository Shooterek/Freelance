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
                .Include(j => j.Employer.ReceivedOpinions)
                .Include(j => j.Employer.Photo)
                .FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
            {
                return new RepositoryActionResult<Job>(null, RepositoryStatus.NotFound);
            }
            return new RepositoryActionResult<Job>(job, RepositoryStatus.Ok);
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

                var addedJob = await _context.JobOffers.Include(o => o.Job.Employer)
                    .FirstOrDefaultAsync(o => o.JobOfferId == job.JobOfferId);

                return new RepositoryActionResult<JobOffer>(entity, RepositoryStatus.Created);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<JobOffer>(entity, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<JobOffer>> AcceptOfferAsync(JobOffer offer)
        {
            try
            {
                offer.IsAccepted = true;
                _context.Entry(offer).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<JobOffer>(offer, RepositoryStatus.Ok);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<JobOffer>(offer, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<JobOffer>> GetJobOfferAsync(int offerId)
        {
            var offer = await _context.JobOffers.Include(o => o.Job).FirstOrDefaultAsync(a => a.JobOfferId == offerId);

            if (offer == null)
            {
                return new RepositoryActionResult<JobOffer>(null, RepositoryStatus.NotFound);
            }
            return new RepositoryActionResult<JobOffer>(offer, RepositoryStatus.Ok);
        }

        public async Task<List<Job>> GetOldJobsAsync()
        {
            var maxCorrect = DateTime.Now.Subtract(new TimeSpan(336, 0, 0));
            var maxTime = DateTime.Now.Subtract(new TimeSpan(408, 0, 0));

            var jobsToDelete = await _context.Jobs.Where(a => a.LastActivation < maxTime).ToListAsync();
            _context.Jobs.RemoveRange(jobsToDelete);
            await _context.SaveChangesAsync();

            var jobs = await _context.Jobs
                .Where(a => a.LastActivation < maxCorrect && a.WasNotified == false).ToListAsync();

            return jobs;
        }

        public async Task<RepositoryActionResult<JobOffer>> EndOfferAsync(JobOffer offer)
        {
            try
            {
                offer.IsFinished = true;
                _context.Entry(offer).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<JobOffer>(offer, RepositoryStatus.Ok);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<JobOffer>(offer, RepositoryStatus.Error);
            }
        }
    }
}
