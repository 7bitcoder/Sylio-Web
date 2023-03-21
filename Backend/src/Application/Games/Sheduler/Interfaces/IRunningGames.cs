using Backend.Application.Games.Engine.Interfaces;

namespace Backend.Application.Games.Sheduler.Interfaces;
public interface IRunningGames
{
    IDisposable Add(Guid gameId, IGameEngine gameEngine);

    void Remove(Guid gameId);

    IGameEngine? TryGet(Guid gameId);
}
