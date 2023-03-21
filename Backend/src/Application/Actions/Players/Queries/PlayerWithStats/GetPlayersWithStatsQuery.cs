using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Players.Queries.PlayerWithStats;

public record class GetPlayersWithStatsQuery : IRequest<List<PlayerWithStatsDto>>
{
    public Guid GameId { get; set; }
}

public class GetPlayersWithStatsQueryHandler : IRequestHandler<GetPlayersWithStatsQuery, List<PlayerWithStatsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetPlayersWithStatsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<List<PlayerWithStatsDto>> Handle(GetPlayersWithStatsQuery request, CancellationToken cancellationToken)
    {
        var entites = await _context.Players
                   .Where(x => x.Game.Id == request.GameId)
                   .Include(x => x.RoundsStats)
                   .ProjectTo<PlayerWithStatsDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

        if (entites == null)
        {
            throw new NotFoundException(nameof(Round), request.GameId);
        }

        return entites;
    }
}
