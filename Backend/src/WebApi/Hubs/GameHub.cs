using Backend.Application.Common.Interfaces;
using Backend.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

public class GameHub : Hub
{
    private readonly IGameHubManager _manager;

    public GameHub(IGameHubManager connectionManager)
    {
        _manager = connectionManager;
    }

    public override async Task OnConnectedAsync()
    {
        await _manager.OnConnected(Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _manager.OnDisconnected(Context.ConnectionId, exception);
        await base.OnDisconnectedAsync(exception);
    }

    public Task ConnectToGame(string gameId, Guid playerId)
    {
        return _manager.OnConnectToGame(Context.ConnectionId, gameId, playerId);
    }

    public Task DisconectFromGame(string gameId)
    {
        return _manager.OnDiconnectFromGame(Context.ConnectionId, gameId);
    }
    public Task K(KeyType key)
    {
        return _manager.OnPlayerKeyPressed(Context.ConnectionId, key);
    }

    public Task R(KeyType key)
    {
        return _manager.OnPlayerKeyReleased(Context.ConnectionId, key);
    }

    public Task Ready()
    {
        return _manager.OnReady(Context.ConnectionId);
    }
}
