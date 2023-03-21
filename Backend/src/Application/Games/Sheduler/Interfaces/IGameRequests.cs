namespace Backend.Application.Games.Sheduler.Interfaces;
public interface IGameRequests : IAsyncEnumerable<Guid>
{
    Task<Guid> Wait(CancellationToken stoppingToken);

    void Add(Guid id);
}
