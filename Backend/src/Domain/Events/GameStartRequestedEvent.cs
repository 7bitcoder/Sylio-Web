namespace Backend.Domain.Events;

public class GameStartRequestedEvent : BaseEvent
{
    public GameStartRequestedEvent(Game game)
    {
        Game = game;
    }

    public Game Game{ get; }
}
