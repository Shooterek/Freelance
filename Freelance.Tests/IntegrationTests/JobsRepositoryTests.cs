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
    public class JobsRepositoryTests
    {
        [Test]
        public async Task AddAsync_ShouldAddOneEntity_WhenItIsValid()
        {
            var context = new ApplicationDbContext();
            var repository = new JobsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var initialAmount = context.Jobs.Count();
                var user = context.Users.First();
                var title = "Title1";
                var result = await repository.AddAsync(new Job()
                {
                    Title = title, Description = "Description", MinimumWage = 10, MaximumWage = 30, ServiceTypeId = 1,
                    EmployerId = user.Id
                });

                var jobs = await repository.GetAllAsync();

                Assert.AreEqual(initialAmount + 1, jobs.Entity.Count);
                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntityWithCorrectId()
        {
            var context = new ApplicationDbContext();
            var repository = new JobsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var job = await repository.GetByIdAsync(1);

                Assert.AreEqual(RepositoryStatus.Ok, job.Status);
                Assert.AreEqual(1, job.Entity.JobId);
                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task RemoveAsync_ShouldRemoveOneEntity()
        {
            var context = new ApplicationDbContext();
            var repository = new JobsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var initialAmount = context.Jobs.Count();
                var result = await repository.RemoveAsync(1);
                var currentAmount = context.Jobs.Count();

                Assert.AreEqual(RepositoryStatus.Deleted, result.Status);
                Assert.AreEqual(initialAmount - 1, currentAmount);

                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            var context = new ApplicationDbContext();
            var repository = new JobsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var job = context.Jobs.First(a => a.JobId == 1);
                var initialValue = job.MinimumWage;

                job.MinimumWage = job.MinimumWage + 1;

                var result = await repository.UpdateAsync(job);
                var updated = context.Jobs.First(a => a.JobId == 1);

                Assert.AreEqual(RepositoryStatus.Updated, result.Status);
                Assert.AreEqual(updated.MinimumWage, initialValue + 1);

                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task AddOfferAsync_ShouldAddNewOffer()
        {
            var context = new ApplicationDbContext();
            var repository = new JobsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var user = context.Users.First();
                var result = await repository.AddOfferAsync(new JobOffer()
                {
                    OffererId = user.Id,
                    JobId = 1,
                    Message = "Message",
                    ProposedRate = 40,
                    SubmissionDate = DateTime.Now
                });

                var job = context.Jobs.First();

                Assert.AreEqual(1, job.Offers.Count);

                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task AcceptOfferAsync_ShouldSetOffersIsAcceptedToTrue()
        {
            var context = new ApplicationDbContext();
            var repository = new JobsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var user = context.Users.First();
                var result = await repository.AddOfferAsync(new JobOffer()
                {
                    OffererId = user.Id,
                    JobId = 1,
                    Message = "Message",
                    ProposedRate = 40,
                    SubmissionDate = DateTime.Now
                });

                await repository.AcceptOfferAsync(result.Entity);

                var jobOffer = context.JobOffers.First();

                Assert.AreEqual(true, jobOffer.IsAccepted);

                dbContextTransaction.Rollback();
            }
        }

        [Test]
        public async Task EndOfferAsync_ShouldSetOffersIsAcceptedToTrue()
        {
            var context = new ApplicationDbContext();
            var repository = new JobsRepository(context);
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                var user = context.Users.First();
                var result = await repository.AddOfferAsync(new JobOffer()
                {
                    OffererId = user.Id,
                    JobId = 1,
                    Message = "Message",
                    ProposedRate = 40,
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