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
                .WithMessage($"Длина средней части стержня l1 должна быть в диапазоне {DumbbellConstraints.CenterLengthMin}–{DumbbellConstraints.CenterLengthMax} мм.");

            RuleFor(r => r.SeatLength)
                .InclusiveBetween(DumbbellConstraints.SeatLengthMin, DumbbellConstraints.SeatLengthMax)
                .WithMessage($"Длина посадочной части стержня l2 должна быть в диапазоне {DumbbellConstraints.SeatLengthMin} – {DumbbellConstraints.SeatLengthMax} мм.");

            RuleFor(r => r.HandleDiameter)
                .InclusiveBetween(DumbbellConstraints.HandleDiameterMin, DumbbellConstraints.HandleDiameterMax)
                .WithMessage($"Диаметр рукояти d"
                             + $"1"
                             + $" должен быть в диапазоне {DumbbellConstraints.HandleDiameterMin} – {DumbbellConstraints.HandleDiameterMax} мм.");

            RuleFor(r => r.SeatDiameter)
                .InclusiveBetween(DumbbellConstraints.SeatDiameterMin, DumbbellConstraints.SeatDiameterMax)
                .WithMessage($"Диаметр посадочной части d2 должен быть в диапазоне {DumbbellConstraints.SeatDiameterMin} – {DumbbellConstraints.SeatDiameterMax} мм.");

            RuleFor(r => r)
                .Custom((rod, context) =>
                {
                    var L = rod.TotalLength;
                    if (L < DumbbellConstraints.TotalLengthMin || L > DumbbellConstraints.TotalLengthMax)
                    {
                        // Сообщение для L
                        var messageForL =
                            $"Расчётная длина стержня L = l1 + 2·l2 = {L} мм должна быть в диапазоне {DumbbellConstraints.TotalLengthMin}–{DumbbellConstraints.TotalLengthMax} мм.";

                        context.AddFailure("TotalLength", messageForL);

                        // Сообщение для l1
                        var messageForL1 =
                            $"При текущем значении l2 = {rod.SeatLength} мм длина l1 должна быть выбрана так, " +
                            $"чтобы L = l1 + 2·l2 попала в диапазон {DumbbellConstraints.TotalLengthMin}–{DumbbellConstraints.TotalLengthMax} мм.";

                        context.AddFailure("CenterLength", messageForL1);

                        // Сообщение для l2
                        var messageForL2 =
                            $"При текущем значении l1 = {rod.CenterLength} мм длина l2 должна быть выбрана так, " +
                            $"чтобы L = l1 + 2·l2 попала в диапазон {DumbbellConstraints.TotalLengthMin}–{DumbbellConstraints.TotalLengthMax} мм.";

                        context.AddFailure("SeatLength", messageForL2);
                    }
                });
        }
    }
}
