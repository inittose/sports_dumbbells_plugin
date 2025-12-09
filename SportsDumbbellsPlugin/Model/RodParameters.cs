namespace SportsDumbbellsPlugin.Model
{
    public class RodParameters
    {
        private const double CenterLengthMin = 100.0;

        private const double CenterLengthMax = 200.0;

        private const double SeatLengthMin = 70.0;

        private const double SeatLengthMax = 150.0;

        private const double HandleDiameterMin = 24.0;

        private const double HandleDiameterMax = 34.0;

        private const double SeatDiameterMin = 24.0;

        private const double SeatDiameterMax = 34.0;

        private const double TotalLengthMin = 200.0;

        private const double TotalLengthMax = 500.0;

        public double CenterLength { get; set; } = 140.0;

        public double SeatLength { get; set; } = 90.0;

        public double HandleDiameter { get; set; } = 28.0;

        public double SeatDiameter { get; set; } = 26.0;

        public double TotalLength => CenterLength + 2 * SeatLength;

        public IReadOnlyList<ValidationError> Validate()
        {
            var errors = new List<ValidationError>();

            if (CenterLength < CenterLengthMin || CenterLength > CenterLengthMax)
            {
                errors.Add(
                    new ValidationError(
                        "Rod.CenterLength",
                        $"Длина центральной части l₁ должна быть в диапазоне {CenterLengthMin}–{CenterLengthMax} мм."));
            }

            if (SeatLength < SeatLengthMin || SeatLength > SeatLengthMax)
            {
                errors.Add(
                    new ValidationError(
                        "Rod.SeatLength",
                        $"Длина посадочной части l₂ должна быть в диапазоне {SeatLengthMin}–{SeatLengthMax} мм."));
            }

            if (HandleDiameter < HandleDiameterMin || HandleDiameter > HandleDiameterMax)
            {
                errors.Add(
                    new ValidationError(
                        "Rod.HandleDiameter",
                        $"Диаметр рукояти d₁ должен быть в диапазоне {HandleDiameterMin}–{HandleDiameterMax} мм."));
            }

            if (SeatDiameter < SeatDiameterMin || SeatDiameter > SeatDiameterMax)
            {
                errors.Add(
                    new ValidationError(
                        "Rod.SeatDiameter",
                        $"Диаметр посадочной части стержня d₂ должен быть в диапазоне {SeatDiameterMin}–{SeatDiameterMax} мм."));
            }

            if (!(TotalLength < TotalLengthMin) && !(TotalLength > TotalLengthMax))
            {
                return errors;
            }

            var message =
                $"Общая длина стержня L = l₁ + 2·l₂ должна быть в диапазоне {TotalLengthMin}–{TotalLengthMax} мм.";

            errors.Add(new ValidationError("Rod.CenterLength", message));
            errors.Add(new ValidationError("Rod.SeatLength", message));

            return errors;
        }
    }
}
