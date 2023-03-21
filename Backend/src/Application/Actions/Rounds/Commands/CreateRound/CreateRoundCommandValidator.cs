using FluentValidation;

namespace Backend.Application.Actions.Rounds.Commands.CreateRound;

public class CreateRoundCommandValidator : AbstractValidator<CreateRoundCommand>
{
    public CreateRoundCommandValidator()
    {
        RuleFor(v => v.RoundNumber)
            .GreaterThan(0);
    }
}
