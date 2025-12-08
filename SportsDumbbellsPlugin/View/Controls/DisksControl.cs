using SportsDumbbellsPlugin.Model;
using System.Text.RegularExpressions;

namespace SportsDumbbellsPlugin.View.Controls
{
    public partial class DisksControl : UserControl
    {
        public event EventHandler? ParametersChanged;

        public DisksControl()
        {
            InitializeComponent();

            numericUpDownDisksPerSide.ValueChanged += OnDisksCountChanged;
            EnsureDisksCount((int)numericUpDownDisksPerSide.Value);
        }

        public int DisksPerSide
        {
            get => (int)numericUpDownDisksPerSide.Value;
            set => numericUpDownDisksPerSide.Value = value;
        }

        public IReadOnlyList<DiskParameters> GetDiskModels()
        {
            return tableLayoutDisksPanel.Controls
                .OfType<DiskParametersControl>()
                .Select(c => c.GetModel())
                .ToList();
        }

        public void SetDefault()
        {
            DisksPerSide = 1;

            var first = tableLayoutDisksPanel.Controls
                .OfType<DiskParametersControl>()
                .FirstOrDefault();

                first?.SetDefault();

            ClearErrors();
        }

        public void ClearErrors()
        {
            foreach (var ctrl in tableLayoutDisksPanel.Controls.OfType<DiskParametersControl>())
                ctrl.ClearErrors();
        }

        /// <summary>
        /// Применить список ошибок ко всем дискам.
        /// </summary>
        public void ApplyErrors(IReadOnlyList<ValidationError> errors)
        {
            ClearErrors();

            var diskErrors = errors
                .Where(e => e.Source.StartsWith("Disks[", StringComparison.Ordinal))
                .ToList();

            if (diskErrors.Count == 0)
                return;

            // Группируем по индексу и имени свойства
            var grouped = diskErrors
                .Select(e =>
                {
                    var match = Regex.Match(e.Source, @"Disks\[(\d+)\]\.(.+)");
                    if (!match.Success)
                        return null;

                    return new
                    {
                        Index = int.Parse(match.Groups[1].Value),
                        PropertyName = match.Groups[2].Value,
                        e.Message
                    };
                })
                .Where(x => x != null)!
                .GroupBy(x => new { x.Index, x.PropertyName });

            foreach (var group in grouped)
            {
                int index = group.Key.Index;
                string propertyName = group.Key.PropertyName;

                var message = string.Join(
                    Environment.NewLine,
                    group.Select(g => g.Message).Distinct()
                );

                var ctrl = tableLayoutDisksPanel.Controls
                    .OfType<DiskParametersControl>()
                    .ElementAtOrDefault(index);

                ctrl?.SetError(propertyName, message);
            }
        }

        private void OnDisksCountChanged(object? sender, EventArgs e)
        {
            EnsureDisksCount((int)numericUpDownDisksPerSide.Value);
            ParametersChanged?.Invoke(this, EventArgs.Empty);
        }

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

        private DiskParametersControl CreateDiskControl(int index)
        {
            var diskControl = new DiskParametersControl
            {
                Dock = DockStyle.Top,
                Margin = new Padding(3),
                DiskNumber = index + 1
            };

            diskControl.ParametersChanged += (_, _) =>
            {
                ParametersChanged?.Invoke(this, EventArgs.Empty);
            };

            return diskControl;
        }

        private void AddDiskControl(DiskParametersControl ctrl, int rowIndex)
        {
            // растягиваем таблицу
            if (rowIndex >= tableLayoutDisksPanel.RowCount)
            {
                tableLayoutDisksPanel.RowCount = rowIndex + 1;
                tableLayoutDisksPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            tableLayoutDisksPanel.Controls.Add(ctrl, 0, rowIndex);
        }

        private void RemoveLastDiskControl()
        {
            if (tableLayoutDisksPanel.RowCount == 0)
                return;

            int lastRowIndex = tableLayoutDisksPanel.RowCount - 1;

            var ctrl = tableLayoutDisksPanel.GetControlFromPosition(0, lastRowIndex)
                       as DiskParametersControl;

            if (ctrl != null)
            {
                ctrl.ParametersChanged -= OnDiskParametersChanged;
                tableLayoutDisksPanel.Controls.Remove(ctrl);
                ctrl.Dispose();
            }

            tableLayoutDisksPanel.RowStyles.RemoveAt(lastRowIndex);
            tableLayoutDisksPanel.RowCount--;
        }

        private void OnDiskParametersChanged(object? sender, EventArgs e)
        {
            ParametersChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
