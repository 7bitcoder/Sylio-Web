using Backend.Domain.ValueObjects;

namespace Backend.Application.Games.Engine.Interfaces;
public interface IGenerator
{
    Point GeneratePoint(float offset);

    Vector GenerateVector();
}
