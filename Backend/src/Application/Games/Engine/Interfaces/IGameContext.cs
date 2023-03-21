using Backend.Application.Actions.Games.Queries.GetGame;
using Backend.Application.Actions.Rounds.Queries.GetRound;

namespace Backend.Application.Games.Engine.Interfaces;
public interface IGameContext
{
    public Guid GameId { get; set; }

    public GameDto Game { get; set; }

    public RoundDto CurrentRound { get; set; }
}
