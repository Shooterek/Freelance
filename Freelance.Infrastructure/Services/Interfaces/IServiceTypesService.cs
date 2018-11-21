using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.Services.Interfaces
{
    public interface IServiceTypesService
    {
        Task<ICollection<ServiceType>> GetServiceTypesAsync();
        Task<ServiceType> GetServiceTypeByIdAsync(int serviceTypeId);
        Task AddServiceTypeAsync(ServiceType serviceType);
        Task RemoveServiceTypeAsync(int serviceTypeId);
        Task UpdateServiceTypeAsync(ServiceType serviceType);
    }
}
