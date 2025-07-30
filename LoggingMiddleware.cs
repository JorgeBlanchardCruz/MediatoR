namespace Mediator;

public interface IMediatorMiddleware
{
    Task InvokeAsync(object request, Func<object, Task> next);
}

public class LoggingMiddleware : IMediatorMiddleware
{
    public async Task InvokeAsync(object request, Func<object, Task> next)
    {
        Console.WriteLine("Middleware: Antes");

        await next(request);

        Console.WriteLine("Middleware: Después");
    }
}
