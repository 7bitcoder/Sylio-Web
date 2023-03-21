using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Notification.Notifications;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Stages.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Stages.InitGame.SubStages;
public class WaitForConnections : IGameStage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IGameConnection _gameConnection;
    private readonly IPlayersManager _players;
    private readonly IGameTime _time;

    private readonly TimeSpan _maxWaitTime = TimeSpan.FromSeconds(20);
    private int _connectedPlayersCnt = 0;

    private IEnumerable<IGamePlayer> Players => _players.Players;

    private IEnumerable<IGamePlayer> ConnectedPlayers => Players.Where(p => p.Connected);

    private bool TimePassed => _time.StageTime > _maxWaitTime;

    private bool AllPlayersConnected => _connectedPlayersCnt == Players.Count();

    public bool IsDone => TimePassed || AllPlayersConnected;

    public WaitForConnections(
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
        var connectedPlayers = ConnectedPlayers.ToArray();
        if (_connectedPlayersCnt != connectedPlayers.Count())
        {
            _connectedPlayersCnt = connectedPlayers.Count();
            await SendConnectedNotification(connectedPlayers);
        }
    }

    private async Task SendConnectedNotification(IGamePlayer[] connectedPlayers)
    {
        var connectedPlayerIds = connectedPlayers.Select(p => p.Id.ToString());
        await _gameConnection.SendNotification(new PlayersConnectedNotification { Data = connectedPlayerIds.ToList() });
    }

    public IGameStage? Next()
    {
        return _serviceProvider.GetRequiredService<SetupGame>();
    }
}
