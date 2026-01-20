using System.Security.Cryptography;
using SportsDumbbellsPluginCore.Model;

namespace SportsDumbbellsPluginCore.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Validate_DiskHoleTooSmallRelativeToSeat_AddsErrorsForDiskAndRod()
        {
            var p = new DumbbellParameters
            {
                Rod = new RodParameters { SeatDiameter = 30.0, SeatLength = 100, HandleLength = 140, HandleDiameter = 32 },
                DisksPerSide = 1
            };
            p.Disks.Add(new DiskParameters { HoleDiameter = 30.2, OuterDiameter = 150, Thickness = 20 });

            var errors = p.Validate();

            Assert.That(errors.Any(e => e.Source == "Disks[0].HoleDiameter"));
            Assert.That(errors.Any(e => e.Source == "Rod.SeatDiameter"));
        }
    }
}