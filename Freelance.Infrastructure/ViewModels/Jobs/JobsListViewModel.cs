using System.Collections.Generic;
using Freelance.Core.Models;
using Freelance.Infrastructure.Utils;

namespace Freelance.Infrastructure.ViewModels.Jobs
{
    public class JobsListViewModel
    {
        public ICollection<JobViewModel> Jobs { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public JobFilter Filter { get; set; }
    }
}
