namespace SportsDumbbellsPlugin.Model
{
    public class DumbbellParameters
    {
        /// <summary>
        /// Параметры стержня.
        /// </summary>
        public RodParameters Rod { get; } = new RodParameters();

        /// <summary>
        /// Параметры дисков.
        /// </summary>
        public IList<DiskParameters> Disks { get; } = new List<DiskParameters>();

        /// <summary>
        /// Количество дисков на одной стороне стержня n.
        /// </summary>
        public int DisksPerSide { get; set; }

        /// <summary>
        /// Суммарная толщина дисков на одной стороне H.
        /// </summary>
        public double TotalDiskWidthPerSide => Disks.Sum(d => d.DiskThickness);
    }
}