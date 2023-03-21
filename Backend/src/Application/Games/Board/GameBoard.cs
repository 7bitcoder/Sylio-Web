using Backend.Application.Games.Board.Interfaces;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Games.Board;
public class GameBoard : IGameBoard
{
    public Point Point { get; set; }

    public Vector Size { get; set; }

    public Point LeftTop => new Point(Point.X, Point.Y);

    public Point LeftBottom => new Point(Point.X, Point.Y + Size.H);

    public Point RightTop => new Point(Point.X + Size.W, Point.Y);

    public Point RightBottom => new Point(Point.X + Size.W, Point.Y + Size.H);

    public float Top => Point.Y;

    public float Left => Point.X;

    public float Right => Point.X + Size.W;

    public float Bottom => Point.Y + Size.H;

    public void Reset()
    {
        Size = new Vector(1920, 1080);
        Point = new Point(0, 0);
    }
}
