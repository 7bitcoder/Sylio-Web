using Backend.Application.Common.Interfaces;
using Backend.Application.Games.Sheduler.Interfaces;
using Backend.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Actions.Games.EventHandlers;

public class GameStartRequestedEventHandler : INotificationHandler<GameStartRequestedEvent>
{
    private readonly ILogger<GameStartRequestedEventHandler> _logger;
    private readonly ILobbyHubManager _lobbyHubManager;
    private readonly IGameRequests _gameRequests;

    public GameStartRequestedEventHandler(
        ILogger<GameStartRequestedEventHandler> logger,
        ILobbyHubManager lobbyHubManager,
        IGameRequests gameRequests
        )
    {
        _logger = logger;
        _lobbyHubManager = lobbyHubManager;
        _gameRequests = gameRequests;
    }

    public async Task Handle(GameStartRequestedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Backend Domain Event: {DomainEvent}", notification.GetType().Name);

        _gameRequests.Add(notification.Game.Id);
        await _lobbyHubManager.GameStarted(notification.Game.Id.ToString());
    }
}
