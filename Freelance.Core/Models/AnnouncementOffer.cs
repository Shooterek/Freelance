﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Core.Models
{
    public class AnnouncementOffer
    {
        public int AnnouncementOfferId { get; set; }

        [Required]
        public int AnnouncementId    { get; set; }
        public Announcement Announcement { get; set; }

        public DateTime SubmissionDate { get; set; }

        [Required]
        public string OffererId { get; set; }
        public ApplicationUser Offerer { get; set; }

        [Required]
        [Range(0.01, Double.MaxValue)]
        public decimal ProposedRate { get; set; }
        
        public bool IsAccepted { get; set; }
        public bool IsFinished { get; set; }
    }
}