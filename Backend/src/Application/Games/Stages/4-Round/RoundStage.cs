using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Notification.Notifications;
using Backend.Application.Games.Stages.Base;
using Backend.Application.Games.Stages.EndGame;
using Backend.Application.Games.Stages.Enums;
using Backend.Application.Games.Stages.Interfaces;
using Backend.Application.Games.Stages.Round.SubStages;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Stages.Round;
public class RoundStage : GameStagesBase, IGameStage
{
    private readonly IGameConnection _connection;
    private readonly IServiceProvider _serviceProvider;

    public RoundStage(
        IGameConnection connection,
        IServiceProvider serviceProvider,
        IGameTime time,
        RunRound stage) : base(time, stage)
    {
        _connection = connection;
        _serviceProvider = serviceProvider;
        _connection.SendNotification(new StageChangedNotification { Data = StageType.Round });
    }

    public IGameStage? Next()
    {
        return _serviceProvider.GetRequiredService<EndRoundStage>();
    }
}
