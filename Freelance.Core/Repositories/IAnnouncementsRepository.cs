using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Core.Repositories
{
    public interface IAnnouncementsRepository : IRepository
    {
        Task<RepositoryActionResult<ICollection<Announcement>>> GetAllAsync();
        Task<RepositoryActionResult<Announcement>> GetByIdAsync(int id);
        Task<RepositoryActionResult<ICollection<Announcement>>> GetByServiceTypeAsync(ServiceType serviceType);
        Task<RepositoryActionResult<Announcement>> UpdateAsync(Announcement entity);
        Task<RepositoryActionResult<Announcement>> RemoveAsync(int id);
        Task<RepositoryActionResult<Announcement>> AddAsync(Announcement entity);

    }
}