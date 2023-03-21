using Backend.Application.Common.Interfaces;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Sheduler.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.Sheduler;
public class GamesManager : IGamesManager
{
    private readonly IRunningGames _runningGames;
    private readonly IServiceProvider _serviceProvider;

    public GamesManager(
        IRunningGames runningGames,
        IServiceProvider serviceProvider)
    {
        _runningGames = runningGames;
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(Guid id, CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var game = scope.ServiceProvider.GetRequiredService<IGameEngine>();

        var user = scope.ServiceProvider.GetRequiredService<ICurrentUserService>(); 
        using var remover = _runningGames.Add(id, game);
        await game.Run(id, stoppingToken);
    }
}


