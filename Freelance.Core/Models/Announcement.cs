﻿using System;
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
        }

        public int AnnouncementId { get; set; }

        public string AdvertiserId { get; set; }
        public virtual ApplicationUser Advertiser { get; set; }

        [Required]
        [StringLength(255)]
        [MinLength(30)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        
        public Availability Availability { get; set; }

        [Required]
        public decimal ExpectedHourlyWage { get; set; }

        public string Localization { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }

        public virtual ServiceType ServiceType { get; set; }
        public virtual ICollection <AnnouncementOffer> Offers { get; set; }
    }
}
