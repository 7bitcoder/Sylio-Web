using Backend.Application.Common.Enums;
using Backend.Application.Games.Notification.Base;
using static Backend.Application.Games.Notification.Notifications.RoundSetupNotification;

namespace Backend.Application.Games.Notification.Notifications;
public class RoundSetupNotification : NotificationBase<Setup>
{
    public override NotificationType Type => NotificationType.RoundSetup;

    public override Setup Data { get; init; } = default!;

    public class Setup
    {
        public int RoundNumber { get; set; }
    }
}
