namespace Backend.Domain.Events;

public class GamePlayersUpdatedEvent : BaseEvent
{
    public GamePlayersUpdatedEvent(Game game)
    {
        Game = game;
    }

    public Game Game { get; }
}
