using Backend.Domain.ValueObjects;

namespace Backend.Application.Games.Player.Models;

public record class PlayerState(bool Visible, Point Position, float Radious, string Color)
{
}
