using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Notification.Notifications;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Stages.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Stages.StartRound.SubStages;
public class WaitForReady : IGameStage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IGameConnection _gameConnection;
    private readonly IPlayersManager _players;
    private readonly IGameTime _time;

    private readonly TimeSpan _maxWaitTime = TimeSpan.FromSeconds(20);
    private int _readyPlayersCnt = 0;

    private IEnumerable<IGamePlayer> Players => _players.Players;

    private IEnumerable<IGamePlayer> ReadyPlayers => Players.Where(p => p.Ready);

    private bool TimePassed => _time.StageTime > _maxWaitTime;

    private bool AllPlayersReady => _readyPlayersCnt == Players.Count();

    public bool IsDone => TimePassed || AllPlayersReady;

    public WaitForReady(
        IServiceProvider serviceProvider,
        IGameConnection gameConnection,
        IPlayersManager players,
        IGameTime time)
    {
        _serviceProvider = serviceProvider;
        _gameConnection = gameConnection;
        _players = players;
        _time = time;
    }

    public async Task Run(CancellationToken token)
    {
        await CheckConnections();
    }

    private async Task CheckConnections()
    {
        var readyPlayersCnt = ReadyPlayers.ToArray();
        if (_readyPlayersCnt != readyPlayersCnt.Count())
        {
            _readyPlayersCnt = readyPlayersCnt.Count();
            await SendReadyNotification(readyPlayersCnt);
        }
    }

    private async Task SendReadyNotification(IGamePlayer[] readyPlayers)
    {
        var readyPlayersIds = readyPlayers.Select(p => p.Id.ToString());
        await _gameConnection.SendNotification(new PlayersReadydNotification { Data = readyPlayersIds.ToList() });
    }

    public IGameStage? Next()
    {
        return _serviceProvider.GetRequiredService<CountDown>();
    }
}
