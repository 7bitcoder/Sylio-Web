using System.Diagnostics;
using Backend.Application.Games.Engine.Interfaces;

namespace Backend.Application.Games.Engine;
public class GameTime : IGameTime
{
    private readonly Stopwatch _stopwatch = new();

    private TimeSpan _totalTime = TimeSpan.Zero;

    private TimeSpan _stageTime = TimeSpan.Zero;

    private TimeSpan _lastFrameTime = TimeSpan.Zero;

    public void Restart()
    {
        _lastFrameTime = CurrentFrameTime;
        _totalTime += _lastFrameTime;
        _stageTime += _lastFrameTime;
        _stopwatch.Restart();
    }

    public void RestartStageTime()
    {
        _stageTime = TimeSpan.Zero;
    }

    public TimeSpan LastFrameTime => _lastFrameTime;

    public TimeSpan CurrentFrameTime => _stopwatch.Elapsed;

    public TimeSpan TotalTime => _totalTime;

    public TimeSpan StageTime => _stageTime;
}
