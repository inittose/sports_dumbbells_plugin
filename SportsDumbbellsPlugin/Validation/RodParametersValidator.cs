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
                .InclusiveBetween(DumbbellConstraints.CenterLengthMin, DumbbellConstraints.CenterLengthMax)
                .WithMessage($"Длина средней части стержня l₁ должна быть в диапазоне {DumbbellConstraints.CenterLengthMin}–{DumbbellConstraints.CenterLengthMax} мм.");

            RuleFor(r => r.SeatLength)
                .InclusiveBetween(DumbbellConstraints.SeatLengthMin, DumbbellConstraints.SeatLengthMax)
                .WithMessage($"Длина посадочной части стержня l₂ должна быть в диапазоне {DumbbellConstraints.SeatLengthMin} – {DumbbellConstraints.SeatLengthMax} мм.");

            RuleFor(r => r.HandleDiameter)
                .InclusiveBetween(DumbbellConstraints.HandleDiameterMin, DumbbellConstraints.HandleDiameterMax)
                .WithMessage($"Диаметр рукояти d₁ должен быть в диапазоне {DumbbellConstraints.HandleDiameterMin} – {DumbbellConstraints.HandleDiameterMax} мм.");

            RuleFor(r => r.SeatDiameter)
                .InclusiveBetween(DumbbellConstraints.SeatDiameterMin, DumbbellConstraints.SeatDiameterMax)
                .WithMessage($"Диаметр посадочной части d₂ должен быть в диапазоне {DumbbellConstraints.SeatDiameterMin} – {DumbbellConstraints.SeatDiameterMax} мм.");
        }
    }
}
