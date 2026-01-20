using SportsDumbbellsPluginCore.Model;
using System.Text.RegularExpressions;

namespace SportsDumbbellsPlugin.View.Controls
{
    /// <summary>
    /// Пользовательский элемент управления для задания количества дисков на сторону
    /// и ввода параметров каждого диска.
    /// </summary>
    public partial class DisksControl : UserControl
    {
        /// <summary>
        /// Регулярное выражение для разбора источника ошибки вида:
        /// "Disks[0].Thickness".
        /// </summary>
        private static readonly Regex DiskErrorSourceRegex =
            new Regex(@"^Disks\[(\d+)\]\.(.+)$", RegexOptions.Compiled);

        /// <summary>
        /// Событие, возникающее при изменении параметров дисков или их количества.
        /// </summary>
        public event EventHandler? ParametersChanged;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DisksControl"/>.
        /// </summary>
        public DisksControl()
        {
            InitializeComponent();

            numericUpDownDisksPerSide.ValueChanged += OnDisksCountChanged;

            EnsureDisksCount((int)numericUpDownDisksPerSide.Value);
        }

        /// <summary>
        /// Количество дисков на одной стороне.
        /// Значение синхронизируется с элементом <see cref="NumericUpDown"/>.
        /// </summary>
        public int DisksPerSide
        {
            get => (int)numericUpDownDisksPerSide.Value;
            set => numericUpDownDisksPerSide.Value = value;
        }

        /// <summary>
        /// Возвращает модели параметров дисков из дочерних контролов.
        /// </summary>
        /// <returns>Список моделей дисков.</returns>
        public IReadOnlyList<DiskParameters> GetDiskModels()
        {
            var diskModels = tableLayoutDisksPanel.Controls
                .OfType<DiskParametersControl>()
                .Select(control => control.GetModel())
                .ToList();

            return diskModels;
        }

        /// <summary>
        /// Устанавливает значения по умолчанию.
        /// </summary>
        public void SetDefault()
        {
            DisksPerSide = 1;

            var firstDiskControl = tableLayoutDisksPanel.Controls
                .OfType<DiskParametersControl>()
                .FirstOrDefault();

            firstDiskControl?.SetDefault();

            ClearErrors();
        }

        /// <summary>
        /// Очищает визуальные ошибки на всех контролах дисков.
        /// </summary>
        public void ClearErrors()
        {
            foreach (var diskControl in tableLayoutDisksPanel.Controls.OfType<DiskParametersControl>())
            {
                diskControl.ClearErrors();
            }
        }

        /// <summary>
        /// Применяет список ошибок валидации к контролам дисков.
        /// </summary>
        /// <param name="errors">Список ошибок валидации.</param>
        public void ApplyErrors(IReadOnlyList<ValidationError> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            ClearErrors();

            var diskErrors = errors
                .Where(error => error.Source.StartsWith("Disks[", StringComparison.Ordinal))
                .ToList();

            if (diskErrors.Count == 0)
            {
                return;
            }

            var groupedDiskErrors = GroupDiskErrors(diskErrors);

            foreach (var group in groupedDiskErrors)
            {
                var diskIndex = group.Key.DiskIndex;
                var propertyName = group.Key.PropertyName;

                var combinedMessage = string.Join(
                    Environment.NewLine,
                    group.Select(item => item.Message).Distinct());

                var diskControl = tableLayoutDisksPanel.Controls
                    .OfType<DiskParametersControl>()
                    .ElementAtOrDefault(diskIndex);

                diskControl?.SetError(propertyName, combinedMessage);
            }
        }

