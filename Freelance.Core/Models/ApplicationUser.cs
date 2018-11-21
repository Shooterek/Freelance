using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Freelance.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? PhotoId { get; set; }
        public Photo Photo { get; set; }

        public virtual ICollection<Announcement> PublishedAnnouncements { get; set; }
        public virtual ICollection<Job> PublishedJobs { get; set; }
        public virtual ICollection<JobOffer> ProposedOffers { get; set; }
        public virtual ICollection<Opinion> ReceivedOpinions { get; set; }

        public ApplicationUser()
        {
            PublishedAnnouncements = new List<Announcement>();
            PublishedJobs = new List<Job>();
            ProposedOffers = new List<JobOffer>();
            ReceivedOpinions = new List<Opinion>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}