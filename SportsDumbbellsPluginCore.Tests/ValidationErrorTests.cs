using SportsDumbbellsPluginCore.Validation;

namespace SportsDumbbellsPluginCore.Tests
{
    [TestFixture]
    public class ValidationErrorTests
    {
        [Test]
        [Description(
            "Проверяет, что конструктор ValidationError " +
            "корректно сохраняет источник ошибки и её сообщение.")]
        public void Constructor_SetsSourceAndMessage()
        {
            var validationError = new ValidationError(
                "Rod.SeatDiameter",
                "Ошибка проверки.");

            Assert.Multiple(() =>
            {
                Assert.That(
                    validationError.Source,
                    Is.EqualTo("Rod.SeatDiameter"));

                Assert.That(
                    validationError.Message,
                    Is.EqualTo("Ошибка проверки."));
            });
        }
    }
}