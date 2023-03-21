using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Stages.Interfaces;

namespace Backend.Application.Games.Stages.Base;
public abstract class GameStagesBase : IGameStages
{
    protected IGameStage? _stage;
    protected IGameTime _time;

    public GameStagesBase(IGameTime time, IGameStage initStage)
    {
        _stage = initStage;
        _time = time;
    }

    public bool IsDone => _stage is null;

    public Task Run(CancellationToken token)
    {
        if (_stage?.IsDone ?? false)
        {
            _stage = _stage.Next();
            OnNewStage();
        }
        return _stage?.Run(token) ?? Task.CompletedTask;
    }

    public void OnNewStage()
    {
        _time.RestartStageTime();
    }
}
