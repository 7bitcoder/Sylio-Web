using Backend.Application.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using Backend.Application.Actions.Games.Commands.CreateGame;
using Backend.Domain.Enums;
using Backend.Domain.Entities;

namespace Backend.Application.IntegrationTests.Games.Commands;

using static Testing;

public class CreateGameTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateGameCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRejectNotEmptyTitle()
    {
        var command = new CreateGameCommand
        {
            Title = "",
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 6
        };

        await ShouldFailValidation(command, "Title", "The length of 'Title' must be at least 1 characters. You entered 0 characters.");
    }

    [Test]
    public async Task ShoulRejectTooLongTitle()
    {
        var command = new CreateGameCommand
        {
            Title = new string('a', 100),
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 6
        };

        await ShouldFailValidation(command, "Title", "The length of 'Title' must be 70 characters or fewer. You entered 100 characters.");
    }

    [Test]
    public async Task ShoulRejectTooBigRoundsNumber()
    {
        var command = new CreateGameCommand
        {
            Title = "Test 1234",
            IsPublic = true,
            RoundsNumber = 0,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 6
        };

        await ShouldFailValidation(command, "RoundsNumber", "'Rounds Number' must be greater than or equal to '1'.");
    }

    [Test]
    public async Task ShoulRejectTooSmallRoundsNumber()
    {
        var command = new CreateGameCommand
        {
            Title = "Test 1234",
            IsPublic = true,
            RoundsNumber = 123,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 6
        };

        await ShouldFailValidation(command, "RoundsNumber", "'Rounds Number' must be less than or equal to '20'.");
    }

    [Test]
    public async Task ShoulRejectTooBigMaxPlayers()
    {
        var command = new CreateGameCommand
        {
            Title = "Test 1234",
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 16
        };

        await ShouldFailValidation(command, "MaxPlayers", "'Max Players' must be less than or equal to '8'.");
    }

    [Test]
    public async Task ShoulRejectTooSmallMaxPlayers()
    {
        var command = new CreateGameCommand
        {
            Title = "Test 1234",
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>(),
            MaxPlayers = 1
        };

        await ShouldFailValidation(command, "MaxPlayers", "'Max Players' must be greater than or equal to '2'.");
    }

    [Test]
    public async Task ShoulRejectNullBlockedPowerUps()
    {
        var command = new CreateGameCommand
        {
            Title = "Test 1234",
            IsPublic = true,
            RoundsNumber = 15,
#pragma warning disable CS8625 // Nie można przekonwertować literału o wartości null na nienullowalny typ referencyjny.
            BlockedPowerUps = null,
#pragma warning restore CS8625 // Nie można przekonwertować literału o wartości null na nienullowalny typ referencyjny.
            MaxPlayers = 6
        };

        await ShouldFailValidation(command, "BlockedPowerUps", "'Blocked Power Ups' must not be empty.");
    }

    [Test]
    public async Task ShoulRejectWrongBlockedPowerUpType()
    {
        var command = new CreateGameCommand
        {
            Title = "Test 1234",
            IsPublic = true,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType> { (PowerUpType)12134 },
            MaxPlayers = 6
        };

        await ShouldFailValidation(command, "BlockedPowerUps[0]", "'Blocked Power Ups' has a range of values which does not include '12134'.");
    }

    [Test]
    public async Task ShouldCreateGame()
    {
        var userId = RunAsDefaultUser();

        var command = new CreateGameCommand
        {
            Title = "test 123",
            IsPublic = false,
            RoundsNumber = 15,
            BlockedPowerUps = new List<PowerUpType>() { PowerUpType.CleanBoard, PowerUpType.PlayerCrossWalls_Self },
            MaxPlayers = 6
        };

        var id = await SendAsync(command);

        var list = await FindAsync<Game>(id);

        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Title);
        list!.IsPublic.Should().Be(command.IsPublic);
        list!.RoundsNumber.Should().Be(command.RoundsNumber);
        list!.BlockedPowerUps.Should().BeEquivalentTo(command.BlockedPowerUps);
        list!.MaxPlayers.Should().Be(command.MaxPlayers);
        list!.State.Should().Be(GameState.Created);
        list!.UserId.Should().Be(userId);
    }
}
