using AutoMapper;
using Backend.Application.Actions.PlayerRoundStats.Queries.GetPlayerRoundStats;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Mappings;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.Actions.PlayerRoundStats.Queries.GetRoundPlayersStats;

public record class GetRoundPlayersStatsQuery : IRequest<List<PlayerRoundStatDto>>
{
    public int RoundId { get; init; }
}

public class GetRoundPlayersStatsQueryHandler : IRequestHandler<GetRoundPlayersStatsQuery, List<PlayerRoundStatDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRoundPlayersStatsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PlayerRoundStatDto>> Handle(GetRoundPlayersStatsQuery request, CancellationToken cancellationToken)
    {
        var entites = await _context.PlayerRoundStats
            .Where(x => x.Round.Id == request.RoundId)
            .ProjectToListAsync<PlayerRoundStatDto>(_mapper.ConfigurationProvider, cancellationToken);

        if (entites == null)
        {
            throw new NotFoundException(nameof(Round), request.RoundId);
        }

        return entites;
    }
}
