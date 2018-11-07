﻿using System;
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
using Moq;
using NUnit.Framework;

namespace Freelance.Tests.Services
{
    [TestFixture]
    public class AnnouncementsServiceTests
    {
        private AnnouncementsService _announcementsService;
        private int _existingId;
        private int _notExistingId;
        private int _initialAmount;

        #region Initialize

        [SetUp]
        public void Initialize()
        {
            _existingId = 1;
            _notExistingId = 30000;

            var serviceTypesServiceMock = new Mock<IServiceTypesService>();

            var data = new List<Announcement>()
            {
                new Announcement() {AnnouncementId = _existingId, Title = "Announcement1", ServiceTypeId = 1, AdvertiserId = "User1", Availability = Availability.Monday, ExpectedHourlyWage = 10.0M},
                new Announcement() {AnnouncementId = 2, Title = "Announcement2", ServiceTypeId = 1, AdvertiserId = "User2", Availability = Availability.Tuesday, ExpectedHourlyWage = 20.0M},
                new Announcement() {AnnouncementId = 3, Title = "Announcement3", ServiceTypeId = 2, AdvertiserId = "User1", Availability = Availability.Wednesday, ExpectedHourlyWage = 30.0M}
            };
            _initialAmount = data.Count;

            var emailServiceMock = new Mock<IEmailService>();
            var announcementsRepositoryMock = new Mock<IAnnouncementsRepository>();
            announcementsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new RepositoryActionResult<ICollection<Announcement>>(data, RepositoryStatus.Ok));

            announcementsRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int x) =>
                {
                    var entity = data.FirstOrDefault(a => a.AnnouncementId == x);
                    var status = entity != null ? RepositoryStatus.Ok : RepositoryStatus.NotFound;

                    return new RepositoryActionResult<Announcement>(entity, status);
                });

            announcementsRepositoryMock.Setup(r => r.GetByServiceTypeAsync(It.IsAny<ServiceType>()))
                .ReturnsAsync((ServiceType x) =>
                {
                    var entities = data.Where(a => a.ServiceTypeId == x.ServiceTypeId).ToList();

                    return new RepositoryActionResult<ICollection<Announcement>>(entities, RepositoryStatus.Ok);
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

            _announcementsService = new AnnouncementsService(announcementsRepositoryMock.Object, emailServiceMock.Object, serviceTypesServiceMock.Object);
        }

        #endregion

        [Test]
        public async Task
            GetAnnouncements_ShouldReturnAllItems_WhenAmountParameterIsGreaterOrEqualToTotalAmountOfItems()
        {
            int amountOfItemsToGet = 5;
            int page = 1;

            var result = await _announcementsService.GetAnnouncementsAsync(page, amountOfItemsToGet, Decimal.Zero, Decimal.MaxValue, null, null, null);

            Assert.AreEqual(_initialAmount, result.Announcements.Count);
        }

        [Test]
        public async Task GetAnnouncements_ShouldntReturnMoreItemsThanSpecified()
        {
            int amountOfItemsToGet = 2;
            int page = 1;

            var result = await _announcementsService.GetAnnouncementsAsync(page, amountOfItemsToGet, Decimal.Zero, Decimal.MaxValue, null, null, null);

            Assert.AreEqual(amountOfItemsToGet, result.Announcements.Count);
        }

        [Test]
        public async Task GetAnnouncements_ShouldReturnCorrectPaginationInfo()
        {
            int amountOfItemsToGet = 2;
            int page = 2;

            var result = await _announcementsService.GetAnnouncementsAsync(page, amountOfItemsToGet, Decimal.Zero, Decimal.MaxValue, null, null, null);

            Assert.AreEqual(amountOfItemsToGet, result.PagingInfo.ItemsPerPage);
            Assert.AreEqual(page, result.PagingInfo.CurrentPage);
            Assert.AreEqual(_initialAmount, result.PagingInfo.TotalItems);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnItemWithSpecifiedId_IfContainsItemWithSpecifiedId()
        {
            var result = await _announcementsService.GetAnnouncementByIdAsync(_existingId);

            Assert.IsNotNull(result);
            Assert.AreEqual(_existingId, result.AnnouncementId);
        }

        [Test]
        public async Task GetByServiceType_ShouldReturnItemsFromServiceType()
        {
            int amountOfItemsToGet = 5;
            int page = 1;
            var result = await _announcementsService.GetAnnouncementsByServiceTypeAsync(new ServiceType() {ServiceTypeId = 1},
                page, amountOfItemsToGet);

            foreach (var announcement in result.Announcements)
            {
                Assert.AreEqual(1, announcement.ServiceTypeId);
            }
        }

        [Test]
        public async Task AddAsync_ShouldAddSpecifiedElement()
        {
            var result =
                await _announcementsService.AddAnnouncementAsync(new Announcement() {AnnouncementId = _notExistingId, Availability = Availability.Monday, ExpectedHourlyWage = 1230});

            var entity = await _announcementsService.GetAnnouncementByIdAsync(_notExistingId);
            var allEntities = await _announcementsService.GetAnnouncementsAsync(1, _initialAmount + 1, Decimal.Zero, Decimal.MaxValue, null, null, null);

            Assert.AreEqual(result, entity);
            Assert.AreEqual(_initialAmount + 1, allEntities.Announcements.Count);
        }
    }
}
