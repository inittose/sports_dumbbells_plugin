using SportsDumbbellsPluginCore.Model;
using System.Globalization;

namespace SportsDumbbellsPlugin.View.Controls
{
    /// <summary>
    /// Пользовательский элемент управления для ввода и отображения параметров грифа гантели.
    /// Выполняет отображение суммарной длины и применяет ошибки валидации на UI.
    /// </summary>
    public partial class RodParametersControl : UserControl
    {
        /// <summary>
        /// Префикс источника ошибок, относящихся к грифу.
        /// </summary>
        private const string RodErrorSourcePrefix = "Rod.";

        /// <summary>
        /// Событие, возникающее при изменении любых параметров грифа.
        /// Используется для повторной валидации на уровне формы/родительского контролла.
        /// </summary>
        public event EventHandler? ParametersChanged;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RodParametersControl"/>.
        /// </summary>
        public RodParametersControl()
        {
            InitializeComponent();

            textBoxCenterLength.TextChanged += OnAnyParameterChanged;
            textBoxSeatLength.TextChanged += OnAnyParameterChanged;
            textBoxHandleDiameter.TextChanged += OnAnyParameterChanged;
            textBoxSeatDiameter.TextChanged += OnAnyParameterChanged;

            SetDefault();
        }

        /// <summary>
        /// Формирует модель <see cref="RodParameters"/> на основе значений в полях ввода.
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
            //todo: duplication

            textBoxCenterLength.Text =
                model.HandleLength.ToString("F1", CultureInfo.InvariantCulture);

            textBoxSeatLength.Text =
                model.SeatLength.ToString("F1", CultureInfo.InvariantCulture);

            textBoxHandleDiameter.Text =
                model.HandleDiameter.ToString("F1", CultureInfo.InvariantCulture);

            textBoxSeatDiameter.Text =
                model.SeatDiameter.ToString("F1", CultureInfo.InvariantCulture);

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

            var rodErrors = errors
                .Where(error => error.Source.StartsWith(RodErrorSourcePrefix, StringComparison.Ordinal))
                .ToList();

            if (rodErrors.Count == 0)
            {
                return;
            }

            var groupedErrors = rodErrors
                .GroupBy(error => error.Source.Substring(RodErrorSourcePrefix.Length));

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
        /// Применяет ошибку к конкретному полю ввода, соответствующему свойству модели.
        /// </summary>
        /// <param name="propertyName">Имя свойства модели.</param>
        /// <param name="message">Сообщение об ошибке.</param>
        private void ApplyError(string propertyName, string message)
        {
            //TODO: switch-case
            if (propertyName == nameof(RodParameters.HandleLength))
            {
                SetError(textBoxCenterLength, message);
                return;
            }

            if (propertyName == nameof(RodParameters.SeatLength))
            {
                SetError(textBoxSeatLength, message);
                return;
            }

            if (propertyName == nameof(RodParameters.HandleDiameter))
            {
                SetError(textBoxHandleDiameter, message);
                return;
            }

            if (propertyName == nameof(RodParameters.SeatDiameter))
            {
                SetError(textBoxSeatDiameter, message);
            }
        }

        /// <summary>
        /// Устанавливает ошибку <see cref="ErrorProvider"/> и подсвечивает контрол.
        /// </summary>
        /// <param name="control">Контрол, к которому применяется ошибка.</param>
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
        /// Пересчитывает суммарную длину и генерирует событие <see cref="ParametersChanged"/>.
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
            //todo: duplication
                totalLength.ToString("F1", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Пытается распарсить число с плавающей точкой в инвариантной культуре.
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
