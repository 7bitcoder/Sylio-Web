using Backend.Application.Games.Effect.Interfaces;

namespace Backend.Application.Games.Effect.Base;

public abstract class EffectBase<T> : IEffect<T>
{
    protected TimeSpan _timeLeft;
    protected TimeSpan _duration;

    public EffectBase()
    {

    }

    public TimeSpan Duration
    {
        get => _duration;
        init
        {
            _timeLeft = value;
            _duration = value;
        }
    }

    public bool IsActive => _timeLeft > TimeSpan.Zero;

    public void addTime(TimeSpan time)
    {
        _timeLeft -= time;
    }

    public abstract void apply(T state);
}
