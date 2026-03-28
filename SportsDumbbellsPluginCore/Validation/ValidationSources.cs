namespace SportsDumbbellsPluginCore.Validation
{
    /// <summary>
    /// Содержит строковые префиксы источников ошибок валидации.
    /// Используется как общий контракт между слоями Core и View.
    /// </summary>
    public static class ValidationSources
    {
        /// <summary>
        /// Префикс источников ошибок, относящихся к грифу.
        /// </summary>
        public const string RodPrefix = "Rod.";

        /// <summary>
        /// Префикс источников ошибок, относящихся к отдельному диску.
        /// </summary>
        public const string DiskPrefix = "Disk.";

        /// <summary>
        /// Префикс источников ошибок, относящихся к дискам в коллекции.
        /// </summary>
        public const string DisksPrefix = "Disks[";
    }
}
