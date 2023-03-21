using Backend.Application.Games.Effect.Interfaces;
using Backend.Application.Games.Engine.Interfaces;

namespace Backend.Application.Games.Effect.Base;
public class EffectsManagerBase<T> : IEffectsManager<T>
{
    private readonly List<IEffect<T>> _effects = new();

    private readonly IGameTime _time;

    public EffectsManagerBase(IGameTime time)
    {
        _time = time;
    }

    public void Add(IEffect<T> effect)
    {
        _effects.Add(effect);
    }

    public bool Apply(T state)
    {
        bool activated = false;
        foreach (var effect in _effects)
        {
            effect.addTime(_time.LastFrameTime);
            if (effect.IsActive)
            {
                effect.apply(state);
                activated = true;
            }
        }
        _effects.RemoveAll(eff => !eff.IsActive);
        return activated;
    }

    public void Clear()
    {
        _effects.Clear();
    }
}
