namespace Mediator;

public class MediatoR : IMediator
{
    public delegate Task<object> RequestDelegate(object request);

    private readonly Dictionary<Type, object> _handlers = [];
    private readonly List<IMediatorMiddleware> _middlewares = [];

    public void RegisterHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse>
    {
        _handlers[typeof(TRequest)] = handler;
    }

    public void RegisterMiddleware(IMediatorMiddleware middleware)
    {
        _middlewares.Add(middleware);
    }

    private async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        if (!_handlers.TryGetValue(typeof(TRequest), out var handlerObj))
        {
            throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");
        }

        var handler = (IRequestHandler<TRequest, TResponse>)handlerObj;

        // Construir la cadena de middlewares
        RequestDelegate pipeline = async (req) =>
        {
            var result = await handler.Handle((TRequest)req, cancellationToken);
            return result!;
        };

        foreach (var middleware in _middlewares.AsEnumerable().Reverse())
        {
            var next = pipeline;
            pipeline = async (req) =>
            {
                object? response = null;
                await middleware.InvokeAsync(req, async (innerReq) =>
                {
                    response = await next(innerReq);
                });
                return response!;
            };
        }

        return (TResponse)await pipeline(request);
    }

    private async Task Publish<TNotification>(TNotification notification) where TNotification : INotification
    {
        if (!_handlers.TryGetValue(typeof(TNotification), out var handlerObj))
        {
            return;
        }

        var handlers = (List<INotificationHandler<TNotification>>)handlerObj;

        foreach (var handler in handlers)
        {
            await handler.Handle(notification);
        }
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return await Send<IRequest<TResponse>, TResponse>(request, cancellationToken);
    }

    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) 
        where TRequest : IRequest
    {
        await Send<IRequest<TRequest>, TRequest>((IRequest<TRequest>)(IRequest)request, cancellationToken);
    }

    public async Task Publish(object notification, CancellationToken cancellationToken = default)
    {
        await Publish((INotification)notification);
    }

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        return Publish(notification);
    }
}