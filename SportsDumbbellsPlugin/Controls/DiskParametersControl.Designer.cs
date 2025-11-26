namespace SportsDumbbellsPlugin.Controls
{
    partial class DiskParametersControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiskParametersControl));
            groupBox = new GroupBox();
            textBoxThickness = new TextBox();
            textBoxHoleDiameter = new TextBox();
            textBoxOuterDiameter = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            errorProvider = new ErrorProvider(components);
            groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // groupBox
            // 
            groupBox.Controls.Add(textBoxThickness);
            groupBox.Controls.Add(textBoxHoleDiameter);
            groupBox.Controls.Add(textBoxOuterDiameter);
            groupBox.Controls.Add(label3);
            groupBox.Controls.Add(label2);
            groupBox.Controls.Add(label1);
            groupBox.Dock = DockStyle.Fill;
            groupBox.Location = new Point(0, 0);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(360, 131);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "Параметры диска n";
            // 
            // textBoxThickness
            // 
            textBoxThickness.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxThickness.Location = new Point(230, 96);
            textBoxThickness.Name = "textBoxThickness";
            textBoxThickness.Size = new Size(100, 23);
            textBoxThickness.TabIndex = 5;
            // 
            // textBoxHoleDiameter
            // 
            textBoxHoleDiameter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxHoleDiameter.Location = new Point(230, 61);
            textBoxHoleDiameter.Name = "textBoxHoleDiameter";
            textBoxHoleDiameter.Size = new Size(100, 23);
            textBoxHoleDiameter.TabIndex = 4;
            // 
            // textBoxOuterDiameter
            // 
            textBoxOuterDiameter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxOuterDiameter.Location = new Point(230, 26);
            textBoxOuterDiameter.Name = "textBoxOuterDiameter";
            textBoxOuterDiameter.Size = new Size(100, 23);
            textBoxOuterDiameter.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 99);
            label3.Margin = new Padding(10);
            label3.Name = "label3";
            label3.Size = new Size(168, 15);
            label3.TabIndex = 2;
            label3.Text = "Толщина одного диска t, мм:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 64);
            label2.Margin = new Padding(10);
            label2.Name = "label2";
            label2.Size = new Size(184, 15);
            label2.TabIndex = 1;
            label2.Text = "Диаметр отверстия диска d, мм:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 29);
            label1.Margin = new Padding(10);
            label1.Name = "label1";
            label1.Size = new Size(180, 15);
            label1.TabIndex = 0;
            label1.Text = "Внешний диаметр диска D, мм:";
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            errorProvider.Icon = (Icon)resources.GetObject("errorProvider.Icon");
            // 
            // DiskParametersControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox);
            MaximumSize = new Size(0, 131);
            MinimumSize = new Size(0, 131);
            Name = "DiskParametersControl";
            Size = new Size(360, 131);
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBoxThickness;
        private TextBox textBoxHoleDiameter;
        private TextBox textBoxOuterDiameter;
        private ErrorProvider errorProvider;
    }
}
