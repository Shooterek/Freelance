using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.Services.Interfaces
{
    public interface IOpinionsService
    {
        Task<bool> CanAddOpinion(string reviewerId, string evaluatedUserId, int offerId, string offerType);
        Task<Opinion> AddOpinionAsync(Opinion opinion);
        Task<List<Opinion>> GetOpinionsByEvaluatedUserId(string userId);
    }
}