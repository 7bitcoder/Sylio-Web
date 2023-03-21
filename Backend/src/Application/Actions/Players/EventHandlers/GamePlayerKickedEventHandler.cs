using Backend.Application.Common.Interfaces;
using Backend.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Actions.Players.EventHandlers;

public class GamePlayerKickedEventHandler : INotificationHandler<GamePlayerKickedEvent>
{
    private readonly ILogger<GamePlayerKickedEventHandler> _logger;
    private readonly ILobbyHubManager _lobbyHubManager;

    public GamePlayerKickedEventHandler(
        ILogger<GamePlayerKickedEventHandler> logger,
        ILobbyHubManager lobbyHubManager
        )
    {
        _logger = logger;
        _lobbyHubManager = lobbyHubManager;
    }

    public async Task Handle(GamePlayerKickedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Backend Domain Event: {DomainEvent}", notification.GetType().Name);

        var gameId = notification.Player.Game.Id.ToString();
        await _lobbyHubManager.SendSystemMessage(gameId, $"Player: {notification.Player.Name} was kicked");
        await _lobbyHubManager.PlayerKicked(gameId, notification.Player.Id);
    }
}
