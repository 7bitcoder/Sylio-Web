namespace Backend.Application.Games.Effect.Interfaces;
public interface IEffect<T>
{
    TimeSpan Duration { get; init; }

    bool IsActive { get; }

    void addTime(TimeSpan time);

    void apply(T state);
}
