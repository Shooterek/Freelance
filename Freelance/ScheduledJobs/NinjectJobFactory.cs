using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace Freelance.ScheduledJobs
{
    public class NinjectJobFactory : SimpleJobFactory
    {
        readonly IKernel _kernel;

        public NinjectJobFactory(IKernel kernel)
        {
            this._kernel = kernel;
        }
    }
}