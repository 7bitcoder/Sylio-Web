using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities;
public class Player : BaseAuditableEntity
{
    public Guid Id { get; set; }

    public string? UserId { get; set; }

    public bool GameAdmin { get; set; }

    public string Name { get; set; } = "Player";

    public string? ConnectionId { get; set; }

    public string Colour { get; set; } = string.Empty;

    public Game Game { get; set; } = default!;

    public List<PlayerRoundStat> RoundsStats { get; set; } = new();
}
