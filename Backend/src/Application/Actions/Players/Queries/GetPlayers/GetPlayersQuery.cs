using AutoMapper;
using Backend.Application.Actions.Players.Queries.GetPLayer;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Players.Queries.GetPlayers;

public record class GetPlayersQuery : IRequest<List<PlayerDto>>
{
    public Guid GameId { get; init; }
}

public class GetPlayersQueryHandler : IRequestHandler<GetPlayersQuery, List<PlayerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetPlayersQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<List<PlayerDto>> Handle(GetPlayersQuery request, CancellationToken cancellationToken)
    {
        var entites = await _context.Players
            .Where(x => x.Game.Id == request.GameId)
            .OrderBy(x => x.Created)
            .IgnoreAutoIncludes()
            .ToListAsync(cancellationToken);

        if (entites == null)
        {
            throw new NotFoundException(nameof(Player), request.GameId);
        }

        var mappedList = _mapper.Map<List<PlayerDto>>(entites);
        var currentUserId = _currentUserService.UserId;
        foreach (var (entity, mapped) in entites.Zip(mappedList))
        {
            mapped.ThisPlayer = entity.UserId == currentUserId;
        }

        return mappedList;
    }
}
