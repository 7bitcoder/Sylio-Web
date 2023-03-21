using Backend.Application.Games.Effect.Effects;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.PowerUp.Base;
using Backend.Application.Games.PowerUp.Interfaces;
using Backend.Application.Games.PowerUp.PowerUps;
using Backend.Application.Games.Update.Updates;
using Backend.Domain.Enums;
using Backend.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Games.PowerUp;
public class PowerUpsManager : IPowerUpsManager
{
    private readonly IGameTime _time;
    private readonly IGenerator _generator;
    private readonly IGameContext _gameContext;
    private readonly IPlayersManager _players;
    private readonly ICollisionDetector _collisionDetector;
    private readonly IServiceProvider _serviceProvider;


    private readonly List<IPowerUp> _powerUps = new();
    private List<PowerUpUpdate> _updates = new();

    private int _idSeed = 1;
    private TimeSpan _nextPowerUpTime = TimeSpan.Zero;
    private readonly int _maxPowerUps = 8;

    public PowerUpsManager(
        IGameTime time,
        IGenerator generator,
        IGameContext gameContext,
        IPlayersManager players,
        ICollisionDetector collisionDetector,
        IServiceProvider serviceProvider)
    {
        _time = time;
        _generator = generator;
        _gameContext = gameContext;
        _players = players;
        _collisionDetector = collisionDetector;
        _serviceProvider = serviceProvider;
    }

    public List<IPowerUp> PowerUps => _powerUps;

    public void Reset()
    {
        Clear();
    }

    public void Clear()
    {
        _powerUps.Clear();
        _powerUps.Clear();
        _updates.Clear();
    }

    public void Update()
    {
        foreach (var player in _players.AlivePlayers)
        {
            CheckActivation(player);
        }
        if (CanSpawnPowerUp())
        {
            GeneratePowerUp();
        }
    }

    private IPowerUp? CheckActivation(IGamePlayer player)
    {
        var powerUp = _collisionDetector.PlayerCollidesWithPowerUps(player, _powerUps);
        if (powerUp is not null)
        {
            powerUp.Activate(player);
            _updates.Add(new() { Id = powerUp.Id, S = false });
            _powerUps.Remove(powerUp);
        }
        return powerUp;
    }

    private bool CanSpawnPowerUp() => _time.StageTime > _nextPowerUpTime && _powerUps.Count < _maxPowerUps;

    private void GeneratePowerUp()
    {
        GenerateNextPowerUpTime();

        var enumValues = Enum.GetValues<PowerUpType>().Except(_gameContext.Game.BlockedPowerUps).ToArray();
        var randomType = enumValues[Random.Shared.Next(enumValues.Length)];
        var powerUp = BuildPowerUp(randomType);
        GeneratePowerUpUpdate(powerUp);
        _powerUps.Add(powerUp);
    }
    private void GenerateNextPowerUpTime()
    {
        var seconds = Random.Shared.Next(5, 11);
        _nextPowerUpTime = TimeSpan.FromSeconds(seconds) + _time.StageTime;
    }

    private void GeneratePowerUpUpdate(IPowerUp powerUp)
    {
        _updates.Add(new()
        {
            Id = powerUp.Id,
            X = powerUp.Position.X,
            Y = powerUp.Position.Y,
            U = powerUp.Type,
            S = true,
        });
    }

    private PowerUpBase BuildPowerUp(PowerUpType type)
    {
        var powerUp = CreatePowerUp(type);
        powerUp.Id = _idSeed++;
        powerUp.Type = type;
        SetRandomPosition(powerUp);
        return powerUp;
    }

    private void SetRandomPosition(PowerUpBase powerUp)
    {
        Point position;
        do
        {
            position = _generator.GeneratePoint(powerUp.Radious);
        } while (_collisionDetector.PowerUpCollidesWithPowerUps(powerUp, _powerUps) != null);
        powerUp.Position = position;
    }

    public IEnumerable<PowerUpUpdate> GetUpdates()
    {
        if (_updates.Any())
        {
            var updates = _updates;
            _updates = new();
            return updates;
        }
        return _updates;
    }

