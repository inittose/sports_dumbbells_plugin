using SportsDumbbellsPluginCore.Model;

namespace SportsDumbbellsPluginCore.Tests
{
    /// <summary>
    /// Набор модульных тестов для проверки взаимных ограничений
    /// параметров гантели.
    /// </summary>
    [TestFixture]
    public class DumbbellParametersTests
    {
        /// <summary>
        /// Создаёт корректный экземпляр параметров гантели,
        /// удовлетворяющий всем ограничениям валидации.
        /// Используется как базовое состояние для модульных тестов.
        /// </summary>
        private static DumbbellParameters CreateValidDumbbellParameters(int disksPerSide = 1)
        {
            var dumbbellParameters = new DumbbellParameters
            {
                Rod = new RodParameters
                {
                    HandleLength = 140,
                    SeatLength = 100,
                    HandleDiameter = 32,
                    SeatDiameter = 30,
                },
                DisksPerSide = disksPerSide,
            };

            dumbbellParameters.Disks.Add(new DiskParameters
            {
                OuterDiameter = 150,
                HoleDiameter = 31,
                Thickness = 20,
            });

            return dumbbellParameters;
        }

        [Test]
        [Description(
            "Проверяет, что при количестве дисков на сторону вне диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_DisksPerSideOutOfRange_AddsError()
        {
            var dumbbellParameters = CreateValidDumbbellParameters();
            dumbbellParameters.DisksPerSide = 9;

            var validationErrors = dumbbellParameters.Validate();

            var hasError = false;
            foreach (var error in validationErrors)
            {
                if (error.Source == "Dumbbell.DisksPerSide")
                {
                    hasError = true;
                    break;
                }
            }

            Assert.That(hasError);
        }

        [Test]
        [Description(
            "Проверяет, что при слишком малой разнице между диаметром отверстия диска " +
            "и посадочным диаметром стержня добавляются ошибки валидации.")]
        public void Validate_HoleDiameterDeltaTooSmall_AddsErrors()
        {
            var dumbbellParameters = CreateValidDumbbellParameters();
            dumbbellParameters.Disks[0].HoleDiameter = 30.2;

            var validationErrors = dumbbellParameters.Validate();

            var hasDiskError = false;
            var hasRodError = false;

            foreach (var error in validationErrors)
            {
                if (error.Source == "Disks[0].HoleDiameter")
                {
                    hasDiskError = true;
                }

                if (error.Source == "Rod.SeatDiameter")
                {
                    hasRodError = true;
                }
            }

            Assert.That(hasDiskError);
            Assert.That(hasRodError);
        }

        [Test]
        [Description(
            "Проверяет, что при слишком большой разнице между диаметром отверстия диска " +
            "и посадочным диаметром стержня добавляются ошибки валидации.")]
        public void Validate_HoleDiameterDeltaTooLarge_AddsErrors()
        {
            var dumbbellParameters = CreateValidDumbbellParameters();
            dumbbellParameters.Disks[0].HoleDiameter = 32.0;

            var validationErrors = dumbbellParameters.Validate();

            var hasDiskError = false;
            var hasRodError = false;

            foreach (var error in validationErrors)
            {
                if (error.Source == "Disks[0].HoleDiameter")
                {
                    hasDiskError = true;
                }

                if (error.Source == "Rod.SeatDiameter")
                {
                    hasRodError = true;
                }
            }

            Assert.That(hasDiskError);
            Assert.That(hasRodError);
        }

        [Test]
        [Description(
            "Проверяет, что при превышении суммарной толщины дисков " +
            "над длиной посадочной части добавляются ошибки валидации.")]
        public void Validate_TotalDiskWidthExceedsSeatLength_AddsErrors()
        {
            var dumbbellParameters = CreateValidDumbbellParameters(disksPerSide: 2);
            dumbbellParameters.Rod.SeatLength = 30;

            dumbbellParameters.Disks.Add(new DiskParameters
            {
                OuterDiameter = 150,
                HoleDiameter = 31,
                Thickness = 20,
            });

            var validationErrors = dumbbellParameters.Validate();

            var hasRodError = false;
            var hasDiskError = false;

            foreach (var error in validationErrors)
            {
                if (error.Source == "Rod.SeatLength")
                {
                    hasRodError = true;
                }

                if (error.Source.StartsWith("Disks"))
                {
                    hasDiskError = true;
                }
            }

            Assert.That(hasRodError);
            Assert.That(hasDiskError);
        }

        [Test]
        [Description(
            "Проверяет, что при количестве дисков равном нулю " +
            "взаимные проверки с дисками не выполняются.")]
        public void Validate_DisksPerSideZero_DoesNotCheckDisks()
        {
            var dumbbellParameters = CreateValidDumbbellParameters(disksPerSide: 0);
            dumbbellParameters.Disks[0].HoleDiameter = 30.1;

            var validationErrors = dumbbellParameters.Validate();

            foreach (var error in validationErrors)
            {
                Assert.That(error.Source != "Rod.SeatDiameter");
                Assert.That(!error.Source.StartsWith("Disks"));
            }
        }

        [Test]
        [Description(
            "Проверяет, что при корректных параметрах гантели " +
            "валидация не возвращает ошибок.")]
        public void Validate_ValidParameters_NoErrors()
        {
            var dumbbellParameters = CreateValidDumbbellParameters();

            var validationErrors = dumbbellParameters.Validate();

            Assert.That(validationErrors, Is.Empty);
        }
    }
}
