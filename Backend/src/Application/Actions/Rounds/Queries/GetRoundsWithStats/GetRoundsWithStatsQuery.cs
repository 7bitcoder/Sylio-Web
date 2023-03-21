using AutoMapper;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Mappings;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Rounds.Queries.GetRoundsWithStats;

public record class GetRoundsWithStatsQuery : IRequest<List<RoundWithStatsDto>>
{
    public Guid GameId { get; init; }
}

public class GetRoundsWithStatsHandler : IRequestHandler<GetRoundsWithStatsQuery, List<RoundWithStatsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRoundsWithStatsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<RoundWithStatsDto>> Handle(GetRoundsWithStatsQuery request, CancellationToken cancellationToken)
    {
        var entites = await _context.Rounds
            .Where(x => x.Game.Id == request.GameId)
            .Include(x => x.PlayersRoundStats)
            .ProjectToListAsync<RoundWithStatsDto>(_mapper.ConfigurationProvider, cancellationToken);

        if (entites == null)
        {
            throw new NotFoundException(nameof(Round), request.GameId);
        }

        return entites;
    }
}
