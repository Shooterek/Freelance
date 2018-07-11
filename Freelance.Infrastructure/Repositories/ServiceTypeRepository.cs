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
    public class ServiceTypeRepository : IServiceTypeRepository
    {
        private ApplicationDbContext _context;

        public ServiceTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryActionResult<ICollection<ServiceType>>> GetAllAsync()
        {
            var serviceTypes = await _context.ServiceTypes.ToListAsync();

            return new RepositoryActionResult<ICollection<ServiceType>>(serviceTypes, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<ServiceType>> GetByIdAsync(int id)
        {
            var serviceType = await _context.ServiceTypes.FirstOrDefaultAsync(s => s.ServiceTypeId == id);

            if (serviceType == null)
            {
                return new RepositoryActionResult<ServiceType>(null, RepositoryStatus.NotFound);
            }
            return new RepositoryActionResult<ServiceType>(serviceType, RepositoryStatus.Ok);
        }

        public async Task<RepositoryActionResult<ServiceType>> UpdateAsync(ServiceType entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<ServiceType>(entity, RepositoryStatus.Updated);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<ServiceType>(entity, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<ServiceType>> RemoveAsync(int id)
        {
            try
            {
                var serviceType = await _context.ServiceTypes.FirstOrDefaultAsync(s => s.ServiceTypeId == id);

                if (serviceType == null)
                {
                    return new RepositoryActionResult<ServiceType>(null, RepositoryStatus.NotFound);
                }

                _context.ServiceTypes.Remove(serviceType);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<ServiceType>(serviceType, RepositoryStatus.Deleted);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<ServiceType>(null, RepositoryStatus.Error);
            }
        }

        public async Task<RepositoryActionResult<ServiceType>> AddAsync(ServiceType entity)
        {
            try
            {
                _context.ServiceTypes.Add(entity);
                await _context.SaveChangesAsync();

                return new RepositoryActionResult<ServiceType>(entity, RepositoryStatus.Created);
            }
            catch (Exception e)
            {
                return new RepositoryActionResult<ServiceType>(entity, RepositoryStatus.Error);
            }
        }
    }
}
