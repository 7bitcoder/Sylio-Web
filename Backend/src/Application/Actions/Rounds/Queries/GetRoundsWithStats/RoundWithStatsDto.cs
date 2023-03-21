using Backend.Application.Actions.PlayerRoundStats.Queries.GetPlayerRoundStats;
using Backend.Application.Common.Mappings;
using Backend.Domain.Entities;

namespace Backend.Application.Actions.Rounds.Queries.GetRoundsWithStats;

public class RoundWithStatsDto : IMapFrom<Round>
{
    public int Id { get; set; }

    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public int RoundNumber { get; set; }

    public List<PlayerRoundStatDto> PlayersRoundStats { get; set; } = default!;
}
