using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Games.Queries.GetGame;

public record class GetGameQuery : IRequest<GameDto>
{
    public Guid Id { get; init; }
}

public class GetGameQueryHandler : IRequestHandler<GetGameQuery, GameDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetGameQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<GameDto> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Games
            .Where(x => x.Id == request.Id)
            .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Game), request.Id);
        }

        return entity;
    }
}
