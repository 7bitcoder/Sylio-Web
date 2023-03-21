using Backend.Application.Common.Models;
using Backend.Application.Actions.Players.Commands.DeletePlayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Application.Actions.Players.Commands.UpdatePlayer;
using Backend.Application.Actions.Players.Commands.CreatePlayer;
using Backend.Application.Actions.Players.Queries.GetPLayer;
using Backend.Application.Actions.Players.Queries.GetPlayers;
using Backend.Application.Actions.Games.Queries.GetUserGames;
using Backend.Application.Actions.Players.Queries.GetUserPlayers;
using Backend.Application.Actions.Rounds.Queries.GetRoundsWithStats;
using Backend.Application.Actions.Players.Queries.PlayerWithStats;

namespace WebApi.Controllers;

[Authorize]
public class PlayersController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> Get(Guid id, CancellationToken token)
    {
        return await Mediator.Send(new GetPlayerQuery { Id = id }, token);
    }

    [HttpGet("game/{id}")]
    public async Task<ActionResult<List<PlayerDto>>> GetForGame(Guid id , CancellationToken token)
    {
        return await Mediator.Send(new GetPlayersQuery { GameId = id }, token);
    }

    [HttpGet("game/{id}/stats")]
    public async Task<ActionResult<List<PlayerWithStatsDto>>> GetForGameWithStats(Guid id, CancellationToken token)
    {
        return await Mediator.Send(new GetPlayersWithStatsQuery { GameId = id }, token);
    }

    [HttpGet("user")]
    public async Task<ActionResult<PlayersDto>> GetForUser([FromQuery] GetUserPlayersQuery query, CancellationToken token)
    {
        return await Mediator.Send(query, token);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreatePlayerCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdatePlayerCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeletePlayerCommand(id));

        return NoContent();
    }
}
