using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Core.Models
{
    public class Announcement : IEntity
    {
        public int AnnouncementId { get; set; }

        public string AdvertiserId { get; set; }

        public virtual ApplicationUser Advertiser { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public decimal ExpectedHourlyWage { get; set; }

        public string Location { get; set; }

        public int ServiceTypeId { get; set; }

        public virtual ServiceType ServiceType { get; set; }
    }
}
