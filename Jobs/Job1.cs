using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;

namespace QuartzConsoleAppTrail.Jobs
{
    public class Job1 : IJob
    {

        public Task Execute(IJobExecutionContext context)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();

            });

            var logger = loggerFactory.CreateLogger<Job1>();
            logger.LogInformation($"Job 1: Running every 5 seconds " + DateTime.Now.ToLongTimeString());
            //Console.WriteLine("Job 1: Running every 5 seconds " + DateTime.Now.ToLongTimeString());
            return Task.CompletedTask;
        }
    }
}
