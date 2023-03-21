using Backend.Application.Common.Enums;
using Backend.Application.Games.Notification.Base;
using static Backend.Application.Games.Notification.Notifications.GameSetupNotification;

namespace Backend.Application.Games.Notification.Notifications;
public class GameSetupNotification : NotificationBase<Setup>
{
    public override NotificationType Type => NotificationType.GameSetup;

    public override Setup Data { get; init; } = default!;

    public class Setup
    {
        public List<PlayerIdMap> PlayerIdMap { get; set; } = default!;
    }

    public class PlayerIdMap
    {
        public Guid PlayerId { get; set; }

        public int ShortId { get; set; }
    }
}
