namespace SportsDumbbellsPluginCore.Model
{
    /// <summary>
    /// Параметры гантели.
    /// Содержит параметры грифа, параметры дисков и выполняет комплексную валидацию,
    /// включая взаимные ограничения между элементами.
    /// </summary>
    public class DumbbellParameters
    {
        //TODO: XML
        private const int DisksPerSideMin = 0;

        private const int DisksPerSideMax = 8;

        private const double HoleDiameterOffsetMin = 0.5;

        private const double HoleDiameterOffsetMax = 1.5;

        /// <summary>
        /// Зазор между соседними дисками, мм.
        /// </summary>
        public static double GapBetweenDisks => 0.1;

        /// <summary>
        /// Возвращает и задает параметры грифа гантели.
        /// </summary>
        public RodParameters Rod { get; set; } = new ();

        /// <summary>
        /// Возвращает и задает список параметров дисков.
        /// Диски в списке считаются расположенными последовательно от грифа наружу.
        /// </summary>
        public List<DiskParameters> Disks { get; } = new ();

        /// <summary>
        /// Возвращает и задает количество дисков на одной стороне гантели.
        /// </summary>
        public int DisksPerSide { get; set; }

        /// <summary>
        /// Суммарная ширина пакета дисков на одной стороне, мм.
        /// Считается как сумма толщин первых <see cref="DisksPerSide"/> дисков.
        /// </summary>
        public double TotalDiskWidthPerSide =>
            Disks.Take(DisksPerSide).Sum(d => d.Thickness);

        /// <summary>
        /// Выполняет валидацию параметров гантели и возвращает список ошибок.
        /// Включает валидацию грифа, дисков и взаимные ограничения.
        /// </summary>
        /// <returns>Список ошибок валидации.
        /// Если ошибок нет, возвращается пустой список.</returns>
        public IReadOnlyList<ValidationError> Validate()
        {
            var validationErrors = new List<ValidationError>();

            validationErrors.AddRange(Rod.Validate());

            for (var diskIndex = 0; diskIndex < Disks.Count; diskIndex++)
            {
                var diskValidationErrors = Disks[diskIndex].Validate();

                foreach (var diskError in diskValidationErrors)
                {
                    var errorSourceSuffix = diskError.Source.Replace("Disk.", string.Empty);
                    var source = $"Disks[{diskIndex}].{errorSourceSuffix}";

                    validationErrors.Add(new ValidationError(source, diskError.Message));
                }
            }

            if (DisksPerSide < DisksPerSideMin || DisksPerSide > DisksPerSideMax)
            {
                validationErrors.Add(new ValidationError(
                    "Dumbbell.DisksPerSide",
                    "Количество дисков на стороне должно быть в диапазоне " +
                    $"{DisksPerSideMin}–{DisksPerSideMax}."));
            }

            ValidateHoleDiameterOffset(validationErrors);
            ValidateDiskPackWidth(validationErrors);

            return validationErrors;
        }

        /// <summary>
        /// Проверяет ограничение на разницу между диаметром отверстия диска
        /// и посадочным диаметром грифа. Для каждого из первых <see cref="DisksPerSide"/> дисков
        /// разница должна быть в диапазоне
        /// <see cref="HoleDiameterOffsetMin"/>–<see cref="HoleDiameterOffsetMax"/>.
        /// </summary>
        /// <param name="validationErrors">
        /// Список ошибок, в который добавляются результаты проверки.</param>
        private void ValidateHoleDiameterOffset(List<ValidationError> validationErrors)
        {
            for (
                var diskIndex = 0;
                diskIndex < DisksPerSide && diskIndex < Disks.Count;
                diskIndex++)
            {
                var diskParameters = Disks[diskIndex];

                var holeDiameterDelta = diskParameters.HoleDiameter - Rod.SeatDiameter;
                var deltaIsTooSmall = holeDiameterDelta < HoleDiameterOffsetMin;
                var deltaIsTooLarge = holeDiameterDelta > HoleDiameterOffsetMax;

                if (!deltaIsTooSmall && !deltaIsTooLarge)
                {
                    continue;
                }

                var messageForDisk =
                    "Диаметр отверстия диска d должен быть на " +
                    $"{HoleDiameterOffsetMin:0.0}–{HoleDiameterOffsetMax:0.0} мм больше " +
                    $"диаметра посадочной части стержня d₂ (d₂ = {Rod.SeatDiameter:F1} мм).";

                var messageForRod =
                    "Диаметр посадочной части стержня d₂ должен быть на " +
                    $"{HoleDiameterOffsetMin:0.0}–{HoleDiameterOffsetMax:0.0} мм меньше " +
                    $"диаметра отверстия диска d (d = {diskParameters.HoleDiameter:F1} мм).";

                validationErrors.Add(
                    new ValidationError($"Disks[{diskIndex}].HoleDiameter", messageForDisk));

                validationErrors.Add(new ValidationError("Rod.SeatDiameter", messageForRod));
            }
        }

        /// <summary>
        /// Проверяет ограничение на суммарную ширину пакета дисков.
        /// Суммарная ширина H не должна превышать длину посадочной части грифа l₂.
        /// </summary>
        /// <param name="validationErrors">
        /// Список ошибок, в который добавляются результаты проверки.</param>
        private void ValidateDiskPackWidth(List<ValidationError> validationErrors)
        {
            var totalDiskWidthPerSide = TotalDiskWidthPerSide;

            if (totalDiskWidthPerSide <= Rod.SeatLength)
            {
                return;
            }

            var messageForDisks =
                $"Суммарная ширина пакета дисков H = {totalDiskWidthPerSide:F1} мм не должна " +
                $"превышать длину посадочной части стержня l₂ = {Rod.SeatLength:F1} мм.";

            var messageForRod =
                $"Длина посадочной части стержня l₂ = {Rod.SeatLength:F1} мм меньше суммарной " +
                $"ширины пакета дисков H = {totalDiskWidthPerSide:F1} мм.";

            for (
                var diskIndex = 0;
                diskIndex < DisksPerSide && diskIndex < Disks.Count;
                diskIndex++)
            {
                validationErrors.Add(
                    new ValidationError($"Disks[{diskIndex}].Thickness", messageForDisks));
            }

            validationErrors.Add(new ValidationError("Rod.SeatLength", messageForRod));
        }
    }
}
