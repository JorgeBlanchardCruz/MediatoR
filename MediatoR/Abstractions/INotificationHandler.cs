namespace MediatoR;

public interface INotificationHandler<TNotification> where TNotification : INotification
{
    Task Handle(TNotification notification);
}
