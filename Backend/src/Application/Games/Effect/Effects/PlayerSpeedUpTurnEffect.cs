using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerSpeedUpTurnEffect : EffectBase<IGamePlayer>
{
    private const float SPEED_FACTOR = 1.5f;
    private const float MAX_SPEED = 0.03f;

    public override void apply(IGamePlayer player)
    {
        player.TurnSpeed *= SPEED_FACTOR;
        if (player.TurnSpeed > MAX_SPEED)
        {
            player.TurnSpeed = MAX_SPEED;
        }
    }
}
