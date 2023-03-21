using Backend.Domain.Enums;

namespace Backend.Application.Games.Player.Interfaces;
public interface IPlayerActionsObserver
{
    void OnKeyPressed(KeyType key);

    void OnKeyReleased(KeyType key);

    void OnConnected();

    void OnDisconnected();

    void OnReady();
}
