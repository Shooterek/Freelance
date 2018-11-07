using System;
using System.ComponentModel.DataAnnotations;

namespace Freelance.Core.Models
{
    [Serializable]
    [Flags]
    public enum Availability
    {
        [Display(Name = "Poniedziałek")]
        Monday = 1 << 0,
        [Display(Name = "Wtorek")]
        Tuesday = 1 << 1,
        [Display(Name = "Środa")]
        Wednesday = 1 << 2,
        [Display(Name = "Czwartek")]
        Thursday = 1 << 3,
        [Display(Name = "Piątek")]
        Friday = 1 << 4,
        [Display(Name = "Sobota")]
        Saturday = 1 << 5,
        [Display(Name = "Niedziela")]
        Sunday = 1 << 6
    }
}