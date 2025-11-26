using SportsDumbbellsPlugin.Controls;
using SportsDumbbellsPlugin.Model;
using SportsDumbbellsPlugin.Validation;

namespace SportsDumbbellsPlugin
{
    public partial class MainForm : Form
    {
        private readonly DumbbellParametersValidator _validator = new DumbbellParametersValidator();

        public MainForm()
        {
            InitializeComponent();
            tableLayoutDisksPanel.RowStyles.Clear();
            tableLayoutDisksPanel.RowCount = 1;
            tableLayoutDisksPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var diskControl = new DiskParametersControl
            {
                Margin = new Padding(3),
                Dock = DockStyle.Fill,
                DiskNumber = 1
            };

            AddDiskControl(diskControl, 0);
            rodParametersControl.ParametersChanged += AnyParametersChanged;
            numericUpDownDisksPerSide.ValueChanged += AnyParametersChanged;
        }

        private void AnyParametersChanged(object? sender, EventArgs e)
        {
            ValidateAllParameters();
        }

        private void ValidateAllParameters()
        {
            // 1. Собираем модель из UI
            var model = BuildModelFromControls();

            // 2. Валидируем
            var result = _validator.Validate(model);

            // 3. Чистим старые ошибки
            rodParametersControl.ClearErrors();
            foreach (var diskControl in tableLayoutDisksPanel.Controls.OfType<DiskParametersControl>())
                diskControl.ClearErrors();

            // 4. Разбрасываем ошибки по контролам
            foreach (var failure in result.Errors)
            {
                var property = failure.PropertyName; // типа "Rod.CenterLength" или "Disks[2].DiskThickness"

                if (property.StartsWith("Rod."))
                {
                    string propName = property.Substring("Rod.".Length);
                    rodParametersControl.SetError(propName, failure.ErrorMessage);
                }
                else if (property.StartsWith("Disks["))
                {
                    // пример: "Disks[2].DiskHoleDiameter"
                    var match = System.Text.RegularExpressions.Regex.Match(
                        property,
                        @"Disks\[(\d+)\]\.(.+)");

                    if (match.Success)
                    {
                        int index = int.Parse(match.Groups[1].Value);
                        string propName = match.Groups[2].Value;

                        var diskControl = tableLayoutDisksPanel
                            .Controls
                            .OfType<DiskParametersControl>()
                            .ElementAtOrDefault(index);

                        diskControl?.SetError(propName, failure.ErrorMessage);
                    }
                }
                else if (property == nameof(DumbbellParameters.TotalDiskWidthPerSide))
                {
                    // Можно подсветить все толщины дисков,
                    // т.к. ошибка относится к сумме H
                    foreach (var diskControl in tableLayoutDisksPanel.Controls.OfType<DiskParametersControl>())
                        diskControl.SetError(nameof(DiskParameters.DiskThickness), failure.ErrorMessage);
                }
            }

            // 5. Кнопка "Спроектировать" активна только если нет ошибок
            buttonDesign.Enabled = result.IsValid;
        }

        private DumbbellParameters BuildModelFromControls()
        {
            var model = new DumbbellParameters
            {
                Rod = rodParametersControl.GetModel(),
                DisksPerSide = (int)numericUpDownDisksPerSide.Value
            };

            foreach (var diskControl in tableLayoutDisksPanel.Controls.OfType<DiskParametersControl>())
                model.Disks.Add(diskControl.GetModel());

            return model;
        }

        private void AddDiskControl(DiskParametersControl diskControl, int rowIndex)
        {
            diskControl.Margin = new Padding(3);
            diskControl.Dock = DockStyle.Fill;

            // подписка на изменения для живой валидации
            diskControl.ParametersChanged += AnyParametersChanged;

            tableLayoutDisksPanel.Controls.Add(diskControl, 0, rowIndex);
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var desiredCount = (int)numericUpDownDisksPerSide.Value;
            var currentCount = tableLayoutDisksPanel.Controls.Count;

            // добавляем недостающие контролы
            while (currentCount < desiredCount)
            {
                currentCount++;

                var diskControl = new DiskParametersControl
                {
                    DiskNumber = currentCount // номер в заголовке groupBox
                };

                tableLayoutDisksPanel.RowCount++;
                tableLayoutDisksPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var rowIndex = tableLayoutDisksPanel.RowCount - 1;

                AddDiskControl(diskControl, rowIndex);
            }

            // удаляем лишние контролы
            while (currentCount > desiredCount)
            {
                var lastRowIndex = tableLayoutDisksPanel.RowCount - 1;
                if (lastRowIndex < 0)
                    break;

                // получаем контрол по позиции (0, lastRowIndex)
                var ctrl = tableLayoutDisksPanel.GetControlFromPosition(0, lastRowIndex);

                if (ctrl is DiskParametersControl diskControl)
                {
                    diskControl.ParametersChanged -= AnyParametersChanged;
                    tableLayoutDisksPanel.Controls.Remove(diskControl);
                    diskControl.Dispose();
                }

                tableLayoutDisksPanel.RowStyles.RemoveAt(lastRowIndex);
                tableLayoutDisksPanel.RowCount--;

                currentCount--;
            }

            // пересчитать модель и провалидировать всё сразу
            ValidateAllParameters();
        }
    }
}
