using AutoMapper;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Application.Actions.Players.Queries.GetPLayer;

public record class GetPlayerQuery : IRequest<PlayerDto>
{
    public Guid Id { get; init; }
}

public class GetPlayerQueryHandler : IRequestHandler<GetPlayerQuery, PlayerDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetPlayerQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<PlayerDto> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Players
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Player), request.Id);
        }

        var mapped = _mapper.Map<PlayerDto>(entity);
        mapped.ThisPlayer = entity.UserId == _currentUserService.UserId;
        return mapped;
    }
}
