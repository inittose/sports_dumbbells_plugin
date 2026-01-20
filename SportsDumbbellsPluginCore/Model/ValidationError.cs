namespace SportsDumbbellsPluginCore.Model
{
    public class ValidationError(string source, string message)
    {
        public string Source { get; } = source;

        public string Message { get; } = message;
    }
}
