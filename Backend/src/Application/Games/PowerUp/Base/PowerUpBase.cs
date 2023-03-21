using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.PowerUp.Interfaces;
using Backend.Application.Games.Update.Updates;
using Backend.Domain.Enums;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Games.PowerUp.Base;

public abstract class PowerUpBase : IPowerUp
{
    public PowerUpType Type { get; set; }

    public int Id { get; set; }

    public Point Position { get; set; }

    public float Radious { get; set; } = 25;

    public abstract void Activate(IGamePlayer activator);

    public TimeSpan RandomDuration(int min = 5, int max = 10)
    {
        return TimeSpan.FromSeconds(Random.Shared.Next(min, max));
    }
}
