namespace SportsDumbbellsPlugin.View
{
    public partial class RodInformationForm : Form
    {
        private static RodInformationForm? _instance;

        public RodInformationForm()
        {
            InitializeComponent();
        }

        public static RodInformationForm GetInstance()
        {
            return _instance ??= new RodInformationForm();
        }

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
