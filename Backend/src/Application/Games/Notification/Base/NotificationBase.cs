using Backend.Application.Common.Enums;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.Games.Notification.Base;
public abstract class NotificationBase<T> : IGameNotification
{
    public abstract NotificationType Type { get; }

    public abstract T Data { get; init; }

    public object Payload => Data!;
}
