using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerShrinkEffect : EffectBase<IGamePlayer>
{
    private const float SHRINK_FACTOR = 0.5f;
    private const float MIN_RADIOUS = 5;

    public override void apply(IGamePlayer player)
    {
        player.Radious *= SHRINK_FACTOR;
        if (player.Radious < MIN_RADIOUS)
        {
            player.Radious = MIN_RADIOUS;
        }
    }
}
