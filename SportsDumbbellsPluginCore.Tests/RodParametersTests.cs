using SportsDumbbellsPluginCore.Model;

namespace SportsDumbbellsPluginCore.Tests
{
    /// <summary>
    /// Набор модульных тестов для проверки валидации
    /// параметров стержня гантели.
    /// </summary>
    [TestFixture]
    public class RodParametersTests
    {
        /// <summary>
        /// Создаёт корректный экземпляр параметров стержня,
        /// удовлетворяющий всем ограничениям валидации.
        /// Используется как базовое состояние для модульных тестов.
        /// </summary>
        private static RodParameters CreateValidRodParameters()
        {
            return new RodParameters
            {
                HandleLength = 140,
                SeatLength = 90,
                HandleDiameter = 30,
                SeatDiameter = 28,
            };
        }

        [Test]
        [Description(
            "Проверяет, что при длине рукояти вне допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_HandleLengthOutOfRange_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.HandleLength = 99;

            var validationErrors = rodParameters.Validate();

            var hasError = false;

            foreach (var error in validationErrors)
            {
                if (error.Source == "Rod.HandleLength")
                {
                    hasError = true;
                    break;
                }
            }

            Assert.That(hasError);
        }

        [Test]
        [Description(
            "Проверяет, что при длине посадочной части вне допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_SeatLengthOutOfRange_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.SeatLength = 151;

            var validationErrors = rodParameters.Validate();

            var hasError = false;

            foreach (var error in validationErrors)
            {
                if (error.Source == "Rod.SeatLength")
                {
                    hasError = true;
                    break;
                }
            }

            Assert.That(hasError);
        }

        [Test]
        [Description(
            "Проверяет, что при диаметре рукояти вне допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_HandleDiameterOutOfRange_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.HandleDiameter = 35;

            var validationErrors = rodParameters.Validate();

            var hasError = false;

            foreach (var error in validationErrors)
            {
                if (error.Source == "Rod.HandleDiameter")
                {
                    hasError = true;
                    break;
                }
            }

            Assert.That(hasError);
        }

        [Test]
        [Description(
            "Проверяет, что при диаметре посадочной части вне допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_SeatDiameterOutOfRange_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.SeatDiameter = 23;

            var validationErrors = rodParameters.Validate();

            var hasError = false;

            foreach (var error in validationErrors)
            {
                if (error.Source == "Rod.SeatDiameter")
                {
                    hasError = true;
                    break;
                }
            }

            Assert.That(hasError);
        }

        [Test]
        [Description(
            "Проверяет, что при диаметре рукояти меньше либо равном диаметру посадки " +
            "добавляются ошибки для обоих параметров.")]
        public void Validate_HandleDiameterLessOrEqualSeatDiameter_AddsErrors()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.HandleDiameter = 28;
            rodParameters.SeatDiameter = 28;

            var validationErrors = rodParameters.Validate();

            var hasHandleDiameterError = false;
            var hasSeatDiameterError = false;

            foreach (var error in validationErrors)
            {
                if (error.Source == "Rod.HandleDiameter")
                {
                    hasHandleDiameterError = true;
                }

                if (error.Source == "Rod.SeatDiameter")
                {
                    hasSeatDiameterError = true;
                }
            }

            Assert.That(hasHandleDiameterError);
            Assert.That(hasSeatDiameterError);
        }

        [Test]
        [Description(
            "Проверяет, что при общей длине стержня вне допустимого диапазона " +
            "добавляются ошибки для длины рукояти и посадочной части.")]
        public void Validate_TotalLengthOutOfRange_AddsErrors()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.HandleLength = 10;
            rodParameters.SeatLength = 10;

            var validationErrors = rodParameters.Validate();

            var hasHandleLengthError = false;
            var hasSeatLengthError = false;

            foreach (var error in validationErrors)
            {
                if (error.Source == "Rod.HandleLength")
                {
                    hasHandleLengthError = true;
                }

                if (error.Source == "Rod.SeatLength")
                {
                    hasSeatLengthError = true;
                }
            }

            Assert.That(hasHandleLengthError);
            Assert.That(hasSeatLengthError);
        }

        [Test]
        [Description(
            "Проверяет, что при корректных параметрах стержня " +
            "валидация не возвращает ошибок.")]
        public void Validate_ValidParameters_NoErrors()
        {
            var rodParameters = CreateValidRodParameters();

            var validationErrors = rodParameters.Validate();

            Assert.That(validationErrors, Is.Empty);
        }

        [Test]
        [Description(
    "Проверяет, что при длине рукояти выше допустимого диапазона " +
    "добавляется ошибка валидации.")]
        public void Validate_HandleLengthAboveMax_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.HandleLength = 201;

            var validationErrors = rodParameters.Validate();

            Assert.That(validationErrors.Any(error =>
                error.Source == "Rod.HandleLength"));
        }

        [Test]
        [Description(
            "Проверяет, что при длине посадочной части ниже допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_SeatLengthBelowMin_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.SeatLength = 69;

            var validationErrors = rodParameters.Validate();

            Assert.That(validationErrors.Any(error =>
                error.Source == "Rod.SeatLength"));
        }

        [Test]
        [Description(
            "Проверяет, что при диаметре рукояти ниже допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_HandleDiameterBelowMin_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.HandleDiameter = 23;

            var validationErrors = rodParameters.Validate();

            Assert.That(validationErrors.Any(error =>
                error.Source == "Rod.HandleDiameter"));
        }

        [Test]
        [Description(
            "Проверяет, что при диаметре посадочной части выше допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_SeatDiameterAboveMax_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.SeatDiameter = 35;

            var validationErrors = rodParameters.Validate();

            Assert.That(validationErrors.Any(error =>
                error.Source == "Rod.SeatDiameter"));
        }

        [Test]
        [Description(
            "Проверяет, что при общей длине стержня выше допустимого диапазона " +
            "добавляются ошибки для длины рукояти и посадочной части.")]
        public void Validate_TotalLengthAboveMax_AddsErrors()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.HandleLength = 200;
            rodParameters.SeatLength = 151;

            var validationErrors = rodParameters.Validate();

            Assert.Multiple(() =>
            {
                Assert.That(validationErrors.Any(error =>
                    error.Source == "Rod.HandleLength"));

                Assert.That(validationErrors.Any(error =>
                    error.Source == "Rod.SeatLength"));
            });
        }

        [Test]
        [Description("Проверяет, что при количестве прорезей вне допустимого диапазона добавляется ошибка валидации.")]
        public void Validate_GrooveCountOutOfRange_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.GrooveCount = 20;

            var validationErrors = rodParameters.Validate();

            Assert.That(validationErrors.Any(error => error.Source == "Rod.GrooveCount"));
        }

        [Test]
        [Description("Проверяет, что при слишком большой глубине прорези добавляется ошибка валидации.")]
        public void Validate_GrooveDepthTooLarge_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.GrooveCount = 5;
            rodParameters.GrooveDepth = 5.0;

            var validationErrors = rodParameters.Validate();

            Assert.That(validationErrors.Any(error => error.Source == "Rod.GrooveDepth"));
        }

        [Test]
        [Description("Проверяет, что при слишком большом количестве прорезей для длины рукояти добавляются ошибки валидации.")]
        public void Validate_GroovesDoNotFitHandleLength_AddsErrors()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.HandleLength = 100;
            rodParameters.GrooveCount = 12;

            var validationErrors = rodParameters.Validate();

            Assert.Multiple(() =>
            {
                Assert.That(validationErrors.Any(error => error.Source == "Rod.GrooveCount"));
                Assert.That(validationErrors.Any(error => error.Source == "Rod.HandleLength"));
            });
        }

