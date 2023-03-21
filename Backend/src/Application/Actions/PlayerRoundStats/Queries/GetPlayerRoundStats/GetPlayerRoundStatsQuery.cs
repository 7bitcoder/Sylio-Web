using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.PlayerRoundStats.Queries.GetPlayerRoundStats;

public record class GetPlayerRoundStatsQuery : IRequest<PlayerRoundStatDto>
{
    public int Id { get; init; }
}

public class GetPlayerRoundStatsQueryHandler : IRequestHandler<GetPlayerRoundStatsQuery, PlayerRoundStatDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlayerRoundStatsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PlayerRoundStatDto> Handle(GetPlayerRoundStatsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.PlayerRoundStats
            .Where(x => x.Id == request.Id)
            .ProjectTo<PlayerRoundStatDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(PlayerRoundStat), request.Id);
        }

        return entity;
    }
}
