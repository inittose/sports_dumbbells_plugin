using SportsDumbbellsPluginCore.Validation;

namespace SportsDumbbellsPluginCore.Model
{
    /// <summary>
    /// Параметры грифа гантели.
    /// Содержит геометрические размеры и выполняет их валидацию по заданным ограничениям.
    /// </summary>
    public class RodParameters
    {
        //TODO: XML
        // +

        /// <summary>
        /// Ширина одной кольцевой прорези, мм.
        /// </summary>
        public const double GrooveWidth = 6.0;

        /// <summary>
        /// Отступ первой и последней прорези от края рукояти, мм.
        /// </summary>
        public const double GrooveEdgeIndent = 8.0;

        /// <summary>
        /// Минимальный зазор между соседними прорезями, мм.
        /// </summary>
        public const double GrooveMinGap = 4.0;

        /// <summary>
        /// Минимально допустимая длина рукояти, мм.
        /// </summary>
        private const double HandleLengthMin = 100.0;

        /// <summary>
        /// Максимально допустимая длина рукояти, мм.
        /// </summary>
        private const double HandleLengthMax = 200.0;

        /// <summary>
        /// Минимально допустимая длина посадочной части, мм.
        /// </summary>
        private const double SeatLengthMin = 70.0;

        /// <summary>
        /// Максимально допустимая длина посадочной части, мм.
        /// </summary>
        private const double SeatLengthMax = 150.0;

        /// <summary>
        /// Минимально допустимый диаметр рукояти, мм.
        /// </summary>
        private const double HandleDiameterMin = 24.0;

        /// <summary>
        /// Максимально допустимый диаметр рукояти, мм.
        /// </summary>
        private const double HandleDiameterMax = 34.0;

        /// <summary>
        /// Минимально допустимый диаметр посадочной части, мм.
        /// </summary>
        private const double SeatDiameterMin = 24.0;

        /// <summary>
        /// Максимально допустимый диаметр посадочной части, мм.
        /// </summary>
        private const double SeatDiameterMax = 34.0;

        /// <summary>
        /// Минимально допустимая общая длина грифа, мм.
        /// </summary>
        private const double TotalLengthMin = 200.0;

        /// <summary>
        /// Максимально допустимая общая длина грифа, мм.
        /// </summary>
        private const double TotalLengthMax = 500.0;

        /// <summary>
        /// Минимально допустимое количество прорезей.
        /// </summary>
        private const int GrooveCountMin = 0;

        /// <summary>
        /// Максимально допустимое количество прорезей.
        /// </summary>
        private const int GrooveCountMax = 12;

        /// <summary>
        /// Минимально допустимая глубина прорези, мм.
        /// </summary>
        private const double GrooveDepthMin = 0.0;

        /// <summary>
        /// Максимально допустимая глубина прорези, мм.
        /// </summary>
        private const double GrooveDepthMax = 5.0;

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
        /// Возвращает и задает количество кольцевых прорезей на рукояти.
        /// </summary>
        public int GrooveCount { get; set; } = 5;

        /// <summary>
        /// Возвращает и задает глубину кольцевой прорези на рукояти, мм.
        /// </summary>
        public double GrooveDepth { get; set; } = 0.5;

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
            ValidateGrooveCount(validationErrors);
            ValidateGrooveDepth(validationErrors);
            ValidateGrooveDepthAgainstRod(validationErrors);
            ValidateGroovesFitHandleLength(validationErrors);

            return validationErrors;
        }

        /// <summary>
        /// Проверяет допустимость длины рукояти.
        /// </summary>
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
        private void ValidateTotalLength(List<ValidationError> validationErrors)
        {
            if (TotalLength >= TotalLengthMin && TotalLength <= TotalLengthMax)
            {
                return;
            }

            var totalLengthMessage =
                "Общая длина стержня L = l₁ + 2·l₂ должна быть в диапазоне " +
                $"{TotalLengthMin}–{TotalLengthMax} мм.";

            validationErrors.Add(new ValidationError("Rod.HandleLength", totalLengthMessage));
            validationErrors.Add(new ValidationError("Rod.SeatLength", totalLengthMessage));
        }

        /// <summary>
        /// Проверяет количество прорезей.
        /// </summary>
        private void ValidateGrooveCount(List<ValidationError> validationErrors)
        {
            if (GrooveCount >= GrooveCountMin && GrooveCount <= GrooveCountMax)
            {
                return;
            }

            validationErrors.Add(new ValidationError(
                "Rod.GrooveCount",
                $"Количество прорезей должно быть в диапазоне {GrooveCountMin}–{GrooveCountMax}."));
        }

        /// <summary>
        /// Проверяет глубину прорезей.
        /// </summary>
        private void ValidateGrooveDepth(List<ValidationError> validationErrors)
        {
            if (GrooveDepth >= GrooveDepthMin && GrooveDepth <= GrooveDepthMax)
            {
                return;
            }

            validationErrors.Add(new ValidationError(
                "Rod.GrooveDepth",
                $"Глубина прорези должна быть в диапазоне {GrooveDepthMin:F1}–{GrooveDepthMax:F1} мм."));
        }

        /// <summary>
        /// Проверяет, что после прорези рукоять остаётся толще посадочной части.
        /// </summary>
        private void ValidateGrooveDepthAgainstRod(List<ValidationError> validationErrors)
        {
            if (GrooveCount <= 0)
            {
                return;
            }

            var grooveBottomDiameter = HandleDiameter - (2.0 * GrooveDepth);
            if (grooveBottomDiameter > SeatDiameter)
            {
                return;
            }

            validationErrors.Add(new ValidationError(
                "Rod.GrooveDepth",
                "Глубина прорези слишком большая: после выполнения прорезей " +
                "диаметр рукояти в зоне прорези должен оставаться больше диаметра " +
                $"посадочной части d₂ = {SeatDiameter:F1} мм."));
        }

        /// <summary>
        /// Проверяет, что все прорези помещаются на длине рукояти.
        /// </summary>
        private void ValidateGroovesFitHandleLength(List<ValidationError> validationErrors)
        {
            if (GrooveCount <= 0)
            {
                return;
            }

            var requiredLength =
                (2.0 * GrooveEdgeIndent) +
                (GrooveCount * GrooveWidth) +
                (Math.Max(0, GrooveCount - 1) * GrooveMinGap);

            if (HandleLength >= requiredLength)
            {
                return;
            }

            validationErrors.Add(new ValidationError(
                "Rod.GrooveCount",
                $"Выбранное количество прорезей не помещается на рукояти. " +
                $"Требуемая минимальная длина рукояти: {requiredLength:F1} мм."));

            validationErrors.Add(new ValidationError(
                "Rod.HandleLength",
                $"Длины рукояти l₁ = {HandleLength:F1} мм недостаточно " +
                $"для размещения {GrooveCount} прорезей."));
        }
    }
}