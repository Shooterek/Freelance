using System.ComponentModel.DataAnnotations;

namespace Freelance.Core.Models
{
    public class Opinion
    {
        public int OpinionId { get; set; }

        public int? AnnouncementOfferId { get; set; }
        public virtual AnnouncementOffer AnnouncementOffer { get; set; }

        public int? JobOfferId { get; set; }
        public virtual JobOffer JobOffer { get; set; }

        public string EvaluatedUserId { get; set; }
        public virtual ApplicationUser EvaluatedUser{ get; set; }

        [Range(1, 5)]
        [Required(ErrorMessage = "{0} jest wymagana")]
        [Display(Name = "Ocena")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "{0} jest wymagany")]
        [StringLength(100, ErrorMessage = "Maksymalna ilośc znaków to {1}")]
        [Display(Name = "Opis")]
        public string Review { get; set; }
    }
}