namespace SportsDumbbellsPlugin.View
{
    /// <summary>
    /// Окно со справочной информацией по грифу гантели.
    /// Реализовано как Singleton-форма: при закрытии пользователем окно скрывается,
    /// чтобы повторное открытие происходило без пересоздания экземпляра.
    /// </summary>
    public partial class RodInformationForm : Form
    {
        /// <summary>
        /// Единственный экземпляр формы.
        /// </summary>
        private static RodInformationForm? _instance;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RodInformationForm"/>.
        /// </summary>
        public RodInformationForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Возвращает экземпляр формы. Если экземпляр ещё не создан, создаёт его.
        /// Если ранее созданный экземпляр был уничтожен (disposed), создаёт новый.
        /// </summary>
        /// <returns>Экземпляр формы <see cref="RodInformationForm"/>.</returns>
        public static RodInformationForm GetInstance()
        {
            if (_instance == null || _instance.IsDisposed)
            {
                _instance = new RodInformationForm();
            }

            return _instance;
        }

        /// <summary>
        /// Обработчик закрытия формы.
        /// При закрытии пользователем отменяет закрытие и скрывает форму.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события закрытия формы.</param>
        private void RodInformationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                return;
            }

            e.Cancel = true;
            Hide();
        }
    }
}