namespace SportsDumbbellsPlugin.Model
{
    public class DumbbellParameters
    {
        private const int DisksPerSideMin = 0;

        private const int DisksPerSideMax = 8;

        private const double HoleDiameterOffsetMin = 0.5;

        private const double HoleDiameterOffsetMax = 1.5;

        public RodParameters Rod { get; set; } = new ();

        public List<DiskParameters> Disks { get; } = new ();

        public int DisksPerSide { get; set; }

        public double TotalDiskWidthPerSide => Disks.Take(DisksPerSide).Sum(d => d.DiskThickness);

        public IReadOnlyList<ValidationError> Validate()
        {
            var errors = new List<ValidationError>();

            errors.AddRange(Rod.Validate());

            for (var i = 0; i < Disks.Count; i++)
            {
                var diskErrors = Disks[i].Validate();
                foreach (var err in diskErrors)
                {
                    var source = $"Disks[{i}].{err.Source.Replace("Disk.", "")}";
                    errors.Add(new ValidationError(source, err.Message));
                }
            }

            if (DisksPerSide < DisksPerSideMin || DisksPerSide > DisksPerSideMax)
            {
                errors.Add(
                    new ValidationError(
                        "Dumbbell.DisksPerSide",
                        $"Количество дисков на стороне должно быть в диапазоне {DisksPerSideMin}–{DisksPerSideMax}."));
            }

            for (var i = 0; i < DisksPerSide && i < Disks.Count; i++)
            {
                var disk = Disks[i];

                var delta = disk.DiskHoleDiameter - Rod.SeatDiameter;
                if (delta < HoleDiameterOffsetMin || delta > HoleDiameterOffsetMax)
                {
                    var msgForDisk =
                        $"Диаметр отверстия диска d должен быть на {HoleDiameterOffsetMin:0.0}–{HoleDiameterOffsetMax:0.0} мм больше "
                        + $"диаметра посадочной части стержня d₂ (d₂ = {Rod.SeatDiameter:F1} мм).";

                    var msgForRod =
                        $"Диаметр посадочной части стержня d₂ должен быть на {HoleDiameterOffsetMin:0.0}–{HoleDiameterOffsetMax:0.0} мм меньше "
                        + $"диаметра отверстия диска d (d = {disk.DiskHoleDiameter:F1} мм).";

                    errors.Add(new ValidationError($"Disks[{i}].DiskHoleDiameter", msgForDisk));
                    errors.Add(new ValidationError("Rod.SeatDiameter", msgForRod));
                }
            }

            var H = TotalDiskWidthPerSide;
            if (!(H > Rod.SeatLength))
            {
                return errors;
            }

            var messageForDisks =
                $"Суммарная ширина пакета дисков H = {H:F1} мм не должна превышать длину посадочной части стержня l₂ = {Rod.SeatLength:F1} мм.";

            var messageForRod =
                $"Длина посадочной части стержня l₂ = {Rod.SeatLength:F1} мм меньше суммарной ширины пакета дисков H = {H:F1} мм.";

            for (var i = 0; i < DisksPerSide && i < Disks.Count; i++)
            {
                errors.Add(new ValidationError($"Disks[{i}].DiskThickness", messageForDisks));
            }

            errors.Add(new ValidationError("Rod.SeatLength", messageForRod));

            return errors;

        }
    }
}