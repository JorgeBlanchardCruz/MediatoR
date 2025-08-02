using MediatoR;

namespace Mediator.Middlewares;

public class OtherMiddleware : IMediatorMiddleware
{
    public async Task InvokeAsync(object request, Func<object, Task> next)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("2 --> Delay: Before");
        Console.ResetColor();

        await Task.Delay(500);

        await next(request);

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("2 --> Delay: After");
        Console.ResetColor();
    }
}
