using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Core.Models
{
    public class ServiceType
    {
        public int ServiceTypeId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }
    }
}
