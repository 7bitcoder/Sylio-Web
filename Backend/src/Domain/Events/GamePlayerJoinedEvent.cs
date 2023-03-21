namespace Backend.Domain.Events;

public class GamePlayerJoinedEvent : BaseEvent
{
    public GamePlayerJoinedEvent(Player player)
    {
        Player = player;
    }

    public Player Player { get; }
}
