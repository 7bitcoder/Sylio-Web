using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerLongerGapsEffect : EffectBase<IGamePlayer>
{
    private const float GROW_FACTOR = 1.5f;
    private const float MAX = 150;

    public override void apply(IGamePlayer player)
    {
        player.AverageGapLength *= GROW_FACTOR;
        if (player.AverageGapLength > MAX)
        {
            player.AverageGapLength = MAX;
        }
    }
}
