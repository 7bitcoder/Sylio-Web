using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Notification.Notifications;
using Backend.Application.Games.Stages.Base;
using Backend.Application.Games.Stages.Enums;
using Backend.Application.Games.Stages.InitGame.SubStages;
using Backend.Application.Games.Stages.Interfaces;
using Backend.Application.Games.Stages.StartGame;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Stages.InitGame;
public class InitGameStage : GameStagesBase, IGameStage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IGameConnection _connection;

    public InitGameStage(
        IServiceProvider serviceProvider,
        IGameConnection connection,
        IGameTime time,
        LoadDataFromDatabase stage) : base(time, stage)
    {
        _serviceProvider = serviceProvider;
        _connection = connection;
        _connection.SendNotification(new StageChangedNotification { Data = StageType.InitGame });
    }


    public IGameStage? Next()
    {
        return _serviceProvider.GetRequiredService<StartGameStage>();
    }
}
