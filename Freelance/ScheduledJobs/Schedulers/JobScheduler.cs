﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Freelance.ScheduledJobs.Jobs;
using Quartz;
using Quartz.Impl;

namespace Freelance.ScheduledJobs.Schedulers
{
    public class JobScheduler
    {
        public static async Task Start()
        {
            var scheduler = DependencyResolver.Current.GetService<IScheduler>();
            await scheduler.Start();

            var job = JobBuilder.Create<ReminderJob>().Build();

            var trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                (s =>
                    s.WithIntervalInHours(24)
                        .OnEveryDay()
                        .StartingDailyAt(new TimeOfDay(2, 0))
                )
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}