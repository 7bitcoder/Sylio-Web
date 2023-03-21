using FluentValidation;

namespace Backend.Application.Actions.Players.Commands.CreatePlayer;

public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
{
    private readonly string[] _availableColors = new[] {
        "#800000",
        "#9a6324",
        "#808000",
        "#469990",
        "#000075",
        "#000000",
        "#e6194b",
        "#f58231",
        "#ffe119",
        "#bfef45",
        "#3cb44b",
        "#42d4f4",
        "#4363d8",
        "#911eb4",
        "#f032e6",
        "#a9a9a9",
        "#dcbeff",
        "#aaffc3",
    };

    public CreatePlayerCommandValidator()
    {
        RuleFor(v => v.Name)
            .MinimumLength(1)
            .MaximumLength(30);

        RuleFor(v => v.Colour)
            .Must(x => _availableColors.Contains(x))
            .WithMessage("Please chose one of colors: " + String.Join(",", _availableColors));
    }
}
