using Backend.Domain.Enums;

namespace Backend.Application.Common.Interfaces;
public interface IGameHubManager
{
    Task OnConnected(string connectionId);

    Task OnDisconnected(string connectionId, Exception? exception);

    Task OnConnectToGame(string connectionId, string gameId, Guid playerId);

    Task OnDiconnectFromGame(string connectionId, string gameId);

    Task SendNotification(string groupId, IGameNotification notification);

    Task SendUpdate(string groupId, object[] updates);

    Task OnPlayerKeyPressed(string connectionId, KeyType key);

    Task OnPlayerKeyReleased(string connectionId, KeyType key);

    Task OnReady(string connectionId);
}
