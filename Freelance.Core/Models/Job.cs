using System;
using System.Collections.Generic;

namespace Freelance.Core.Models
{
    public class Job
    {
        public Job()
        {
            Offers = new List<Offer>();
            PublicationDate = DateTime.Now;
        }

        public int JobId { get; set; }

        public string EmployerId { get; set; }
        public ApplicationUser Employer { get; set; }

        public int ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PublicationDate { get; set; }

        public decimal MinimumWage { get; set; }
        public decimal MaximumWage { get; set; }

        public ICollection<Offer> Offers { get; set; }
    }
}