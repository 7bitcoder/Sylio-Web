using Backend.Application.Actions.PlayerRoundStats.Queries.GetPlayerRoundStats;
using Backend.Application.Common.Mappings;
using Backend.Domain.Entities;

namespace Backend.Application.Actions.Players.Queries.PlayerWithStats;

public class PlayerWithStatsDto : IMapFrom<Player>
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;

    public bool GameAdmin { get; set; }

    public string Name { get; set; } = default!;

    public string? ConnectionId { get; set; }

    public string Colour { get; set; } = default!;

    public List<PlayerRoundStatDto> RoundsStats { get; set; } = default!;
}
