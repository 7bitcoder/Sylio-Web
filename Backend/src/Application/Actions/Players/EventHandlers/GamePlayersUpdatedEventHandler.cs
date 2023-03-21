using Backend.Application.Actions.Games.EventHandlers;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Actions.Players.EventHandlers;

public class GamePlayersUpdatedEventHandler : INotificationHandler<GamePlayersUpdatedEvent>
{
    private readonly ILogger<GameStartRequestedEventHandler> _logger;
    private readonly ILobbyHubManager _lobbyHubManager;

    public GamePlayersUpdatedEventHandler(
        ILogger<GameStartRequestedEventHandler> logger,
        ILobbyHubManager lobbyHubManager
        )
    {
        _logger = logger;
        _lobbyHubManager = lobbyHubManager;
    }

    public Task Handle(GamePlayersUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Backend Domain Event: {DomainEvent}", notification.GetType().Name);

        _lobbyHubManager.PlayersUpdated(notification.Game.Id.ToString());

        return Task.CompletedTask;
    }
}
