using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Core.Repositories
{
    public class RepositoryActionResult<T>
    {
        public T Entity { get; set; }
        public RepositoryStatus Status { get; set; }

        public RepositoryActionResult(T entity, RepositoryStatus status)
        {
            Entity = entity;
            Status = status;
        }
    }
}
