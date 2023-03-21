using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerClearEffect : EffectBase<IGamePlayer>
{
    public override void apply(IGamePlayer player)
    {
        player.LastStates.Clear();
        _timeLeft = TimeSpan.Zero;
    }
}
