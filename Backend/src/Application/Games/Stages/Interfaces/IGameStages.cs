namespace Backend.Application.Games.Stages.Interfaces;
public interface IGameStages
{
    bool IsDone { get; }

    Task Run(CancellationToken token);
}
