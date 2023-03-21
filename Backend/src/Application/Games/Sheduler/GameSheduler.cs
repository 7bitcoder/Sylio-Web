using Backend.Application.Games.Sheduler.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Games.Sheduler;
public class GameSheduler : BackgroundService
{

    private readonly ILogger<GameSheduler> _logger;

    private readonly IGameRequests _gamesRequests;

    private readonly IGamesManager _gamesExecutor;

    public GameSheduler(
        ILogger<GameSheduler> logger,
        IGameRequests gamesRequests,
        IGamesManager gamesExecutor)
    {
        _logger = logger;
        _gamesRequests = gamesRequests;
        _gamesExecutor = gamesExecutor;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Run(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Global Error in {nameof(GameSheduler)}");
        }
    }

    private async Task Run(CancellationToken stoppingToken)
    {
        await foreach (var id in _gamesRequests.WithCancellation(stoppingToken))
        {
#pragma warning disable CS4014 
            ExecuteGame(id, stoppingToken);
#pragma warning restore CS4014 
        }
    }

    private async Task ExecuteGame(Guid id, CancellationToken stoppingToken)
    {
        try
        {
            await _gamesExecutor.Execute(id, stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Scoped Error in {nameof(ExecuteGame)}");
        }
    }
}
