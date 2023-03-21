using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Games.Commands.StartGame;

public record StartGameCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}

public class StartGameCommandHandler : IRequestHandler<StartGameCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public StartGameCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(StartGameCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Games
            .Where(g => g.Id == request.Id)
            .Include(g => g.Players)
            .FirstOrDefaultAsync(cancellationToken);

        var player = entity?.Players.Where(p => p.UserId == _currentUserService.UserId).FirstOrDefault();
        if (entity == null || player is null || !entity.Players.Any())
        {
            throw new NotFoundException(nameof(Game), request.Id);
        }
        if (!player.GameAdmin
            || entity.State != GameState.WaitingForPlayers
            || entity.Players.Count() < 2)
        {
            throw new ForbiddenAccessException();
        }
        entity.State = GameState.StartRequested;
        entity.AddDomainEvent(new GameStartRequestedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
