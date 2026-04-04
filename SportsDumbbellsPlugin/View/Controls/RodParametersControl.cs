using SportsDumbbellsPluginCore.Model;
using System.Globalization;
using SportsDumbbellsPlugin.View.Formatting;
using SportsDumbbellsPluginCore.Validation;

namespace SportsDumbbellsPlugin.View.Controls
{
    /// <summary>
    /// Пользовательский элемент управления для ввода и отображения
    /// параметров грифа гантели.
    /// Выполняет отображение суммарной длины и
    /// применяет ошибки валидации на UI.
    /// </summary>
    public partial class RodParametersControl : UserControl
    {
        /// <summary>
        /// Событие, возникающее при изменении любых параметров грифа.
        /// Используется для повторной валидации на уровне
        /// формы/родительского контролла.
        /// </summary>
        public event EventHandler? ParametersChanged;

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// <see cref="RodParametersControl"/>.
        /// </summary>
        public RodParametersControl()
        {
            InitializeComponent();

            textBoxCenterLength.TextChanged += OnAnyParameterChanged;
            textBoxSeatLength.TextChanged += OnAnyParameterChanged;
            textBoxHandleDiameter.TextChanged += OnAnyParameterChanged;
            textBoxSeatDiameter.TextChanged += OnAnyParameterChanged;
            textBoxGrooveDepth.TextChanged += OnAnyParameterChanged;
            numericUpDownGrooveCount.ValueChanged += OnAnyParameterChanged;

            SetDefault();
        }

        /// <summary>
        /// Формирует модель <see cref="RodParameters"/> на основе значений в
        /// полях ввода.
        /// Некорректные значения интерпретируются как 0.
        /// </summary>
        /// <returns>Экземпляр <see cref="RodParameters"/>.</returns>
        public RodParameters GetModel()
        {
            var rodParameters = new RodParameters
            {
                HandleLength = ParseDoubleOrDefault(textBoxCenterLength.Text),
                SeatLength = ParseDoubleOrDefault(textBoxSeatLength.Text),
                HandleDiameter = ParseDoubleOrDefault(textBoxHandleDiameter.Text),
                SeatDiameter = ParseDoubleOrDefault(textBoxSeatDiameter.Text),
                GrooveCount = (int)numericUpDownGrooveCount.Value,
                GrooveDepth = ParseDoubleOrDefault(textBoxGrooveDepth.Text),
            };

            return rodParameters;
        }

        /// <summary>
        /// Применяет модель к элементам управления (заполняет поля ввода).
        /// </summary>
        /// <param name="model">Модель параметров грифа.</param>
        public void SetModel(RodParameters model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            textBoxCenterLength.Text =
                ParameterValueFormatter.FormatDouble(model.HandleLength);

            textBoxSeatLength.Text =
                ParameterValueFormatter.FormatDouble(model.SeatLength);

            textBoxHandleDiameter.Text =
                ParameterValueFormatter.FormatDouble(model.HandleDiameter);

            textBoxSeatDiameter.Text =
                ParameterValueFormatter.FormatDouble(model.SeatDiameter);

            textBoxGrooveDepth.Text =
                ParameterValueFormatter.FormatDouble(model.GrooveDepth);

            numericUpDownGrooveCount.Value = Math.Max(
                numericUpDownGrooveCount.Minimum,
                Math.Min(numericUpDownGrooveCount.Maximum, model.GrooveCount));

            UpdateTotalLength(model.TotalLength);
        }

        /// <summary>
        /// Устанавливает значения по умолчанию.
        /// </summary>
        public void SetDefault()
        {
            var defaultRodParameters = new RodParameters();
            SetModel(defaultRodParameters);
        }

        /// <summary>
        /// Очищает все ошибки валидации и возвращает стандартный фон полей ввода.
        /// </summary>
        public void ClearErrors()
        {
            errorProvider.Clear();

            ResetBackColor(textBoxCenterLength);
            ResetBackColor(textBoxSeatLength);
            ResetBackColor(textBoxHandleDiameter);
            ResetBackColor(textBoxSeatDiameter);
            ResetBackColor(textBoxGrooveDepth);
            ResetBackColor(numericUpDownGrooveCount);
        }