        [Test]
        [Description(
            "Проверяет, что при количестве прорезей ниже допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_GrooveCountBelowMin_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.GrooveCount = -1;

            var validationErrors = rodParameters.Validate();

            Assert.That(validationErrors.Any(error =>
                error.Source == "Rod.GrooveCount"));
        }

        [Test]
        [Description(
            "Проверяет, что при количестве прорезей выше допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_GrooveCountAboveMax_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.GrooveCount = 13;

            var validationErrors = rodParameters.Validate();

            Assert.That(validationErrors.Any(error =>
                error.Source == "Rod.GrooveCount"));
        }

        [Test]
        [Description(
            "Проверяет, что при глубине прорези ниже допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_GrooveDepthBelowMin_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.GrooveDepth = -0.1;

            var validationErrors = rodParameters.Validate();

            Assert.That(validationErrors.Any(error =>
                error.Source == "Rod.GrooveDepth"));
        }

        [Test]
        [Description(
            "Проверяет, что при глубине прорези выше допустимого диапазона " +
            "добавляется ошибка валидации.")]
        public void Validate_GrooveDepthAboveMax_AddsError()
        {
            var rodParameters = CreateValidRodParameters();
            rodParameters.GrooveDepth = 5.1;

            var validationErrors = rodParameters.Validate();

            Assert.That(validationErrors.Any(error =>
                error.Source == "Rod.GrooveDepth"));
        }
    }
}
