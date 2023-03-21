using Backend.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

public interface LobbyActions
{
    Task GameUpdated();
    
    Task GameDeleted();

    Task PlayersUpdated();

    Task ChatMessage(string user, string message);

    Task PlayerKicked(Guid playersId);

    Task PlayerJoined(Guid playerId);
    
    Task PlayerLeft(Guid playerId);

    Task GameStarted();
}

public class LobbyHub : Hub<LobbyActions>
{
    private readonly ILobbyHubManager _lobbyHubManager;

    public LobbyHub(ILobbyHubManager lobbyHubManager)
    {
        _lobbyHubManager = lobbyHubManager;
    }

    public Task JoinLobby(string gameId, string user)
    {
        return _lobbyHubManager.JoinLobby(Context.ConnectionId, gameId, user);
    }

    public Task LeaveLobby(string gameId, string user)
    {
        return _lobbyHubManager.LeaveLobby(Context.ConnectionId, gameId, user);
    }

    public Task SendChatMessage(string gameId, string user, string message)
    {
        return _lobbyHubManager.SendChatMessage(gameId, user, message);
    }
}
