using Backend.Application.Games.Effect.Base;
using Backend.Application.Games.Effect.Interfaces;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Player.Interfaces;

namespace Backend.Application.Games.Effect;

public class PlayerEffectsManager : EffectsManagerBase<IGamePlayer>, IPlayerEffectsManager
{
    public PlayerEffectsManager(IGameTime time) : base(time)
    {
    }
}
