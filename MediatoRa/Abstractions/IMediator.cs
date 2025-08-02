﻿using System.Reflection;

namespace MediatoR;

public interface IMediator : ISender, IPublisher
{
    /// <summary>
    /// Registers all handlers and notification handlers found in the specified assembly.
    /// </summary>
    /// <remarks>This method scans the provided assembly for types that implement specific handler interfaces
    /// and registers them for use. Both command handlers and notification handlers are included in the registration
    /// process.</remarks>
    /// <param name="assembly">The assembly to scan for handlers and notification handlers. Cannot be <see langword="null"/>.</param>
    void RegisterHandlersFromAssembly(Assembly assembly);

    /// <summary>
    /// Registers all middleware types from the specified assembly that implement the <see cref="IMediatorMiddleware"/>
    /// interface.
    /// </summary>
    /// <remarks>This method identifies all non-abstract types in the provided assembly that implement the
    /// <see cref="IMediatorMiddleware"/> interface. Each identified type is instantiated and registered as middleware.
    /// Ensure that the types have a parameterless constructor, as the method uses <see
    /// cref="Activator.CreateInstance(Type)"/> to create instances.</remarks>
    /// <param name="assembly">The assembly to scan for middleware types. Must not be <see langword="null"/>.</param>
    void RegisterMiddlewareFromAssembly(Assembly assembly);

    /// <summary>
    /// Registers a handler for processing requests of a specific type.
    /// </summary>
    /// <remarks>This method associates the specified handler with the request type <typeparamref
    /// name="TRequest"/>.  Subsequent calls to process requests of this type will invoke the registered
    /// handler.</remarks>
    /// <typeparam name="TRequest">The type of the request that the handler processes. Must implement <see cref="IRequest{TResponse}"/>.</typeparam>
    /// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
    /// <param name="handler">The handler instance responsible for processing requests of type <typeparamref name="TRequest"/> and producing a
    /// response of type <typeparamref name="TResponse"/>. Cannot be <see langword="null"/>.</param>
    void RegisterHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse>;

    /// <summary>
    /// Registers a handler for processing requests of the specified type.
    /// </summary>
    /// <remarks>Use this method to associate a specific request type with a corresponding handler. The
    /// handler will be invoked whenever a request of the specified type is processed.</remarks>
    /// <typeparam name="TRequest">The type of request that the handler processes. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="handler">The handler instance responsible for processing requests of type <typeparamref name="TRequest"/>. Cannot be <see
    /// langword="null"/>.</param>
    void RegisterHandler<TRequest>(IRequestHandler<TRequest> handler)
        where TRequest : IRequest;

    /// <summary>
    /// Registers a middleware component to be executed in the mediator pipeline.
    /// </summary>
    /// <remarks>Middleware components are executed in the order they are registered. Each middleware can 
    /// process requests and responses, or pass them to the next middleware in the pipeline.</remarks>
    /// <param name="middleware">The middleware instance to register. Cannot be <see langword="null"/>.</param>
    void RegisterMiddleware(IMediatorMiddleware middleware);

    /// <summary>
    /// Registers a handler for processing notifications of the specified type.
    /// </summary>
    /// <remarks>This method associates the specified handler with the notification type <typeparamref
    /// name="TNotification"/>. When a notification of this type is published, the registered handler will be invoked to
    /// process it.</remarks>
    /// <typeparam name="TNotification">The type of notification that the handler processes. Must implement <see cref="INotification"/>.</typeparam>
    /// <param name="handler">The notification handler to register. Cannot be <see langword="null"/>.</param>
    void RegisterHandler<TNotification>(INotificationHandler<TNotification> handler)
        where TNotification : INotification;
}