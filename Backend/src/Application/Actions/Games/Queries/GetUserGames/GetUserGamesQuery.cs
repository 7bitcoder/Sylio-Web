using AutoMapper;
using Backend.Application.Actions.Games.Queries.GetGame;
using Backend.Application.Actions.Games.Queries.GetGames;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Games.Queries.GetUserGames;

public record class GetUserGamesQuery : IRequest<GamesDto>
{
    public int ItemsPerPage { get; set; }

    public int Page { get; set; }
}

public class GetUserGamesQueryHandler : IRequestHandler<GetUserGamesQuery, GamesDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetUserGamesQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<GamesDto> Handle(GetUserGamesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Games
            .Where(x => x.Players.Any(p => p.UserId == _currentUserService.UserId))
            .AsQueryable();

        var count = await query.CountAsync(cancellationToken);

        var paginated = await query
            .Skip(request.Page * request.ItemsPerPage)
            .Take(request.ItemsPerPage)
            .OrderByDescending(x => x.Created)
            .ProjectToListAsync<GameDto>(_mapper.ConfigurationProvider, cancellationToken);

        return new GamesDto { Count = count, Games = paginated };
    }
}
