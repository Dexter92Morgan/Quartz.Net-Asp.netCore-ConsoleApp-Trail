using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using Quartz.Impl.AdoJobStore;
using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using QuartzConsoleAppTrail.Jobs;

namespace QuartzConsoleAppTrail
{
    public class Program
    {
        private static async Task Main(string[] args)
        {

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();

            });

            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Starting application");

            // Create a connection string to the database
            string connectionString = "Server=ES-LAPTOP-953\\MSSQLSERVER2017;Database=QuartzTrail;User Id=SA;Password=Sa123456;Trusted_Connection=True;TrustServerCertificate=True;";

            NameValueCollection props = new NameValueCollection
            {
                    { "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                    { "quartz.jobStore.dataSource", "default" },
                    { "quartz.jobStore.tablePrefix", "QRTZ_" },
                    { "quartz.dataSource.default.connectionString", connectionString },
                    { "quartz.dataSource.default.provider", "SqlServer" },
                    { "quartz.serializer.type", "binary" },
                    { "quartz.jobStore.performSchemaValidation", "false" }

            };

            // Create a new scheduler factory with the JobStoreTX configuration
            StdSchedulerFactory schedulerFactory = new StdSchedulerFactory(); //props need to add for Jobstore
            // Grab the Scheduler instance from the Factory
            var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            // and start it off
            await scheduler.Start();


            // Schedule Job 1 to run every 5 seconds
            IJobDetail job1 = JobBuilder.Create<Job1>().Build();
            ITrigger trigger1 = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()//system UTC time
                    .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
                    .Build();

            await scheduler.ScheduleJob(job1, trigger1);

            // Schedule Job 2 to run every day at 12:19 PM
            IJobDetail job2 = JobBuilder.Create<Job2>().Build();
            ITrigger trigger2 = TriggerBuilder.Create()
                .WithIdentity("trigger2", "group2")
                .WithCronSchedule("0 53 20 1/1 * ? *")
                .Build();

         
            await scheduler.ScheduleJob(job2, trigger2);


            // some sleep to show what's happening
            await Task.Delay(TimeSpan.FromSeconds(20));

            Console.ReadKey();
            // and last shut down the scheduler when you are ready to close your program
            await scheduler.Shutdown();

            logger.LogInformation("Shutdown Scheduler");
            Console.WriteLine("Press any key to close the application");
         

        }
    }
}