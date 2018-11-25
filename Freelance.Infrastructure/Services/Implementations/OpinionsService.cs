using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Services.Interfaces;

namespace Freelance.Infrastructure.Services.Implementations
{
    public class OpinionsService : IOpinionsService
    {
        private IOpinionsRepository _opinionsRepository;

        public OpinionsService(IOpinionsRepository opinionsRepository)
        {
            _opinionsRepository = opinionsRepository;
        }

        public async Task<bool> CanAddOpinion(string reviewerId, string evaluatedUserId, int offerId, string offerType)
        {
            var result = await _opinionsRepository.CanAddOpinion(reviewerId, evaluatedUserId, offerId, offerType);

            return result.Entity;
        }

        public async Task<Opinion> AddOpinionAsync(Opinion opinion)
        {
            var result = await _opinionsRepository.AddOpinionAsync(opinion);

            if (result.Status == RepositoryStatus.Created)
            {
                return result.Entity;
            }

            return null;
        }
    }
}