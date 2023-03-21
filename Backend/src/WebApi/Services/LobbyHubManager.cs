using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Backend.Application.Common.Interfaces;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Player;
using Backend.Domain.Entities;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services;
public class LobbyHubManager : ILobbyHubManager
{
    private readonly IHubContext<LobbyHub, LobbyActions> _hubContext;

    public LobbyHubManager(IHubContext<LobbyHub, LobbyActions> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task JoinLobby(string connectionId, string gameId, string user)
    {
        await _hubContext.Groups.AddToGroupAsync(connectionId, gameId);
    }

    public async Task LeaveLobby(string connectionId, string gameId, string user)
    {
        await _hubContext.Groups.RemoveFromGroupAsync(connectionId, gameId);
    }

    public async Task SendChatMessage(string gameId, string user, string message)
    {
        await _hubContext.Clients.Group(gameId).ChatMessage(user, message);
    }

    public async Task SendSystemMessage(string gameId, string message)
    {
        await SendChatMessage(gameId, "System", message);
    }

    public async Task PlayersUpdated(string gameId)
    {
        await _hubContext.Clients.Group(gameId).PlayersUpdated();
    }

    public async Task GameUpdated(string gameId)
    {
        await _hubContext.Clients.Group(gameId).GameUpdated();
    }

    public Task GameDeleted(string gameId)
    {
        return _hubContext.Clients.Group(gameId).GameDeleted();
    }

    public Task PlayerLeft(string gameId, Guid playerId)
    {
        return _hubContext.Clients.Group(gameId).PlayerLeft(playerId);
    }

    public Task PlayerJoined(string gameId, Guid playerId)
    {
        return _hubContext.Clients.Group(gameId).PlayerJoined(playerId);
    }

    public Task PlayerKicked(string gameId, Guid playerId)
    {
        return _hubContext.Clients.Group(gameId).PlayerKicked(playerId);
    }

    public Task GameStarted(string gameId)
    {
        return _hubContext.Clients.Group(gameId).GameStarted();
    }
}
