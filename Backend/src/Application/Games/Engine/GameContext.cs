using Backend.Application.Actions.Games.Queries.GetGame;
using Backend.Application.Actions.Rounds.Queries.GetRound;
using Backend.Application.Games.Engine.Interfaces;

namespace Backend.Application.Games.Engine;
public class GameContext : IGameContext
{
    public Guid GameId { get; set; }

    public GameDto Game { get; set; } = default!;

    public RoundDto CurrentRound { get; set; } = default!;
}
