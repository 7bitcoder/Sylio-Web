using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities;
public class Round : BaseAuditableEntity
{
    public int Id { get; set; }

    public Game Game { get; set; } = default!;

    public int RoundNumber { get; set; }

    public List<PlayerRoundStat> PlayersRoundStats { get; set; } = new();
}
