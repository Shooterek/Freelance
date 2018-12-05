using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Freelance.Core.Models;
using Freelance.Infrastructure;
using Freelance.ScheduledJobs;
using Freelance.ScheduledJobs.Schedulers;

namespace Freelance
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new RequireHttpsAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.DefaultBinder = new AvailabilityModelBinder();
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "ValidationResources";
            DefaultModelBinder.ResourceClassKey = "ValidationResources";
            Task.Run(JobScheduler.Start);
        }
    }
}
