using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.ViewModels;

namespace Freelance.Infrastructure.Services.Interfaces
{
    public interface IJobsService
    {
        Task<JobsListViewModel> GetJobsAsync(int page, int amount);
        Task<JobsListViewModel> GetJobsByServiceTypeAsync(ServiceType serviceType, int page, int amount);
        Task<JobsListViewModel> GetJobsByUserIdAsync(string userId, int page, int amount);
        Task<Job> GetJobByIdAsync(int jobId);
        Task<Job> AddJobAsync(Job job);
        Task UpdateJobAsync(Job job);
        Task RemoveJobAsync(int jobId);
    }
}
