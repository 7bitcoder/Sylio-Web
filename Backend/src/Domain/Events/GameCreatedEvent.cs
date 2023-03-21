namespace Backend.Domain.Events;

public class GameCreatedEvent : BaseEvent
{
    public GameCreatedEvent(Game item)
    {
        Item = item;
    }

    public Game Item { get; }
}
