using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.ViewModels;
using Freelance.Infrastructure.ViewModels.Jobs;

namespace Freelance.Infrastructure.Services.Interfaces
{
    public interface IJobsService
    {
        Task<JobsListViewModel> GetJobsAsync(int page, int amount, decimal minWage, decimal maxWage, string[] availability,
            string localization, int? serviceTypeId, string sort);
        Task<JobViewModel> GetJobByIdAsync(int jobId);
        Task<JobViewModel> AddJobAsync(JobViewModel job);
        Task UpdateJobAsync(JobViewModel job);
        Task RemoveJobAsync(int jobId);
        Task<JobOfferViewModel> AddOfferAsync(JobOfferViewModel offer);
        Task<JobOfferViewModel> AcceptOfferAsync(int id, string userId);
        Task<JobOfferViewModel> EndOfferAsync(int id, string userId);
        Task<List<JobViewModel>> GetOldJobsAsync();
        Task<JobViewModel> ActivateJobAsync(int id, string userId);
    }
}
