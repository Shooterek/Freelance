using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Freelance.Core.Models;

namespace Freelance.Infrastructure.Utils
{
    public class JobFilter
    {
        public string Title { get; set; }

        [Display(Name = "Dostępność")]
        public Availability Availability { get; set; }

        [Display(Name = "Stawka minimalna")]
        public int? MinWage { get; set; }


        [Display(Name = "Stawka maksymalna")]
        public int? MaxWage { get; set; }

        [Display(Name = "Lokalizacja")]
        public string Localization { get; set; }

        [Display(Name = "Kategoria")]
        public int? ServiceTypeId { get; set; }

        public string SortOrder { get; set; }

        public List<SelectListItem> ServiceTypes { get; set; }
    }
}