namespace MediatoR;

public interface IMediatorMiddleware
{
    Task InvokeAsync(object request, Func<object, Task> next);
}