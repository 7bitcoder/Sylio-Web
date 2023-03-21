using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Notification.Notifications;
using Backend.Application.Games.Stages.Base;
using Backend.Application.Games.Stages.Enums;
using Backend.Application.Games.Stages.Interfaces;
using Backend.Application.Games.Stages.Round;
using Backend.Application.Games.Stages.StartRound.SubStages;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Stages.StartRound;
public class StartRoundStage : GameStagesBase, IGameStage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IGameConnection _connection;

    public StartRoundStage(
        IServiceProvider serviceProvider,
        IGameConnection connection,
        IGameTime time,
        InitRound stage) : base(time, stage)
    {
        _serviceProvider = serviceProvider;
        _connection = connection;
        _connection.SendNotification(new StageChangedNotification { Data = StageType.StartRound });
    }

    public IGameStage? Next()
    {
        return _serviceProvider.GetRequiredService<RoundStage>();
    }
}