    private PowerUpBase CreatePowerUp(PowerUpType type) => type switch
    {
        PowerUpType.PlayerBlind_All => GetPowerUp<PlayerPowerUp<PlayerBlindEffect>>().ForAll(),
        PowerUpType.PlayerCrossWalls_Self => GetPowerUp<PlayerPowerUp<PlayerCrossWallsEffect>>(),
        PowerUpType.PlayerCrossWalls_All => GetPowerUp<PlayerPowerUp<PlayerCrossWallsEffect>>().ForAll(),
        PowerUpType.CleanBoard => GetPowerUp<PlayerPowerUp<PlayerClearEffect>>().ForAll(),
        PowerUpType.PlayerFreeze_Others => GetPowerUp<PlayerPowerUp<PlayerFreezeEffect>>().ForOthers(),
        PowerUpType.PlayerFreeze_All => GetPowerUp<PlayerPowerUp<PlayerFreezeEffect>>().ForAll(),
        PowerUpType.PlayerGrow_Others => GetPowerUp<PlayerPowerUp<PlayerGrowEffect>>().ForOthers(),
        PowerUpType.PlayerGrow_All => GetPowerUp<PlayerPowerUp<PlayerGrowEffect>>().ForAll(),
        PowerUpType.PlayerImmortal_Self => GetPowerUp<PlayerPowerUp<PlayerImmortalEffect>>(),
        PowerUpType.PlayerLongerGaps_Self => GetPowerUp<PlayerPowerUp<PlayerLongerGapsEffect>>(),
        PowerUpType.PlayerLongerGaps_All => GetPowerUp<PlayerPowerUp<PlayerLongerGapsEffect>>().ForAll(),
        PowerUpType.PlayerGapsMoreOften_Self => GetPowerUp<PlayerPowerUp<PlayerGapsMoreOftenEffect>>(),
        PowerUpType.PlayerGapsMoreOften_All => GetPowerUp<PlayerPowerUp<PlayerGapsMoreOftenEffect>>().ForAll(),
        PowerUpType.PlayerLockLeft_Others => GetPowerUp<PlayerPowerUp<PlayerLockLeftEffect>>().ForOthers(),
        PowerUpType.PlayerLockRight_Others => GetPowerUp<PlayerPowerUp<PlayerLockRightEffect>>().ForOthers(),
        PowerUpType.PlayerShrink_Self => GetPowerUp<PlayerPowerUp<PlayerShrinkEffect>>(),
        PowerUpType.PlayerShrink_All => GetPowerUp<PlayerPowerUp<PlayerShrinkEffect>>().ForAll(),
        PowerUpType.PlayerSlowDown_Self => GetPowerUp<PlayerPowerUp<PlayerSlowDownEffect>>(),
        PowerUpType.PlayerSlowDown_Others => GetPowerUp<PlayerPowerUp<PlayerSlowDownEffect>>().ForOthers(),
        PowerUpType.PlayerSlowDown_All => GetPowerUp<PlayerPowerUp<PlayerSlowDownEffect>>().ForAll(),
        PowerUpType.PlayerSpeedUp_Self => GetPowerUp<PlayerPowerUp<PlayerSpeedUpEffect>>(),
        PowerUpType.PlayerSpeedUp_Others => GetPowerUp<PlayerPowerUp<PlayerSpeedUpEffect>>().ForOthers(),
        PowerUpType.PlayerSpeedUp_All => GetPowerUp<PlayerPowerUp<PlayerSpeedUpEffect>>().ForAll(),
        PowerUpType.PlayerSpeedUpTurn_Self => GetPowerUp<PlayerPowerUp<PlayerSpeedUpTurnEffect>>(),
        PowerUpType.PlayerSpeedUpTurn_Others => GetPowerUp<PlayerPowerUp<PlayerSpeedUpTurnEffect>>().ForOthers(),
        PowerUpType.PlayerSpeedUpTurn_All => GetPowerUp<PlayerPowerUp<PlayerSpeedUpTurnEffect>>().ForAll(),
        PowerUpType.PlayerSlowDownTurn_Self => GetPowerUp<PlayerPowerUp<PlayerSlowDownTurnEffect>>(),
        PowerUpType.PlayerSlowDownTurn_Others => GetPowerUp<PlayerPowerUp<PlayerSlowDownTurnEffect>>().ForOthers(),
        PowerUpType.PlayerSlowDownTurn_All => GetPowerUp<PlayerPowerUp<PlayerSlowDownTurnEffect>>().ForAll(),
        PowerUpType.PlayerSwitchControls_Others => GetPowerUp<PlayerPowerUp<PlayerSwitchControlsEffect>>().ForOthers(),
        _ => throw new InvalidOperationException("bad powerup type")
    };

    private T GetPowerUp<T>() where T : notnull => _serviceProvider.GetRequiredService<T>();
}