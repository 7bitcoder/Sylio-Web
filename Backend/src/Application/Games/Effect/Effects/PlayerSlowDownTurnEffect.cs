using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerSlowDownTurnEffect : EffectBase<IGamePlayer>
{
    private const float SPEED_FACTOR = 0.5f;
    private const float MIN_SPEED = 0.00003f;

    public override void apply(IGamePlayer player)
    {
        player.TurnSpeed *= SPEED_FACTOR;
        if (player.TurnSpeed < MIN_SPEED)
        {
            player.TurnSpeed = MIN_SPEED;
        }
    }
}
