using FluentValidation;
using SportsDumbbellsPlugin.Model;

namespace SportsDumbbellsPlugin.Validation
{
    /// <summary>
    /// Валидация полных параметров гантели:
    /// стержня, дисков и связей между ними.
    /// </summary>
    public class DumbbellParametersValidator : AbstractValidator<DumbbellParameters>
    {
        public DumbbellParametersValidator()
        {
            RuleFor(x => x.Rod).SetValidator(new RodParametersValidator());
            RuleForEach(x => x.Disks).SetValidator(new DiskParametersValidator());

            RuleFor(x => x.DisksPerSide)
                .InclusiveBetween(DumbbellConstraints.DisksPerSideMin, DumbbellConstraints.DisksPerSideMax)
                .WithMessage($"Количество дисков на одной стороне n должно быть в диапазоне {DumbbellConstraints.DisksPerSideMin}–{DumbbellConstraints.DisksPerSideMax}.");

            RuleFor(x => x)
                .Custom((model, context) =>
                {
                    if (model.Disks.Count != model.DisksPerSide)
                    {
                        context.AddFailure(
                            nameof(model.DisksPerSide),
                            $"Число описанных дисков ({model.Disks.Count}) должно совпадать с n = {model.DisksPerSide}.");
                    }
                });

            RuleFor(x => x)
                .Custom((model, context) =>
                {
                    var rod = model.Rod;

                    if (rod.HandleDiameter > 0 &&
                        rod.SeatDiameter > 0 &&
                        rod.HandleDiameter <= rod.SeatDiameter)
                    {
                        context.AddFailure(
                            "Rod.HandleDiameter",
                            "Диаметр рукояти d₁ должен быть больше диаметра посадочной части d₂ (d₁ > d₂).");
                    }

                    if (rod.SeatDiameter > 0)
                    {
                        for (int i = 0; i < model.Disks.Count; i++)
                        {
                            var disk = model.Disks[i];
                            double diff = disk.DiskHoleDiameter - rod.SeatDiameter;

                            if (disk.DiskHoleDiameter > 0 &&
                                (diff < DumbbellConstraints.HoleDiameterOffsetMin || diff > DumbbellConstraints.HoleDiameterOffsetMax))
                            {
                                context.AddFailure(
                                    $"Disks[{i}].DiskHoleDiameter",
                                    $"Диаметр отверстия диска d должен быть на {DumbbellConstraints.HoleDiameterOffsetMin}–{DumbbellConstraints.HoleDiameterOffsetMax} мм " +
                                    $"больше диаметра посадочной части стержня d₂ (d₂ = {rod.SeatDiameter} мм).");
                            }
                        }
                    }

                    if (rod.SeatLength > 0)
                    {
                        double totalWidth = model.TotalDiskWidthPerSide;
                        if (totalWidth > rod.SeatLength)
                        {
                            context.AddFailure(
                                nameof(model.TotalDiskWidthPerSide),
                                $"Суммарная ширина пакета дисков H = {totalWidth} мм " +
                                $"превышает длину посадочной части стержня l₂ = {rod.SeatLength} мм.");
                        }
                    }
                });
        }
    }
}