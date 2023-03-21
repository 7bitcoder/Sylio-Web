namespace Backend.Application.Games.Sheduler.Interfaces;
public interface IGamesManager
{
    Task Execute(Guid id, CancellationToken stoppingToken);
}
