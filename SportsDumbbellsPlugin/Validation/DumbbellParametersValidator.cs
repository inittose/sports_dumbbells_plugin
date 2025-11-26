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
                            "Диаметр рукояти d1 должен быть больше диаметра посадочной части d2 (d1 > d2).");

                        context.AddFailure(
                            "Rod.SeatDiameter",
                            "Диаметр рукояти d1 должен быть больше диаметра посадочной части d2 (d1 > d2).");
                    }

                    if (rod.SeatDiameter > 0)
                    {
                        for (var i = 0; i < model.Disks.Count; i++)
                        {
                            var disk = model.Disks[i];
                            var diff = disk.DiskHoleDiameter - rod.SeatDiameter;

                            if (!(disk.DiskHoleDiameter > 0)
                                || (!(diff < DumbbellConstraints.HoleDiameterOffsetMin)
                                    && !(diff > DumbbellConstraints.HoleDiameterOffsetMax)))
                                continue;
                            context.AddFailure(
                                $"Disks[{i}].DiskHoleDiameter",
                                $"Диаметр отверстия диска d должен быть на {DumbbellConstraints.HoleDiameterOffsetMin}–{DumbbellConstraints.HoleDiameterOffsetMax} мм " +
                                $"больше диаметра посадочной части стержня d2 (d2 = {rod.SeatDiameter} мм).");

                            context.AddFailure(
                                $"Rod.SeatDiameter",
                                $"Диаметр посадочной части стержня d2 должен быть на {DumbbellConstraints.HoleDiameterOffsetMin}–{DumbbellConstraints.HoleDiameterOffsetMax} мм " +
                                $"меньше диаметра отверстия диска d " +
                                $"(для диска {i + 1}: d = {disk.DiskHoleDiameter} мм).");
                        }
                    }

                    if (!(rod.SeatLength > 0)) return;
                    var totalWidth = model.TotalDiskWidthPerSide;
                    if (!(totalWidth > rod.SeatLength)) return;
                    context.AddFailure(
                        nameof(model.TotalDiskWidthPerSide),
                        $"Суммарная ширина дисков H = {totalWidth} мм " +
                        $"превышает длину посадочной части стержня l2 = {rod.SeatLength} мм.");

                    context.AddFailure(
                        "Rod.SeatLength",
                        $"При текущей суммарной толщине дисков H = {totalWidth} мм длина посадочной части стержня l2 " +
                        $"должна быть не меньше H.");
                });
        }
    }
}