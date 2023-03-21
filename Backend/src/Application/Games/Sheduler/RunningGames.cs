using System.Collections.Concurrent;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Sheduler.Interfaces;

namespace Backend.Application.Games.Sheduler;

public class RunningGames : IRunningGames
{
    private readonly ConcurrentDictionary<Guid, IGameEngine> _runningGames = new();

    public IDisposable Add(Guid gameId, IGameEngine gameEngine)
    {
        if (!_runningGames.TryAdd(gameId, gameEngine))
        {
            throw new InvalidOperationException("Could not register game");
        }
        return new GameRemover(gameId, this);
    }

    public void Remove(Guid gameId)
    {
        if (!_runningGames.TryRemove(gameId, out var _))
        {
            throw new InvalidOperationException("Could not unregister game");
        }
    }

    public IGameEngine? TryGet(Guid gameId)
    {
        if (_runningGames.TryGetValue(gameId, out var engine))
        {
            return engine;
        }
        return null;
    }

    public class GameRemover : IDisposable
    {
        private readonly Guid _gameId;
        private readonly RunningGames _runningGames;

        public GameRemover(Guid gameId, RunningGames runningGames)
        {
            _gameId = gameId;
            _runningGames = runningGames;
        }

        public void Dispose()
        {
            _runningGames.Remove(_gameId);
        }
    }
}
