using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Application.Actions.PlayerRoundStats.Queries.GetPlayerRoundStats;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.PlayerRoundStats.Queries.GetPlayerRoundsStats;

public record class GetPlayerRoundsStatsQuery : IRequest<List<PlayerRoundStatDto>>
{
    public Guid PlayerId { get; init; }
}

public class GetPlayerRoundsStatsQueryHandler : IRequestHandler<GetPlayerRoundsStatsQuery, List<PlayerRoundStatDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlayerRoundsStatsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PlayerRoundStatDto>> Handle(GetPlayerRoundsStatsQuery request, CancellationToken cancellationToken)
    {
        var entites = await _context.PlayerRoundStats
            .Where(x => x.Player.Id == request.PlayerId)
            .ProjectTo<PlayerRoundStatDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (entites == null)
        {
            throw new NotFoundException(nameof(Player), request.PlayerId);
        }

        return entites;
    }
}
