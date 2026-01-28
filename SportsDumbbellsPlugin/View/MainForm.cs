using SportsDumbbellsPlugin.Wrapper;

using SportsDumbbellsPluginCore.Model;

namespace SportsDumbbellsPlugin.View
{
    /// <summary>
    /// Главная форма приложения.
    /// Содержит UI для ввода параметров гантели, отображения ошибок валидации
    /// и запуска построения 3D-модели в KOMPAS-3D.
    /// </summary>
    public partial class MainForm : Form
    {
        //todo: duplication

        /// <summary>
        /// Префикс источника ошибок, относящихся к грифу.
        /// </summary>
        private const string RodErrorSourcePrefix = "Rod.";

        //todo: duplication

        /// <summary>
        /// Префикс источника ошибок, относящихся к дискам.
        /// </summary>
        private const string DisksErrorSourcePrefix = "Disks[";

        /// <summary>
        /// Обёртка над KOMPAS API.
        /// Создаётся лениво при первом построении модели.
        /// </summary>
        private Wrapper.Wrapper? _wrapper;

        /// <summary>
        /// Признак наличия ошибок валидации на вкладке параметров грифа.
        /// </summary>
        private bool _rodTabHasErrors;

        /// <summary>
        /// Признак наличия ошибок валидации на вкладке параметров дисков.
        /// </summary>
        private bool _disksTabHasErrors;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainForm"/>.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            rodParametersControl.ParametersChanged += OnAnyParametersChanged;
            disksControl.ParametersChanged += OnAnyParametersChanged;
        }

        /// <summary>
        /// Обработчик изменения любых параметров в дочерних контролах.
        /// Выполняет валидацию и обновляет UI.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void OnAnyParametersChanged(object? sender, EventArgs e)
        {
            ValidateAllParameters();
        }

        /// <summary>
        /// Выполняет валидацию всех параметров и применяет ошибки к UI.
        /// Также обновляет состояние кнопки построения и подсветку вкладок.
        /// </summary>
        private void ValidateAllParameters()
        {
            var dumbbellParameters = BuildModelFromControls();
            var validationErrors = dumbbellParameters.Validate();

            _rodTabHasErrors = validationErrors.Any(error =>
                error.Source.StartsWith(RodErrorSourcePrefix, StringComparison.Ordinal));

            _disksTabHasErrors = validationErrors.Any(error =>
                error.Source.StartsWith(DisksErrorSourcePrefix, StringComparison.Ordinal));

            rodParametersControl.ApplyErrors(validationErrors);
            disksControl.ApplyErrors(validationErrors);

            buttonDesign.Enabled = !validationErrors.Any();

            tabControl.Invalidate();
        }

        /// <summary>
        /// Собирает модель гантели на основе текущих значений UI-контролов.
        /// </summary>
        /// <returns>Экземпляр <see cref="DumbbellParameters"/>.</returns>
        private DumbbellParameters BuildModelFromControls()
        {
            var dumbbellParameters = new DumbbellParameters
            {
                Rod = rodParametersControl.GetModel(),
                DisksPerSide = disksControl.DisksPerSide,
            };

            dumbbellParameters.Disks.AddRange(disksControl.GetDiskModels());

            return dumbbellParameters;
        }

        /// <summary>
        /// Обработчик отрисовки вкладок.
        /// Подсвечивает вкладки розовым фоном при наличии ошибок в
        /// соответствующей группе параметров.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы отрисовки.</param>
        private void DrawTabValidation(object sender, DrawItemEventArgs e)
        {
            var currentTabControl = (TabControl)sender;
            var tabPage = currentTabControl.TabPages[e.Index];

            var isRodTab = tabPage == tabPageRod;
            var isDisksTab = tabPage == tabPageDisks;

            var hasError =
                (isRodTab && _rodTabHasErrors) ||
                (isDisksTab && _disksTabHasErrors);

            var backColor = GetTabBackColor(hasError, e.State);

            using var backgroundBrush = new SolidBrush(backColor);
            e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

            TextRenderer.DrawText(
                e.Graphics,
                tabPage.Text,
                e.Font,
                e.Bounds,
                SystemColors.ControlText,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        /// <summary>
        /// Возвращает цвет фона вкладки в зависимости от состояния выбора и наличия ошибок.
        /// </summary>
        /// <param name="hasError">Признак наличия ошибок, связанных с вкладкой.</param>
        /// <param name="state">Состояние отрисовки вкладки.</param>
        /// <returns>Цвет фона вкладки.</returns>
        private static Color GetTabBackColor(bool hasError, DrawItemState state)
        {
            if (hasError)
            {
                return Color.MistyRose;
            }

            if (state.HasFlag(DrawItemState.Selected))
            {
                return SystemColors.ControlLightLight;
            }

            return SystemColors.Control;
        }

        /// <summary>
        /// Открывает окно со справочной информацией по грифу.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ShowRodInformation(object sender, EventArgs e)
        {
            var informationForm = RodInformationForm.GetInstance();
            informationForm.Show();
            informationForm.Activate();
        }

        /// <summary>
        /// Открывает окно со справочной информацией по дискам.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ShowDisksInformation(object sender, EventArgs e)
        {
            var informationForm = DiskInformationForm.GetInstance();
            informationForm.Show();
            informationForm.Activate();
        }

        /// <summary>
        /// Устанавливает значения по умолчанию для параметров грифа.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SetRodDefault(object sender, EventArgs e)
        {
            rodParametersControl.SetDefault();
        }

        /// <summary>
        /// Устанавливает значения по умолчанию для параметров дисков.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void SetDisksDefault(object sender, EventArgs e)
        {
            disksControl.SetDefault();
        }

        /// <summary>
        /// Запускает построение 3D-модели гантели в KOMPAS-3D.
        /// Перед построением выполняет валидацию и, при наличии ошибок, не продолжает построение.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void ButtonDesignClick(object sender, EventArgs e)
        {
            var dumbbellParameters = BuildModelFromControls();
            var validationErrors = dumbbellParameters.Validate();

            if (validationErrors.Count > 0)
            {
                return;
            }

            _wrapper ??= new Wrapper.Wrapper();

            var builder = new Builder(_wrapper);
            builder.Build(dumbbellParameters);
        }
    }
}
