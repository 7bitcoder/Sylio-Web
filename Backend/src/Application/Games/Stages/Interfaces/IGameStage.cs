namespace Backend.Application.Games.Stages.Interfaces;

public interface IGameStage
{
    bool IsDone { get; }

    Task Run(CancellationToken token);

    IGameStage? Next();
}