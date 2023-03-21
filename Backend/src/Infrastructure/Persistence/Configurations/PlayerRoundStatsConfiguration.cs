using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Persistence.Configurations;

public class PlayerRoundStatsConfiguration : IEntityTypeConfiguration<PlayerRoundStat>
{
    public void Configure(EntityTypeBuilder<PlayerRoundStat> builder)
    {
        builder.HasOne(p => p.Player)
            .WithMany(p => p.RoundsStats);

        builder.HasOne(p => p.Round)
            .WithMany(p => p.PlayersRoundStats);
    }
}
