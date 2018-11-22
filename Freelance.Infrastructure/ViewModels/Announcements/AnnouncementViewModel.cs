using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels.Announcements
{
    public class AnnouncementViewModel
    {
        public AnnouncementViewModel()
        {
            
        }

        public int AnnouncementId { get; set; }

        public string AdvertiserId { get; set; }
        public  ApplicationUserViewModel Advertiser { get; set; }

        [Required(ErrorMessage = "{0} jest wymagany")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "Ilość znaków powinna wynosić od {2} do {1}")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Dostępność")]
        public Availability Availability { get; set; }

        [Required]
        [Display(Name = "Oczekiwana stawka godzinowa")]
        public decimal ExpectedHourlyWage { get; set; }

        [Display(Name = "Lokalizacja")]
        public string Localization { get; set; }

        [Required]
        [Display(Name = "Kategoria")]
        public int ServiceTypeId { get; set; }

        public ServiceType ServiceType { get; set; }
        public ICollection<AnnouncementOfferViewModel> Offers { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