        /// <summary>
        /// Обработчик изменения количества дисков на сторону.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void OnDisksCountChanged(object? sender, EventArgs e)
        {
            EnsureDisksCount((int)numericUpDownDisksPerSide.Value);
            ParametersChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Обеспечивает наличие нужного количества контролов дисков в таблице.
        /// </summary>
        /// <param name="desiredCount">Требуемое количество контролов.</param>
        private void EnsureDisksCount(int desiredCount)
        {
            var currentCount = tableLayoutDisksPanel.Controls
                .OfType<DiskParametersControl>()
                .Count();

            while (currentCount < desiredCount)
            {
                var diskControl = CreateDiskControl(currentCount);
                AddDiskControl(diskControl, currentCount);
                currentCount++;
            }

            while (currentCount > desiredCount)
            {
                RemoveLastDiskControl();
                currentCount--;
            }
        }

        /// <summary>
        /// Создаёт контрол параметров диска и подписывает его на изменения.
        /// </summary>
        /// <param name="index">Индекс диска (0-based).</param>
        /// <returns>Созданный контрол параметров диска.</returns>
        private DiskParametersControl CreateDiskControl(int index)
        {
            var diskControl = new DiskParametersControl
            {
                Dock = DockStyle.Top,
                Margin = new Padding(3),
                DiskNumber = index + 1,
            };

            diskControl.ParametersChanged += OnDiskParametersChanged;

            return diskControl;
        }

        /// <summary>
        /// Добавляет контрол диска в таблицу на указанную строку.
        /// </summary>
        /// <param name="diskControl">Контрол диска.</param>
        /// <param name="rowIndex">Индекс строки.</param>
        private void AddDiskControl(DiskParametersControl diskControl, int rowIndex)
        {
            if (rowIndex >= tableLayoutDisksPanel.RowCount)
            {
                tableLayoutDisksPanel.RowCount = rowIndex + 1;
                tableLayoutDisksPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            tableLayoutDisksPanel.Controls.Add(diskControl, 0, rowIndex);
        }

        /// <summary>
        /// Удаляет последний контрол диска из таблицы.
        /// </summary>
        private void RemoveLastDiskControl()
        {
            if (tableLayoutDisksPanel.RowCount == 0)
            {
                return;
            }

            var lastRowIndex = tableLayoutDisksPanel.RowCount - 1;

            if (
                tableLayoutDisksPanel.GetControlFromPosition(0, lastRowIndex) is
                DiskParametersControl diskControl)
            {
                diskControl.ParametersChanged -= OnDiskParametersChanged;

                tableLayoutDisksPanel.Controls.Remove(diskControl);
                diskControl.Dispose();
            }

            tableLayoutDisksPanel.RowStyles.RemoveAt(lastRowIndex);
            tableLayoutDisksPanel.RowCount--;
        }

        /// <summary>
        /// Обработчик изменения параметров отдельного диска.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void OnDiskParametersChanged(object? sender, EventArgs e)
        {
            ParametersChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Группирует ошибки дисков по индексу диска и имени свойства.
        /// </summary>
        /// <param name="diskErrors">Ошибки, относящиеся к дискам.</param>
        /// <returns>Группировка ошибок.</returns>
        private static IEnumerable<IGrouping<(int DiskIndex, string PropertyName), ValidationError>> GroupDiskErrors(
            IReadOnlyList<ValidationError> diskErrors)
        {
            var groupedDiskErrors = diskErrors
                .Select(TryParseDiskError)
                .Where(parsed => parsed.HasValue)
                .Select(parsed => parsed!.Value)
                .GroupBy(parsed => (parsed.DiskIndex, parsed.PropertyName), parsed => parsed.Error);

            return groupedDiskErrors;
        }

        /// <summary>
        /// Пытается разобрать ошибку диска из источника ошибки.
        /// </summary>
        /// <param name="error">Ошибка валидации.</param>
        /// <returns>Разобранная структура или <see langword="null"/>,
        /// если источник не соответствует ожидаемому формату.</returns>
        private static (int DiskIndex, string PropertyName, ValidationError Error)? TryParseDiskError(ValidationError error)
        {
            var match = DiskErrorSourceRegex.Match(error.Source);
            if (!match.Success)
            {
                return null;
            }

            var diskIndex = int.Parse(match.Groups[1].Value);
            var propertyName = match.Groups[2].Value;

            return (diskIndex, propertyName, error);
        }
    }
}
