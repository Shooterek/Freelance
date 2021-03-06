﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.ViewModels.Jobs
{
    public class JobViewModel
    {
        public JobViewModel()
        {
            Offers = new List<JobOfferViewModel>();
            Photos = new List<Photo>();
            PublicationDate = DateTime.Now;
            LastActivation = DateTime.Now;
        }

        public int JobId { get; set; }
        
        public string EmployerId { get; set; }
        public ApplicationUserViewModel Employer { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Display(Name = "Kategoria")]
        public int ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [StringLength(128, MinimumLength = 6, ErrorMessage = "Ilość znaków powinna wynosić od {2} do {1}")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Dostępność")]
        public Availability Availability { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Data publikacji")]
        public DateTime PublicationDate { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Range(1, Int32.MaxValue)]
        [Display(Name = "Stawka minimalna [zł/h]")]
        public int MinimumWage { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane")]
        [Range(1, Int32.MaxValue)]
        [Display(Name = "Stawka maksymalna [zł/h]")]
        public int MaximumWage { get; set; }

        [Display(Name = "Lokalizacja")]
        [StringLength(32, ErrorMessage = "Maksymalna długość to {1}")]
        public string Localization { get; set; }

        public DateTime LastActivation { get; set; }

        public bool WasNotified { get; set; }

        public virtual ICollection<JobOfferViewModel> Offers { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
