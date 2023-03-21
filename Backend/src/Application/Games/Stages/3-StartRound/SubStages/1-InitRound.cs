using Backend.Application.Actions.Rounds.Commands.CreateRound;
using Backend.Application.Actions.Rounds.Queries.GetRound;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Notification.Notifications;
using Backend.Application.Games.Stages.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Stages.StartRound.SubStages;
public class InitRound : IGameStage
{
    private readonly IMediator _mediator;
    private readonly IGameContext _gameContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly IGameConnection _gameConnection;

    public bool IsDone { get; set; }

    public InitRound(
        IMediator mediator,
        IGameContext gameContext,
        IServiceProvider serviceProvider,
        IGameConnection gameConnection)
    {
        _mediator = mediator;
        _gameContext = gameContext;
        _serviceProvider = serviceProvider;
        _gameConnection = gameConnection;
    }

    public async Task Run(CancellationToken token)
    {
        await CreateAndSetNewRound(token);
        await SendSetupNotification();
        IsDone = true;
    }

    private async Task CreateAndSetNewRound(CancellationToken token)
    {
        var oldRoundNumber = _gameContext.CurrentRound?.RoundNumber ?? 0;
        var newRoundNumber = oldRoundNumber + 1;

        var id = await _mediator.Send(new CreateRoundCommand { GameId = _gameContext.GameId, RoundNumber = newRoundNumber });

        _gameContext.CurrentRound = await _mediator.Send(new GetRoundQuery { Id = id }, token);
    }

    private async Task SendSetupNotification()
    {
        await _gameConnection.SendNotification(new RoundSetupNotification
        {
            Data = new RoundSetupNotification.Setup
            {
                RoundNumber = _gameContext.CurrentRound.RoundNumber
            }
        });
    }

    public IGameStage? Next()
    {
        return _serviceProvider.GetRequiredService<Reset>();
    }
}
