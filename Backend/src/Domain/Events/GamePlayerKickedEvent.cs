namespace Backend.Domain.Events;

public class GamePlayerKickedEvent : BaseEvent
{
    public GamePlayerKickedEvent(Player player)
    {
        Player = player;
    }

    public Player Player { get; }
}
