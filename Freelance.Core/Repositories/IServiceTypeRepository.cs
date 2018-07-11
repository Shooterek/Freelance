using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Core.Repositories
{
    public interface IServiceTypeRepository : IRepository
    {
        Task<RepositoryActionResult<ICollection<ServiceType>>> GetAllAsync();
        Task<RepositoryActionResult<ServiceType>> GetByIdAsync(int id);
        Task<RepositoryActionResult<ServiceType>> UpdateAsync(ServiceType entity);
        Task<RepositoryActionResult<ServiceType>> RemoveAsync(int id);
        Task<RepositoryActionResult<ServiceType>> AddAsync(ServiceType entity);
    }
}