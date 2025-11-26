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
                .InclusiveBetween(120, 260)
                .WithMessage("Внешний диаметр диска D должен быть в диапазоне 120–260 мм.");

            RuleFor(d => d.DiskHoleDiameter)
                .InclusiveBetween(26, 34)
                .WithMessage("Диаметр отверстия диска d должен быть в диапазоне 26–34 мм.");

            RuleFor(d => d.DiskThickness)
                .InclusiveBetween(10, 40)
                .WithMessage("Толщина одного диска t должна быть в диапазоне 10–40 мм.");
        }
    }
}
