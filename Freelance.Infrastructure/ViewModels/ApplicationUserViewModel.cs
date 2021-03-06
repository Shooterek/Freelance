﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Infrastructure.ViewModels.Announcements;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Freelance.Infrastructure.ViewModels
{
    public class ApplicationUserViewModel : IdentityUser
    {
        public int? PhotoId { get; set; }
        public Photo Photo { get; set; }

        public int ReceivedOpinionsCount { get; set; }
        public double ReceivedOpinionsAverage => ReceivedOpinions.Average(o => o.Rating);
        
        public ICollection<Opinion> ReceivedOpinions { get; set; }

        public ApplicationUserViewModel()
        {
            ReceivedOpinions = new List<Opinion>();
        }
    }
}
