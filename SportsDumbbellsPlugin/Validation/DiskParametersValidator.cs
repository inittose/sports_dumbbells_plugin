using FluentValidation;
using SportsDumbbellsPlugin.Model;

namespace SportsDumbbellsPlugin.Validation
{
    /// <summary>
    /// Валидация параметров одного диска (без связей со стержнем).
    /// </summary>
    public class DiskParametersValidator : AbstractValidator<DiskParameters>
    {
        public DiskParametersValidator()
        {
            RuleFor(d => d.DiskOuterDiameter)
                .InclusiveBetween(DumbbellConstraints.DiskOuterDiameterMin, DumbbellConstraints.DiskOuterDiameterMax)
                .WithMessage($"Внешний диаметр диска D должен быть в диапазоне {DumbbellConstraints.DiskOuterDiameterMin} – {DumbbellConstraints.DiskOuterDiameterMax} мм.");

            RuleFor(d => d.DiskHoleDiameter)
                .InclusiveBetween(DumbbellConstraints.DiskHoleDiameterMin, DumbbellConstraints.DiskHoleDiameterMax)
                .WithMessage($"Диаметр отверстия диска d должен быть в диапазоне {DumbbellConstraints.DiskHoleDiameterMin}–{DumbbellConstraints.DiskHoleDiameterMax} мм.");

            RuleFor(d => d.DiskThickness)
                .InclusiveBetween(DumbbellConstraints.DiskThicknessMin, DumbbellConstraints.DiskThicknessMax)
                .WithMessage($"Толщина одного диска t должна быть в диапазоне {DumbbellConstraints.DiskThicknessMin}–{DumbbellConstraints.DiskThicknessMax} мм.");
        }
    }
}
