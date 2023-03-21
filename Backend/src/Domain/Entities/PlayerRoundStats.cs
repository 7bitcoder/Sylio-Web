namespace Backend.Domain.Entities;
public class PlayerRoundStat : BaseAuditableEntity
{
    public int Id { get; set; }

    public Player Player { get; set; } = default!;

    public Round Round { get; set; } = default!;

    public int Score { get; set; }

    public int Place { get; set; }

    public int Length { get; set; }

    public TimeSpan LiveTime { get; set; }

    public Player? KilledBy { get; set; }
}
