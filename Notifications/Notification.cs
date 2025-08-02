using MediatoR;

namespace Mediator.Notifications;

public sealed record Notification(string Content) : INotification
{
}

public sealed class NotificationHandler : INotificationHandler<Notification>
{
    public Task Handle(Notification notification, CancellationToken cancellationToken = default)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("");
        Console.WriteLine($"{notification.Content}");
        Console.WriteLine("");
        Console.ResetColor();

        return Task.CompletedTask;
    }
}