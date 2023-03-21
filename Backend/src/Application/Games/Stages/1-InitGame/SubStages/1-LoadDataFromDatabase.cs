using Backend.Application.Actions.Games.Queries.GetGame;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Stages.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Stages.InitGame.SubStages;
public class LoadDataFromDatabase : IGameStage
{
    private readonly IMediator _mediator;
    private readonly IPlayersManager _playersManager;
    private readonly IGameContext _gameContext;
    private readonly IGameConnection _connection;
    private readonly IServiceProvider _serviceProvider;

    public bool IsDone { get; private set; }

    public LoadDataFromDatabase(
        IMediator mediator,
        IPlayersManager playersManager,
        IGameContext gameContext,
        IGameConnection connection,
        IServiceProvider serviceProvider)
    {
        _mediator = mediator;
        _playersManager = playersManager;
        _gameContext = gameContext;
        _connection = connection;
        _serviceProvider = serviceProvider;
    }

    public async Task Run(CancellationToken token)
    {
        var game = await GetGame(token);
        await Init(game, token);
        IsDone = true;
    }

    private async Task<GameDto> GetGame(CancellationToken token)
    {
        var game = await _mediator.Send(new GetGameQuery { Id = _gameContext.GameId }, token);
        if (game is null)
        {
            throw new ArgumentNullException(nameof(game));
        }
        return game;
    }

    private async Task Init(GameDto game, CancellationToken token)
    {
        // Initialization order matters!!!
        _gameContext.Game = game;
        await _connection.Init(token);
        await _playersManager.Init(token);
    }

    public IGameStage? Next()
    {
        return _serviceProvider.GetRequiredService<WaitForConnections>();
    }
}
