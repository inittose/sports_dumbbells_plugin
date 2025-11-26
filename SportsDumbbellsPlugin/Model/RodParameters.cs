namespace SportsDumbbellsPlugin.Model
{
    public class RodParameters
    {
        /// <summary>
        /// Длина средней части стержня l1.
        /// </summary>
        public double CenterLength { get; set; }

        /// <summary>
        /// Длина посадочной части стержня l2 (одна сторона).
        /// </summary>
        public double SeatLength { get; set; }

        /// <summary>
        /// Диаметр рукояти стержня d1.
        /// </summary>
        public double HandleDiameter { get; set; }

        /// <summary>
        /// Диаметр посадочной части стержня d2.
        /// </summary>
        public double SeatDiameter { get; set; }

        /// <summary>
        /// Полная длина стержня L.
        /// </summary>
        public double TotalLength => CenterLength + 2 * SeatLength;
    }
}
