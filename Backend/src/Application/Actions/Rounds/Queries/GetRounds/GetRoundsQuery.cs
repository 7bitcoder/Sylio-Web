using AutoMapper;
using Backend.Application.Actions.Rounds.Queries.GetRound;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Mappings;
using Backend.Domain.Entities;
using MediatR;

namespace Backend.Application.Actions.Rounds.Queries.GetRounds;

public record class GetRoundsQuery : IRequest<List<RoundDto>>
{
    public Guid GameId { get; init; }
}

public class GetRoundsQueryHandler : IRequestHandler<GetRoundsQuery, List<RoundDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRoundsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<RoundDto>> Handle(GetRoundsQuery request, CancellationToken cancellationToken)
    {
        var entites = await _context.Rounds
            .Where(x => x.Game.Id == request.GameId)
            .ProjectToListAsync<RoundDto>(_mapper.ConfigurationProvider, cancellationToken);

        if (entites == null)
        {
            throw new NotFoundException(nameof(Round), request.GameId);
        }

        return entites;
    }
}
