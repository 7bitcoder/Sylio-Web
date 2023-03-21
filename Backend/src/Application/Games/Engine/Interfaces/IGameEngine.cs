using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Engine.Interfaces;
public interface IGameEngine
{
    Task Run(Guid gameId, CancellationToken token);

    IPlayerActionsObserver? GetPlayerActionsObserver(Guid playerId);
}
