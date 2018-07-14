using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Freelance.Tests.Repositories
{
    [TestClass]
    public class ServiceTypesRepositoryTests
    {
        private Mock<ApplicationDbContext> _dbContextMock;
        private Mock<DbSet<ServiceType>> _announcementsDbSetMock;
        private int _initialAmount;
        private int _notExistingId;
        private int _existingId;

        #region Initialize
        [TestInitialize]
        public void Prepare()
        {
            _existingId = 1;

            var data = new List<ServiceType>
            {
                new ServiceType() {ServiceTypeId = _existingId, Name = "IT"},
                new ServiceType() {ServiceTypeId = 2, Name = "Gardening"},
                new ServiceType() {ServiceTypeId = 3, Name = "Architecture"}
            }.AsQueryable();

            _initialAmount = data.Count();
            _notExistingId = 300000;

            _announcementsDbSetMock = new Mock<DbSet<ServiceType>>();
            _announcementsDbSetMock.As<IDbAsyncEnumerable<ServiceType>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new DbAsyncEnumerator<ServiceType>(data.GetEnumerator()));

            _announcementsDbSetMock.As<IQueryable<ServiceType>>()
                .Setup(m => m.Provider)
                .Returns(new DbAsyncQueryProvider<ServiceType>(data.Provider));

            _announcementsDbSetMock.As<IQueryable<ServiceType>>().Setup(m => m.Expression).Returns(data.Expression);
            _announcementsDbSetMock.As<IQueryable<ServiceType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _announcementsDbSetMock.As<IQueryable<ServiceType>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            _dbContextMock = new Mock<ApplicationDbContext>();
            _dbContextMock.Setup(c => c.ServiceTypes).Returns(_announcementsDbSetMock.Object);
        }

        #endregion

        #region CleanUp
        [TestCleanup]
        public void CleanUp()
        {
            _dbContextMock = null;
            _announcementsDbSetMock = null;
            _initialAmount = 0;
            _notExistingId = 0;
            _existingId = 0;

        }
        #endregion

        [TestMethod]
        public async Task AddAsync_ShouldCallAddAndSaveChangesAsyncOnce()
        {
            var repository = new ServiceTypesRepository(_dbContextMock.Object);
            await repository.AddAsync(new ServiceType());

            _announcementsDbSetMock.Verify(m => m.Add(It.IsAny<ServiceType>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllAnnouncements()
        {
            var result = await new ServiceTypesRepository(_dbContextMock.Object).GetAllAsync();

            Assert.AreEqual(_initialAmount, result.Entity.Count);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnRepositoryStatusNotFound_WhenNotContainingEntityWithSpecifiedId()
        {
            var repository = new ServiceTypesRepository(_dbContextMock.Object);

            var result = await repository.GetByIdAsync(_notExistingId);

            Assert.AreEqual(RepositoryStatus.NotFound, result.Status);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnRepositoryStatusOk_WhenContainingEntityWithSpecifiedId()
        {
            var repository = new ServiceTypesRepository(_dbContextMock.Object);

            var result = await repository.GetByIdAsync(_existingId);

            Assert.AreEqual(RepositoryStatus.Ok, result.Status);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnEntityWithCorrectId_WhenContainingEntityWithSpecifiedId()
        {
            var repository = new ServiceTypesRepository(_dbContextMock.Object);

            var result = await repository.GetByIdAsync(_existingId);

            Assert.AreEqual(_existingId, result.Entity.ServiceTypeId);
        }

        [TestMethod]
        public async Task RemoveAsync_ShouldCallRemoveAndSaveChangesAsyncOnce_WhenContainingEntityWithSpecifiedId()
        {
            var repository = new ServiceTypesRepository(_dbContextMock.Object);

            await repository.RemoveAsync(_existingId);

            _announcementsDbSetMock.Verify(m => m.Remove(It.IsAny<ServiceType>()), Times.Once());
            _dbContextMock.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task RemoveAsync_ShouldReturnRepositoryStatusDeleted_WhenContainingEntityWithSpecifiedId()
        {
            var repository = new ServiceTypesRepository(_dbContextMock.Object);

            var result = await repository.RemoveAsync(_existingId);

            Assert.AreEqual(RepositoryStatus.Deleted, result.Status);
        }

        [TestMethod]
        public async Task RemoveAsync_ShouldReturnRepositoryStatusNotFound_WhenNotContainingEntityWithSpecifiedId()
        {
            var repository = new ServiceTypesRepository(_dbContextMock.Object);

            var result = await repository.RemoveAsync(_notExistingId);

            Assert.AreEqual(RepositoryStatus.NotFound, result.Status);
        }

        [TestMethod]
        public async Task RemoveAsync_ShouldNotCallRemoveAndSaveChangesAsync_WhenNotContainingEntityWithSpecifiedId()
        {
            var repository = new ServiceTypesRepository(_dbContextMock.Object);

            await repository.RemoveAsync(_notExistingId);

            _announcementsDbSetMock.Verify(m => m.Remove(It.IsAny<ServiceType>()), Times.Never);
            _dbContextMock.Verify(m => m.SaveChangesAsync(), Times.Never);
        }
    }
}
