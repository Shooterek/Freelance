using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels.Jobs
{
    public class JobOfferViewModel
    {
        public int JobOfferId { get; set; }

        [Required]
        public int JobId { get; set; }
        public Job Job { get; set; }

        [Display(Name = "Data zgłoszenia")]
        public DateTime SubmissionDate { get; set; }

        [Required]
        public string OffererId { get; set; }
        public ApplicationUserViewModel Offerer { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Błedna wartość")]
        [Display(Name = "Zaproponowana stawka")]
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
