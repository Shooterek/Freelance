using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Services.Interfaces;

namespace Freelance.Infrastructure.Services.Implementations
{
    public class ServiceTypesService : IServiceTypesService
    {
        private IServiceTypesRepository _serviceTypesRepository;

        public ServiceTypesService(IServiceTypesRepository serviceTypesRepository)
        {
            _serviceTypesRepository = serviceTypesRepository;
        }

        public async Task<ICollection<ServiceType>> GetServiceTypesAsync()
        {
            var result = await _serviceTypesRepository.GetAllAsync();

            return result.Entity;
        }

        public async Task<ServiceType> GetServiceTypeByIdAsync(int serviceTypeId)
        {
            var result = await _serviceTypesRepository.GetByIdAsync(serviceTypeId);

            return result.Status == RepositoryStatus.Ok ? result.Entity : null;
        }

        public async Task AddServiceTypeAsync(ServiceType serviceType)
        {
            await _serviceTypesRepository.AddAsync(serviceType);
        }

        public async Task RemoveServiceTypeAsync(int serviceTypeId)
        {
            await _serviceTypesRepository.RemoveAsync(serviceTypeId);
        }

        public async Task UpdateServiceTypeAsync(ServiceType serviceType)
        {
            await _serviceTypesRepository.UpdateAsync(serviceType);
        }
    }
}
