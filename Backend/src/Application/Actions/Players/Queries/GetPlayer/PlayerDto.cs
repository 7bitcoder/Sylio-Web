using AutoMapper;
using Backend.Application.Common.Mappings;
using Backend.Domain.Entities;

namespace Backend.Application.Actions.Players.Queries.GetPLayer;

public class PlayerDto : IMapFrom<Player>
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public bool ThisPlayer { get; set; }

    public bool GameAdmin { get; set; }

    public string Name { get; set; } = default!;

    public string Colour { get; set; } = default!;


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Player, PlayerDto>()
            .ForMember(d => d.ThisPlayer, opt => opt.Ignore());
    }
}
