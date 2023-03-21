namespace Backend.Domain.Events;

public class GameDeletedEvent : BaseEvent
{
    public GameDeletedEvent(Game game)
    {
        Game = game;
    }

    public Game Game { get; }
}
