using Backend.Application.Games.Effect.Interfaces;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.PowerUp.Base;

namespace Backend.Application.Games.PowerUp.PowerUps;
public class PlayerPowerUp<T> : PowerUpBase where T : IEffect<IGamePlayer>, new()
{
    private readonly IPlayersManager _manager;

    private Affect _affect = Affect.Player;

    public PlayerPowerUp(IPlayersManager manager)
    {
        _manager = manager;
    }

    public override void Activate(IGamePlayer activator)
    {
        IEnumerable<IGamePlayer> players = _affect switch
        {
            Affect.Player => AsOne(activator),
            Affect.AllPlayers => _manager.Players,
            Affect.OtherPlayers => _manager.Players.Where(p => p != activator),
            _ => AsOne(activator),
        };
        foreach (var player in players)
        {
            player.AddEffect(new T() { Duration = RandomDuration() });
        }
    }

    private PlayerPowerUp<T> WithAffect(Affect affect)
    {
        _affect = affect;
        return this;
    }

    public PlayerPowerUp<T> ForOthers() => WithAffect(Affect.OtherPlayers);

    public PlayerPowerUp<T> ForAll() => WithAffect(Affect.AllPlayers);

    private IEnumerable<IGamePlayer> AsOne(IGamePlayer player)
    {
        yield return player;
    }

    private enum Affect
    {
        Player,
        OtherPlayers,
        AllPlayers
    }
}
