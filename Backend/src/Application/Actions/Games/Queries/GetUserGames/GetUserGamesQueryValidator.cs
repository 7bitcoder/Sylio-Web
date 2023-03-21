using FluentValidation;

namespace Backend.Application.Actions.Games.Queries.GetUserGames;

public class GetUserGamesQueryValidator : AbstractValidator<GetUserGamesQuery>
{
    public GetUserGamesQueryValidator()
    {
        RuleFor(v => v.ItemsPerPage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100);

        RuleFor(v => v.Page)
            .GreaterThanOrEqualTo(0);
    }
}
