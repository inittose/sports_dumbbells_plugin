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

        public double DiskHoleDiameter { get; set; } = 27.0;
        public double DiskOuterDiameter { get; set; } = 150.0;
        public double DiskThickness { get; set; } = 20.0;

        public IReadOnlyList<ValidationError> Validate()
        {
            var errors = new List<ValidationError>();

            if (DiskOuterDiameter < DiskOuterDiameterMin || DiskOuterDiameter > DiskOuterDiameterMax)
                errors.Add(new ValidationError(
                    "Disk.DiskOuterDiameter",
                    $"Внешний диаметр диска D должен быть в диапазоне {DiskOuterDiameterMin}–{DiskOuterDiameterMax} мм."));

            if (DiskHoleDiameter < DiskHoleDiameterMin || DiskHoleDiameter > DiskHoleDiameterMax)
                errors.Add(new ValidationError(
                    "Disk.DiskHoleDiameter",
                    $"Диаметр отверстия диска d должен быть в диапазоне {DiskHoleDiameterMin}–{DiskHoleDiameterMax} мм."));

            if (DiskThickness < DiskThicknessMin || DiskThickness > DiskThicknessMax)
                errors.Add(new ValidationError(
                    "Disk.DiskThickness",
                    $"Толщина диска t должна быть в диапазоне {DiskThicknessMin}–{DiskThicknessMax} мм."));

            return errors;
        }
    }
}
