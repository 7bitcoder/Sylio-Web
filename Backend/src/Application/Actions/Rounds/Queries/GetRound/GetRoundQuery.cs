using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Rounds.Queries.GetRound;

public record class GetRoundQuery : IRequest<RoundDto>
{
    public int Id { get; init; }
}

public class GetRoundQueryHandler : IRequestHandler<GetRoundQuery, RoundDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRoundQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoundDto> Handle(GetRoundQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Rounds
            .Where(x => x.Id == request.Id)
            .ProjectTo<RoundDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Round), request.Id);
        }

        return entity;
    }
}
