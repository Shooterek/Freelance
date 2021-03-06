﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels.Announcements
{
    public class AnnouncementViewModel
    {
        public AnnouncementViewModel()
        {
            Offers = new List<AnnouncementOfferViewModel>();
            Photos = new List<Photo>();
            PublicationDate = DateTime.Now;
            LastActivation = DateTime.Now;
        }

        public int AnnouncementId { get; set; }
        
        public string AdvertiserId { get; set; }
        public  ApplicationUserViewModel Advertiser { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [StringLength(128, MinimumLength = 6, ErrorMessage = "Ilość znaków powinna wynosić od {2} do {1}")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Dostępność")]
        public Availability Availability { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Display(Name = "Oczekiwana stawka [zł/h]")]
        [Range(1, Int32.MaxValue)]
        public int ExpectedHourlyWage { get; set; }

        [Display(Name = "Lokalizacja")]
        [StringLength(32, ErrorMessage = "Maksymalna długość to {1}")]
        public string Localization { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Display(Name = "Kategoria")]
        public int ServiceTypeId { get; set; }

        public DateTime PublicationDate { get; set; }

        public DateTime LastActivation { get; set; }

        public bool WasNotified { get; set; }

        public ServiceType ServiceType { get; set; }
        public ICollection<AnnouncementOfferViewModel> Offers { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
