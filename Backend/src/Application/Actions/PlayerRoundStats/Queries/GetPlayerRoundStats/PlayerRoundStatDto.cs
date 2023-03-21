using AutoMapper;
using Backend.Application.Common.Mappings;
using Backend.Domain.Entities;

namespace Backend.Application.Actions.PlayerRoundStats.Queries.GetPlayerRoundStats;

public class PlayerRoundStatDto : IMapFrom<PlayerRoundStat>
{
    public int Id { get; set; }

    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public int Score { get; set; }

    public int Place { get; set; }

    public int Length { get; set; }

    public TimeSpan LiveTime { get; set; }

    public int RoundNumber { get; set; }

    public string? KilledBy { get; set; }

    public void Mapping(Profile profile)
    {
#pragma warning disable CS8602 // Wyłuskanie odwołania, które może mieć wartość null.
        profile.CreateMap<PlayerRoundStat, PlayerRoundStatDto>()
            .ForMember(d => d.KilledBy, opt => opt.MapFrom(p => p.KilledBy.Name))
            .ForMember(d => d.RoundNumber, opt => opt.MapFrom(p => p.Round.RoundNumber));
#pragma warning restore CS8602 // Wyłuskanie odwołania, które może mieć wartość null.
    }
}
