using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.Actions.Rounds.Commands.CreateRound;

public record CreateRoundCommand : IRequest<int>
{
    public Guid GameId { get; set; }

    public int RoundNumber { get; set; }
}

public class CreateRoundCommandHandler : IRequestHandler<CreateRoundCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateRoundCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateRoundCommand request, CancellationToken cancellationToken)
    {
        var entity = new Round
        {
            RoundNumber = request.RoundNumber,
        };

        var game = await _context.Games
             .FindAsync(new object[] { request.GameId }, cancellationToken);

        if (game is null)
        {
            throw new NotFoundException(nameof(Game), request.GameId);
        }

        game.Rounds.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
