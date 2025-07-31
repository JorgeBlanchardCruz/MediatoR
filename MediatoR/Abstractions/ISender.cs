namespace MediatoR;

public interface ISender
{
    Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>;

    //Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    //Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
    //    where TRequest : IRequest;
}