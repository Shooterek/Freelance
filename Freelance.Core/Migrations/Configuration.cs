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
//            if (context.Users.Any(u => u.UserName == "example@email.com"))
//            {
//                //Step 1 Create the user.
//                var passwordHasher = new PasswordHasher();
//                var user = new ApplicationUser
//                {
//                    UserName = "example@email.com",
//                    Email = "example@email.com",
//                    PasswordHash = passwordHasher.HashPassword("!23Qwe"),
//                    SecurityStamp = Guid.NewGuid().ToString()
//                };
//
//                //Step 2 Create and add the new Role.
//                var roleToChoose = new IdentityRole("Admin");
//                context.Roles.Add(roleToChoose);
//
//                //Step 3 Create a role for a user
//                var role = new IdentityUserRole
//                {
//                    RoleId = roleToChoose.Id,
//                    UserId = user.Id
//                };
//
//                //Step 4 Add the role row and add the user to DB)
//                user.Roles.Add(role);
//                context.Users.Add(user);
//            }
        }
    }
}
