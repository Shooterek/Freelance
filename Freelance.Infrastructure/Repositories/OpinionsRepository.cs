using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Utils;

namespace Freelance.Infrastructure.Repositories
{
    public class OpinionsRepository : IOpinionsRepository
    {
        private ApplicationDbContext _context;

        public OpinionsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryActionResult<bool>> CanAddOpinion(string reviewerId, string evaluatedUserId, int offerId, string offerType)
        {
            Opinion opinion = null;
            if (offerType.Equals(Constants.AnnouncementOffer))
            {
                var offer = await _context.AnnouncementOffers.Include(a => a.Announcement).FirstOrDefaultAsync(a => a.AnnouncementOfferId == offerId);

                if ((offer?.OffererId == evaluatedUserId && offer?.Announcement.AdvertiserId == reviewerId) ||
                    (offer?.OffererId == reviewerId && offer?.Announcement.AdvertiserId == evaluatedUserId))
                {
                    opinion = await _context.Opinions
                        .FirstOrDefaultAsync(o => o.EvaluatedUserId.Equals(evaluatedUserId) && o.AnnouncementOfferId == offerId);
                }
            }
            else if (offerType.Equals(Constants.JobOffer))
            {
                var offer = await _context.JobOffers.Include(j => j.Job).FirstOrDefaultAsync(a => a.JobOfferId == offerId);

                if ((offer?.OffererId == evaluatedUserId && offer?.Job.EmployerId == reviewerId) ||
                    (offer?.OffererId == reviewerId && offer?.Job.EmployerId == evaluatedUserId))

                    opinion = await _context.Opinions
                    .FirstOrDefaultAsync(o => o.EvaluatedUserId.Equals(evaluatedUserId) && o.JobOfferId == offerId);
            }

            if (opinion == null)
            {
                return new RepositoryActionResult<bool>(true, RepositoryStatus.Ok);
            }

            return new RepositoryActionResult<bool>(false, RepositoryStatus.BadRequest);
        }

        public async Task<RepositoryActionResult<Opinion>> AddOpinionAsync(Opinion opinion)
        {
            try
            {
                var savedOpinion = _context.Opinions.Add(opinion);
                await _context.SaveChangesAsync();

                var opinionWithOffer = await _context.Opinions
                    .Include(o => o.AnnouncementOffer)
                    .Include(o => o.JobOffer)
                    .FirstOrDefaultAsync(o => o.OpinionId == savedOpinion.OpinionId);
                return new RepositoryActionResult<Opinion>(opinionWithOffer, RepositoryStatus.Created);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<Opinion>(opinion, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<ICollection<Opinion>>> GetOpinionsByEvaluatedUserId(string userId)
        {
            var opinions = await _context.Opinions.Where(o => o.EvaluatedUserId == userId).ToListAsync();

            return new RepositoryActionResult<ICollection<Opinion>>(opinions, RepositoryStatus.Ok);
        }
    }
}
