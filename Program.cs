using Mediator;

public class Ping : IRequest<string>
{
    public string Message { get; set; }
}

public class PingHandler : IRequestHandler<Ping, string>
{
    public async Task<string> Handle(Ping request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Pong: {request.Message}");

        return await Task.FromResult($"Respuesta");
    }
}

public class Notification : INotification
{
    public string Content { get; set; }
}

public class NotificationHandler : INotificationHandler<Notification>
{
    public Task Handle(Notification notification)
    {
        Console.WriteLine($"Notification received: {notification.Content}");
        return Task.CompletedTask;
    }
}


public class Program
{
    public static async Task Main(string[] args)
    {
        var mediator = new MediatoR();

        // Registrar manejadores
        mediator.RegisterHandler(new PingHandler());

        // Configurar middleware
        mediator.RegisterMiddleware(new LoggingMiddleware());

        // Enviar comando
        var ping = new Ping { Message = "Hola" };
        var response = await mediator.Send(ping);
        Console.WriteLine(response);


        // Publicar notificación
        await mediator.Publish(new Notification { Content = "Evento importante" });

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}
