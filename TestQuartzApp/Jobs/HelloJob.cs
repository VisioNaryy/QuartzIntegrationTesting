using Quartz;
using TestQuartzApp.Services;

namespace TestQuartzApp.Jobs;

public class HelloJob : IJob
{
    private readonly ISimpleService _simpleService;
    private int _counter;

    public int Counter => _counter;
    public HelloJob(ISimpleService simpleService)
    {
        _simpleService = simpleService;
        _counter = 0;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        _counter++;
        
        var number = await _simpleService.TestIntMethod(10);
        await _simpleService.TestVoidMethod();
        
        //await Console.Out.WriteLineAsync($"Greetings from HelloJob! Number = {number}");
    }
}