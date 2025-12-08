namespace SportsDumbbellsPlugin.Model
{
    public class DumbbellParameters
    {
        private const int DisksPerSideMin = 0;

        private const int DisksPerSideMax = 8;

        // допустимое превышение d над d2
        private const double HoleDiameterOffsetMin = 0.5;

        private const double HoleDiameterOffsetMax = 1.5;

        public RodParameters Rod { get; set; } = new ();

        public List<DiskParameters> Disks { get; } = new ();

        public int DisksPerSide { get; set; }

        public double TotalDiskWidthPerSide => Disks.Take(DisksPerSide).Sum(d => d.DiskThickness);

        public IReadOnlyList<ValidationError> Validate()
        {
            var errors = new List<ValidationError>();

            // 1. Локальные ошибки стержня
            errors.AddRange(Rod.Validate());

            // 2. Локальные ошибки каждого диска
            for (int i = 0; i < Disks.Count; i++)
            {
                var diskErrors = Disks[i].Validate();
                foreach (var err in diskErrors)
                {
                    // Перепакуем Source с индексом диска
                    var source = $"Disks[{i}].{err.Source.Replace("Disk.", "")}";
                    errors.Add(new ValidationError(source, err.Message));
                }
            }

            // 3. Проверка количества дисков
            if (DisksPerSide < DisksPerSideMin || DisksPerSide > DisksPerSideMax)
            {
                errors.Add(
                    new ValidationError(
                        "Dumbbell.DisksPerSide",
                        $"Количество дисков на стороне должно быть в диапазоне {DisksPerSideMin}–{DisksPerSideMax}."));
            }

            // 4. d (отверстие диска) vs d2 (посадка стержня) — кроссвалидация
            for (int i = 0; i < DisksPerSide && i < Disks.Count; i++)
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

            // 5. H ≤ l2
            var H = TotalDiskWidthPerSide;
            if (H > Rod.SeatLength)
            {
                var messageForDisks =
                    $"Суммарная ширина пакета дисков H = {H:F1} мм не должна превышать длину посадочной части стержня l₂ = {Rod.SeatLength:F1} мм.";

                var messageForRod =
                    $"Длина посадочной части стержня l₂ = {Rod.SeatLength:F1} мм меньше суммарной ширины пакета дисков H = {H:F1} мм.";

                // Ошибка для всех дисков
                for (int i = 0; i < DisksPerSide && i < Disks.Count; i++)
                {
                    errors.Add(new ValidationError($"Disks[{i}].DiskThickness", messageForDisks));
                }

                // И для стержня
                errors.Add(new ValidationError("Rod.SeatLength", messageForRod));
            }

            return errors;

        }
    }
}