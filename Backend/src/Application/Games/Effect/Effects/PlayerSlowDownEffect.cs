using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerSlowDownEffect : EffectBase<IGamePlayer>
{
    private const float SPEED_FACTOR = 0.5f;
    private const float MIN_SPEED = 0.01f;

    public override void apply(IGamePlayer player)
    {
        player.Speed *= SPEED_FACTOR;
        if (player.Speed < MIN_SPEED)
        {
            player.Speed = MIN_SPEED;
        }
    }
}
