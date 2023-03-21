using Backend.Application.Common.Enums;
using Backend.Application.Games.Notification.Base;

namespace Backend.Application.Games.Notification.Notifications;
public class PlayersReadydNotification : NotificationBase<List<string>>
{
    public override NotificationType Type => NotificationType.PlayersReady;

    public override List<string> Data { get; init; } = default!;
}
