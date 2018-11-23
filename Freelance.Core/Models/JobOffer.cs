using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Freelance.Core.Models
{
    public class JobOffer
    {
        public JobOffer()
        {
            Opinions = new List<Opinion>();
        }

        public int JobOfferId { get; set; }

        [Required]
        public int JobId { get; set; }
        public Job Job { get; set; }

        public DateTime SubmissionDate{ get; set; }

        [Required]
        public string OffererId { get; set; }
        public ApplicationUser Offerer { get; set; }

        [Required]
        [Range(0.01, Double.MaxValue)]
        public decimal ProposedRate { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Message { get; set; }

        public bool IsAccepted { get; set; }

        public bool IsFinished { get; set; }
        public virtual ICollection<Opinion> Opinions { get; set; }
    }
}