        /// <summary>
        /// Применяет список ошибок валидации к контролу грифа (UI-валидация).
        /// </summary>
        /// <param name="errors">Список ошибок валидации.</param>
        public void ApplyErrors(IReadOnlyList<ValidationError> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            ClearErrors();
            var rodErrors = errors.Where(
                error => error.Source.StartsWith(
                    ValidationSources.RodPrefix,
                    StringComparison.Ordinal)).ToList();

            if (rodErrors.Count == 0)
            {
                return;
            }

            var groupedErrors = rodErrors.GroupBy(
                error => error.Source.Substring(ValidationSources.RodPrefix.Length));

            foreach (var group in groupedErrors)
            {
                var propertyName = group.Key;

                var message = string.Join(
                    Environment.NewLine,
                    group.Select(item => item.Message).Distinct());

                ApplyError(propertyName, message);
            }
        }

        /// <summary>
        /// Возвращает контрол, соответствующий имени свойства модели грифа.
        /// </summary>
        /// <param name="propertyName">Имя свойства модели.</param>
        /// <returns>
        /// Соответствующий контрол или <see langword="null"/>.
        /// </returns>
        private Control? GetControlByPropertyName(string propertyName)
        {
            return propertyName switch
            {
                nameof(RodParameters.HandleLength)   => textBoxCenterLength,
                nameof(RodParameters.SeatLength)     => textBoxSeatLength,
                nameof(RodParameters.HandleDiameter) => textBoxHandleDiameter,
                nameof(RodParameters.SeatDiameter)   => textBoxSeatDiameter,
                nameof(RodParameters.GrooveCount)    => numericUpDownGrooveCount,
                nameof(RodParameters.GrooveDepth)   => textBoxGrooveDepth,
                _                                    => null,
            };
        }

        /// <summary>
        /// Применяет ошибку к конкретному полю ввода, соответствующему свойству модели.
        /// </summary>
        /// <param name="propertyName">Имя свойства модели.</param>
        /// <param name="message">Сообщение об ошибке.</param>
        private void ApplyError(string propertyName, string message)
        {
            var control = GetControlByPropertyName(propertyName);
            if (control != null)
            {
                SetError(control, message);
            }
        }

        /// <summary>
        /// Устанавливает ошибку <see cref="ErrorProvider"/> и
        /// подсвечивает контрол.
        /// </summary>
        /// <param name="control">
        /// Контрол, к которому применяется ошибка.
        /// </param>
        /// <param name="message">Сообщение об ошибке.</param>
        private void SetError(Control control, string message)
        {
            errorProvider.SetError(control, message);
            control.BackColor = Color.MistyRose;
        }

        /// <summary>
        /// Сбрасывает цвет фона контрола к стандартному цвету окна.
        /// </summary>
        /// <param name="control">Контрол для сброса цвета фона.</param>
        private void ResetBackColor(Control control)
        {
            control.BackColor = SystemColors.Window;
        }

        /// <summary>
        /// Обработчик изменения любого параметра.
        /// Пересчитывает суммарную длину и генерирует событие
        /// <see cref="ParametersChanged"/>.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void OnAnyParameterChanged(object? sender, EventArgs e)
        {
            RecalculateTotalLength();
            ParametersChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Пересчитывает суммарную длину L = l₁ + 2·l₂ и отображает её.
        /// При невозможности парсинга очищает поле суммарной длины.
        /// </summary>
        private void RecalculateTotalLength()
        {
            var handleLengthParsed = double.TryParse(
                textBoxCenterLength.Text,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var handleLength);

            var seatLengthParsed = double.TryParse(
                textBoxSeatLength.Text,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var seatLength);

            if (!handleLengthParsed || !seatLengthParsed)
            {
                textBoxTotalLength.Text = string.Empty;
                return;
            }

            var totalLength = handleLength + (2.0 * seatLength);
            UpdateTotalLength(totalLength);
        }

        /// <summary>
        /// Обновляет отображение суммарной длины.
        /// </summary>
        /// <param name="totalLength">Суммарная длина.</param>
        private void UpdateTotalLength(double totalLength)
        {
            textBoxTotalLength.Text =
                ParameterValueFormatter.FormatDouble(totalLength);
        }

        /// <summary>
        /// Пытается распарсить число с плавающей точкой в
        /// инвариантной культуре.
        /// При ошибке возвращает 0.
        /// </summary>
        /// <param name="value">Строковое представление числа.</param>
        /// <returns>Результат парсинга или 0.</returns>
        private static double ParseDoubleOrDefault(string value)
        {
            var parsed = double.TryParse(
                value,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var number);

            return parsed ? number : 0.0;
        }
    }
}
