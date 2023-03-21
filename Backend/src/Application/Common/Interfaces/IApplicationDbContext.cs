using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Game> Games { get; }

    DbSet<Player> Players { get; }

    DbSet<Round> Rounds { get; }

    DbSet<PlayerRoundStat> PlayerRoundStats { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
