using MediatoR;

namespace Mediator.Requests;

public sealed record JoinCommand(string Id) : IRequest
{
}

public sealed class JoinHandler : IRequestHandler<JoinCommand>
{
    public async Task Handle(JoinCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine($"Join us: {request.Id}");
    }
}