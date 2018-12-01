using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Services.Implementations;
using Freelance.Infrastructure.Services.Interfaces;
using Freelance.Infrastructure.ViewModels;
using Freelance.Infrastructure.ViewModels.Jobs;
using Freelance.Utilities;
using Moq;
using Ninject.Activation;
using NUnit.Framework;

namespace Freelance.Tests.Services
{
    [TestFixture]
    public class JobsServiceTests
    {
        private JobsService _jobsService;
        private IMapper _mapper;
        private int _existingId;
        private int _notExistingId;
        private int _initialAmount;

        #region Initialize

        [SetUp]
        public void Initialize()
        {
            _mapper = AutoMapperFactory.AutoMapper(new Mock<IContext>().Object);
            _existingId = 1;
            _notExistingId = 30000;

            var serviceTypesServiceMock = new Mock<IServiceTypesService>();

            var data = new List<Job>()
            {
                new Job() {JobId = _existingId, Title = "Job1", ServiceTypeId = 1, EmployerId = "User1", Availability = Availability.Monday, MinimumWage = 10.0M},
                new Job() {JobId = 2, Title = "Job2", ServiceTypeId = 1, EmployerId = "User2", Availability = Availability.Tuesday, MinimumWage = 20.0M},
                new Job() {JobId = 3, Title = "Job3", ServiceTypeId = 2, EmployerId = "User1", Availability = Availability.Wednesday, MinimumWage = 30.0M}
            };
            _initialAmount = data.Count;

            var emailServiceMock = new Mock<IEmailService>();
            var jobsRepositoryMock = new Mock<IJobsRepository>();
            jobsRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new RepositoryActionResult<ICollection<Job>>(data, RepositoryStatus.Ok));

            jobsRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int x) =>
                {
                    var entity = data.FirstOrDefault(a => a.JobId == x);
                    var status = entity != null ? RepositoryStatus.Ok : RepositoryStatus.NotFound;

                    return new RepositoryActionResult<Job>(entity, status);
                });

            jobsRepositoryMock.Setup(r => r.AddAsync(It.IsNotNull<Job>()))
                .Callback((Job entity) => data.Add(entity))
                .ReturnsAsync((Job entity) => new RepositoryActionResult<Job>(entity, RepositoryStatus.Created));

            jobsRepositoryMock.Setup(r => r.RemoveAsync(It.IsAny<int>()))
                .Callback((int id) =>
                {
                    var entity = data.FirstOrDefault(a => a.JobId == id);
                    if (entity != null) data.Remove(entity);
                });

            jobsRepositoryMock.Setup(r => r.UpdateAsync(It.IsNotNull<Job>()))
                .Callback((Job entity) =>
                {
                    var entityToDelete = data.FirstOrDefault(a => a.JobId == entity.JobId);
                    if (entityToDelete != null)
                    {
                        data.Remove(entityToDelete);
                        data.Add(entity);
                    }
                });

            _jobsService = new JobsService(jobsRepositoryMock.Object, serviceTypesServiceMock.Object, _mapper);
        }

        #endregion

        #region Teardown

        [TearDown]
        public void TearDown()
        {
            Mapper.Reset();
        }

        #endregion

        [Test]
        public async Task
            GetJobs_ShouldReturnAllItems_WhenAmountParameterIsGreaterOrEqualToTotalAmountOfItems()
        {
            int amountOfItemsToGet = 5;
            int page = 1;

            var result = await _jobsService.GetJobsAsync(page, amountOfItemsToGet, Decimal.Zero, Decimal.MaxValue, null, null, null, null);

            Assert.AreEqual(_initialAmount, result.Jobs.Count);
        }

        [Test]
        public async Task GetJobs_ShouldntReturnMoreItemsThanSpecified()
        {
            int amountOfItemsToGet = 2;
            int page = 1;

            var result = await _jobsService.GetJobsAsync(page, amountOfItemsToGet, Decimal.Zero, Decimal.MaxValue, null, null, null, null);

            Assert.AreEqual(amountOfItemsToGet, result.Jobs.Count);
        }

        [Test]
        public async Task GetJobs_ShouldReturnCorrectPaginationInfo()
        {
            int amountOfItemsToGet = 2;
            int page = 2;

            var result = await _jobsService.GetJobsAsync(page, amountOfItemsToGet, Decimal.Zero, Decimal.MaxValue, null, null, null, null);

            Assert.AreEqual(amountOfItemsToGet, result.PagingInfo.ItemsPerPage);
            Assert.AreEqual(page, result.PagingInfo.CurrentPage);
            Assert.AreEqual(_initialAmount, result.PagingInfo.TotalItems);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnItemWithSpecifiedId_IfContainsItemWithSpecifiedId()
        {
            var result = await _jobsService.GetJobByIdAsync(_existingId);

            Assert.IsNotNull(result);
            Assert.AreEqual(_existingId, result.JobId);
        }

        [Test]
        public async Task AddAsync_ShouldAddSpecifiedElement()
        {
            var result =
                await _jobsService.AddJobAsync(new JobViewModel() { JobId = _notExistingId, Availability = Availability.Monday, MinimumWage = 1230 });

            var entity = await _jobsService.GetJobByIdAsync(_notExistingId);
            var allEntities = await _jobsService.GetJobsAsync(1, _initialAmount + 1, Decimal.Zero, Decimal.MaxValue, null, null, null, null);

            Assert.AreEqual(result.JobId, entity.JobId);
            Assert.AreEqual(_initialAmount + 1, allEntities.Jobs.Count);
        }
    }
}
