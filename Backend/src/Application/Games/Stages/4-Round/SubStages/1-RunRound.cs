using Backend.Application.Games.Engine.Helpers;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Player.Helpers;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.PowerUp.Interfaces;
using Backend.Application.Games.Stages.EndGame;
using Backend.Application.Games.Stages.Interfaces;
using Backend.Application.Games.Update.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Stages.Round.SubStages;
public class RunRound : IGameStage
{
    private readonly IPowerUpsManager _powerUps;
    private readonly IGameTime _gameTime;
    private readonly IGameConnection _gameConnection;
    private readonly IPlayersManager _players;

    private readonly TimeSpan _maxRoundTime = TimeSpan.FromMinutes(60);

    private bool TimePassed => _gameTime.StageTime > _maxRoundTime;

    private bool HaveWinner => _players.AlivePlayers.Count() <= 1;

    public bool IsDone => TimePassed || HaveWinner;

    public RunRound(
        IPowerUpsManager powerUps,
        IGameTime gameTime,
        IGameConnection gameConnection,
        IPlayersManager players)
    {
        _powerUps = powerUps;
        _gameTime = gameTime;
        _gameConnection = gameConnection;
        _players = players;
    }

    public async Task Run(CancellationToken token)
    {
        _players.Update();
        _powerUps.Update();
        IEnumerable<IGameUpdate> update = _players.GetUpdates();
        update = update.Concat(_powerUps.GetUpdates());
        await _gameConnection.SendUpdate(update);
    }

    public IGameStage? Next()
    {
        return null;
    }
}
