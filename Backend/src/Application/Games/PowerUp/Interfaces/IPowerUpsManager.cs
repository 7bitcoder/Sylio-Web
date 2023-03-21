using Backend.Application.Games.Update.Updates;

namespace Backend.Application.Games.PowerUp.Interfaces;

public interface IPowerUpsManager
{
    void Update();

    List<IPowerUp> PowerUps { get; }

    void Reset();

    void Clear();

    IEnumerable<PowerUpUpdate> GetUpdates();
}