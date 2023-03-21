using Backend.Application.Games.Player.Helpers;
using Backend.Application.Games.Update.Updates;

namespace Backend.Application.Games.Player.Interfaces;

public interface IPlayersManager
{
    Task Init(CancellationToken token);

    void Update();

    void Reset();

    void Clear();

    IGamePlayer? GetPlayer(Guid playerId);

    IEnumerable<IGamePlayer> Players { get; }

    IEnumerable<IGamePlayer> AlivePlayers { get; }

    IEnumerable<PlayerUpdate> GetUpdates();
}