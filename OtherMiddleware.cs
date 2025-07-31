namespace MediatoR;

public class OtherMiddleware : IMediatorMiddleware
{
    public async Task InvokeAsync(object request, Func<object, Task> next)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("--> Delay: Before");
        Console.ResetColor();

        await Task.Delay(1000);

        await next(request);

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("--> Delay: After");
        Console.ResetColor();
    }
}
