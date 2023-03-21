using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.Actions.Rounds.Commands.DeleteRound;

public record DeleteRoundCommand(int Id) : IRequest<Unit>;

public class DeleteRoundCommandHandler : IRequestHandler<DeleteRoundCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRoundCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRoundCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Rounds
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Round), request.Id);
        }

        _context.Rounds.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
