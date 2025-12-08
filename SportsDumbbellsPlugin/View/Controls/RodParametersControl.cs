using SportsDumbbellsPlugin.Model;
using System.Globalization;
using System.Windows.Forms;

namespace SportsDumbbellsPlugin.View.Controls
{
    public partial class RodParametersControl : UserControl
    {
        public event EventHandler? ParametersChanged;

        public RodParametersControl()
        {
            InitializeComponent();

            textBoxCenterLength.TextChanged += OnAnyParameterChanged;
            textBoxSeatLength.TextChanged += OnAnyParameterChanged;
            textBoxHandleDiameter.TextChanged += OnAnyParameterChanged;
            textBoxSeatDiameter.TextChanged += OnAnyParameterChanged;

            SetDefault();
        }

        public RodParameters GetModel()
        {
            var model = new RodParameters();

            if (double.TryParse(textBoxCenterLength.Text, out var l1))
            {
                model.CenterLength = l1;
            }

            if (double.TryParse(textBoxSeatLength.Text, out var l2))
            {
                model.SeatLength = l2;
            }

            if (double.TryParse(textBoxHandleDiameter.Text, out var d1))
            {
                model.HandleDiameter = d1;
            }

            if (double.TryParse(textBoxSeatDiameter.Text, out var d2))
            {
                model.SeatDiameter = d2;
            }

            return model;
        }

        public void SetModel(RodParameters model)
        {
            textBoxCenterLength.Text = model.CenterLength.ToString("F1");
            textBoxSeatLength.Text = model.SeatLength.ToString("F1");
            textBoxHandleDiameter.Text = model.HandleDiameter.ToString("F1");
            textBoxSeatDiameter.Text = model.SeatDiameter.ToString("F1");

            UpdateTotalLength(model.TotalLength);
        }

        public void SetDefault()
        {
            var model = new RodParameters();
            SetModel(model);

            ClearErrors();
        }

        /// <summary>
        /// Сброс всех ошибок.
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
        /// Применяет список ошибок к контролу (UI-валидация).
        /// Модель уже провалидирована, сюда прилетает готовый список.
        /// </summary>
        public void ApplyErrors(IReadOnlyList<ValidationError> errors)
        {
            ClearErrors();

            // Берём только ошибки для стержня
            var rodErrors = errors
                .Where(error => error.Source.StartsWith("Rod.", StringComparison.Ordinal))
                .ToList();

            if (rodErrors.Count == 0)
                return;

            // Группируем по имени свойства после "Rod."
            var grouped = rodErrors
                .GroupBy(e => e.Source.Substring("Rod.".Length));

            foreach (var group in grouped)
            {
                var propertyName = group.Key;

                // Склеиваем все сообщения (убираем дубли)
                var message = string.Join(
                    Environment.NewLine,
                    group.Select(g => g.Message).Distinct()
                );

                ApplyError(propertyName, message);
            }
        }

        private void ApplyError(string propertyName, string message)
        {
            switch (propertyName)
            {
                case nameof(RodParameters.CenterLength):
                    SetError(textBoxCenterLength, message);
                    break;

                case nameof(RodParameters.SeatLength):
                    SetError(textBoxSeatLength, message);
                    break;

                case nameof(RodParameters.HandleDiameter):
                    SetError(textBoxHandleDiameter, message);
                    break;

                case nameof(RodParameters.SeatDiameter):
                    SetError(textBoxSeatDiameter, message);
                    break;
            }
        }

        private void SetError(Control control, string message)
        {
            errorProvider.SetError(control, message);
            control.BackColor = Color.MistyRose;
        }

        private void ResetBackColor(Control control)
        {
            control.BackColor = SystemColors.Window;
        }

        private void OnAnyParameterChanged(object? sender, EventArgs e)
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
                UpdateTotalLength(L);
            }
            else
            {
                textBoxTotalLength.Text = string.Empty;
            }
        }

        private void UpdateTotalLength(double totalLength)
        {
            textBoxTotalLength.Text = totalLength.ToString("F1");
        }
    }
}
