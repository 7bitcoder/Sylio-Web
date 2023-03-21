using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Update.Updates;
using Backend.Domain.Enums;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Games.PowerUp.Interfaces;
public interface IPowerUp
{
    int Id { get; }

    PowerUpType Type { get; }

    Point Position { get; set; }

    float Radious { get; set; }

    void Activate(IGamePlayer activator);
}
