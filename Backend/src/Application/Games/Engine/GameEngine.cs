using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Stages.Interfaces;

namespace Backend.Application.Games.Engine;
internal class GameEngine : IGameEngine
{
    private const int TICK_RATE = 64;
    private readonly TimeSpan TICK_TIME = TimeSpan.FromSeconds(1.0 / TICK_RATE);

    private readonly IGameStages _stages;
    private readonly IGameContext _gameContext;
    private readonly IGameWatchDog _watchDog;
    private readonly IPlayersManager _playerPlayersManager;
    private readonly IGameTime _time;

    public GameEngine(
        IGameStages gameStages,
        IGameContext gameContext,
        IGameWatchDog watchDog,
        IPlayersManager playerPlayersManager,
        IGameTime time)
    {
        _stages = gameStages;
        _gameContext = gameContext;
        _watchDog = watchDog;
        _playerPlayersManager = playerPlayersManager;
        _time = time;
    }

    public IPlayerActionsObserver? GetPlayerActionsObserver(Guid playerId)
    {
        return _playerPlayersManager.GetPlayer(playerId);
    }

    public async Task Run(Guid gameId, CancellationToken token)
    {
        _gameContext.GameId = gameId;

        while (!token.IsCancellationRequested && !_stages.IsDone && !_watchDog.Triggered)
        {
            _time.Restart();

            await _stages.Run(token);

            await WaitForTickTime(token);
        }
    }

    private Task WaitForTickTime(CancellationToken token)
    {
        TimeSpan delay = TICK_TIME - _time.CurrentFrameTime;
        if (delay > TimeSpan.Zero)
        {
            return Task.Delay(delay, token);
        }
        return Task.CompletedTask;
    }
}
