using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.Actions.Rounds.Commands.UpdateRound;

public record UpdateRoundCommand : IRequest<Unit>
{
    public int Id { get; set; }
}

public class UpdateRoundCommandHandler : IRequestHandler<UpdateRoundCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateRoundCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateRoundCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Rounds
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Round), request.Id);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
