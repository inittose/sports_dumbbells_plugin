namespace SportsDumbbellsPlugin.Model
{
    public class DiskParameters
    {
        private const double DiskOuterDiameterMin = 120.0;
        private const double DiskOuterDiameterMax = 260.0;

        private const double DiskHoleDiameterMin = 26.0;
        private const double DiskHoleDiameterMax = 34.0;

        private const double DiskThicknessMin = 10.0;
        private const double DiskThicknessMax = 40.0;

        public double HoleDiameter { get; set; } = 27.0;
        public double OuterDiameter { get; set; } = 150.0;
        public double Thickness { get; set; } = 20.0;

        public IReadOnlyList<ValidationError> Validate()
        {
            var errors = new List<ValidationError>();

            if (OuterDiameter < DiskOuterDiameterMin || OuterDiameter > DiskOuterDiameterMax)
                errors.Add(new ValidationError(
                    "Disk.OuterDiameter",
                    $"Внешний диаметр диска D должен быть в диапазоне {DiskOuterDiameterMin}–{DiskOuterDiameterMax} мм."));

            if (HoleDiameter < DiskHoleDiameterMin || HoleDiameter > DiskHoleDiameterMax)
                errors.Add(new ValidationError(
                    "Disk.HoleDiameter",
                    $"Диаметр отверстия диска d должен быть в диапазоне {DiskHoleDiameterMin}–{DiskHoleDiameterMax} мм."));

            if (Thickness < DiskThicknessMin || Thickness > DiskThicknessMax)
                errors.Add(new ValidationError(
                    "Disk.Thickness",
                    $"Толщина диска t должна быть в диапазоне {DiskThicknessMin}–{DiskThicknessMax} мм."));

            return errors;
        }
    }
}
