using SportsDumbbellsPluginCore.Model;

namespace SportsDumbbellsPluginCore.Tests
{
    /// <summary>
    /// Набор модульных тестов для проверки корректности
    /// валидации параметров диска гантели.
    /// </summary>
    [TestFixture]
    public class DiskParametersTests
    {
        /// <summary>
        /// Создаёт корректный экземпляр параметров диска,
        /// удовлетворяющий всем ограничениям валидации.
        /// </summary>
        private static DiskParameters CreateValidDiskParameters()
        {
            return new DiskParameters
            {
                OuterDiameter = 150.0,
                HoleDiameter = 31.0,
                Thickness = 20.0,
            };
        }

        [Test]
        [Description(
            "Проверяет, что при внешнем диаметре диска вне допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_OuterDiameterOutOfRange_AddsError()
        {
            var diskParameters = CreateValidDiskParameters();
            diskParameters.OuterDiameter = 119.0;

            var validationErrors = diskParameters.Validate();

            Assert.That(validationErrors.Any(error =>
                error.Source == "Disk.OuterDiameter"));
        }

        [Test]
        [Description(
            "Проверяет, что при диаметре отверстия диска вне допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_HoleDiameterOutOfRange_AddsError()
        {
            var diskParameters = CreateValidDiskParameters();
            diskParameters.HoleDiameter = 35.0;

            var validationErrors = diskParameters.Validate();

            Assert.That(validationErrors.Any(error =>
                error.Source == "Disk.HoleDiameter"));
        }

        [Test]
        [Description(
            "Проверяет, что при толщине диска вне допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_ThicknessOutOfRange_AddsError()
        {
            var diskParameters = CreateValidDiskParameters();
            diskParameters.Thickness = 41.0;

            var validationErrors = diskParameters.Validate();

            Assert.That(validationErrors.Any(error =>
                error.Source == "Disk.Thickness"));
        }

        [Test]
        [Description(
            "Проверяет, что при корректных параметрах диска " +
            "валидация не возвращает ошибок.")]
        public void Validate_ValidParameters_NoErrors()
        {
            var diskParameters = CreateValidDiskParameters();

            var validationErrors = diskParameters.Validate();

            Assert.That(validationErrors, Is.Empty);
        }
    }
}
