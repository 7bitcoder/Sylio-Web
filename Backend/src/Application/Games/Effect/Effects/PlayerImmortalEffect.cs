using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerImmortalEffect : EffectBase<IGamePlayer>
{
    public override void apply(IGamePlayer player)
    {
        player.Immortal = true;
    }
}
