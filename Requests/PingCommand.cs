using MediatoR;

namespace Mediator.Requests;

public sealed record PingCommand(string Message) : IRequest<string>
{
}

public sealed class PingHandler : IRequestHandler<PingCommand, string>
{
    public async Task<string> Handle(PingCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ping: {request.Message}");

        return await Task.FromResult($"Respuesta");
    }
}