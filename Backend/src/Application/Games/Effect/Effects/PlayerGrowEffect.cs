using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerGrowEffect : EffectBase<IGamePlayer>
{
    private const float GROW_FACTOR = 1.5f;
    private const float MAX_RADIOUS = 40;

    public override void apply(IGamePlayer player)
    {
        player.Radious *= GROW_FACTOR;
        if (player.Radious > MAX_RADIOUS)
        {
            player.Radious = MAX_RADIOUS;
        }
    }
}
