using SportsDumbbellsPluginCore.Validation;

namespace SportsDumbbellsPluginCore.Model
{
    /// <summary>
    /// Параметры диска гантели.
    /// Содержит геометрические размеры и выполняет их валидацию
    /// по заданным ограничениям.
    /// </summary>
    public class DiskParameters
    {
        /// <summary>
        /// Минимально допустимый внешний диаметр диска, мм.
        /// </summary>
        private const double OuterDiameterMin = 120.0;

        /// <summary>
        /// Максимально допустимый внешний диаметр диска, мм.
        /// </summary>
        private const double OuterDiameterMax = 260.0;

        /// <summary>
        /// Минимально допустимый диаметр отверстия диска, мм.
        /// </summary>
        private const double HoleDiameterMin = 26.0;

        /// <summary>
        /// Максимально допустимый диаметр отверстия диска, мм.
        /// </summary>
        private const double HoleDiameterMax = 34.0;

        /// <summary>
        /// Минимально допустимая толщина диска, мм.
        /// </summary>
        private const double ThicknessMin = 10.0;

        /// <summary>
        /// Максимально допустимая толщина диска, мм.
        /// </summary>
        private const double ThicknessMax = 40.0;

        /// <summary>
        /// Минимально допустимый диаметр скругления, мм.
        /// </summary>
        private const double FilletDiameterMin = 0.0;

        /// <summary>
        /// Возвращает и задает диаметр отверстия диска (d), мм.
        /// </summary>
        public double HoleDiameter { get; set; } = 27.0;

        /// <summary>
        /// Возвращает и задает внешний диаметр диска (D), мм.
        /// </summary>
        public double OuterDiameter { get; set; } = 150.0;

        /// <summary>
        /// Возвращает и задает толщину диска (t), мм.
        /// </summary>
        public double Thickness { get; set; } = 20.0;

        /// <summary>
        /// Возвращает и задает диаметр скругления диска, мм.
        /// Значение 0 означает отсутствие скругления.
        /// </summary>
        public double FilletDiameter { get; set; } = 4.0;

        /// <summary>
        /// Выполняет валидацию параметров диска и возвращает список ошибок.
        /// </summary>
        /// <returns>Список ошибок валидации.
        /// Если ошибок нет, возвращается пустой список.</returns>
        public IReadOnlyList<ValidationError> Validate()
        {
            var errors = new List<ValidationError>();

            if (OuterDiameter < OuterDiameterMin
                || OuterDiameter > OuterDiameterMax)
            {
                errors.Add(new ValidationError(
                    "Disk.OuterDiameter",
                    $"Внешний диаметр диска D должен быть в диапазоне " +
                    $"{OuterDiameterMin}–{OuterDiameterMax} мм."));
            }

            if (HoleDiameter < HoleDiameterMin || HoleDiameter > HoleDiameterMax)
            {
                errors.Add(new ValidationError(
                    "Disk.HoleDiameter",
                    $"Диаметр отверстия диска d должен быть в диапазоне " +
                    $"{HoleDiameterMin}–{HoleDiameterMax} мм."));
            }

            if (Thickness < ThicknessMin || Thickness > ThicknessMax)
            {
                errors.Add(new ValidationError(
                    "Disk.Thickness",
                    $"Толщина диска t должна быть в диапазоне " +
                    $"{ThicknessMin}–{ThicknessMax} мм."));
            }

            ValidateFilletDiameter(errors);

            return errors;
        }

        /// <summary>
        /// Проверяет допустимость диаметра скругления.
        /// </summary>
        private void ValidateFilletDiameter(List<ValidationError> errors)
        {
            if (FilletDiameter < FilletDiameterMin)
            {
                errors.Add(new ValidationError(
                    "Disk.FilletDiameter",
                    "Диаметр скругления не может быть отрицательным."));
                return;
            }

            var maxFilletDiameter = Math.Max(
                0.0,
                Math.Min(Thickness, OuterDiameter - HoleDiameter));

            if (FilletDiameter <= maxFilletDiameter)
            {
                return;
            }

            errors.Add(new ValidationError(
                "Disk.FilletDiameter",
                "Диаметр скругления диска должен быть "
                + "не больше минимального из двух " +
                $"значений: толщины диска t = {Thickness:F1} мм и "
                + "разности D - d = " +
                $"{(OuterDiameter - HoleDiameter):F1} мм."));
        }
    }
}