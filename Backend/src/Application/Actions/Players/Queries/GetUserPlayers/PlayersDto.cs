using Backend.Application.Actions.Players.Queries.GetPLayer;

namespace Backend.Application.Actions.Players.Queries.GetUserPlayers;

public class PlayersDto
{
    public int Count { get; set; }
    public List<PlayerDto> Players { get; set; } = default!;
}
