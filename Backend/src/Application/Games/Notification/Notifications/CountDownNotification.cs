using Backend.Application.Common.Enums;
using Backend.Application.Games.Notification.Base;

namespace Backend.Application.Games.Notification.Notifications;
public class CountDownNotification : NotificationBase<int>
{
    public override NotificationType Type => NotificationType.RoundStartCountDown;

    public override int Data { get; init; }
}
