using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Games.Commands.UpdateGameState;

public record UpdateGameStateCommand : IRequest<bool>
{
    public Guid Id { get; init; }

    public GameState GameState { get; init; }
}

public class UpdateGameStateCommandHandler : IRequestHandler<UpdateGameStateCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateGameStateCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(UpdateGameStateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Games
            .Where(g => g.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Game), request.Id);
        }
        entity.State = request.GameState;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
