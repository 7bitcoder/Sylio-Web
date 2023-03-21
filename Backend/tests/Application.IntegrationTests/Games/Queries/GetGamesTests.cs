using System;
using Backend.Application.Actions.Games.Commands.CreateGame;
using Backend.Application.Actions.Games.Queries.GetGame;
using Backend.Application.Actions.Games.Queries.GetGames;
using Backend.Application.Common.Exceptions;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Backend.Application.IntegrationTests.Games.Queries;

using static Testing;

public class GetGamesTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnEmptyGames()
    {

        var list = await SendAsync(new GetGamesQuery { 
            SearchString = "",
            ItemsPerPage = 10,
            Page = 0,
        });

        list.Count.Should().Be(0);
        list.Games.Should().BeEmpty();
    }

    [Test]
    public async Task ShouldRejectTooLongSearchString()
    {

        var query = new GetGamesQuery
        {
            SearchString = new string('a', 100),
            ItemsPerPage = 10,
            Page = 0,
        };

        await ShouldFailValidation(query, "SearchString", "The length of 'Search String' must be 70 characters or fewer. You entered 100 characters.");
    }

    [Test]
    public async Task ShouldRejectTooBigItemsPerPage()
    {
        var query = new GetGamesQuery
        {
            SearchString = "test",
            ItemsPerPage = 1000,
            Page = 0,
        };

        await ShouldFailValidation(query, "ItemsPerPage", "'Items Per Page' must be less than or equal to '100'.");
    }

    [Test]
    public async Task ShouldRejectSmallItemsPerPage()
    {
        var query = new GetGamesQuery
        {
            SearchString = "test",
            ItemsPerPage = -1000,
            Page = 0,
        };

        await ShouldFailValidation(query, "ItemsPerPage", "'Items Per Page' must be greater than or equal to '0'.");
    }

    [Test]
    public async Task ShouldRejectWrongPage()
    {
        var query = new GetGamesQuery
        {
            SearchString = "test",
            ItemsPerPage = 10,
            Page = -1,
        };

        await ShouldFailValidation(query, "Page", "'Page' must be greater than or equal to '0'.");
    }

    [Test]
    public async Task ShouldReturnGames()
    {
        await AddAsync(new Game
        {
            BlockedPowerUps = new() { },
            IsPublic = true, 
            MaxPlayers = 5,
            RoundsNumber = 12,
            State = GameState.WaitingForPlayers,
            Title = "test 1",
        });
        await AddAsync(new Game
        {
            BlockedPowerUps = new() { },
            IsPublic = true,
            MaxPlayers = 5,
            RoundsNumber = 12,
            State = GameState.WaitingForPlayers,
            Title = "test 2",
        });
        await AddAsync(new Game
        {
            BlockedPowerUps = new() { },
            IsPublic = true,
            MaxPlayers = 5,
            RoundsNumber = 12,
            State = GameState.WaitingForPlayers,
            Title = "test 3",
        });

        await AddAsync(new Game
        {
            BlockedPowerUps = new() { },
            IsPublic = true,
            MaxPlayers = 5,
            RoundsNumber = 12,
            State = GameState.WaitingForPlayers,
            Title = "other 3",
        });


        var list = await SendAsync(new GetGamesQuery
        {
            SearchString = "test",
            ItemsPerPage = 10,
            Page = 0,
        });

        list.Count.Should().Be(3);
        list.Games.Should().HaveCount(3);
    }
}
