using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerSpeedUpEffect : EffectBase<IGamePlayer>
{
    private const float SPEED_FACTOR = 1.5f;
    private const float MAX_SPEED = 0.3f;

    public override void apply(IGamePlayer player)
    {
        player.Speed *= SPEED_FACTOR;
        if (player.Speed > MAX_SPEED)
        {
            player.Speed = MAX_SPEED;
        }
    }
}
