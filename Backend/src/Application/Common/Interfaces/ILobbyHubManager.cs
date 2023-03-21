namespace Backend.Application.Common.Interfaces;
public interface ILobbyHubManager
{
    Task JoinLobby(string connectionId, string gameId, string user);

    Task LeaveLobby(string connectionId, string gameId, string user);

    Task SendChatMessage(string gameId, string user, string message);

    Task SendSystemMessage(string gameId, string message);

    Task PlayersUpdated(string gameId);

    Task GameUpdated(string gameId);

    Task GameDeleted(string gameId);

    Task PlayerKicked(string gameId, Guid playerId);

    Task PlayerLeft(string gameId, Guid playerId);

    Task PlayerJoined(string gameId, Guid playerId);

    Task GameStarted(string gameId);
}
