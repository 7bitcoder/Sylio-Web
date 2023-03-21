using Backend.Domain.ValueObjects;

namespace Backend.Application.Games.Board.Interfaces;
public interface IGameBoard
{
    Point Point { get; }

    Vector Size { get; }

    float Top { get; }

    float Left { get; }

    float Right { get; }

    float Bottom { get; }

    Point LeftTop { get; }

    Point LeftBottom { get; }

    Point RightBottom { get; }

    Point RightTop { get; }

    void Reset();
}
