﻿using MediatoR;
using System.Reflection;

#region PingCommand
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

#endregion

#region JoinCommand
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

#endregion

#region Notification
public sealed record Notification(string Content) : INotification
{
}

public sealed class NotificationHandler : INotificationHandler<Notification>
{
    public Task Handle(Notification notification)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("");
        Console.WriteLine($"{notification.Content}");
        Console.WriteLine("");
        Console.ResetColor();

        return Task.CompletedTask;
    }
}

#endregion

public class Program
{
    public static async Task Main(string[] args)
    {
        var mediator = new MediatoR.MediatoR();
        mediator.RegisterHandlersFromAssembly(Assembly.GetExecutingAssembly());
        mediator.RegisterMiddlewareFromAssembly(Assembly.GetExecutingAssembly());

        // Enviar comandos
        var response = await mediator.Send<PingCommand, string>(new PingCommand("Pong"));
        Console.WriteLine(response);

        await mediator.Send(new JoinCommand("WIOOAJSKDIIWJKASI2929J"));

        // Enviar notificación
        await mediator.Publish(new Notification("Bye, World!"));

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }

}
