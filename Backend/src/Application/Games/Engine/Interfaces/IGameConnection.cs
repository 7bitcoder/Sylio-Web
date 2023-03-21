using Backend.Application.Common.Interfaces;
using Backend.Application.Games.Update.Interfaces;

namespace Backend.Application.Games.Engine.Interfaces;
public interface IGameConnection
{
    Task Init(CancellationToken token);

    Task SendUpdate(IEnumerable<IGameUpdate> updates);

    Task SendNotification(IGameNotification notification);
}
