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

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Range(1, Int32.MaxValue)]
        [Display(Name = "Stawka")]
        public int ProposedRate { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [StringLength(255, ErrorMessage = "Maksymalna długość to {2}")]
        [Display(Name = "Wiadomość")]
        public string Message { get; set; }

        public bool IsAccepted { get; set; }
        public bool IsFinished { get; set; }
        
        public ICollection<Opinion> Opinions { get; set; }
    }
}
