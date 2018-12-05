using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Core.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        [StringLength(32)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public int? AnnouncementId { get; set; }
        public int? JobId { get; set; }
    }
}
