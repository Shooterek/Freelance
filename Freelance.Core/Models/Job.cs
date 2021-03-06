﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Freelance.Core.Models
{
    public class Job
    {
        public Job()
        {
            Offers = new List<JobOffer>();
            Photos = new List<Photo>();
            PublicationDate = DateTime.Now;
            LastActivation = DateTime.Now;
        }

        public int JobId { get; set; }
        
        [Required]
        public string EmployerId { get; set; }
        public ApplicationUser Employer { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }

        [Required]
        [StringLength(128)]
        [MinLength(6)]
        public string Title { get; set; }
        
        public Availability Availability { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime PublicationDate { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int MinimumWage { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int MaximumWage { get; set; }

        [StringLength(32)]
        public string Localization { get; set; }

        public DateTime LastActivation { get; set; }

        public bool WasNotified { get; set; }

        public virtual ICollection<JobOffer> Offers { get; set; }
        public virtual ICollection<Photo> Photos{ get; set; }
    }
}