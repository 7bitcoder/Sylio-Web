using Backend.Application.Actions.Games.Commands.CreateGame;
using Backend.Application.Common.Behaviours;
using Backend.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Backend.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateGameCommand>> _logger = null!;
    private Mock<ICurrentUserService> _currentUserService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateGameCommand>>();
        _currentUserService = new Mock<ICurrentUserService>();
    }
}
