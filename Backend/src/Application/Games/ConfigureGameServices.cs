using Backend.Application.Games.Board;
using Backend.Application.Games.Board.Interfaces;
using Backend.Application.Games.Effect;
using Backend.Application.Games.Effect.Interfaces;
using Backend.Application.Games.Engine;
using Backend.Application.Games.Engine.Helpers;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Player;
using Backend.Application.Games.Player.Helpers;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.PowerUp;
using Backend.Application.Games.PowerUp.Interfaces;
using Backend.Application.Games.PowerUp.PowerUps;
using Backend.Application.Games.Sheduler;
using Backend.Application.Games.Sheduler.Interfaces;
using Backend.Application.Games.Stages;
using Backend.Application.Games.Stages.EndGame;
using Backend.Application.Games.Stages.InitGame;
using Backend.Application.Games.Stages.InitGame.SubStages;
using Backend.Application.Games.Stages.Interfaces;
using Backend.Application.Games.Stages.Round;
using Backend.Application.Games.Stages.Round.SubStages;
using Backend.Application.Games.Stages.StartGame;
using Backend.Application.Games.Stages.StartRound;
using Backend.Application.Games.Stages.StartRound.SubStages;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games;

public static class ConfigureGameServices
{
    public static IServiceCollection AddGameServices(this IServiceCollection services)
    {
        AddSheduler(services);
        AddEngine(services);
        AddBoard(services);
        AddPlayer(services);
        AddEffects(services);
        AddStages(services);
        AddPowerUps(services);

        return services;
    }

    private static void AddSheduler(IServiceCollection services)
    {
        services.AddHostedService<GameSheduler>();

        services.AddSingleton<IGamesManager, GamesManager>();
        services.AddSingleton<IGameRequests, GameRequests>();
        services.AddSingleton<IRunningGames, RunningGames>();
    }

    private static void AddEngine(IServiceCollection services)
    {
        services.AddScoped<IGameContext, GameContext>();
        services.AddScoped<IGameTime, GameTime>();
        services.AddScoped<IGameWatchDog, GameWatchDog>();
        services.AddScoped<IGameEngine, GameEngine>();
        services.AddScoped<ICollisionDetector, CollisionDetector>();
        services.AddScoped<IGenerator, Generator>();
        services.AddScoped<IGameConnection, GameConnection>();
    }

    private static void AddBoard(IServiceCollection services)
    {
        services.AddScoped<IGameBoard, GameBoard>();
    }

    private static void AddPlayer(IServiceCollection services)
    {
        services.AddScoped<IPlayersManager, PlayersManager>();
        services.AddScoped<IPlayerMover, PlayerMover>();
        services.AddScoped<IPlayerGapGenerator, PlayerGapGenerator>();

        services.AddTransient<IGamePlayer, GamePlayer>();
    }

    private static void AddEffects(IServiceCollection services)
    {
        services.AddScoped<IPlayerEffectsManager, PlayerEffectsManager>();
    }

    private static void AddStages(IServiceCollection services)
    {
        services.AddScoped<IGameStages, GameStages>();

        services.AddTransient<InitGameStage>();
        services.AddTransient<LoadDataFromDatabase>();
        services.AddTransient<WaitForConnections>();
        services.AddTransient<SetupGame>();

        services.AddTransient<StartGameStage>();

        services.AddTransient<StartRoundStage>();
        services.AddTransient<InitRound>();
        services.AddTransient<Reset>();
        services.AddTransient<WaitForReady>();
        services.AddTransient<CountDown>();

        services.AddTransient<RoundStage>();
        services.AddTransient<RunRound>();

        services.AddTransient<EndRoundStage>();

        services.AddTransient<EndGameStage>();
    }

    private static void AddPowerUps(IServiceCollection services)
    {
        services.AddScoped<IPowerUpsManager, PowerUpsManager>();

        services.AddTransient(typeof(PlayerPowerUp<>));
    }
}
