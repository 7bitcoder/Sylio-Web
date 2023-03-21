using Backend.Application.Common.Models;
using Backend.Application.Actions.Players.Commands.DeletePlayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Application.Actions.Players.Commands.UpdatePlayer;
using Backend.Application.Actions.Players.Commands.CreatePlayer;
using Backend.Application.Actions.Players.Queries.GetPLayer;
using Backend.Application.Actions.Players.Queries.GetPlayers;
using Backend.Application.Actions.PlayerRoundStats.Queries.GetPlayerRoundStats;
using Backend.Application.Actions.PlayerRoundStats.Queries.GetRoundPlayersStats;
using Backend.Application.Actions.PlayerRoundStats.Queries.GetPlayerRoundsStats;

namespace WebApi.Controllers;

[Authorize]
public class PlayerRoundStatsController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerRoundStatDto>> Get(int id, CancellationToken token)
    {
        return await Mediator.Send(new GetPlayerRoundStatsQuery { Id = id }, token);
    }

    [HttpGet("round/{id}")]
    public async Task<ActionResult<List<PlayerRoundStatDto>>> GetForRound(int id, CancellationToken token)
    {
        return await Mediator.Send(new GetRoundPlayersStatsQuery { RoundId = id }, token);
    }

    [HttpGet("player/{id}")]
    public async Task<ActionResult<List<PlayerRoundStatDto>>> GetForPlayer(Guid id, CancellationToken token)
    {
        return await Mediator.Send(new GetPlayerRoundsStatsQuery { PlayerId = id }, token);
    }
}
