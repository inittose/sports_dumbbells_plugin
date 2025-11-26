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
            RuleFor(x => x.Rod)
                .SetValidator(new RodParametersValidator());

            RuleForEach(x => x.Disks)
                .SetValidator(new DiskParametersValidator());

            RuleFor(x => x.DisksPerSide)
                .InclusiveBetween(0, 8)
                .WithMessage("Количество дисков на одной стороне n должно быть в диапазоне 0–8.");

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
                        for (var i = 0; i < model.Disks.Count; i++)
                        {
                            var disk = model.Disks[i];
                            var diff = disk.DiskHoleDiameter - rod.SeatDiameter;

                            if (disk.DiskHoleDiameter > 0 &&
                                (diff < 0.5 || diff > 1.5))
                            {
                                context.AddFailure(
                                    $"Disks[{i}].DiskHoleDiameter",
                                    $"Диаметр отверстия диска d должен быть на 0,5–1,5 мм больше " +
                                    $"диаметра посадочной части стержня d₂. Сейчас d₂ = {rod.SeatDiameter:F1} мм.");
                            }
                        }
                    }

                    if (!(rod.SeatLength > 0)) return;
                    var totalWidth = model.TotalDiskWidthPerSide;
                    if (totalWidth > rod.SeatLength)
                    {
                        context.AddFailure(
                            nameof(model.TotalDiskWidthPerSide),
                            $"Суммарная ширина пакета дисков H = {totalWidth:F1} мм " +
                            $"превышает длину посадочной части стержня l₂ = {rod.SeatLength:F1} мм.");
                    }
                });
        }
    }
}