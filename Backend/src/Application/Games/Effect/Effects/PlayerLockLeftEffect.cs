using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Player.Interfaces;
using Backend.Domain.Enums;

namespace Backend.Application.Games.Effect.Effects;
public class PlayerLockLeftEffect : EffectBase<IGamePlayer>
{
    public override void apply(IGamePlayer player)
    {
        player.KeyPressed &= ~KeyType.Left;
    }
}
