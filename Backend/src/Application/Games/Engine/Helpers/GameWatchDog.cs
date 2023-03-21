using Backend.Application.Games.Engine.Interfaces;

namespace Backend.Application.Games.Engine.Helpers;
public class GameWatchDog : IGameWatchDog
{
    private readonly TimeSpan _maxGameTime = TimeSpan.FromMinutes(120);

    private readonly IGameTime _gameTime;

    public GameWatchDog(IGameTime gameTime)
    {
        _gameTime = gameTime;
    }

    public bool Triggered => _gameTime.TotalTime > _maxGameTime;
}
