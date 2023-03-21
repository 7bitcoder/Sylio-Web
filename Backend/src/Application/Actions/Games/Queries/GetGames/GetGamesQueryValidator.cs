using FluentValidation;

namespace Backend.Application.Actions.Games.Queries.GetGames;

public class GetGamesQueryValidator : AbstractValidator<GetGamesQuery>
{
    public GetGamesQueryValidator()
    {
        RuleFor(v => v.SearchString)
            .MaximumLength(70);

        RuleFor(v => v.ItemsPerPage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100);

        RuleFor(v => v.Page)
            .GreaterThanOrEqualTo(0);
    }
}
