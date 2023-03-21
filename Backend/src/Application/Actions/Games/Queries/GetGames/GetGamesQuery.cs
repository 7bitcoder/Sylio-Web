using AutoMapper;
using Backend.Application.Actions.Games.Queries.GetGame;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Mappings;
using Backend.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Games.Queries.GetGames;

public record class GetGamesQuery : IRequest<GamesDto>
{
    public string SearchString { get; set; } = default!;

    public int ItemsPerPage { get; set; }

    public int Page { get; set; }
}

public class GetGamesQueryHandler : IRequestHandler<GetGamesQuery, GamesDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetGamesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GamesDto> Handle(GetGamesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Games
            .Where(x => x.IsPublic && x.State == GameState.WaitingForPlayers)
            .Where(x => string.IsNullOrEmpty(request.SearchString) || x.Title.Contains(request.SearchString))
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
