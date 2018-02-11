using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Quartz_POC.Service
{
    public static class Agendador
    {
        public static async Task Start()
        {
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }

                };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            if (scheduler.IsShutdown || scheduler.InStandbyMode)
                await scheduler.Start();
        }

        public static async Task Stop()
        {
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            if (scheduler.IsStarted)
                await scheduler.Shutdown ();
        }

        public static async Task RunJob(string text)
        {
            try
            {
                // Grab the Scheduler instance from the Factory
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();

                // and start it off
                if (scheduler.IsShutdown || scheduler.InStandbyMode)
                    await scheduler.Start();

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity("job1", "group1")
                    .UsingJobData("param", text )
                    .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(10)
                        .RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                await scheduler.ScheduleJob(job, trigger);

                // some sleep to show what's happening
                //await Task.Delay(TimeSpan.FromSeconds(120));

                // and last shut down the scheduler when you are ready to close your program
                //await scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {
                Console.WriteLine(se);
            }
        }

        public class HelloJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                JobKey key = context.JobDetail.Key;

                JobDataMap dataMap = context.JobDetail.JobDataMap;

                string text = dataMap.GetString("param");

                WriteText(text);
                await Console.Out.WriteLineAsync("Greetings from HelloJob!");
            }
        }

        public static void WriteText(string text)
        {
            try
            {
                string path = @"c:\teste";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
               

                //string text = string.Format("Data: {0}", DateTime.Now);
                //System.IO.File.WriteAllText(string.Format(@"C:\Users\Public\Documents\{0}.txt", DateTime.Now.ToString("yyyyMMddTHHmmsszzz")), text);
                System.IO.File.WriteAllText(@"C:\teste\"+ System.Guid.NewGuid().ToString() + ".txt", text);
            }
            catch (Exception e)
            {

                throw;
            }
          
        }
   
    }
}