using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Players.Commands.CreatePlayer;

public record CreatePlayerCommand : IRequest<Guid>
{
    public Guid GameId { get; set; }

    public string Name { get; set; } = "Player";

    public string Colour { get; set; } = default!;
}

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreatePlayerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var game = await _context.Games
            .Where(g => g.Id == request.GameId)
            .Include(g => g.Players)
            .FirstOrDefaultAsync(cancellationToken);

        if (game is null)
        {
            throw new NotFoundException(nameof(Game), request.GameId);
        }

        var currentUserId = _currentUser.UserId;
        if (game.Players.Any(p => p.UserId == currentUserId) || game.Players.Count >= game.MaxPlayers)
        {
            throw new ForbiddenAccessException();
        }
        var hasAdmin = game.Players.Any(p => p.GameAdmin);
        var entity = new Player
        {
            Colour = request.Colour,
            Name = request.Name,
            UserId = currentUserId ?? "",
            GameAdmin = !hasAdmin && currentUserId == game.UserId,
            Game = game
        };
        if (entity.GameAdmin)
        {
            game.State = GameState.WaitingForPlayers;
        }
        game.Players.Add(entity);

        entity.AddDomainEvent(new GamePlayerJoinedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
