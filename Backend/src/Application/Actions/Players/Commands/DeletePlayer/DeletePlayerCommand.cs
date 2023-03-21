using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using Backend.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Players.Commands.DeletePlayer;

public record DeletePlayerCommand(Guid Id) : IRequest<Unit>;

public class DeletePlayerCommandHandler : IRequestHandler<DeletePlayerCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeletePlayerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _context.Players
            .Where(g => g.Id == request.Id)
            .Include(g => g.Game)
            .ThenInclude(g => g.Players)
            .FirstOrDefaultAsync(cancellationToken);

        if (player == null)
        {
            throw new NotFoundException(nameof(Player), request.Id);
        }

        var isThisPlayer = player.UserId == _currentUserService.UserId;

        var isAdminKick = player.Game.Players
            .Where(p => p.Id != player.Id)
            .Where(p => p.UserId == _currentUserService.UserId)
            .Select(p => p.GameAdmin)
            .FirstOrDefault();

        if (isThisPlayer)
        {
            HandleThisPlayer(player);
        }
        else if (isAdminKick)
        {
            HandleAdminKick(player);
        }
        else
        {
            throw new ForbiddenAccessException();
        }

        _context.Players.Remove(player);


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private void HandleThisPlayer(Player player)
    {
        player.AddDomainEvent(new GamePlayerLeftEvent(player));

        if (!player.GameAdmin)
        {
            return;
        }

        var nextAdmin = player.Game.Players
            .Where(p => p.Id != player.Id)
            .OrderBy(p => p.Created)
            .FirstOrDefault();

        if (nextAdmin == null)
        {
            _context.Games.Remove(player.Game);
        }
        else
        {
            player.Game.UserId = nextAdmin.UserId;
            nextAdmin.GameAdmin = true;
        }
    }

    private void HandleAdminKick(Player player)
    {
        player.AddDomainEvent(new GamePlayerKickedEvent(player));
    }
}
