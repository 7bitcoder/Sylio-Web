using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Games.Commands.DeleteGame;

public record DeleteGameCommand(Guid Id) : IRequest<Unit>;

public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteGameCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Games
            .Where(g => g.Id == request.Id)
            .Include(g => g.Players.Where(p => p.UserId == _currentUserService.UserId))
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null || !entity.Players.Any())
        {
            throw new NotFoundException(nameof(Game), request.Id);
        }
        var player = entity.Players.First();
        if (!player.GameAdmin)
        {
            throw new ForbiddenAccessException();
        }
        _context.Games.Remove(entity);

        entity.AddDomainEvent(new GameDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
