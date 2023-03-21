namespace Backend.Application.Games.Engine.Interfaces;
public interface IGameTime
{
    void Restart();

    void RestartStageTime();

    TimeSpan LastFrameTime { get; }

    TimeSpan CurrentFrameTime { get; }

    TimeSpan TotalTime { get; }

    TimeSpan StageTime { get; }
}
