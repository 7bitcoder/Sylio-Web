using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Games.Commands.UpdateGame;

public record UpdateGameCommand : IRequest<Unit>
{
    public Guid Id { get; init; }

    public int RoundsNumber { get; init; }

    public string Title { get; init; } = default!;

    public bool IsPublic { get; set; }

    public int MaxPlayers { get; set; }

    public List<PowerUpType> BlockedPowerUps { get; set; } = default!;
}

public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateGameCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
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
        entity.Title = request.Title;
        entity.RoundsNumber = request.RoundsNumber;
        entity.MaxPlayers = request.MaxPlayers;
        entity.IsPublic = request.IsPublic;
        entity.BlockedPowerUps = request.BlockedPowerUps;

        entity.AddDomainEvent(new GameUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
