

namespace Mediator.Middlewares;

public class LoggingMiddleware : IMediatorMiddleware
{
    public async Task InvokeAsync(object request, Func<object, Task> next)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("--> Logging: Before");
        Console.ResetColor();

        await next(request);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("--> Logging: After");
        Console.ResetColor();
    }
}
