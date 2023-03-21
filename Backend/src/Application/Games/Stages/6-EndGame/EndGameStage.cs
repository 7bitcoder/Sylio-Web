using Backend.Application.Actions.Games.Commands.UpdateGameState;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Notification.Notifications;
using Backend.Application.Games.Stages.Enums;
using Backend.Application.Games.Stages.Interfaces;
using MediatR;

namespace Backend.Application.Games.Stages.EndGame;
public class EndGameStage : IGameStage
{
    private readonly IMediator _mediator;
    private readonly IGameContext _gameContext;
    private readonly IGameConnection _connection;

    public bool IsDone { get; private set; }

    public EndGameStage(
        IMediator mediator,
        IGameContext gameContext,
        IGameConnection connection)
    {
        _mediator = mediator;
        _gameContext = gameContext;
        _connection = connection;
        _connection.SendNotification(new StageChangedNotification { Data = StageType.EndGame });

    }

    public async Task Run(CancellationToken token)
    {
        await UpdateGameState();
        IsDone = true;
    }

    private async Task UpdateGameState()
    {
        await _mediator.Send(new UpdateGameStateCommand { Id = _gameContext.GameId, GameState = Domain.Enums.GameState.Finished });
    }

    public IGameStage? Next()
    {
        return null;
    }
}
