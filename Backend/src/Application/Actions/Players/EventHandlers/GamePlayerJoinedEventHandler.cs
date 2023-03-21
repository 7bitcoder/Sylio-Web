using Backend.Application.Common.Interfaces;
using Backend.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Actions.Players.EventHandlers;

public class GamePlayerJoinedEventHandler : INotificationHandler<GamePlayerJoinedEvent>
{
    private readonly ILogger<GamePlayerJoinedEventHandler> _logger;
    private readonly ILobbyHubManager _lobbyHubManager;

    public GamePlayerJoinedEventHandler(
        ILogger<GamePlayerJoinedEventHandler> logger,
        ILobbyHubManager lobbyHubManager
        )
    {
        _logger = logger;
        _lobbyHubManager = lobbyHubManager;
    }

    public async Task Handle(GamePlayerJoinedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Backend Domain Event: {DomainEvent}", notification.GetType().Name);

        var gameId = notification.Player.Game.Id.ToString();

        await _lobbyHubManager.SendSystemMessage(gameId, $"Player: {notification.Player.Name} joined");
        await _lobbyHubManager.PlayerJoined(gameId, notification.Player.Id);
    }
}
