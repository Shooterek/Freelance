using System;

namespace Freelance.Core.Models
{
    public class JobOffer
    {
        public int JobOfferId { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        public DateTime SubmissionDate{ get; set; }

        public string OffererId { get; set; }
        public ApplicationUser Offerer { get; set; }

        public decimal ProposedRate { get; set; }
    }
}