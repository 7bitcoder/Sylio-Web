using Backend.Application.Common.Interfaces;
using Backend.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Actions.Games.EventHandlers;

public class GameUpdatedEventHandler : INotificationHandler<GameUpdatedEvent>
{
    private readonly ILogger<GameStartRequestedEventHandler> _logger;
    private readonly ILobbyHubManager _lobbyHubManager;

    public GameUpdatedEventHandler(
        ILogger<GameStartRequestedEventHandler> logger,
        ILobbyHubManager lobbyHubManager
        )
    {
        _logger = logger;
        _lobbyHubManager = lobbyHubManager;
    }

    public Task Handle(GameUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Backend Domain Event: {DomainEvent}", notification.GetType().Name);

        _lobbyHubManager.GameUpdated(notification.Game.Id.ToString());

        return Task.CompletedTask;
    }
}
