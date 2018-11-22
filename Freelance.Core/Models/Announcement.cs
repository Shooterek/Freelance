using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Core.Models
{
    public class Announcement
    {
        public Announcement()
        {
            Offers = new List<AnnouncementOffer>();
            Photos = new List<Photo>();
            PublicationDate = DateTime.Now;
        }

        public int AnnouncementId { get; set; }

        public string AdvertiserId { get; set; }
        public virtual ApplicationUser Advertiser { get; set; }

        [Required]
        [StringLength(25)]
        [MinLength(6)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        
        public Availability Availability { get; set; }

        [Required]
        public decimal ExpectedHourlyWage { get; set; }

        [StringLength(31)]
        public string Localization { get; set; }
        public DateTime PublicationDate { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }

        public virtual ServiceType ServiceType { get; set; }
        public virtual ICollection<AnnouncementOffer> Offers { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
