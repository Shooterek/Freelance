using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Core.Models
{
    public class AnnouncementOffer
    {
        public AnnouncementOffer()
        {
            Opinions = new List<Opinion>();
            SubmissionDate = DateTime.Now;
        }

        public int AnnouncementOfferId { get; set; }

        [Required]
        public int AnnouncementId    { get; set; }
        public Announcement Announcement { get; set; }

        public DateTime SubmissionDate { get; set; }

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
