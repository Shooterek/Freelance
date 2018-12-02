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
                    OffererId = user.Id, AnnouncementId = 1, Message = "Message", ProposedRate = 40M,
                    SubmissionDate = DateTime.Now
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
                    ProposedRate = 40M,
                    SubmissionDate = DateTime.Now
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
                    ProposedRate = 40M,
                    SubmissionDate = DateTime.Now
                });

                var acceptedOffer = await repository.AcceptOfferAsync(result.Entity);

                var finishedOffer = await repository.EndOfferAsync(acceptedOffer.Entity);

                Assert.AreEqual(true, finishedOffer.Entity.IsFinished);

                dbContextTransaction.Rollback();
            }
        }
    }
}