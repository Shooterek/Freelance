using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels
{
    public class AnnouncementViewModel
    {
        public AnnouncementViewModel()
        {
            
        }

        public int AnnouncementId { get; set; }

        public string AdvertiserId { get; set; }
        public  ApplicationUser Advertiser { get; set; }

        [Required]
        [StringLength(25)]
        [MinLength(6)]
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
        public ICollection<AnnouncementOffer> Offers { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
