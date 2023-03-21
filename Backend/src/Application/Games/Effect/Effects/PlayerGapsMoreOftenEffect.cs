using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerGapsMoreOftenEffect : EffectBase<IGamePlayer>
{
    private const float GROW_FACTOR = 0.5f;
    private const float MIN = 50;

    public override void apply(IGamePlayer player)
    {
        player.AverageLength *= GROW_FACTOR;
        if (player.AverageLength < MIN)
        {
            player.AverageLength = MIN;
        }
    }
}
