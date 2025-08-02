

namespace Mediator.Middlewares;

public class LoggingMiddleware : IMediatorMiddleware
{
    public async Task InvokeAsync(object request, Func<object, Task> next)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("1 --> Logging: Before");
        Console.ResetColor();

        await next(request);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("1 --> Logging: After");
        Console.ResetColor();
    }
}
