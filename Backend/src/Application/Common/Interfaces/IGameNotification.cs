using Backend.Application.Common.Enums;

namespace Backend.Application.Common.Interfaces;

public interface IGameNotification
{
    NotificationType Type { get; }

    object Payload { get; }
}
