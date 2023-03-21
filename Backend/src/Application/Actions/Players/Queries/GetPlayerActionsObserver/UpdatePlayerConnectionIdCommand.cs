using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Sheduler.Interfaces;
using MediatR;

namespace Backend.Application.Actions.Players.Queries.GetPlayerActionsObserver;

public record GetPlayerActionsObserverCommand : IRequest<IPlayerActionsObserver?>
{
    public Guid PlayerId { get; set; }

    public Guid GameId { get; set; }
}

public class GetPlayerActionsObserverCommandHandler : IRequestHandler<GetPlayerActionsObserverCommand, IPlayerActionsObserver?>
{
    private readonly IRunningGames _runningGames;

    public GetPlayerActionsObserverCommandHandler(IRunningGames runningGames)
    {
        _runningGames = runningGames;
    }

    public Task<IPlayerActionsObserver?> Handle(GetPlayerActionsObserverCommand request, CancellationToken cancellationToken)
    {
        var game = _runningGames.TryGet(request.GameId);
        if (game == null)
        {
            return Task.FromResult<IPlayerActionsObserver?>(null);
        }
        return Task.FromResult(game.GetPlayerActionsObserver(request.PlayerId));
    }
}
