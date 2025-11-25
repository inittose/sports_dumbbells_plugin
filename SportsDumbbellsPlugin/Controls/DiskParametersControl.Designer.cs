namespace SportsDumbbellsPlugin.Controls
{
    partial class DiskParametersControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private int _diskNumber;

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
            groupBox = new GroupBox();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            groupBox.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox
            // 
            groupBox.Controls.Add(textBox3);
            groupBox.Controls.Add(textBox2);
            groupBox.Controls.Add(textBox1);
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
            // textBox3
            // 
            textBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox3.Location = new Point(248, 96);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 5;
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox2.Location = new Point(248, 61);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 4;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox1.Location = new Point(248, 26);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 3;
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
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox3;
        private TextBox textBox2;
        private TextBox textBox1;

        public int DiskNumber
        {
            get => _diskNumber;
            set
            {
                _diskNumber = value;
                groupBox.Text = $"Параметры диска {value}";
            }
        }
    }
}
