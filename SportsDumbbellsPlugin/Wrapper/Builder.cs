using SportsDumbbellsPluginCore.Model;

namespace SportsDumbbellsPlugin.Wrapper
{
    //TODO: refactor
    // +
    /// <summary>
    /// Оркестратор построения 3D-модели гантели в KOMPAS-3D.
    /// Выполняет последовательное построение грифа и дисков, используя <see cref="Wrapper"/>.
    /// </summary>
    public class Builder
    {
        /// <summary>
        /// Обёртка над KOMPAS API, выполняющая низкоуровневые операции моделирования.
        /// </summary>
        private readonly Wrapper _wrapper;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Builder"/>.
        /// </summary>
        /// <param name="wrapper">Обёртка KOMPAS API.</param>
        public Builder(Wrapper wrapper)
        {
            _wrapper = wrapper ?? throw new ArgumentNullException(nameof(wrapper));
        }

        /// <summary>
        /// Выполняет построение 3D-модели гантели по заданным параметрам.
        /// </summary>
        /// <param name="parameters">Параметры гантели.</param>
        public void Build(DumbbellParameters parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);

            _wrapper.AttachOrRunCad(visible: true);
            _wrapper.CreateDocument3D();

            BuildRod(parameters.Rod);
            BuildDisks(parameters);

            _wrapper.UpdateModel();
        }

        /// <summary>
        /// Строит гриф гантели в виде двух коаксиальных цилиндров:
        /// базовый цилиндр посадочной части и утолщение рукояти.
        /// </summary>
        /// <param name="rodParameters">Параметры грифа.</param>
        private void BuildRod(RodParameters rodParameters)
        {
            if (rodParameters == null)
            {
                throw new ArgumentNullException(nameof(rodParameters));
            }

            var seatRadius = rodParameters.SeatDiameter / 2.0;
            var handleRadius = rodParameters.HandleDiameter / 2.0;
            var totalRodLength = (rodParameters.SeatLength * 2.0) + rodParameters.HandleLength;

            _wrapper.BuildCylinderAtX(seatRadius, totalRodLength);
            _wrapper.BuildCylinderAtX(handleRadius, rodParameters.HandleLength);
        }

        /// <summary>
        /// Строит диски гантели с обеих сторон грифа.
        /// Диски размещаются симметрично относительно центра рукояти.
        /// </summary>
        /// <param name="parameters">Параметры гантели.</param>
        private void BuildDisks(DumbbellParameters parameters)
        {
            if (parameters.DisksPerSide <= 0)
            {
                return;
            }

            var currentOffsetX = (parameters.Rod.HandleLength / 2.0) +
                                 DumbbellParameters.GapBetweenDisks;

            foreach (var disk in parameters.Disks.Take(parameters.DisksPerSide))
            {
                BuildDiskPair(disk, currentOffsetX);
                currentOffsetX += disk.Thickness + DumbbellParameters.GapBetweenDisks;
            }
        }

        /// <summary>
        /// Строит пару одинаковых дисков симметрично относительно центра грифа.
        /// </summary>
        /// <param name="disk">Параметры диска.</param>
        /// <param name="offsetX">Смещение первого диска по оси X.</param>
        private void BuildDiskPair(DiskParameters disk, double offsetX)
        {
            var outerRadius = disk.OuterDiameter / 2.0;
            var holeRadius = disk.HoleDiameter / 2.0;

            _wrapper.BuildDiskAtX(outerRadius, holeRadius, disk.Thickness, offsetX);
            _wrapper.BuildDiskAtX(
                outerRadius,
                holeRadius,
                disk.Thickness,
                -offsetX - disk.Thickness);
        }
    }
}
