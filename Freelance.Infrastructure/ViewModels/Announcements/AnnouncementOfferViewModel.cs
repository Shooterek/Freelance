using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels.Announcements
{
    public class AnnouncementOfferViewModel
    {
        public int AnnouncementOfferId { get; set; }

        [Required]
        public int AnnouncementId { get; set; }
        public AnnouncementViewModel Announcement { get; set; }

        [Display(Name = "Data zgłoszenia")]
        public DateTime SubmissionDate { get; set; }

        [Required]
        public string OffererId { get; set; }
        public ApplicationUserViewModel Offerer { get; set; }

        [Required(ErrorMessage = "{0} jest wymagana")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Błędna wartość")]
        [Display(Name = "Stawka")]
        public decimal ProposedRate { get; set; }

        [Required(ErrorMessage = "{0} jest wymagana")]
        [StringLength(255, ErrorMessage = "Zbyt długi tekst")]
        [Display(Name = "Wiadomość")]
        public string Message { get; set; }

        public bool IsAccepted { get; set; }
        public bool IsFinished { get; set; }
        
        public ICollection<Opinion> Opinions { get; set; }
    }
}
