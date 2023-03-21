using Backend.Application.Common.Exceptions;
using Backend.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Backend.Application.Actions.Games.Commands.DeleteGame;
using Backend.Domain.Enums;
using Backend.Application.Actions.Games.Commands.CreateGame;
using Backend.Application.Actions.Players.Commands.CreatePlayer;
using Backend.Application.Actions.Games.Commands.UpdateGameState;
using Backend.Application.Actions.Games.Commands.StartGame;

namespace Backend.Application.IntegrationTests.Games.Commands;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Testing;

public class StartGameStateTests : BaseTestFixture
{
    [Test]
    public async Task ShouldNotFoundGame()
    {
        var command = new StartGameCommand { Id = Guid.NewGuid() };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldNotFoundGameForEmptyUsers()
    {
        var gameId = await SendAsync(new CreateGameCommand
        {
            Title = "test 12345",
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 6
        });

        var command = new StartGameCommand { Id = gameId };

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldForbidStartingForLessThanTwoPlayers()
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

        var command = new StartGameCommand { Id = gameId };

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldForbidStartingForNonAdmin()
    {
        var gameId = await SendAsync(new CreateGameCommand
        {
            Title = "test 12345",
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 6
        });

        await SendAsync(new CreatePlayerCommand
        {
            GameId = gameId,
            Colour = "#800000",
            Name = "tester"
        });

        RunAsOtherUser();

        await SendAsync(new CreatePlayerCommand
        {
            GameId = gameId,
            Colour = "#800000",
            Name = "tester"
        });

        var command = new StartGameCommand { Id = gameId };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldStartGame()
    {
        var gameId = await SendAsync(new CreateGameCommand
        {
            Title = "test 12345",
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 6
        });

        await SendAsync(new CreatePlayerCommand
        {
            GameId = gameId,
            Colour = "#800000",
            Name = "tester"
        });

        RunAsOtherUser();

        await SendAsync(new CreatePlayerCommand
        {
            GameId = gameId,
            Colour = "#800000",
            Name = "tester"
        });

        RunAsDefaultUser();
        await SendAsync(new UpdateGameStateCommand { Id = gameId, GameState = GameState.Running });

        var list = await FindAsync<Game>(gameId);

        await Task.Delay(100);

        list.Should().NotBeNull();
        list!.State.Should().Be(GameState.Running);
    }
}
