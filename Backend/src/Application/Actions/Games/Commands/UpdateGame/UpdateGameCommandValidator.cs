using FluentValidation;

namespace Backend.Application.Actions.Games.Commands.UpdateGame;

public class UpdateGameCommandValidator : AbstractValidator<UpdateGameCommand>
{
    public UpdateGameCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty()
            .NotNull()
            .MinimumLength(1)
            .MaximumLength(70);

        RuleFor(v => v.RoundsNumber)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(20);

        RuleFor(v => v.MaxPlayers)
            .GreaterThanOrEqualTo(2)
            .LessThanOrEqualTo(8);

        RuleFor(v => v.BlockedPowerUps)
            .NotNull();

        RuleForEach(v => v.BlockedPowerUps)
            .IsInEnum();
    }
}
