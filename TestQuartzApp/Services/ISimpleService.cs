namespace TestQuartzApp.Services;

public interface ISimpleService
{
    Task TestVoidMethod();
    Task<int> TestIntMethod(int number);
}