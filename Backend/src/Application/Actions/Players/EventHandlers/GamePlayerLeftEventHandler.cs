using Backend.Application.Common.Interfaces;
using Backend.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Actions.Players.EventHandlers;

public class GamePlayerLeftEventHandler : INotificationHandler<GamePlayerLeftEvent>
{
    private readonly ILogger<GamePlayerLeftEventHandler> _logger;
    private readonly ILobbyHubManager _lobbyHubManager;

    public GamePlayerLeftEventHandler(
        ILogger<GamePlayerLeftEventHandler> logger,
        ILobbyHubManager lobbyHubManager
        )
    {
        _logger = logger;
        _lobbyHubManager = lobbyHubManager;
    }

    public async Task Handle(GamePlayerLeftEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Backend Domain Event: {DomainEvent}", notification.GetType().Name);

        var gameId = notification.Player.Game.Id.ToString();

        await _lobbyHubManager.SendSystemMessage(gameId, $"Player: {notification.Player.Name} left");
        await _lobbyHubManager.PlayerLeft(gameId, notification.Player.Id);
    }
}
