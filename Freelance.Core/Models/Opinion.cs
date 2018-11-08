using System.ComponentModel.DataAnnotations;

namespace Freelance.Core.Models
{
    public class Opinion
    {
        public int OpinionId { get; set; }

        public string EvaluatedUserId { get; set; }
        public virtual ApplicationUser EvaluatedUser{ get; set; }

        [Range(1, 5)]
        [Required]
        public int Rating { get; set; }

        [Required]
        [StringLength(100)]
        public string Review { get; set; }
    }
}