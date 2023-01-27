using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace QuartzConsoleAppTrail.Jobs
{
    public class Job2 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();

            });

            var logger = loggerFactory.CreateLogger<Job2>();
            logger.LogInformation($"Job 2: Running every day at 12:15 PM");
            //Console.WriteLine("Job 2: Running every day at 12:15 PM");
            return Task.CompletedTask;
        }
    }
}
