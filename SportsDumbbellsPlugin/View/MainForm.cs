using SportsDumbbellsPlugin.Model;
using SportsDumbbellsPlugin.Validation;
using SportsDumbbellsPlugin.View.Controls;

namespace SportsDumbbellsPlugin.View
{
    public partial class MainForm : Form
    {
        private bool _rodTabHasErrors = false;
        private bool _disksTabHasErrors = false;

        private readonly DumbbellParametersValidator _validator = new DumbbellParametersValidator();

        public MainForm()
        {
            InitializeComponent();

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
            var model = BuildModelFromControls();
            var result = _validator.Validate(model);

            _rodTabHasErrors = false;
            _disksTabHasErrors = false;

            // 1. Сначала чистим старые ошибки
            rodParametersControl.ClearErrors();
            foreach (var diskControl in tableLayoutDisksPanel.Controls.OfType<DiskParametersControl>())
                diskControl.ClearErrors();

            // 2. Группируем ошибки по имени свойства
            var errorsByProperty = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToList();

            foreach (var group in errorsByProperty)
            {
                var property = group.Key;

                var message = string.Join(
                    Environment.NewLine,
                    group.Select(g => g.ErrorMessage).Distinct());

                if (property.StartsWith("Rod."))
                {
                    _rodTabHasErrors = true;
                    var propName = property.Substring("Rod.".Length);
                    rodParametersControl.SetError(propName, message);
                }
                else if (property.StartsWith("Disks["))
                {
                    _disksTabHasErrors = true;
                    var match = System.Text.RegularExpressions.Regex.Match(
                        property,
                        @"Disks\[(\d+)\]\.(.+)");

                    if (match.Success)
                    {
                        var index = int.Parse(match.Groups[1].Value);
                        var propName = match.Groups[2].Value;

                        var diskControl = tableLayoutDisksPanel
                            .Controls
                            .OfType<DiskParametersControl>()
                            .ElementAtOrDefault(index);

                        diskControl?.SetError(propName, message);
                    }
                }
                else if (property == nameof(DumbbellParameters.TotalDiskWidthPerSide))
                {
                    foreach (var diskControl in tableLayoutDisksPanel.Controls.OfType<DiskParametersControl>())
                    {
                        diskControl.SetError(nameof(DiskParameters.DiskThickness), message);
                    }
                }
            }

            buttonDesign.Enabled = result.IsValid;
            tabControl.Invalidate();
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

            diskControl.ParametersChanged += AnyParametersChanged;

            tableLayoutDisksPanel.Controls.Add(diskControl, 0, rowIndex);
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var desiredCount = (int)numericUpDownDisksPerSide.Value;
            var currentCount = tableLayoutDisksPanel.Controls.Count;

            while (currentCount < desiredCount)
            {
                currentCount++;

                var diskControl = new DiskParametersControl
                {
                    DiskNumber = currentCount
                };

                tableLayoutDisksPanel.RowCount++;
                tableLayoutDisksPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var rowIndex = tableLayoutDisksPanel.RowCount - 1;

                AddDiskControl(diskControl, rowIndex);
            }

            while (currentCount > desiredCount)
            {
                var lastRowIndex = tableLayoutDisksPanel.RowCount - 1;
                if (lastRowIndex < 0)
                    break;

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

            ValidateAllParameters();
        }

        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabControl = (TabControl)sender;
            var page = tabControl.TabPages[e.Index];

            // Определяем, какая вкладка
            var isRodTab = page == tabPageRod;   // вкладка "Стержень"
            var isDisksTab = page == tabPageDisks; // вкладка "Диски"

            var hasError =
                (isRodTab && _rodTabHasErrors) ||
                (isDisksTab && _disksTabHasErrors);

            // цвет фона заголовка
            Color backColor;
            if (hasError)
                backColor = Color.MistyRose;                 // ошибка
            else if (e.State.HasFlag(DrawItemState.Selected))
                backColor = SystemColors.ControlLightLight;  // выбранная вкладка без ошибок
            else
                backColor = SystemColors.Control;            // обычная вкладка

            using (var b = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }

            // Рисуем текст
            TextRenderer.DrawText(
                e.Graphics,
                page.Text,
                e.Font,
                e.Bounds,
                SystemColors.ControlText,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var informationForm = RodInformationForm.GetInstance();
            informationForm.Show();
            informationForm.Activate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var informationForm = DiskInformationForm.GetInstance();
            informationForm.Show();
            informationForm.Activate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rodParametersControl.SetDefault();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            numericUpDownDisksPerSide.Value = 1;
            (tableLayoutDisksPanel.Controls[0] as DiskParametersControl)?.SetDefault();
        }
    }
}
