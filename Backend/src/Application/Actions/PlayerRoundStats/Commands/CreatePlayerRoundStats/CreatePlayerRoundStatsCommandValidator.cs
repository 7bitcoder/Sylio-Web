using FluentValidation;

namespace Backend.Application.Actions.PlayerRoundStats.Commands.CreatePlayerRoundStats;

public class CreatePlayerRoundStatsCommandValidator : AbstractValidator<CreatePlayerRoundStatsCommand>
{
    public CreatePlayerRoundStatsCommandValidator()
    {
        RuleFor(v => v.Score)
            .GreaterThanOrEqualTo(0);

        RuleFor(v => v.Place)
            .GreaterThan(0);
    }
}
