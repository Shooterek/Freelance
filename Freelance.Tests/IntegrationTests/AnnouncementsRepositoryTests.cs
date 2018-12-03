using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Freelance.Core.Migrations;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Repositories;
using NUnit.Framework;

namespace Freelance.Tests.IntegrationTests
{
    [TestFixture]
    public class AnnouncementsRepositoryTests
    {
        [Test]
        public async Task AddAsync_ShouldAddOneEntity_WhenItIsValid()
        {
            var context = new ApplicationDbContext();
            var repository = new AnnouncementsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var initialAmount = context.Announcements.Count();
                var user = context.Users.First();
                var title = "Title1";
                var result = await repository.AddAsync(new Announcement() {Title = title, Description = "Description", ExpectedHourlyWage = 10M, ServiceTypeId = 1, AdvertiserId = user.Id});

                var announcements = await repository.GetAllAsync();

                Assert.AreEqual(initialAmount + 1, announcements.Entity.Count);
                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntityWithCorrectId()
        {
            var context = new ApplicationDbContext();
            var repository = new AnnouncementsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var announcement = await repository.GetByIdAsync(1);

                Assert.AreEqual(RepositoryStatus.Ok, announcement.Status);
                Assert.AreEqual(1, announcement.Entity.AnnouncementId);
                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task RemoveAsync_ShouldRemoveOneEntity()
        {
            var context = new ApplicationDbContext();
            var repository = new AnnouncementsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var initialAmount = context.Announcements.Count();

                var result = await repository.RemoveAsync(1);
                var currentAmount = context.Announcements.Count();

                Assert.AreEqual(RepositoryStatus.Deleted, result.Status);
                Assert.AreEqual(initialAmount - 1, currentAmount);

                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            var context = new ApplicationDbContext();
            var repository = new AnnouncementsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var announcement = context.Announcements.First(a => a.AnnouncementId == 1);
                var initialValue = announcement.ExpectedHourlyWage;

                announcement.ExpectedHourlyWage = announcement.ExpectedHourlyWage + 1;

                var result = await repository.UpdateAsync(announcement);
                var updated = context.Announcements.First(a => a.AnnouncementId == 1);

                Assert.AreEqual(RepositoryStatus.Updated, result.Status);
                Assert.AreEqual(updated.ExpectedHourlyWage, initialValue + 1);

                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task AddOfferAsync_ShouldAddNewOffer()
        {
            var context = new ApplicationDbContext();
            var repository = new AnnouncementsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var user = context.Users.First();
                var result = await repository.AddOfferAsync(new AnnouncementOffer()
                {
                    OffererId = user.Id, AnnouncementId = 1, Message = "Message", ProposedRate = 40M
                });

                var announcement = context.Announcements.First();
                
                Assert.AreEqual(1, announcement.Offers.Count);

                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task AcceptOfferAsync_ShouldSetOffersIsAcceptedToTrue()
        {
            var context = new ApplicationDbContext();
            var repository = new AnnouncementsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var user = context.Users.First();
                var result = await repository.AddOfferAsync(new AnnouncementOffer()
                {
                    OffererId = user.Id,
                    AnnouncementId = 1,
                    Message = "Message",
                    ProposedRate = 40M
                });

                await repository.AcceptOfferAsync(result.Entity);
                var announcementOffer = context.AnnouncementOffers.First();

                Assert.AreEqual(true, announcementOffer.IsAccepted);

                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task EndOfferAsync_ShouldSetOffersIsAcceptedToTrue()
        {
            var context = new ApplicationDbContext();
            var repository = new AnnouncementsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var user = context.Users.First();
                var result = await repository.AddOfferAsync(new AnnouncementOffer()
                {
                    OffererId = user.Id,
                    AnnouncementId = 1,
                    Message = "Message",
                    ProposedRate = 40M
                });

                var acceptedOffer = await repository.AcceptOfferAsync(result.Entity);
                var finishedOffer = await repository.EndOfferAsync(acceptedOffer.Entity);

                Assert.AreEqual(true, finishedOffer.Entity.IsFinished);

                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task GetOldAnnouncements_ShouldDeleteAllAnnouncementsOlderThan17Days()
        {
            var context = new ApplicationDbContext();
            var repository = new AnnouncementsRepository(context);

            var maxCorrect = DateTime.Now.Subtract(new TimeSpan(336, 0, 0));
            var maxTime = DateTime.Now.Subtract(new TimeSpan(408, 0, 0));

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var user = context.Users.First();
                var msg = "msgasdasd";
                for (int i = 0; i < 100; i++)
                {
                    context.Announcements.Add(new Announcement()
                    {
                        Title = "Title1",
                        Description = msg,
                        ServiceTypeId = 1,
                        ExpectedHourlyWage = 10M,
                        AdvertiserId = user.Id,
                        LastActivation = DateTime.Now.Subtract(new TimeSpan(i * 100, 0, 0))
                    });

                    context.Jobs.Add(new Job()
                    {
                        Title = "Title1",
                        Description = msg,
                        ServiceTypeId = 1,
                        MinimumWage = 10M,
                        MaximumWage = 20M,
                        EmployerId = user.Id,
                        LastActivation = DateTime.Now.Subtract(new TimeSpan(i * 100, 0, 0))
                    });
                }

                await context.SaveChangesAsync();

                await repository.GetOldAnnouncementsAsync();

                var olderThan17DaysAmount = context.Announcements.Count(a => a.LastActivation < maxTime);

                Assert.AreEqual(0, olderThan17DaysAmount);

                dbContextTransaction.Rollback();

            }
        }
    }
}