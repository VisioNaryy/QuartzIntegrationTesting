using Moq;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using TestQuartzApp.Jobs;
using TestQuartzApp.Services;

namespace TestQuartzApp.Tests.Integrational;

public class QuartzSchedulerTest
{
    private MockRepository _mockRepository;
    private Mock<ISimpleService> _mockSimpleService;

    public QuartzSchedulerTest()
    {
        _mockRepository = new MockRepository(MockBehavior.Default);

        _mockSimpleService = _mockRepository.Create<ISimpleService>();
        _mockSimpleService.Setup(x => x.TestVoidMethod()).Returns(Task.CompletedTask);
        _mockSimpleService.Setup(x => x.TestIntMethod(It.IsAny<int>())).Returns(Task.FromResult(100));
    }
    [Fact]
    public async Task Should_Try_3_Times_And_Then_Give_Up_HelloJob()
    {
        var factory = new StdSchedulerFactory();
        var scheduler = await factory.GetScheduler();

        var helloJob = new HelloJob(_mockSimpleService.Object);
        
        var jobFactory = new Mock<IJobFactory>();
        jobFactory
            .Setup(
                jf => jf.NewJob(It.IsAny<TriggerFiredBundle>(), It.IsAny<IScheduler>()))
            .Returns(helloJob);

        var repeatCount = 2;
        
        ITrigger trigger = TriggerBuilder
            .Create()
            .StartNow()
            .WithSimpleSchedule(
                x =>
                {
                    x.WithIntervalInSeconds(1);
                    x.WithRepeatCount(repeatCount);
                })
            .WithIdentity("hellojob1", "group1")
            .Build();
 
        IJobDetail job = JobBuilder
            .Create<HelloJob>()
            .WithIdentity("myTrigger", "group1")
            .Build();
        
        scheduler.JobFactory = jobFactory.Object;
 
        await scheduler.ScheduleJob(job, trigger);
        await scheduler.Start();
        //await scheduler.ResumeAll();
        await Task.Delay(5000);

        Assert.Equal(repeatCount + 1, helloJob.Counter);
    }
}