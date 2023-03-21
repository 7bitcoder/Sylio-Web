using Backend.Application.Games.Board.Interfaces;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Application.Games.Player.Interfaces;
using Backend.Application.Games.Player.Models;
using Backend.Application.Games.PowerUp.Interfaces;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Games.Engine.Helpers;
public class CollisionDetector : ICollisionDetector
{
    public IGamePlayer? PlayerCollideWithPlayers(IGamePlayer player, IEnumerable<IGamePlayer> players)
    {
        return players.FirstOrDefault(p => PlayerCollideWithPlayer(player, p));
    }

    public bool PlayerCollideWithPlayer(IGamePlayer playerA, IGamePlayer playerB)
    {
        if (playerA.Immortal)
        {
            return false;
        }
        IEnumerable<PlayerState> states = playerB.LastStates;
        if (playerA == playerB)
        {
            //if self skip some for head bugs
            states = states.SkipLast(10);
        }
        foreach (var state in states)
        {
            if (!state.Visible)
            {
                continue;
            }
            if (CircleCollideWithCircle(playerA.Position, playerA.Radious, state.Position, state.Radious))
            {
                return true;
            }
        }
        return false;
    }

    public bool PlayerCollideWithBoard(IGamePlayer player, IGameBoard board)
    {
        return CirclePassedLineY(player.Position, player.Radious, board.Top, true)
            || CirclePassedLineY(player.Position, player.Radious, board.Bottom, false)
            || CirclePassedLineX(player.Position, player.Radious, board.Left, true)
            || CirclePassedLineX(player.Position, player.Radious, board.Right, false);
    }

    public IPowerUp? PlayerCollidesWithPowerUps(IGamePlayer player, IEnumerable<IPowerUp> powerUps)
    {
        return powerUps.FirstOrDefault(p => PlayerCollidesWithPowerUp(player, p));
    }

    public bool PlayerCollidesWithPowerUp(IGamePlayer player, IPowerUp powerUp)
    {
        return CircleCollideWithCircle(player.Position, player.Radious, powerUp.Position, powerUp.Radious);
    }

    public IPowerUp? PowerUpCollidesWithPowerUps(IPowerUp powerUp, IEnumerable<IPowerUp> powerUps)
    {
        return powerUps.FirstOrDefault(p => PowerUpCollidesWithPowerUp(powerUp, p));
    }

    public bool PowerUpCollidesWithPowerUp(IPowerUp powerUpA, IPowerUp powerUpB)
    {
        return CircleCollideWithCircle(powerUpA.Position, powerUpA.Radious, powerUpB.Position, powerUpB.Radious);
    }

    private bool CirclePassedLineX(Point point, float radius, float X, bool left)
    {
        if (left)
        {
            return point.X < X + radius;
        }
        else
        {
            return point.X > X - radius;
        }
    }

    private bool CirclePassedLineY(Point point, float radius, float Y, bool up)
    {
        if (up)
        {
            return point.Y < Y + radius;
        }
        else
        {
            return point.Y > Y - radius;
        }
    }

    private bool CircleCollideWithLine(Point point, float radius, Point lineBegin, Point lineEnd)
    {
        lineBegin.X -= point.X;
        lineBegin.Y -= point.Y;
        lineEnd.X -= point.X;
        lineEnd.Y -= point.Y;
        float dx = lineEnd.X - lineBegin.X;
        float dy = lineEnd.Y - lineBegin.Y;
        float dr = (float)Math.Sqrt((double)(dx * dx) + (double)(dy * dy));
        float D = lineBegin.X * lineEnd.Y - lineEnd.X * lineBegin.Y;

        float di = radius * radius * (dr * dr) - D * D;

        return di >= 0;
    }

    private bool CircleCollideWithCircle(Point cenerA, float radiousA, Point cenerB, float radiousB)
    {
        return Math.Abs((cenerA.X - cenerB.X) * (cenerA.X - cenerB.X) + (cenerA.Y - cenerB.Y) * (cenerA.Y - cenerB.Y))
            < (radiousA + radiousB) * (radiousA + radiousB);
    }
}
