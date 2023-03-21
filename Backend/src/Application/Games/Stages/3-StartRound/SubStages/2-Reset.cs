using Backend.Application.Games.Board.Interfaces;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.PowerUp.Interfaces;
using Backend.Application.Games.Stages.Interfaces;
using Backend.Application.Games.Update.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Stages.StartRound.SubStages;
public class Reset : IGameStage
{
    private readonly IPowerUpsManager _powerUps;
    private readonly IGameBoard _board;
    private readonly IServiceProvider _serviceProvider;
    private readonly IGameConnection _gameConnection;
    private readonly IPlayersManager _players;

    public bool IsDone { get; private set; }

    public Reset(

        IPowerUpsManager powerUps,
        IGameBoard board,
        IServiceProvider serviceProvider,
        IGameConnection gameConnection,
        IPlayersManager players)
    {
        _powerUps = powerUps;
        _board = board;
        _serviceProvider = serviceProvider;
        _gameConnection = gameConnection;
        _players = players;
    }

    public async Task Run(CancellationToken token)
    {
        ResetComponents();
        await InitUpdate();
        IsDone = true;
    }

    private void ResetComponents()
    {
        _board.Reset();
        _players.Reset();
        _powerUps.Reset();
    }

    private async Task InitUpdate()
    {
        IEnumerable<IGameUpdate> update = _players.GetUpdates();
        await _gameConnection.SendUpdate(update);
    }

    public IGameStage? Next()
    {
        return _serviceProvider.GetRequiredService<WaitForReady>();
    }
}
