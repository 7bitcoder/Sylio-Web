using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Domain.Events;
using MediatR;

namespace Backend.Application.Actions.Games.Commands.CreateGame;

public record CreateGameCommand : IRequest<Guid>
{
    public int RoundsNumber { get; init; }

    public string Title { get; init; } = default!;

    public bool IsPublic { get; set; }

    public int MaxPlayers { get; set; }

    public List<PowerUpType> BlockedPowerUps { get; set; } = default!;
}

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateGameCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var entity = new Game
        {
            UserId = _currentUser.UserId ?? "",
            RoundsNumber = request.RoundsNumber,
            Title = request.Title,
            State = GameState.Created,
            IsPublic = request.IsPublic,
            MaxPlayers = request.MaxPlayers,
            BlockedPowerUps = request.BlockedPowerUps
        };

        entity.AddDomainEvent(new GameCreatedEvent(entity));

        _context.Games.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
