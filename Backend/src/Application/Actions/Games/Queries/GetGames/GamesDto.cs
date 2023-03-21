using Backend.Application.Actions.Games.Queries.GetGame;

namespace Backend.Application.Actions.Games.Queries.GetGames;

public class GamesDto
{
    public int Count { get; set; }

    public List<GameDto> Games { get; set; } = default!;
}
