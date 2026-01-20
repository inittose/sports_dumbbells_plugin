using Kompas6Constants3D;
using SportsDumbbellsPlugin.Model;

namespace SportsDumbbellsPlugin.Wrapper
{
    public sealed class Builder(Wrapper wrapper)
    {
        private readonly Wrapper _wrapper = wrapper;

        public void Build(DumbbellParameters p)
        {
            _wrapper.AttachOrRunCad(visible: true);
            _wrapper.CreateDocument3D();

            BuildRod(p.Rod);
            BuildDisks(p);

            _wrapper.UpdateModel();
        }

        private void BuildRod(RodParameters rod)
        {
            var rSeat = rod.SeatDiameter / 2.0;
            var rHandle = rod.HandleDiameter / 2.0;
            var totalLen = rod.SeatLength * 2.0 + rod.HandleLength;

            _wrapper.BuildCylinderAtX(rSeat, totalLen);
            _wrapper.BuildCylinderAtX(rHandle, rod.HandleLength);
        }

        private void BuildDisks(DumbbellParameters parameters)
        {
            if (parameters.DisksPerSide <= 0)
            {
                return;
            }

            var offset = parameters.Rod.HandleLength / 2 + parameters.GapBetweenDisks;
            foreach (var disk in parameters.Disks)
            {
                var outerRadius = disk.OuterDiameter / 2.0;
                var holeRadius = disk.HoleDiameter / 2.0;

                _wrapper.BuildDiskAtX(outerRadius, holeRadius, disk.Thickness, offset);
                _wrapper.BuildDiskAtX(
                    outerRadius,
                    holeRadius,
                    disk.Thickness,
                    -offset - disk.Thickness);

                offset += disk.Thickness + parameters.GapBetweenDisks;
            }
        }
    }
}
