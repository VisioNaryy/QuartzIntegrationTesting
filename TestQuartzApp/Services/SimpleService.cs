namespace TestQuartzApp.Services;

public class SimpleService : ISimpleService
{
    public async Task TestVoidMethod()
    {
        await Task.Delay(300);
        Console.WriteLine("TestVoidMethod has executed!");
    }

    public async Task<int> TestIntMethod(int number)
    {
        await Task.Delay(300);
        Console.WriteLine($"TestIntMethod has executed!");
        return number * 10;
    }
}