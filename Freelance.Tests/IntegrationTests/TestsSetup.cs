using System.Data.Entity;
using Freelance.Core.Migrations;
using Freelance.Core.Models;
using NUnit.Framework;

namespace Freelance.Tests.IntegrationTests
{
    [SetUpFixture]
    public class TestsSetup
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {

            var initializer = new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>();

            // set DB initialiser to execute migrations
            Database.SetInitializer(initializer);

            ApplicationDbContext context = new ApplicationDbContext();
            var user = context.Users.Add(new ApplicationUser()
                {Email = "example@email.com", UserName = "example@email.com"});

            context.SaveChanges();

            context.ServiceTypes.Add(new ServiceType() {Name = "Default"});
            context.SaveChanges();
            context.Announcements.Add(new Announcement()
                {Title = "123456", Description = "123", ServiceTypeId = 1, ExpectedHourlyWage = 10, AdvertiserId = user.Id});
            context.Jobs.Add(new Job() {Title = "123456", Description = "1234", ServiceTypeId = 1, MinimumWage = 10, MaximumWage = 20, EmployerId = user.Id});
            context.SaveChanges();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            context.Database.Delete();
        }
    }
}