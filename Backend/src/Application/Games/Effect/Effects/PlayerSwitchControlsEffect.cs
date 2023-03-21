using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;
using Backend.Domain.Enums;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerSwitchControlsEffect : EffectBase<IGamePlayer>
{
    public override void apply(IGamePlayer player)
    {
        if (player.KeyPressed == KeyType.Left)
        {
            player.KeyPressed = KeyType.Right;
        }
        else if (player.KeyPressed == KeyType.Right)
        {
            player.KeyPressed = KeyType.Left;
        }
    }
}
