using SportsDumbbellsPlugin.Controls;

namespace SportsDumbbellsPlugin
{
    public partial class MainForm : Form
    {
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

            tableLayoutDisksPanel.Controls.Add(diskControl, 0, 0);
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var desiredCount = (int)numericUpDown.Value;
            var currentCount = tableLayoutDisksPanel.Controls.Count;

            while (currentCount < desiredCount)
            {
                currentCount++;

                var diskControl = new DiskParametersControl
                {
                    Margin = new Padding(3),
                    Dock = DockStyle.Fill,
                    DiskNumber = currentCount
                };

                tableLayoutDisksPanel.RowCount++;
                tableLayoutDisksPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var rowIndex = tableLayoutDisksPanel.RowCount - 1;
                tableLayoutDisksPanel.Controls.Add(diskControl, 0, rowIndex);
            }

            while (currentCount > desiredCount)
            {
                var lastIndex = tableLayoutDisksPanel.Controls.Count - 1;
                var ctrl = tableLayoutDisksPanel.Controls[lastIndex];

                tableLayoutDisksPanel.Controls.RemoveAt(lastIndex);
                ctrl.Dispose();

                tableLayoutDisksPanel.RowStyles.RemoveAt(tableLayoutDisksPanel.RowCount - 1);
                tableLayoutDisksPanel.RowCount--;

                currentCount--;
            }
        }
    }
}
