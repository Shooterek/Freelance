namespace Freelance.Core.Models
{
    public class Offer
    {
        public int OfferId { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        public string OffererId { get; set; }
        public ApplicationUser Offerer { get; set; }

        public decimal ProposedRate { get; set; }
    }
}