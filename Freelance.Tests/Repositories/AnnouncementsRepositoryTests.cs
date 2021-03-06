﻿using System.Collections.Generic;
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
    public class AnnouncementRepositoryTests
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private Mock<DbSet<Announcement>> _announcementsDbSetMock;
        private int _initialAmount;
        private int _notExistingId;
        private int _existingId;

        #region Initialize
        [SetUp]
        public void Prepare()
        {
            _existingId = 1;

            var announcements = new List<Announcement>
            {
                new Announcement() {AnnouncementId = _existingId, Title = "Announcement1", AdvertiserId = "1"},
                new Announcement() {AnnouncementId = 2, Title = "Announcement2", AdvertiserId = "2"},
                new Announcement() {AnnouncementId = 3, Title = "Announcement3", AdvertiserId = "3"}
            };

            _initialAmount = announcements.Count;
            _dbContextMock = new Mock<ApplicationDbContext>();
            _announcementsDbSetMock = announcements.GetMockSet();
            _dbContextMock.Setup(c => c.Announcements).Returns(_announcementsDbSetMock.Object);
        }

        #endregion

        #region CleanUp
        [TearDown]
        public void CleanUp()
        {
            _dbContextMock = null;
            _announcementsDbSetMock = null;
            _initialAmount = 0;
            _notExistingId = 0;
            _existingId = 0;

        }
        #endregion

        [Test]
        public async Task AddAsync_ShouldCallAddAndSaveChangesAsyncOnce()
        {
            var repository = new AnnouncementsRepository(_dbContextMock.Object);
            await repository.AddAsync(new Announcement());

            _announcementsDbSetMock.Verify(m => m.Add(It.IsAny< Announcement>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllAnnouncements()
        {
            var result = await new AnnouncementsRepository(_dbContextMock.Object).GetAllAsync();

            Assert.AreEqual(_initialAmount, result.Entity.Count);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnRepositoryStatusNotFound_IfEntityWithSpecifiedIdDoesNotExist()
        {
            var repository = new AnnouncementsRepository(_dbContextMock.Object);

            var result = await repository.GetByIdAsync(_notExistingId);

            Assert.AreEqual(RepositoryStatus.NotFound, result.Status);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnRepositoryStatusOk_WhenContainingEntityWithSpecifiedId()
        {
            var repository = new AnnouncementsRepository(_dbContextMock.Object);

            var result = await repository.GetByIdAsync(_existingId);

            Assert.AreEqual(RepositoryStatus.Ok, result.Status);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnEntityWithCorrectId_WhenContainingEntityWithSpecifiedId()
        {
            var repository = new AnnouncementsRepository(_dbContextMock.Object);

            var result = await repository.GetByIdAsync(_existingId);

            Assert.AreEqual(_existingId, result.Entity.AnnouncementId);
        }

        [Test]
        public async Task RemoveAsync_ShouldCallRemoveAndSaveChangesAsyncOnce_WhenContainingEntityWithSpecifiedId()
        {
            var repository = new AnnouncementsRepository(_dbContextMock.Object);

            await repository.RemoveAsync(_existingId);

            _announcementsDbSetMock.Verify(m => m.Remove(It.IsAny<Announcement>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [Test]
        public async Task RemoveAsync_ShouldReturnRepositoryStatusDeleted_IfEntityWithSpecifiedIdExists()
        {
            var repository = new AnnouncementsRepository(_dbContextMock.Object);

            var result = await repository.RemoveAsync(_existingId);

            Assert.AreEqual(RepositoryStatus.Deleted, result.Status);
        }

        [Test]
        public async Task RemoveAsync_ShouldReturnRepositoryStatusNotFound_WhenNotContainingEntityWithSpecifiedId()
        {
            var repository = new AnnouncementsRepository(_dbContextMock.Object);

            var result = await repository.RemoveAsync(_notExistingId);

            Assert.AreEqual(RepositoryStatus.NotFound, result.Status);
        }

        [Test]
        public async Task RemoveAsync_ShouldNotCallRemoveAndSaveChangesAsync_WhenNotContainingEntityWithSpecifiedId()
        {
            var repository = new AnnouncementsRepository(_dbContextMock.Object);

            await repository.RemoveAsync(_notExistingId);

            _announcementsDbSetMock.Verify(m => m.Remove(It.IsAny<Announcement>()), Times.Never);
            _dbContextMock.Verify(m => m.SaveChangesAsync(), Times.Never);
        }

    }
}