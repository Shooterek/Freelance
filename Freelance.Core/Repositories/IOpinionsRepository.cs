using System.Collections.Generic;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Core.Repositories
{
    public interface IOpinionsRepository
    {
        Task<RepositoryActionResult<bool>> CanAddOpinion(string reviewerId, string evaluatedUserId, int offerId, string offerType);
        Task<RepositoryActionResult<Opinion>> AddOpinionAsync(Opinion opinion);
        Task<RepositoryActionResult<ICollection<Opinion>>> GetOpinionsByEvaluatedUserId(string userId);
    }
}