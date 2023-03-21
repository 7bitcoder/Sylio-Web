using Backend.Application.Common.Models;
using Backend.Application.Actions.Players.Commands.DeletePlayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Application.Actions.Players.Commands.UpdatePlayer;
using Backend.Application.Actions.Players.Commands.CreatePlayer;
using Backend.Application.Actions.Players.Queries.GetPLayer;
using Backend.Application.Actions.Players.Queries.GetPlayers;
using Backend.Application.Actions.Rounds.Queries.GetRounds;
using Backend.Application.Actions.Rounds.Queries.GetRound;
using Backend.Application.Actions.Rounds.Queries.GetRoundsWithStats;

namespace WebApi.Controllers;

[Authorize]
public class RoundsController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<RoundDto>> Get(int id, CancellationToken token)
    {
        return await Mediator.Send(new GetRoundQuery { Id = id }, token);
    }

    [HttpGet("game/{id}")]
    public async Task<ActionResult<List<RoundDto>>> GetForGame(Guid id, CancellationToken token)
    {
        return await Mediator.Send(new GetRoundsQuery { GameId = id }, token);
    }

    [HttpGet("game/{id}/stats")]
    public async Task<ActionResult<List<RoundWithStatsDto>>> GetForGameWithStats(Guid id, CancellationToken token)
    {
        return await Mediator.Send(new GetRoundsWithStatsQuery { GameId = id }, token);
    }
}
