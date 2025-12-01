using SportsDumbbellsPlugin.Model;
using System.Globalization;

namespace SportsDumbbellsPlugin.Controls
{
    public partial class RodParametersControl : UserControl
    {
        public event EventHandler? ParametersChanged;

        public RodParametersControl()
        {
            InitializeComponent();

            textBoxCenterLength.TextChanged += OnAnyTextChanged;
            textBoxSeatLength.TextChanged += OnAnyTextChanged;
            textBoxHandleDiameter.TextChanged += OnAnyTextChanged;
            textBoxSeatDiameter.TextChanged += OnAnyTextChanged;

            SetDefault();
        }

        public void SetDefault()
        {
            textBoxCenterLength.Text =
                DumbbellConstraints.DefaultCenterLength.ToString(CultureInfo.InvariantCulture);

            textBoxSeatLength.Text =
                DumbbellConstraints.DefaultSeatLength.ToString(CultureInfo.InvariantCulture);

            textBoxHandleDiameter.Text =
                DumbbellConstraints.DefaultHandleDiameter.ToString(CultureInfo.InvariantCulture);

            textBoxSeatDiameter.Text =
                DumbbellConstraints.DefaultSeatDiameter.ToString(CultureInfo.InvariantCulture);

            textBoxTotalLength.Text =
                (DumbbellConstraints.DefaultCenterLength
                 + DumbbellConstraints.DefaultSeatLength * 2).ToString(CultureInfo.InvariantCulture);
        }

        public RodParameters GetModel()
        {
            double.TryParse(textBoxCenterLength.Text, out var l1);
            double.TryParse(textBoxSeatLength.Text, out var l2);
            double.TryParse(textBoxHandleDiameter.Text, out var d1);
            double.TryParse(textBoxSeatDiameter.Text, out var d2);

            return new RodParameters
            {
                CenterLength = l1,
                SeatLength = l2,
                HandleDiameter = d1,
                SeatDiameter = d2
            };
        }

        public void ClearErrors()
        {
            Clear(textBoxCenterLength);
            Clear(textBoxSeatLength);
            Clear(textBoxHandleDiameter);
            Clear(textBoxSeatDiameter);
        }

        private void Clear(TextBox tb)
        {
            errorProvider.SetError(tb, string.Empty);
            tb.BackColor = SystemColors.Window;
        }

        public void SetError(string propertyName, string message)
        {
            TextBox? target = propertyName switch
            {
                nameof(RodParameters.CenterLength)   => textBoxCenterLength,
                nameof(RodParameters.SeatLength)     => textBoxSeatLength,
                nameof(RodParameters.HandleDiameter) => textBoxHandleDiameter,
                nameof(RodParameters.SeatDiameter)   => textBoxSeatDiameter,
                _                                    => null
            };

            if (target == null) return;

            errorProvider.SetError(target, message);
            target.BackColor = Color.MistyRose;
        }

        private void OnAnyTextChanged(object? sender, EventArgs e)
        {
            RecalculateTotalLength();
            ParametersChanged?.Invoke(this, EventArgs.Empty);
        }

        private void RecalculateTotalLength()
        {
            if (double.TryParse(textBoxCenterLength.Text, out var l1) &&
                double.TryParse(textBoxSeatLength.Text, out var l2))
            {
                var L = l1 + 2 * l2;
                textBoxTotalLength.Text = L.ToString("0.##");
            }
            else
            {
                // Если ничего вменяемого посчитать нельзя — можно очищать
                textBoxTotalLength.Text = string.Empty;
            }
        }
    }
}
