#region License

// Copyright (c) 2025 Jorge Blanchard Cruz
// Licensed under the Creative Commons Attribution 4.0 License (CC BY 4.0).
// For more information, visit: https://creativecommons.org/licenses/by/4.0/
// I accept donations at: contact me at jorgeblanchardcruz@outlook.com

#endregion License

using Mediator.Middlewares;
using Mediator.Notifications;
using Mediator.Requests;
using Requests.Products.Commands;

public class Program
{
    public static async Task Main(string[] args)
    {
        var mediator = new MediatoR.MediatoR();

        mediator.RegisterMiddleware(new LoggingMiddleware());
        mediator.RegisterMiddleware(new OtherMiddleware());
        mediator.RegisterHandler(new PingHandler());
        mediator.RegisterHandler(new JoinHandler());
        mediator.RegisterHandler(new CreateProductHandler(new ProductsRepository()));

        // Enviar comandos
        var response = await mediator.Send<PingCommand, string>(new PingCommand("Pong"));
        Console.WriteLine(response);

        await mediator.Send(new JoinCommand("WIOOAJSKDIIWJKASI2929J"));

        var response2 = await mediator.Send(new CreateProductCommand
        {
            Name = "Product 1",
            Description = "This is a product",
            Price = 100.0m
        });
        Console.WriteLine($"Product created: {response2.Data.Name}, {response2.Data.Description}, {response2.Data.Price}");

        // Enviar notificación
        await mediator.Publish(new Notification("Bye, World!"));

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }

}
