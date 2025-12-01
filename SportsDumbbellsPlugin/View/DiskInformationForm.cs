namespace SportsDumbbellsPlugin.View
{
    public partial class DiskInformationForm : Form
    {
        private static DiskInformationForm? _instance;

        public DiskInformationForm()
        {
            InitializeComponent();
        }

        public static DiskInformationForm GetInstance()
        {
            return _instance ??= new DiskInformationForm();
        }

        private void DiskInformationForm_FormClosing(object sender, FormClosingEventArgs e)
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
