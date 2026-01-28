namespace SportsDumbbellsPluginCore.Model
{
    /// <summary>
    /// Параметры грифа гантели.
    /// Содержит геометрические размеры и выполняет их валидацию по заданным ограничениям.
    /// </summary>
    public class RodParameters
    {
        //TODO: XML
        private const double HandleLengthMin = 100.0;

        private const double HandleLengthMax = 200.0;

        private const double SeatLengthMin = 70.0;

        private const double SeatLengthMax = 150.0;

        private const double HandleDiameterMin = 24.0;

        private const double HandleDiameterMax = 34.0;

        private const double SeatDiameterMin = 24.0;

        private const double SeatDiameterMax = 34.0;

        private const double TotalLengthMin = 200.0;

        private const double TotalLengthMax = 500.0;

        /// <summary>
        /// Возвращает и задает длину рукояти (l₁), мм.
        /// </summary>
        public double HandleLength { get; set; } = 140.0;

        /// <summary>
        /// Возвращает и задает длину посадочной части (l₂), мм.
        /// </summary>
        public double SeatLength { get; set; } = 90.0;

        /// <summary>
        /// Возвращает и задает диаметр рукояти (d₁), мм.
        /// </summary>
        public double HandleDiameter { get; set; } = 28.0;

        /// <summary>
        /// Возвращает и задает диаметр посадочной части (d₂), мм.
        /// </summary>
        public double SeatDiameter { get; set; } = 26.0;

        /// <summary>
        /// Возвращает общую длину грифа (L = l₁ + 2·l₂), мм.
        /// </summary>
        public double TotalLength => HandleLength + 2 * SeatLength;

        /// <summary>
        /// Выполняет валидацию параметров грифа и возвращает список ошибок.
        /// </summary>
        /// <returns>Список ошибок валидации. Если ошибок нет, возвращается пустой список.</returns>
        public IReadOnlyList<ValidationError> Validate()
        {
            var validationErrors = new List<ValidationError>();

            ValidateHandleLength(validationErrors);
            ValidateSeatLength(validationErrors);
            ValidateHandleDiameter(validationErrors);
            ValidateSeatDiameter(validationErrors);
            ValidateDiameterRelation(validationErrors);
            ValidateTotalLength(validationErrors);

            return validationErrors;
        }

        /// <summary>
        /// Проверяет допустимость длины рукояти.
        /// </summary>
        /// <param name="validationErrors">
        /// Список ошибок, в который добавляются результаты проверки.</param>
        private void ValidateHandleLength(List<ValidationError> validationErrors)
        {
            if (HandleLength >= HandleLengthMin && HandleLength <= HandleLengthMax)
            {
                return;
            }

            validationErrors.Add(new ValidationError(
                "Rod.HandleLength",
                "Длина центральной части l₁ должна быть в диапазоне " +
                $"{HandleLengthMin}–{HandleLengthMax} мм."));
        }

        /// <summary>
        /// Проверяет допустимость длины посадочной части.
        /// </summary>
        /// <param name="validationErrors">
        /// Список ошибок, в который добавляются результаты проверки.</param>
        private void ValidateSeatLength(List<ValidationError> validationErrors)
        {
            if (SeatLength >= SeatLengthMin && SeatLength <= SeatLengthMax)
            {
                return;
            }

            validationErrors.Add(new ValidationError(
                "Rod.SeatLength",
                "Длина посадочной части l₂ должна быть в диапазоне " +
                $"{SeatLengthMin}–{SeatLengthMax} мм."));
        }

        /// <summary>
        /// Проверяет допустимость диаметра рукояти.
        /// </summary>
        /// <param name="validationErrors">
        /// Список ошибок, в который добавляются результаты проверки.</param>
        private void ValidateHandleDiameter(List<ValidationError> validationErrors)
        {
            if (HandleDiameter >= HandleDiameterMin && HandleDiameter <= HandleDiameterMax)
            {
                return;
            }

            validationErrors.Add(new ValidationError(
                "Rod.HandleDiameter",
                "Диаметр рукояти d₁ должен быть в диапазоне " +
                $"{HandleDiameterMin}–{HandleDiameterMax} мм."));
        }

        /// <summary>
        /// Проверяет допустимость диаметра посадочной части.
        /// </summary>
        /// <param name="validationErrors">
        /// Список ошибок, в который добавляются результаты проверки.</param>
        private void ValidateSeatDiameter(List<ValidationError> validationErrors)
        {
            if (SeatDiameter >= SeatDiameterMin && SeatDiameter <= SeatDiameterMax)
            {
                return;
            }

            validationErrors.Add(new ValidationError(
                "Rod.SeatDiameter",
                "Диаметр посадочной части стержня d₂ должен быть в диапазоне " +
                $"{SeatDiameterMin}–{SeatDiameterMax} мм."));
        }

        /// <summary>
        /// Проверяет взаимное ограничение диаметров: d₂ &lt; d₁.
        /// </summary>
        /// <param name="validationErrors">
        /// Список ошибок, в который добавляются результаты проверки.</param>
        private void ValidateDiameterRelation(List<ValidationError> validationErrors)
        {
            if (SeatDiameter < HandleDiameter)
            {
                return;
            }

            validationErrors.Add(new ValidationError(
                "Rod.SeatDiameter",
                "Диаметр посадочной части стержня d₂ должен быть меньше чем " +
                $"диаметр рукояти d₁ (d₁ = {HandleDiameter:F1} мм)."));

            validationErrors.Add(new ValidationError(
                "Rod.HandleDiameter",
                "Диаметр рукояти d₁ должен быть больше чем диаметр посадочной " +
                $"части стержня d₂ (d₂ = {SeatDiameter:F1} мм)."));
        }

        /// <summary>
        /// Проверяет допустимость общей длины грифа.
        /// </summary>
        /// <param name="validationErrors">
        /// Список ошибок, в который добавляются результаты проверки.</param>
        private void ValidateTotalLength(List<ValidationError> validationErrors)
        {
            if (TotalLength >= TotalLengthMin && TotalLength <= TotalLengthMax)
            {
                return;
            }

            var totalLengthMessage =
                "Общая длина стержня L = l₁ + 2·l₂ должна быть в диапазоне " +
                $"{TotalLengthMin}–{TotalLengthMax} мм.";

            validationErrors.Add(
                new ValidationError("Rod.HandleLength", totalLengthMessage));

            validationErrors.Add(new ValidationError("Rod.SeatLength", totalLengthMessage));
        }
    }
}