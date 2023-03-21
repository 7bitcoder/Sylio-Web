using Backend.Application.Common.Models;
using Backend.Application.Actions.Games.Commands.DeleteGame;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Application.Actions.Games.Commands.UpdateGame;
using Backend.Application.Actions.Games.Commands.CreateGame;
using Backend.Application.Actions.Games.Queries.GetGame;
using Backend.Application.Actions.Games.Commands.StartGame;
using Backend.Application.Actions.Games.Queries.GetGames;
using Microsoft.AspNetCore.Cors;
using Backend.Application.Actions.Games.Queries.GetUserGames;

namespace WebApi.Controllers;

[Authorize]
public class GamesController : ApiControllerBase
{
    [HttpGet()]
    public async Task<ActionResult<GamesDto>> GetList([FromQuery] GetGamesQuery query, CancellationToken token)
    {
       return await Mediator.Send(query, token);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameDto>> Get(Guid id, CancellationToken token)
    {
        return await Mediator.Send(new GetGameQuery { Id = id }, token);
    }

    [HttpGet("user")]
    public async Task<ActionResult<GamesDto>> GetForUser([FromQuery] GetUserGamesQuery query, CancellationToken token)
    {
        return await Mediator.Send(query, token);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateGameCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateGameCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("{id}/start")]
    public async Task<ActionResult<bool>> Start(Guid id)
    {
        return await Mediator.Send(new StartGameCommand{ Id = id });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteGameCommand(id));

        return NoContent();
    }
}
