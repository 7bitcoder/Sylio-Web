using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using System.Text.Json;
using Backend.Application.Actions.Players.Queries.GetPlayerActionsObserver;
using Backend.Application.Common.Interfaces;
using Backend.Application.Games.Player;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Sheduler.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;
using Backend.Domain.Enums;

namespace WebApi.Services;
public class GameHubManager : IGameHubManager
{
    private readonly ConcurrentDictionary<string, IPlayerActionsObserver> _observers = new();
    private readonly IHubContext<GameHub> _hubContext;
    private readonly IServiceProvider _serviceProvider;

    public GameHubManager(
        IRunningGames runningGames,
        IHubContext<GameHub> hubContext,
        IServiceProvider serviceProvider)
    {
        _hubContext = hubContext;
        _serviceProvider = serviceProvider;
    }

    public Task OnConnected(string connectionId)
    {
        return Task.CompletedTask;
    }

    public Task OnDisconnected(string connectionId, Exception? exception)
    {
        if (_observers.TryRemove(connectionId, out var observer))
        {
            observer.OnDisconnected();
        }
        return Task.CompletedTask;
    }

    public async Task OnConnectToGame(string connectionId, string gameId, Guid playerId)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var observer = await mediator.Send(new GetPlayerActionsObserverCommand { GameId = Guid.Parse(gameId), PlayerId = playerId });
        if (observer is null || !_observers.TryAdd(connectionId, observer))
        {
            return;
        }
        await _hubContext.Groups.AddToGroupAsync(connectionId, gameId);
        observer.OnConnected();
    }

    public async Task OnDiconnectFromGame(string connectionId, string gameId)
    {
        if (_observers.TryRemove(connectionId, out var observer))
        {
            observer.OnDisconnected();
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, gameId);
        }
    }

    public Task SendNotification(string groupId, IGameNotification notification)
    {
        return _hubContext.Clients.Group(groupId).SendAsync("notification", new { notification.Type, notification.Payload });
    }

    public Task SendUpdate(string groupId, object[] updates)
    {
        return _hubContext.Clients.Group(groupId).SendAsync("u", updates);
    }

    public Task OnPlayerKeyPressed(string connectionId, KeyType key)
    {
        getObserver(connectionId)?.OnKeyPressed(key);
        return Task.CompletedTask;
    }

    public Task OnPlayerKeyReleased(string connectionId, KeyType key)
    {
        getObserver(connectionId)?.OnKeyReleased(key);
        return Task.CompletedTask;
    }

    public Task OnReady(string connectionId)
    {
        getObserver(connectionId)?.OnReady();
        return Task.CompletedTask;
    }

    private IPlayerActionsObserver? getObserver(string connectionId)
    {
        if (_observers.TryGetValue(connectionId, out var observer))
        {
            return observer;
        }
        return null;
    }
}
