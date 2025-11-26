using FluentValidation;
using SportsDumbbellsPlugin.Model;

namespace SportsDumbbellsPlugin.Validation
{
    /// <summary>
    /// Валидация параметров стержня.
    /// </summary>
    public class RodParametersValidator : AbstractValidator<RodParameters>
    {
        public RodParametersValidator()
        {
            RuleFor(r => r.CenterLength)
                .InclusiveBetween(100, 200)
                .WithMessage("Длина средней части стержня l₁ должна быть в диапазоне 100–200 мм.");

            RuleFor(r => r.SeatLength)
                .InclusiveBetween(70, 150)
                .WithMessage("Длина посадочной части стержня l₂ должна быть в диапазоне 70–150 мм.");

            RuleFor(r => r.HandleDiameter)
                .InclusiveBetween(24, 34)
                .WithMessage("Диаметр рукояти d₁ должен быть в диапазоне 24–34 мм.");

            RuleFor(r => r.SeatDiameter)
                .InclusiveBetween(24, 34)
                .WithMessage("Диаметр посадочной части d₂ должен быть в диапазоне 24–34 мм.");
        }
    }
}
