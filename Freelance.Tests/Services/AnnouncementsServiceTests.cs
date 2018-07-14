using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Services.Implementations;
using Freelance.Infrastructure.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Freelance.Tests.Services
{
    [TestClass]
    public class AnnouncementsServiceTests
    {
        private AnnouncementsService _announcementsService;
        private int _existingId;
        private int _notExistingId;
        private int _initialAmount;

        #region Initialize

        [TestInitialize]
        public void Initialize()
        {
            _existingId = 1;
            _notExistingId = 30000;

            var data = new List<Announcement>()
            {
                new Announcement() {AnnouncementId = _existingId, Summary = "Announcement1"},
                new Announcement() {AnnouncementId = 2, Summary = "Announcement2"},
                new Announcement() {AnnouncementId = 3, Summary = "Announcement3"}
            };

            _initialAmount = data.Count;

            var announcementsRepositoryMock = new Mock<IAnnouncementsRepository>();
            announcementsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new RepositoryActionResult<ICollection<Announcement>>(data, RepositoryStatus.Ok));

            announcementsRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int x) =>
                {
                    var entity = data.FirstOrDefault(a => a.AnnouncementId == x);
                    var status = entity != null ? RepositoryStatus.Ok : RepositoryStatus.NotFound;

                    return new RepositoryActionResult<Announcement>(entity, status);
                });

            announcementsRepositoryMock.Setup(r => r.AddAsync(It.IsNotNull<Announcement>()))
                .Callback((Announcement entity) => data.Add(entity))
                .ReturnsAsync((Announcement entity) => new RepositoryActionResult<Announcement>(entity, RepositoryStatus.Created));

            announcementsRepositoryMock.Setup(r => r.RemoveAsync(It.IsAny<int>()))
                .Callback((int id) =>
                {
                    var entity = data.FirstOrDefault(a => a.AnnouncementId == id);
                    if (entity != null) data.Remove(entity);
                });

            announcementsRepositoryMock.Setup(r => r.UpdateAsync(It.IsNotNull<Announcement>()))
                .Callback((Announcement entity) =>
                {
                    var entityToDelete = data.FirstOrDefault(a => a.AnnouncementId == entity.AnnouncementId);
                    if (entityToDelete != null)
                    {
                        data.Remove(entityToDelete);
                        data.Add(entity);
                    } 
                });

            _announcementsService = new AnnouncementsService(announcementsRepositoryMock.Object);
        }

        #endregion

        [TestMethod]
        public async Task
            GetAnnouncements_ShouldReturnAllItems_WhenAmountParameterIsGreaterOrEqualToTotalAmountOfItems()
        {
            int amountOfItemsToGet = 5;
            int page = 1;

            var result = await _announcementsService.GetAnnouncementsAsync(page, amountOfItemsToGet);

            Assert.AreEqual(_initialAmount, result.Announcements.Count);
        }

        [TestMethod]
        public async Task GetAnnouncements_ShouldntReturnMoreItemsThanSpecified()
        {
            int amountOfItemsToGet = 2;
            int page = 1;

            var result = await _announcementsService.GetAnnouncementsAsync(page, amountOfItemsToGet);

            Assert.AreEqual(amountOfItemsToGet, result.Announcements.Count);
        }

        [TestMethod]
        public async Task GetAnnouncements_ShouldReturnCorrectPaginationInfo()
        {
            int amountOfItemsToGet = 2;
            int page = 2;

            var result = await _announcementsService.GetAnnouncementsAsync(page, amountOfItemsToGet);

            Assert.AreEqual(amountOfItemsToGet, result.PagingInfo.ItemsPerPage);
            Assert.AreEqual(page, result.PagingInfo.CurrentPage);
            Assert.AreEqual(_initialAmount, result.PagingInfo.TotalItems);
        }
    }
}
