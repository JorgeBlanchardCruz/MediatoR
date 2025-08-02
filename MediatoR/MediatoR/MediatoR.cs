#region License

// Copyright (c) 2025 Jorge Blanchard Cruz
// Licensed under the Creative Commons Attribution 4.0 License (CC BY 4.0).
// For more information, visit: https://creativecommons.org/licenses/by/4.0/
// I accept donations at: contact me at jorgeblanchardcruz@outlook.com

#endregion License
using MediatoR.Abstractions;

namespace MediatoR;

/// <summary>
/// Provides a mediator implementation for handling requests, notifications, and middleware pipelines.
/// </summary>
/// <remarks>The <see cref="MediatoR"/> class acts as a central hub for coordinating the processing of requests
/// and notifications through registered handlers and middleware. It supports the registration of request handlers,
/// notification handlers, and middleware components, enabling a flexible and extensible approach to message-based
/// communication within an application.</remarks>
public class MediatoR : IMediator
{
    /// <summary>
    /// Represents a method that processes a request and returns a response asynchronously.
    /// </summary>
    /// <remarks>The delegate is typically used in scenarios where requests need to be handled asynchronously,
    /// such as in middleware pipelines or custom processing workflows.</remarks>
    /// <param name="request">The request object to be processed. This parameter cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response object.</returns>
    public delegate Task<object> RequestDelegate(object request);

    private readonly Dictionary<Type, object> _handlers;
    private readonly List<IMediatorMiddleware> _middlewares;

    /// <summary>
    /// Initializes a new instance of the <see cref="MediatoR"/> class.
    /// </summary>
    /// <remarks>This constructor initializes the mediator with empty collections of handlers and middlewares.
    /// Handlers and middlewares can be added after instantiation to customize the behavior of the mediator.</remarks>
    public MediatoR()
    {
        _handlers = [];
        _middlewares = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MediatoR"/> class with the specified handlers and middleware
    /// components.
    /// </summary>
    /// <remarks>The <see cref="MediatoR"/> class coordinates the dispatching of requests to their appropriate
    /// handlers,  optionally passing through middleware for additional processing. Ensure that the <paramref
    /// name="handlers"/>  dictionary contains all required handler mappings for the types of requests you intend to
    /// process.</remarks>
    /// <param name="handlers">A dictionary mapping types to their corresponding handler instances.  Each handler is responsible for processing
    /// a specific type of request or message.</param>
    /// <param name="middlewares">A list of middleware components that will be executed in the processing pipeline.  Middleware can modify or
    /// extend the behavior of request handling.</param>
    public MediatoR(Dictionary<Type, object> handlers, List<IMediatorMiddleware> middlewares)
    {
        _handlers = handlers;
        _middlewares = middlewares;
    }

    #region Registration Methods
    /// <inheritdoc />
    public void RegisterHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse>
    {
        _handlers[typeof(TRequest)] = handler;
    }

    /// <inheritdoc />
    public void RegisterHandler<TNotification>(INotificationHandler<TNotification> handler)
        where TNotification : INotification
    {
        _handlers[typeof(TNotification)] = handler;
    }

    /// <inheritdoc />
    public void RegisterHandler<TRequest>(IRequestHandler<TRequest> handler)
        where TRequest : IRequest
    {
        _handlers[typeof(TRequest)] = handler;
    }

    /// <inheritdoc />
    public void RegisterMiddleware(IMediatorMiddleware middleware)
    {
        _middlewares.Add(middleware);
    }

    #endregion

    #region Send Methods
    /// <inheritdoc />
    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        if (!_handlers.TryGetValue(typeof(TRequest), out var handlerObj))
        {
            throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");
        }

        var handler = (IRequestHandler<TRequest, TResponse>)handlerObj;

        RequestDelegate pipeline = async (req) =>
        {
            var result = await handler.Handle((TRequest)req, cancellationToken);
            return result!;
        };

        BuildPipeline(ref pipeline);

        return (TResponse)await pipeline(request);
    }

    /// <inheritdoc />
    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        if (!_handlers.TryGetValue(typeof(TRequest), out var handlerObj))
        {
            throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");
        }

        var handler = (IRequestHandler<TRequest>)handlerObj;

        RequestDelegate pipeline = async (req) =>
        {
            await handler.Handle((TRequest)req, cancellationToken);
            return null!;
        };

        BuildPipeline(ref pipeline);

        await pipeline(request);
    }

    /// <inheritdoc />
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request), "The request cannot be null.");
        }

        if (!_handlers.TryGetValue(request.GetType(), out var handlerObj))
        {
            throw new InvalidOperationException($"No handler registered for {request.GetType().Name}");
        }

        var handler = (IRequestHandler<IRequest<TResponse>, TResponse>)handlerObj;

        RequestDelegate pipeline = async (req) =>
        {
            var result = await handler.Handle((IRequest<TResponse>)req, cancellationToken);
            return result!;
        };

        BuildPipeline(ref pipeline);

        return (TResponse)await pipeline(request);
    }

    /// <summary>
    /// Constructs a request processing pipeline by chaining middleware components in reverse order.
    /// </summary>
    /// <remarks>Each middleware in the pipeline is invoked in reverse order of their registration,  with each
    /// middleware having the opportunity to process the request and invoke the next middleware. The pipeline is built
    /// dynamically, and the resulting delegate processes requests sequentially.</remarks>
    /// <param name="pipeline">The final <see cref="RequestDelegate"/> to be executed at the end of the pipeline. This delegate represents the
    /// next step in the request processing flow.</param>
    private void BuildPipeline(ref RequestDelegate pipeline)
    {
        foreach (var middleware in _middlewares.AsEnumerable().Reverse())
        {
            var next = pipeline;
            pipeline = async (request) =>
            {
                object? response = null;

                await middleware.InvokeAsync(request, async (innerRequest) =>
                {
                    response = await next(innerRequest);
                });

                return response!;
            };
        }
    }

    #endregion

    #region Publish Methods
    /// <inheritdoc />
    public async Task Publish(object notification, CancellationToken cancellationToken = default)
    {
        await ExecutePublish((INotification)notification);
    }

    /// <inheritdoc />
    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        await ExecutePublish(notification);
    }

    private async Task ExecutePublish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        if (!_handlers.TryGetValue(typeof(TNotification), out var handlerObj))
        {
            throw new InvalidOperationException($"No handler registered for {typeof(TNotification).Name}");
        }

        var handler = (INotificationHandler<TNotification>)handlerObj;

        await handler.Handle(notification, cancellationToken);
    }

    #endregion

}