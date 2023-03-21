using Backend.Application.Common.Enums;
using Backend.Application.Games.Notification.Base;
using Backend.Application.Games.Stages.Enums;

namespace Backend.Application.Games.Notification.Notifications;
public class StageChangedNotification : NotificationBase<StageType>
{
    public override NotificationType Type => NotificationType.StageChange;

    public override StageType Data { get; init; }
}
