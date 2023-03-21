using Backend.Application.Common.Interfaces;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Update.Interfaces;

namespace Backend.Application.Games.Engine;

public class GameConnection : IGameConnection
{
    private readonly IGameContext _context;

    private readonly IGameHubManager _manager;

    private string _gameId = "";

    public GameConnection(
        IGameContext context,
        IGameHubManager connectionManager)
    {
        _context = context;
        _manager = connectionManager;
    }

    public Task Init(CancellationToken token)
    {
        _gameId = _context.Game.Id.ToString();
        return Task.CompletedTask;
    }

    public Task SendUpdate(IEnumerable<IGameUpdate> updates)
    {
        var update = updates.Cast<object>().ToArray();
        if (update.Length != 0)
        {
            return _manager.SendUpdate(_gameId, update);
        }
        return Task.CompletedTask;
    }

    public Task SendNotification(IGameNotification notification)
    {
        return _manager.SendNotification(_gameId, notification);
    }
}
