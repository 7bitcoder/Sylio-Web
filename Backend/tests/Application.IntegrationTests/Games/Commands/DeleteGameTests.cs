using Backend.Application.Common.Exceptions;
using Backend.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Backend.Application.Actions.Games.Commands.DeleteGame;
using Backend.Domain.Enums;
using Backend.Application.Actions.Games.Commands.CreateGame;
using Backend.Application.Actions.Players.Commands.CreatePlayer;

namespace Backend.Application.IntegrationTests.Games.Commands;

using static Testing;

public class DeleteGameTests : BaseTestFixture
{
    [Test]
    public async Task ShouldNotFoundGame()
    {
        var command = new DeleteGameCommand(Guid.NewGuid());
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldNotDeleteGameNoPlayers()
    {
        var gameId = await SendAsync(new CreateGameCommand
        {
            Title = "test 12345",
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 6
        });


        await FluentActions.Invoking(() => SendAsync(new DeleteGameCommand(gameId)))
            .Should()
            .ThrowAsync<NotFoundException>();
    }


    [Test]
    public async Task ShouldNotDeleteGameNoAdmin()
    {
        var gameId = await SendAsync(new CreateGameCommand
        {
            Title = "test 12345",
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 6
        });

        RunAsOtherUser();

        var playerId = await SendAsync(new CreatePlayerCommand
        {
            GameId = gameId,
            Colour = "#800000",
            Name = "tester"
        });

        await FluentActions.Invoking(() => SendAsync(new DeleteGameCommand(gameId)))
            .Should()
            .ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldDeleteGame()
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

        await SendAsync(new DeleteGameCommand(gameId));

        var list = await FindAsync<Game>(gameId);

        list.Should().BeNull();
    }
}
