using Freelance.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Freelance.Core.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Freelance.Core.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Freelance.Core.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                //Step 2 Create and add the new Role.
                var roleToChoose = new IdentityRole("Admin");
                context.Roles.Add(roleToChoose);
            }
        }
    }
}
