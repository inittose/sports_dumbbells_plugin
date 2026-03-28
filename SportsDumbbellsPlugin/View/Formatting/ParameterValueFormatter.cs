using System.Globalization;

namespace SportsDumbbellsPlugin.View.Formatting
{
    /// <summary>
    /// Выполняет форматирование числовых значений
    /// для отображения в пользовательском интерфейсе.
    /// </summary>
    internal static class ParameterValueFormatter
    {
        /// <summary>
        /// Форматирует число с одной цифрой после запятой
        /// в инвариантной культуре.
        /// </summary>
        /// <param name="value">Форматируемое значение.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string FormatDouble(double value)
        {
            return value.ToString("F1", CultureInfo.InvariantCulture);
        }
    }
}
