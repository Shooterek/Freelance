using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using WebGrease.Css.Extensions;

namespace Freelance.Infrastructure.Repositories
{
    public class AnnouncementsRepository : IAnnouncementsRepository
    {
        private ApplicationDbContext _context;

        public AnnouncementsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryActionResult<ICollection<Announcement>>> GetAllAsync()
        {
            var announcements = await _context.Announcements.ToListAsync();

            return new RepositoryActionResult<ICollection<Announcement>>(announcements, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<Announcement>> GetByIdAsync(int id)
        {
            var rawAnnouncement = from a in _context.Announcements
                where a.AnnouncementId == id
                join op in _context.Opinions on a.AdvertiserId equals op.EvaluatedUserId
                group new {a, op} by a.AnnouncementId
                into g
                select new
                {
                    Announcement = g.FirstOrDefault().a,
                    AmountOfReviews = g.Count(),
                    Rating = g.Average(x =>(double) x.op.Rating)
                };

            rawAnnouncement.FirstOrDefault().Announcement.Advertiser.Rating = rawAnnouncement.FirstOrDefault().Rating;
            rawAnnouncement.FirstOrDefault().Announcement.Advertiser.AmountOfReviews = rawAnnouncement.FirstOrDefault().AmountOfReviews;

            var announcement = rawAnnouncement.FirstOrDefault()?.Announcement;

            if (announcement == null)
            {
                return new RepositoryActionResult<Announcement>(null, RepositoryStatus.NotFound);
            }

            var rawOffers = from of in _context.AnnouncementOffers
                where of.AnnouncementId == announcement.AnnouncementId
                join op in _context.Opinions on of.OffererId equals op.EvaluatedUserId
                join u in _context.Users on of.OffererId equals u.Id
                group new {of, op, u} by of.OffererId
                into g
                select new
                {
                    Offerer = g.FirstOrDefault().u,
                    Offer = g.FirstOrDefault().of,
                    AmountOfReviews = g.Count(),
                    Rating = g.Average(a => a.op.Rating)
                };

            var rawOffersList = await rawOffers.ToListAsync();
            announcement.Offers = new List<AnnouncementOffer>(rawOffersList.Select(x =>
            {
                x.Offerer.Rating = x.Rating;
                x.Offerer.AmountOfReviews = x.AmountOfReviews;
                x.Offer.Offerer = x.Offerer;
                return x.Offer;
            }));
            return new RepositoryActionResult<Announcement>(announcement, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<ICollection<Announcement>>> GetByServiceTypeAsync(ServiceType serviceType)
        {
            var announcements = await _context.Announcements.Where(a => a.ServiceTypeId == serviceType.ServiceTypeId).ToListAsync();

            return new RepositoryActionResult<ICollection<Announcement>>(announcements, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<Announcement>> UpdateAsync(Announcement entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<Announcement>(entity, RepositoryStatus.Updated);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<Announcement>(entity, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<Announcement>> RemoveAsync(int id)
        {
            try
            {
                var announcement = await _context.Announcements.FirstOrDefaultAsync(a => a.AnnouncementId == id);

                if (announcement == null)
                {
                    return new RepositoryActionResult<Announcement>(null, RepositoryStatus.NotFound);
                }

                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<Announcement>(announcement, RepositoryStatus.Deleted);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<Announcement>(null, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<Announcement>> AddAsync(Announcement entity)
        {
            try
            {
                var announcement = _context.Announcements.Add(entity);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<Announcement>(announcement, RepositoryStatus.Created);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<Announcement>(entity, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<AnnouncementOffer>> AddOfferAsync(AnnouncementOffer entity)
        {
            try
            {
                var offer = _context.AnnouncementOffers.Add(entity);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<AnnouncementOffer>(offer, RepositoryStatus.Created);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<AnnouncementOffer>(entity, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<ICollection<AnnouncementOffer>>> GetPublishedOffersAsync(string userId)
        {
            var offers = await _context.AnnouncementOffers.Where(o => o.OffererId == userId).ToListAsync();

            return new RepositoryActionResult<ICollection<AnnouncementOffer>>(offers, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<ICollection<AnnouncementOffer>>> GetReceivedOffersAsync(string userId)
        {
            var offers = await _context.AnnouncementOffers.Where(o => o.Announcement.AdvertiserId == userId).ToListAsync();

            return new RepositoryActionResult<ICollection<AnnouncementOffer>>(offers, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<AnnouncementOffer>> RemoveOfferAsync(int id)
        {
            try
            {
                var offer = await _context.AnnouncementOffers.FirstOrDefaultAsync(a => a.AnnouncementOfferId == id);

                if (offer == null)
                {
                    return new RepositoryActionResult<AnnouncementOffer>(null, RepositoryStatus.NotFound);
                }

                _context.AnnouncementOffers.Remove(offer);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<AnnouncementOffer>(offer, RepositoryStatus.Deleted);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<AnnouncementOffer>(null, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<bool>> AcceptOfferAsync(int offerId, string userId)
        {
            try
            {
                var offer = await _context.AnnouncementOffers.Include(o => o.Announcement).FirstOrDefaultAsync(a => a.AnnouncementOfferId == offerId);

                if (offer == null)
                {
                    return new RepositoryActionResult<bool>(false, RepositoryStatus.NotFound);
                }

                if (offer.Announcement.AdvertiserId != userId)
                {
                    return new RepositoryActionResult<bool>(false, RepositoryStatus.BadRequest);
                }

                offer.IsAccepted = true;
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<bool>(true, RepositoryStatus.Ok);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<bool>(false, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<bool>> DeclineOfferAsync(int offerId, string userId)
        {
            try
            {
                var offer = await _context.AnnouncementOffers.Include(o => o.Announcement).FirstOrDefaultAsync(a => a.AnnouncementOfferId == offerId);

                if (offer == null)
                {
                    return new RepositoryActionResult<bool>(false, RepositoryStatus.NotFound);
                }

                if (offer.Announcement.AdvertiserId != userId)
                {
                    return new RepositoryActionResult<bool>(false, RepositoryStatus.BadRequest);
                }

                _context.AnnouncementOffers.Remove(offer);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<bool>(true, RepositoryStatus.Ok);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<bool>(false, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<bool>> EndOfferAsync(int id, string userId)
        {
            try
            {
                var offer = await _context.AnnouncementOffers.Include(o => o.Announcement).FirstOrDefaultAsync(a => a.AnnouncementOfferId == id);

                if (offer == null)
                {
                    return new RepositoryActionResult<bool>(false, RepositoryStatus.NotFound);
                }

                if (offer.Announcement.AdvertiserId != userId)
                {
                    return new RepositoryActionResult<bool>(false, RepositoryStatus.BadRequest);
                }

                offer.IsFinished = true;
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<bool>(true, RepositoryStatus.Ok);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<bool>(false, RepositoryStatus.Error);
            }
        }
    }
}