using SportsDumbbellsPluginCore.Model;
using SportsDumbbellsPlugin.Wrapper;

namespace SportsDumbbellsPlugin.View
{
    public partial class MainForm : Form
    {
        private Wrapper.Wrapper? _wrapper;
        private bool _rodTabHasErrors = false;
        private bool _disksTabHasErrors = false;

        public MainForm()
        {
            InitializeComponent();

            rodParametersControl.ParametersChanged += AnyParametersChanged;
            disksControl.ParametersChanged += AnyParametersChanged;
        }

        private void AnyParametersChanged(object? sender, EventArgs e)
        {
            ValidateAllParameters();
        }

        private void ValidateAllParameters()
        {
            var model = BuildModelFromControls();
            var errors = model.Validate();

            _rodTabHasErrors = errors.Any(e => e.Source.StartsWith("Rod."));
            _disksTabHasErrors = errors.Any(e => e.Source.StartsWith("Disks["));

            rodParametersControl.ApplyErrors(errors);
            disksControl.ApplyErrors(errors);

            buttonDesign.Enabled = !errors.Any();
            tabControl.Invalidate();
        }

        private DumbbellParameters BuildModelFromControls()
        {
            var model = new DumbbellParameters
            {
                Rod = rodParametersControl.GetModel(),
                DisksPerSide = disksControl.DisksPerSide
            };

            model.Disks.AddRange(disksControl.GetDiskModels());
            return model;
        }

        private void DrawTabValidation(object sender, DrawItemEventArgs e)
        {
            var tabControl = (TabControl)sender;
            var page = tabControl.TabPages[e.Index];

            var isRodTab = page == tabPageRod;
            var isDisksTab = page == tabPageDisks;

            var hasError =
                (isRodTab && _rodTabHasErrors) ||
                (isDisksTab && _disksTabHasErrors);

            Color backColor;
            if (hasError)
                backColor = Color.MistyRose;
            else if (e.State.HasFlag(DrawItemState.Selected))
                backColor = SystemColors.ControlLightLight;
            else
                backColor = SystemColors.Control;

            using (var b = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }

            TextRenderer.DrawText(
                e.Graphics,
                page.Text,
                e.Font,
                e.Bounds,
                SystemColors.ControlText,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void ShowRodInformation(object sender, EventArgs e)
        {
            var informationForm = RodInformationForm.GetInstance();
            informationForm.Show();
            informationForm.Activate();
        }

        private void ShowDisksInformation(object sender, EventArgs e)
        {
            var informationForm = DiskInformationForm.GetInstance();
            informationForm.Show();
            informationForm.Activate();
        }

        private void SetRodDefault(object sender, EventArgs e)
        {
            rodParametersControl.SetDefault();
        }

        private void SetDisksDefault(object sender, EventArgs e)
        {
            disksControl.SetDefault();
        }

        private void buttonDesign_Click(object sender, EventArgs e)
        {
            // 1) Собираешь параметры из контролов (у тебя это уже есть логикой)
            var parameters = BuildModelFromControls();

            // 2) Валидируешь (у тебя уже есть Validate())
            var errors = parameters.Validate();
            if (errors.Count > 0)
            {
                // показать ошибки как ты уже умеешь
                return;
            }

            _wrapper ??= new Wrapper.Wrapper();
            var builder = new Builder(_wrapper);

            builder.Build(parameters);
        }
    }
}
