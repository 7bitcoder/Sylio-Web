namespace Backend.Domain.Events;

public class GameUpdatedEvent : BaseEvent
{
    public GameUpdatedEvent(Game game)
    {
        Game = game;
    }

    public Game Game { get; }
}
