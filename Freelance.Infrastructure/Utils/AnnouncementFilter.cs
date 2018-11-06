using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.Utils
{
    public class AnnouncementFilter
    {
        public string Title { get; set; }

        public Availability Availability { get; set; }

        public decimal MinWage { get; set; }

        public decimal MaxWage { get; set; }

        public string Localization { get; set; }
        
        public int ServiceTypeId { get; set; }
    }
}