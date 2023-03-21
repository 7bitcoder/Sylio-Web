using Backend.Application.Common.Interfaces;
using Backend.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Actions.Games.EventHandlers;

public class GameDeletedEventHandler : INotificationHandler<GameDeletedEvent>
{
    private readonly ILogger<GameDeletedEventHandler> _logger;
    private readonly ILobbyHubManager _lobbyHubManager;

    public GameDeletedEventHandler(
        ILogger<GameDeletedEventHandler> logger,
        ILobbyHubManager lobbyHubManager
        )
    {
        _logger = logger;
        _lobbyHubManager = lobbyHubManager;
    }

    public Task Handle(GameDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Backend Domain Event: {DomainEvent}", notification.GetType().Name);

        _lobbyHubManager.GameDeleted(notification.Game.Id.ToString());

        return Task.CompletedTask;
    }
}
