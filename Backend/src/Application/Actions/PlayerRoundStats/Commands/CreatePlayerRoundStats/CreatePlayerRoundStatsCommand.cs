using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.Actions.PlayerRoundStats.Commands.CreatePlayerRoundStats;

public record CreatePlayerRoundStatsCommand : IRequest<int>
{
    public Guid PlayerId { get; set; }

    public int RoundId { get; set; }

    public int Score { get; set; }

    public int Place { get; set; }

    public int Length { get; set; }

    public TimeSpan LiveTime { get; set; }

    public Guid? KilledByPlayerId { get; set; }
}

public class CreatePlayerRoundStatsCommandHandler : IRequestHandler<CreatePlayerRoundStatsCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePlayerRoundStatsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePlayerRoundStatsCommand request, CancellationToken cancellationToken)
    {
        var entity = new PlayerRoundStat
        {
            Score = request.Score,
            Place = request.Place,
            Length = request.Length,
            LiveTime = request.LiveTime,
        };

        var player = await _context.Players
             .FindAsync(new object[] { request.PlayerId }, cancellationToken);

        Player? killedBy = null;
        if (request.KilledByPlayerId is not null)
        {
            killedBy = await _context.Players
             .FindAsync(new object[] { request.KilledByPlayerId }, cancellationToken);
        }

        var round = await _context.Rounds
             .FindAsync(new object[] { request.RoundId }, cancellationToken);

        if (player is null || round is null)
        {
            throw new NotFoundException(nameof(Player), request.PlayerId);
        }

        player.RoundsStats.Add(entity);
        entity.KilledBy = killedBy;
        round.PlayersRoundStats.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
