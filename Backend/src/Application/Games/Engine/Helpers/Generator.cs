using Backend.Application.Games.Board.Interfaces;
using Backend.Application.Games.Engine.Interfaces;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Games.Engine.Helpers;
public class Generator : IGenerator
{
    private readonly IGameBoard _board;

    public Generator(IGameBoard board)
    {
        _board = board;
    }

    public Point GeneratePoint(float offset)
    {
        return new Point
        {
            X = offset + (float)Random.Shared.NextDouble() * (_board.Size.W - 2 * offset),
            Y = offset + (float)Random.Shared.NextDouble() * (_board.Size.H - 2 * offset),
        };
    }

    public Vector GenerateVector()
    {
        var angle = Random.Shared.NextDouble() * 2 * Math.PI;
        return new Vector
        {
            W = (float)Math.Cos(angle),
            H = (float)Math.Sin(angle)
        };
    }
}
