using System.Globalization;
using SportsDumbbellsPlugin.Model;

namespace SportsDumbbellsPlugin.Controls
{
    public partial class DiskParametersControl : UserControl
    {
        private int _diskNumber;

        private void OnAnyTextChanged(object? sender, EventArgs e)
            => ParametersChanged?.Invoke(this, EventArgs.Empty);

        public event EventHandler? ParametersChanged;

        public int DiskNumber
        {
            get => _diskNumber;
            set
            {
                _diskNumber = value;
                groupBox.Text = $"Параметры диска {value}";
            }
        }

        public DiskParametersControl()
        {
            InitializeComponent();

            textBoxOuterDiameter.TextChanged += OnAnyTextChanged;
            textBoxHoleDiameter.TextChanged += OnAnyTextChanged;
            textBoxThickness.TextChanged += OnAnyTextChanged;

            SetDefault();
        }

        public void SetDefault()
        {
            textBoxOuterDiameter.Text = DumbbellConstraints.DefaultDiskOuterDiameter.ToString(CultureInfo.InvariantCulture);
            textBoxHoleDiameter.Text = DumbbellConstraints.DefaultDiskHoleDiameter.ToString(CultureInfo.InvariantCulture);
            textBoxThickness.Text = DumbbellConstraints.DefaultDiskThickness.ToString(CultureInfo.InvariantCulture);
        }

        //TODO: переименовать локальные переменные.
        public DiskParameters GetModel()
        {
            double.TryParse(textBoxOuterDiameter.Text, out var D);
            double.TryParse(textBoxHoleDiameter.Text, out var d);
            double.TryParse(textBoxThickness.Text, out var t);

            return new DiskParameters
            {
                DiskOuterDiameter = D,
                DiskHoleDiameter = d,
                DiskThickness = t
            };
        }

        public void ClearErrors()
        {
            errorProvider.SetError(textBoxOuterDiameter, string.Empty);
            errorProvider.SetError(textBoxHoleDiameter, string.Empty);
            errorProvider.SetError(textBoxThickness, string.Empty);

            textBoxOuterDiameter.BackColor = SystemColors.Window;
            textBoxHoleDiameter.BackColor = SystemColors.Window;
            textBoxThickness.BackColor = SystemColors.Window;
        }

        public void SetError(string propertyName, string message)
        {
            TextBox target;
            switch (propertyName)
            {
                case nameof(DiskParameters.DiskOuterDiameter):
                    target = textBoxOuterDiameter;
                    break;
                case nameof(DiskParameters.DiskHoleDiameter):
                    target = textBoxHoleDiameter;
                    break;
                case nameof(DiskParameters.DiskThickness):
                    target = textBoxThickness;
                    break;
                default:
                    return;
            }

            errorProvider.SetError(target, message);
            target.BackColor = Color.MistyRose;
        }
    }
}
