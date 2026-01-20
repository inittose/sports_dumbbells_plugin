namespace SportsDumbbellsPluginCore.Model
{
    /// <summary>
    /// Описывает ошибку валидации параметров модели.
    /// Содержит источник ошибки и диагностическое сообщение.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Источник ошибки валидации.
        /// Представляет собой путь к параметру.
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Текстовое описание ошибки валидации.
        /// Предназначено для отображения пользователю.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ValidationError"/>.
        /// </summary>
        /// <param name="source">Источник ошибки валидации.</param>
        /// <param name="message">Сообщение об ошибке.</param>
        public ValidationError(string source, string message)
        {
            Source = source;
            Message = message;
        }
    }
}
