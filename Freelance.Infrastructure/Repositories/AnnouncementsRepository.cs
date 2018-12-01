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
            var announcement = await _context.Announcements
                .Include(a => a.Offers.Select(o => o.Offerer))
                .FirstOrDefaultAsync(a => a.AnnouncementId == id);

            if (announcement == null)
            {
                return new RepositoryActionResult<Announcement>(null, RepositoryStatus.NotFound);
            }
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

                if (entity.Photos.Count > 0)
                {
                    foreach (var photo in entity.Photos)
                    {
                        photo.AnnouncementId = announcement.AnnouncementId;
                    }

                    var photos = _context.Photos.AddRange(entity.Photos);
                    announcement.Photos = photos.ToList();
                }
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

        public async Task<RepositoryActionResult<AnnouncementOffer>> AcceptOfferAsync(AnnouncementOffer offer)
        {
            try
            {
                offer.IsAccepted = true;
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<AnnouncementOffer>(offer, RepositoryStatus.Ok);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<AnnouncementOffer>(null, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<AnnouncementOffer>> DeclineOfferAsync(AnnouncementOffer offer)
        {
            try
            {
                _context.AnnouncementOffers.Remove(offer);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<AnnouncementOffer>(offer, RepositoryStatus.Ok);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<AnnouncementOffer>(offer, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<AnnouncementOffer>> EndOfferAsync(AnnouncementOffer offer)
        {
            try
            {
                offer.IsFinished = true;
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<AnnouncementOffer>(offer, RepositoryStatus.Ok);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<AnnouncementOffer>(offer, RepositoryStatus.Error);
            }
        }

        public async Task<List<Announcement>> GetOldAnnouncementsAsync()
        {
            var max = DateTime.Now.Subtract(new TimeSpan(336, 0, 0));
            var announcements = await _context.Announcements
                .Where(a => a.PublicationDate < max).ToListAsync();

            return announcements;
        }

        public async Task<RepositoryActionResult<AnnouncementOffer>> GetAnnouncementOfferAsync(int offerId)
        {
            var offer = await _context.AnnouncementOffers.Include(o => o.Announcement).FirstOrDefaultAsync(a => a.AnnouncementOfferId == offerId);

            if (offer == null)
            {
                return new RepositoryActionResult<AnnouncementOffer>(null, RepositoryStatus.NotFound);
            }
            return new RepositoryActionResult<AnnouncementOffer>(offer, RepositoryStatus.Ok);
        }
    }
}