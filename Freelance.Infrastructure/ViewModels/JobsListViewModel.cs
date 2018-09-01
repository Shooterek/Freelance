using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.Utils;

namespace Freelance.Infrastructure.ViewModels
{
    public class JobsListViewModel
    {
        public IList<Job> Jobs { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
