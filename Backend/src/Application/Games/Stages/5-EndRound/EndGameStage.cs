using Backend.Application.Actions.PlayerRoundStats.Commands.CreatePlayerRoundStats;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Notification.Notifications;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Stages.Enums;
using Backend.Application.Games.Stages.Interfaces;
using Backend.Application.Games.Stages.StartRound;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Stages.EndGame;
public class EndRoundStage : IGameStage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMediator _mediator;
    private readonly IGameConnection _connection;
    private readonly IGameContext _gameContext;
    private readonly IPlayersManager _players;

    public bool IsDone { get; private set; }

    public EndRoundStage(
        IServiceProvider serviceProvider,
        IMediator mediator,
        IGameConnection connection,
        IGameContext gameContext,
        IPlayersManager players)
    {
        _serviceProvider = serviceProvider;
        _mediator = mediator;
        _connection = connection;
        _gameContext = gameContext;
        _players = players;
        _connection.SendNotification(new StageChangedNotification { Data = StageType.EndRound });
    }

    public async Task Run(CancellationToken token)
    {
        await CreateRoundStatistics();
        IsDone = true;
    }

    private async Task CreateRoundStatistics()
    {
        foreach (var player in _players.Players)
        {
            await _mediator.Send(new CreatePlayerRoundStatsCommand
            {
                RoundId = _gameContext.CurrentRound.Id,
                PlayerId = player.Id,
                Place = player.Place,
                Score = player.RoundScore,
                LiveTime = player.LiveTime,
                KilledByPlayerId = player.KilledByPlayer?.Id,
                Length = (int)player.Length,
            });
        }
    }

    public IGameStage? Next()
    {
        if (_gameContext.CurrentRound.RoundNumber >= _gameContext.Game.RoundsNumber)
        {
            return _serviceProvider.GetRequiredService<EndGameStage>();
        }
        return _serviceProvider.GetRequiredService<StartRoundStage>();
    }
}
