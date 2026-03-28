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
        /// Строит гриф гантели и кольцевые прорези на рукояти.
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

            BuildHandleGrooves(rodParameters, handleRadius);
        }

        /// <summary>
        /// Строит кольцевые прорези на рукояти.
        /// </summary>
        /// <param name="rodParameters">Параметры грифа.</param>
        /// <param name="handleRadius">Радиус рукояти.</param>
        private void BuildHandleGrooves(RodParameters rodParameters, double handleRadius)
        {
            if (rodParameters.GrooveCount <= 0 || rodParameters.GrooveDepth <= 0.0)
            {
                return;
            }

            var grooveOuterRadius = handleRadius;
            var grooveInnerRadius = handleRadius - rodParameters.GrooveDepth;

            var usableLength =
                rodParameters.HandleLength -
                (2.0 * RodParameters.GrooveEdgeIndent) -
                (rodParameters.GrooveCount * RodParameters.GrooveWidth);

            var gap =
                rodParameters.GrooveCount == 1
                    ? 0.0
                    : usableLength / (rodParameters.GrooveCount - 1);

            var currentGrooveX =
                (-rodParameters.HandleLength / 2.0) + RodParameters.GrooveEdgeIndent;

            for (var grooveIndex = 0; grooveIndex < rodParameters.GrooveCount; grooveIndex++)
            {
                _wrapper.CutRingAtX(
                    grooveOuterRadius,
                    grooveInnerRadius,
                    RodParameters.GrooveWidth,
                    currentGrooveX);

                currentGrooveX += RodParameters.GrooveWidth + gap;
            }
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
            var filletRadius = disk.FilletDiameter / 2.0;

            var rightDisk = _wrapper.BuildDiskAtX(
                outerRadius,
                holeRadius,
                disk.Thickness,
                offsetX);

            _wrapper.ApplyFilletToOperationEdges(rightDisk, filletRadius);

            var leftDisk = _wrapper.BuildDiskAtX(
                outerRadius,
                holeRadius,
                disk.Thickness,
                -offsetX - disk.Thickness);

            _wrapper.ApplyFilletToOperationEdges(leftDisk, filletRadius);
        }
    }
}