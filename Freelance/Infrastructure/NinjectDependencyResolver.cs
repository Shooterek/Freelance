using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Repositories;
using Freelance.Infrastructure.Services.Implementations;
using Freelance.Infrastructure.Services.Interfaces;
using Freelance.Infrastructure.ViewModels;
using Freelance.ScheduledJobs;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;
using Quartz;
using Quartz.Impl;

namespace Freelance.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IScheduler>().ToMethod(x =>
            {
                IScheduler scheduler = null;
                var temp = new StdSchedulerFactory();
                var runSync = Task.Factory.StartNew(async () => { scheduler = await temp.GetScheduler(); }).Unwrap();
                runSync.Wait();

                scheduler.JobFactory = new NinjectJobFactory(kernel);
                return scheduler;
            });

            kernel.Bind<IAnnouncementsRepository>().To<AnnouncementsRepository>().InRequestScope();
            kernel.Bind<IServiceTypesRepository>().To<ServiceTypesRepository>().InRequestScope();
            kernel.Bind<IJobsRepository>().To<JobsRepository>().InRequestScope();

            kernel.Bind<IAnnouncementsService>().To<AnnouncementsService>().InRequestScope();
            kernel.Bind<IServiceTypesService>().To<ServiceTypesService>().InRequestScope();
            kernel.Bind<IJobsService>().To<JobsService>().InRequestScope();
            kernel.Bind<IEmailService>().To<Services.Implementations.EmailService>().InRequestScope();

            kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
            kernel.Bind<IMapper>().ToMethod(AutoMapper).InSingletonScope();

        }

        private IMapper AutoMapper(IContext context)
        {
            Mapper.Initialize(config =>
            {
                config.ConstructServicesUsing(type => kernel.Get(type));

                config.CreateMap<Announcement, AnnouncementViewModel>();
                config.CreateMap<AnnouncementOffer, AnnouncementOfferViewModel>();          

            });

            Mapper.AssertConfigurationIsValid();
            return Mapper.Instance;
        }
    }
}