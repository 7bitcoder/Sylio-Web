using Backend.Application.Games.Board.Interfaces;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.PowerUp.Interfaces;

namespace Backend.Application.Games.Engine.Interfaces;
public interface ICollisionDetector
{
    bool PlayerCollideWithPlayer(IGamePlayer playerA, IGamePlayer playerB);

    IGamePlayer? PlayerCollideWithPlayers(IGamePlayer player, IEnumerable<IGamePlayer> players);

    bool PlayerCollideWithBoard(IGamePlayer player, IGameBoard board);

    bool PlayerCollidesWithPowerUp(IGamePlayer player, IPowerUp powerUp);

    IPowerUp? PlayerCollidesWithPowerUps(IGamePlayer player, IEnumerable<IPowerUp> powerUps);

    bool PowerUpCollidesWithPowerUp(IPowerUp powerUpA, IPowerUp powerUpB);

    IPowerUp? PowerUpCollidesWithPowerUps(IPowerUp powerUp, IEnumerable<IPowerUp> powerUps);
}
