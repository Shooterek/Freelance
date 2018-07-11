using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Freelance.Core.Models;
using Freelance.Core.Repositories;
using Freelance.Infrastructure.Repositories;
using Ninject;
using Ninject.Web.Common;

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
            kernel.Bind<IAnnouncementRepository>().To<AnnouncementRepository>().InRequestScope();
            kernel.Bind<IServiceTypeRepository>().To<ServiceTypeRepository>().InRequestScope();

            kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
        }
    }
}