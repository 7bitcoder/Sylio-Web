
namespace Backend.Domain.Entities;
public class Game : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public string? UserId { get; set; }  

    public int RoundsNumber { get; set; }

    public string Title { get; set; } = string.Empty;

    public GameState State { get; set; }

    public bool IsPublic { get; set; }

    public int MaxPlayers { get; set; }

    //public Guid? JoinToken { get; set; }

    public List<PowerUpType> BlockedPowerUps { get; set; } = new();

    public List<Player> Players { get; set; } = new();

    public List<Round> Rounds { get; set; } = new();
}
