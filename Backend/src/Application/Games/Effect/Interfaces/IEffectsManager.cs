namespace Backend.Application.Games.Effect.Interfaces;
public interface IEffectsManager<T>
{
    void Add(IEffect<T> effect);

    bool Apply(T state);

    void Clear();
}
