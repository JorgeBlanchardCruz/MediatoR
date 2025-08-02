using Mediator.Notifications;
using Mediator.Requests;
using System.Reflection;

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

#region License

// Copyright (c) 2025 Jorge Blanchard Cruz
// Licensed under the Creative Commons Attribution 4.0 License (CC BY 4.0).
// For more information, visit: https://creativecommons.org/licenses/by/4.0/
// I accept donations at: contact me at jorgeblanchardcruz@outlook.com

#endregion License
