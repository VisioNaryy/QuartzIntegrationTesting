using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using TestQuartzApp.Jobs;

namespace TestQuartzApp.Configuration;

public class QuartzConfiguration
{
    public static async Task ConfigureQuartz(IHost builder)
    {
        var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();
        
        //await scheduler.Start();

        // define the job and tie it to our HelloJob class
        var job = JobBuilder.Create<HelloJob>()
            .WithIdentity("hellojob1", "group1")
            .Build();

        // Trigger the job to run now, and then every 40 seconds
        var trigger = TriggerBuilder.Create()
            .WithIdentity("myTrigger", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(5)
                //.RepeatForever())
                .WithRepeatCount(3))
            .Build();

        await scheduler.ScheduleJob(job, trigger);
        
        // some sleep to show what's happening
        // await Task.Delay(TimeSpan.FromSeconds(20));
        //await scheduler.Shutdown();
        //Console.WriteLine("Press any key to close the application");
    }
}