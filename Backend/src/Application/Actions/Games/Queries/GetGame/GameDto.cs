using AutoMapper;
using Backend.Application.Common.Mappings;
using Backend.Domain.Entities;
using Backend.Domain.Enums;

namespace Backend.Application.Actions.Games.Queries.GetGame;

public class GameDto : IMapFrom<Game>
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public int RoundsNumber { get; set; }

    public string? Title { get; set; }

    public GameState State { get; set; }

    public int Players { get; set; }

    public bool IsPublic { get; set; }

    public int MaxPlayers { get; set; }

    public List<PowerUpType> BlockedPowerUps { get; set; } = default!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Game, GameDto>()
            .ForMember(d => d.Players, opt => opt.MapFrom(g => g.Players.Count));
    }
}
