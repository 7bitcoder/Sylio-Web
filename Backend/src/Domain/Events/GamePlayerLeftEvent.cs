namespace Backend.Domain.Events;

public class GamePlayerLeftEvent : BaseEvent
{
    public GamePlayerLeftEvent(Player player)
    {
        Player = player;
    }

    public Player Player { get; }
}
