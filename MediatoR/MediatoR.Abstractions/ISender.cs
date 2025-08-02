namespace MediatoR.Abstractions;

/// <summary>
/// Defines a contract for sending requests and receiving responses asynchronously.
/// </summary>
/// <remarks>This interface is typically implemented by classes that handle the dispatching of requests to their
/// corresponding handlers or services. It supports generic request-response patterns where the request type determines
/// the expected response type.</remarks>
public interface ISender
{
    /// <summary>
    /// Sends a request and asynchronously retrieves the corresponding response.
    /// </summary>
    /// <remarks>This method is typically used to send a request to a handler or service that processes the
    /// request and returns a response. Ensure that the request type is properly mapped to a handler capable of
    /// processing it.</remarks>
    /// <typeparam name="TRequest">The type of the request to be sent. Must implement <see cref="IRequest{TResponse}"/>.</typeparam>
    /// <typeparam name="TResponse">The type of the response expected from the request.</typeparam>
    /// <param name="request">The request object to be sent. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response of type <typeparamref
    /// name="TResponse"/>.</returns>
    Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>;

    /// <summary>
    /// Sends the specified request to the appropriate handler for processing.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request to be sent. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="request">The request object to be processed. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task completes when the request has been
    /// processed.</returns>
    Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest;
}