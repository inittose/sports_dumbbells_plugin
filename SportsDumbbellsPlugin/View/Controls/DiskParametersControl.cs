using SportsDumbbellsPluginCore.Model;
using System.Globalization;
using SportsDumbbellsPlugin.View.Formatting;

namespace SportsDumbbellsPlugin.View.Controls
{
    /// <summary>
    /// Пользовательский элемент управления для ввода и отображения
    /// параметров одного диска гантели.
    /// </summary>
    public partial class DiskParametersControl : UserControl
    {
        /// <summary>
        /// Номер диска, отображаемый в заголовке группы.
        /// </summary>
        private int _diskNumber;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DiskParametersControl"/>.
        /// </summary>
        public DiskParametersControl()
        {
            InitializeComponent();

            textBoxOuterDiameter.TextChanged += OnAnyTextChanged;
            textBoxHoleDiameter.TextChanged += OnAnyTextChanged;
            textBoxThickness.TextChanged += OnAnyTextChanged;
            textBoxFilletDiameter.TextChanged += OnAnyTextChanged;

            SetDefault();
        }

        /// <summary>
        /// Событие, возникающее при изменении любых входных параметров диска.
        /// Используется для пересчёта/валидации на уровне формы/родительского контролла.
        /// </summary>
        public event EventHandler? ParametersChanged;

        /// <summary>
        /// Номер диска, используемый для отображения в заголовке группы.
        /// </summary>
        public int DiskNumber
        {
            get => _diskNumber;
            set
            {
                _diskNumber = value;
                groupBox.Text = $"Параметры диска {value}";
            }
        }

        /// <summary>
        /// Применяет модель к элементам управления (заполняет текстовые поля).
        /// </summary>
        /// <param name="model">Модель параметров диска.</param>
        public void SetModel(DiskParameters model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            //todo: duplication
            // +
            textBoxHoleDiameter.Text = ParameterValueFormatter.FormatDouble(model.HoleDiameter);
            textBoxOuterDiameter.Text = ParameterValueFormatter.FormatDouble(model.OuterDiameter);
            textBoxThickness.Text = ParameterValueFormatter.FormatDouble(model.Thickness);
            textBoxFilletDiameter.Text = ParameterValueFormatter.FormatDouble(model.FilletDiameter);
        }

        /// <summary>
        /// Сбрасывает значения полей к значениям по умолчанию и очищает ошибки.
        /// </summary>
        public void SetDefault()
        {
            ClearErrors();

            var defaultModel = new DiskParameters();
            SetModel(defaultModel);
        }

        /// <summary>
        /// Формирует модель параметров диска на основе введённых пользователем значений.
        /// Некорректные значения интерпретируются как 0.
        /// </summary>
        /// <returns>Экземпляр <see cref="DiskParameters"/>.</returns>
        public DiskParameters GetModel()
        {
            var outerDiameter = ParseDoubleOrDefault(textBoxOuterDiameter.Text);
            var holeDiameter = ParseDoubleOrDefault(textBoxHoleDiameter.Text);
            var thickness = ParseDoubleOrDefault(textBoxThickness.Text);
            var filletDiameter = ParseDoubleOrDefault(textBoxFilletDiameter.Text);

            return new DiskParameters
            {
                OuterDiameter = outerDiameter,
                HoleDiameter = holeDiameter,
                Thickness = thickness,
                FilletDiameter = filletDiameter,
            };
        }

        /// <summary>
        /// Очищает визуальные признаки ошибок на всех полях.
        /// </summary>
        public void ClearErrors()
        {
            errorProvider.SetError(textBoxOuterDiameter, string.Empty);
            errorProvider.SetError(textBoxHoleDiameter, string.Empty);
            errorProvider.SetError(textBoxThickness, string.Empty);
            errorProvider.SetError(textBoxFilletDiameter, string.Empty);

            textBoxOuterDiameter.BackColor = SystemColors.Window;
            textBoxHoleDiameter.BackColor = SystemColors.Window;
            textBoxThickness.BackColor = SystemColors.Window;
            textBoxFilletDiameter.BackColor = SystemColors.Window;
        }

        /// <summary>
        /// Устанавливает ошибку валидации для указанного свойства модели и
        /// подсвечивает соответствующее поле.
        /// </summary>
        /// <param name="propertyName">Имя свойства модели диска.</param>
        /// <param name="message">Текст ошибки.</param>
        public void SetError(string propertyName, string message)
        {
            var targetTextBox = GetTargetTextBox(propertyName);
            if (targetTextBox == null)
            {
                return;
            }

            errorProvider.SetError(targetTextBox, message);
            targetTextBox.BackColor = Color.MistyRose;
        }

        /// <summary>
        /// Обработчик изменения текста в любом поле ввода.
        /// Генерирует событие <see cref="ParametersChanged"/>.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void OnAnyTextChanged(object? sender, EventArgs e)
        {
            ParametersChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Возвращает текстовое поле, соответствующее имени свойства модели диска.
        /// </summary>
        /// <param name="propertyName">Имя свойства модели.</param>
        /// <returns>Соответствующий <see cref="TextBox"/>,
        /// либо <see langword="null"/> если свойство не поддерживается.</returns>
        private TextBox? GetTargetTextBox(string propertyName)
        {
            return propertyName switch
            {
                nameof(DiskParameters.OuterDiameter)  => textBoxOuterDiameter,
                nameof(DiskParameters.HoleDiameter)   => textBoxHoleDiameter,
                nameof(DiskParameters.Thickness)      => textBoxThickness,
                nameof(DiskParameters.FilletDiameter) => textBoxFilletDiameter,
                _                                     => null
            };
        }

        /// <summary>
        /// Пытается распарсить число с плавающей точкой.
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
