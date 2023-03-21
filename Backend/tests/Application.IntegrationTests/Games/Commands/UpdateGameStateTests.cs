using Backend.Application.Common.Exceptions;
using Backend.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Backend.Application.Actions.Games.Commands.DeleteGame;
using Backend.Domain.Enums;
using Backend.Application.Actions.Games.Commands.CreateGame;
using Backend.Application.Actions.Players.Commands.CreatePlayer;
using Backend.Application.Actions.Games.Commands.UpdateGameState;

namespace Backend.Application.IntegrationTests.Games.Commands;
using static Testing;

public class UpdateGameStateTests : BaseTestFixture
{
    [Test]
    public async Task ShouldNotFoundGame()
    {
        var command = new UpdateGameStateCommand { Id = Guid.NewGuid(), GameState = GameState.Running};
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldChangeGameStateToRunning()
    {
        var gameId = await SendAsync(new CreateGameCommand
        {
            Title = "test 12345",
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 6
        });

        var playerId = await SendAsync(new CreatePlayerCommand
        {
            GameId = gameId,
            Colour = "#800000",
            Name = "tester"
        });

        await SendAsync(new UpdateGameStateCommand { Id = gameId, GameState = GameState.Running });

        var list = await FindAsync<Game>(gameId);

        list.Should().NotBeNull();
        list!.State.Should().Be(GameState.Running);
        list!.UserId.Should().Be(CurrentUserId);
    }

    [Test]
    public async Task ShouldChangeGameStateToFinished()
    {
        var gameId = await SendAsync(new CreateGameCommand
        {
            Title = "test 12345",
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 6
        });

        var playerId = await SendAsync(new CreatePlayerCommand
        {
            GameId = gameId,
            Colour = "#800000",
            Name = "tester"
        });

        await SendAsync(new UpdateGameStateCommand { Id = gameId, GameState = GameState.Finished });

        var list = await FindAsync<Game>(gameId);

        list.Should().NotBeNull();
        list!.State.Should().Be(GameState.Finished);
        list!.UserId.Should().Be(CurrentUserId);
    }
}
