using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Core.Repositories;

namespace Freelance.Infrastructure.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private ApplicationDbContext _context;

        public AnnouncementRepository(ApplicationDbContext context)
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
            var announcement = await _context.Announcements.FirstOrDefaultAsync(a => a.AnnouncementId == id);

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
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<Announcement>(announcement, RepositoryStatus.Created);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<Announcement>(entity, RepositoryStatus.Error);
            }
        }
    }
}