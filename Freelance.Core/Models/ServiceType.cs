using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Core.Models
{
    public class ServiceType : IEntity
    {
        public int ServiceTypeId { get; set; }
        public string Name { get; set; }
    }
}
