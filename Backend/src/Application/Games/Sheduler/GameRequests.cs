using System.Collections.Concurrent;
using Backend.Application.Games.Sheduler.Interfaces;

namespace Backend.Application.Games.Sheduler;
public class GameRequests : IGameRequests
{
    private readonly SemaphoreSlim _notifier = new(0);

    private readonly ConcurrentQueue<Guid> _queue = new();

    public async Task<Guid> Wait(CancellationToken stoppingToken)
    {
        await _notifier.WaitAsync(stoppingToken);
        if (_queue.Any() && _queue.TryDequeue(out var id))
        {
            return id;
        }
        throw new InvalidOperationException("Cannot dequeue new game request");
    }

    public void Add(Guid id)
    {
        _queue.Enqueue(id);
        _notifier.Release();
    }

    public async IAsyncEnumerator<Guid> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            yield return await Wait(cancellationToken);
        }
    }
}
