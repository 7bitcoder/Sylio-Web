using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Players.Commands.UpdatePlayer;

public record UpdatePlayerCommand : IRequest<Unit>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string Colour { get; set; } = default!;
}

public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdatePlayerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Players
            .Where(g => g.Id == request.Id)
            .Include(g => g.Game)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Player), request.Id);
        }
        if (entity.UserId != _currentUserService.UserId)
        {
            throw new ForbiddenAccessException();
        }
        entity.Name = request.Name;
        entity.Colour = request.Colour;

        entity.AddDomainEvent(new GamePlayersUpdatedEvent(entity.Game));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
