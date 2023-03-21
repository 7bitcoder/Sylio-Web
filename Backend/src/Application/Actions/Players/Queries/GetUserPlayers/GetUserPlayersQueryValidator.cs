using FluentValidation;

namespace Backend.Application.Actions.Players.Queries.GetUserPlayers;

public class GetUserPlayersQueryValidator : AbstractValidator<GetUserPlayersQuery>
{
    public GetUserPlayersQueryValidator()
    {
        RuleFor(v => v.ItemsPerPage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100);

        RuleFor(v => v.Page)
            .GreaterThanOrEqualTo(0);
    }
}
