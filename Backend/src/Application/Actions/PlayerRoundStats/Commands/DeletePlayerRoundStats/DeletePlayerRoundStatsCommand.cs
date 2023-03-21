using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using MediatR;

namespace Backend.Application.Actions.PlayerRoundStats.Commands.DeletePlayerRoundStats;

public record DeletePlayerRoundStatsCommand(int Id) : IRequest<Unit>;

public class DeletePlayerRoundStatsCommandHandler : IRequestHandler<DeletePlayerRoundStatsCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeletePlayerRoundStatsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeletePlayerRoundStatsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.PlayerRoundStats
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(PlayerRoundStats), request.Id);
        }

        _context.PlayerRoundStats.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
