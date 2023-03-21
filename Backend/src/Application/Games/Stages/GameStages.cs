using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Stages.Base;
using Backend.Application.Games.Stages.InitGame;

namespace Backend.Application.Games.Stages;
public class GameStages : GameStagesBase
{
    public GameStages(
        IGameTime time,
        InitGameStage stage) : base(time, stage)
    {
    }
}
