using Backend.Application.Games.Update.Enums;

namespace Backend.Application.Games.Update.Interfaces;
public interface IGameUpdate
{
    public GameUpdateType T { get; }
}