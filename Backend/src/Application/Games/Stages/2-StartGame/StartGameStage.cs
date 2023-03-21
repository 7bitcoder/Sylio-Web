using Backend.Application.Actions.Games.Commands.UpdateGameState;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Notification.Notifications;
using Backend.Application.Games.Stages.Enums;
using Backend.Application.Games.Stages.Interfaces;
using Backend.Application.Games.Stages.StartRound;
using Backend.Domain.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Stages.StartGame;
public class StartGameStage : IGameStage
{
    private readonly IMediator _mediator;
    private readonly IGameContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly IGameConnection _connection;

    public StartGameStage(
        IMediator mediator,
        IGameContext context,
        IServiceProvider serviceProvider,
        IGameConnection connection)
    {
        _mediator = mediator;
        _context = context;
        _serviceProvider = serviceProvider;
        _connection = connection;
        _connection.SendNotification(new StageChangedNotification { Data = StageType.StartGame });
    }

    public bool IsDone { get; private set; }

    public async Task Run(CancellationToken token)
    {
        await UpdateGameState();
        IsDone = true;
    }

    private async Task UpdateGameState()
    {
        await _mediator.Send(new UpdateGameStateCommand { Id = _context.GameId, GameState = GameState.Running });
    }

    public IGameStage? Next()
    {
        return _serviceProvider.GetRequiredService<StartRoundStage>();
    }
}
