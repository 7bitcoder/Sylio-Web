using Backend.Application.Games.Update.Enums;
using Backend.Application.Games.Update.Interfaces;
using Backend.Domain.Enums;

namespace Backend.Application.Games.Update.Updates;
public class PowerUpUpdate : IGameUpdate
{
    public GameUpdateType T => GameUpdateType.PowerUp;

    public int Id { get; set; }

    public bool S { get; set; }

    public float X { get; set; }

    public float Y { get; set; }

    public PowerUpType U { get; set; }
}