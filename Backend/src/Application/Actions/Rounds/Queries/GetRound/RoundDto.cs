using Backend.Application.Common.Mappings;
using Backend.Domain.Entities;

namespace Backend.Application.Actions.Rounds.Queries.GetRound;

public class RoundDto : IMapFrom<Round>
{
    public int Id { get; set; }

    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public int RoundNumber { get; set; }
}
