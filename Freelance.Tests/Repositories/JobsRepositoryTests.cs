using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Repositories;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Freelance.Tests.Repositories
{
    [TestFixture]
    public class JobsRepositoryTests
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private Mock<DbSet<Job>> _jobsDbSetMock;
        private int _initialAmount;
        private int _notExistingId;
        private int _existingId;

        #region Initialize
        [SetUp]
        public void Prepare()
        {
            _existingId = 1;
            _notExistingId = 30000;

            var jobs = new List<Job>
            {
                new Job() {JobId = _existingId, Title = "Job1", EmployerId = "1"},
                new Job() {JobId = 2, Title = "Job2", EmployerId = "2"},
                new Job() {JobId = 3, Title = "Job3", EmployerId = "3"}
            };

            _initialAmount = jobs.Count;
            _dbContextMock = new Mock<ApplicationDbContext>();
            _jobsDbSetMock = jobs.GetMockSet();
            _dbContextMock.Setup(c => c.Jobs).Returns(_jobsDbSetMock.Object);
        }

        #endregion

        #region CleanUp
        [TearDown]
        public void CleanUp()
        {
            _dbContextMock = null;
            _jobsDbSetMock = null;
            _initialAmount = 0;
            _notExistingId = 0;
            _existingId = 0;

        }
        #endregion

        [Test]
        public async Task AddAsync_ShouldCallAddAndSaveChangesAsyncOnce()
        {
            var repository = new JobsRepository(_dbContextMock.Object);
            await repository.AddAsync(new Job());

            _jobsDbSetMock.Verify(m => m.Add(It.IsAny<Job>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllJobs()
        {
            var result = await new JobsRepository(_dbContextMock.Object).GetAllAsync();

            Assert.AreEqual(_initialAmount, result.Entity.Count);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnRepositoryStatusNotFound_WhenNotContainingEntityWithSpecifiedId()
        {
            var repository = new JobsRepository(_dbContextMock.Object);

            var result = await repository.GetByIdAsync(_notExistingId);

            Assert.AreEqual(RepositoryStatus.NotFound, result.Status);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnRepositoryStatusOk_WhenContainingEntityWithSpecifiedId()
        {
            var repository = new JobsRepository(_dbContextMock.Object);

            var result = await repository.GetByIdAsync(_existingId);

            Assert.AreEqual(RepositoryStatus.Ok, result.Status);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntityWithCorrectId_WhenContainingEntityWithSpecifiedId()
        {
            var repository = new JobsRepository(_dbContextMock.Object);

            var result = await repository.GetByIdAsync(_existingId);

            Assert.AreEqual(_existingId, result.Entity.JobId);
        }

        [Test]
        public async Task RemoveAsync_ShouldCallRemoveAndSaveChangesAsyncOnce_WhenContainingEntityWithSpecifiedId()
        {
            var repository = new JobsRepository(_dbContextMock.Object);

            await repository.RemoveAsync(_existingId);

            _jobsDbSetMock.Verify(m => m.Remove(It.IsAny<Job>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [Test]
        public async Task RemoveAsync_ShouldReturnRepositoryStatusDeleted_WhenContainingEntityWithSpecifiedId()
        {
            var repository = new JobsRepository(_dbContextMock.Object);

            var result = await repository.RemoveAsync(_existingId);

            Assert.AreEqual(RepositoryStatus.Deleted, result.Status);
        }

        [Test]
        public async Task RemoveAsync_ShouldReturnRepositoryStatusNotFound_WhenNotContainingEntityWithSpecifiedId()
        {
            var repository = new JobsRepository(_dbContextMock.Object);

            var result = await repository.RemoveAsync(_notExistingId);

            Assert.AreEqual(RepositoryStatus.NotFound, result.Status);
        }

        [Test]
        public async Task RemoveAsync_ShouldNotCallRemoveAndSaveChangesAsync_WhenNotContainingEntityWithSpecifiedId()
        {
            var repository = new JobsRepository(_dbContextMock.Object);

            await repository.RemoveAsync(_notExistingId);

            _jobsDbSetMock.Verify(m => m.Remove(It.IsAny<Job>()), Times.Never);
            _dbContextMock.Verify(m => m.SaveChangesAsync(), Times.Never);
        }

    }
}