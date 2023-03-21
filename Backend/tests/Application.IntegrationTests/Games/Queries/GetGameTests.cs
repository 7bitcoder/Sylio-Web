using System;
using Backend.Application.Actions.Games.Commands.CreateGame;
using Backend.Application.Actions.Games.Queries.GetGame;
using Backend.Application.Common.Exceptions;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Backend.Application.IntegrationTests.Games.Queries;

using static Testing;

public class GetGameTests : BaseTestFixture
{
    [Test]
    public async Task ShouldNotFoundGame()
    {

        var action = () => SendAsync(new GetGameQuery { Id = Guid.NewGuid() });

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldReturnGame()
    {
        var command = new CreateGameCommand
        {
            Title = "test 123",
            IsPublic = false,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>() { PowerUpType.CleanBoard, PowerUpType.PlayerCrossWalls_Self },
            MaxPlayers = 6
        };
        var id = await SendAsync(command);

        var game = await SendAsync(new GetGameQuery { Id = id });

        game.Should().NotBeNull();
        game!.Title.Should().Be(command.Title);
        game!.IsPublic.Should().Be(command.IsPublic);
        game!.RoundsNumber.Should().Be(command.RoundsNumber);
        game!.BlockedPowerUps.Should().BeEquivalentTo(command.BlockedPowerUps);
        game!.MaxPlayers.Should().Be(command.MaxPlayers);
        game!.State.Should().Be(GameState.Created);
        game!.Players.Should().Be(0);
    }
}
