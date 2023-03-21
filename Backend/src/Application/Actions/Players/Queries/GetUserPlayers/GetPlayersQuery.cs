using AutoMapper;
using Backend.Application.Actions.Players.Queries.GetPLayer;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Players.Queries.GetUserPlayers;

public record class GetUserPlayersQuery : IRequest<PlayersDto>
{
    public int ItemsPerPage { get; set; }

    public int Page { get; set; }
}

public class GetUserPlayersQueryHandler : IRequestHandler<GetUserPlayersQuery, PlayersDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetUserPlayersQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<PlayersDto> Handle(GetUserPlayersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Players
           .Where(p => p.UserId == _currentUserService.UserId)
           .AsQueryable();

        var count = await query.CountAsync(cancellationToken);

        var paginated = await query
            .Skip(request.Page * request.ItemsPerPage)
            .Take(request.ItemsPerPage)
            .OrderByDescending(x => x.Created)
            .ProjectToListAsync<PlayerDto>(_mapper.ConfigurationProvider, cancellationToken);

        return new PlayersDto { Players = paginated, Count = count };
    }
}
