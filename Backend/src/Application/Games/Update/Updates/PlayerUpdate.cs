using Backend.Application.Games.Update.Enums;
using Backend.Application.Games.Update.Interfaces;

namespace Backend.Application.Games.Update.Updates;
public class PlayerUpdate : IGameUpdate
{
    public GameUpdateType T => GameUpdateType.Player;

    public int Id { get; set; }

    public bool V { get; set; }

    public bool B { get; set; }

    public bool A { get; set; }

    public float X { get; set; }

    public float Y { get; set; }

    public float R { get; set; }

    public string C { get; set; } = default!;

    public int S { get; set; }
}