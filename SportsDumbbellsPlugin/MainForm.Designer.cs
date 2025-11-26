namespace SportsDumbbellsPlugin
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl = new TabControl();
            tabPage1 = new TabPage();
            rodParametersControl = new SportsDumbbellsPlugin.Controls.RodParametersControl();
            button2 = new Button();
            tabPage2 = new TabPage();
            disksPanel = new Panel();
            disksScrollPanel = new Panel();
            tableLayoutDisksPanel = new TableLayoutPanel();
            button3 = new Button();
            label6 = new Label();
            numericUpDownDisksPerSide = new NumericUpDown();
            buttonDesign = new Button();
            tabControl.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            disksPanel.SuspendLayout();
            disksScrollPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDisksPerSide).BeginInit();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabPage1);
            tabControl.Controls.Add(tabPage2);
            tabControl.Location = new Point(12, 12);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.ShowToolTips = true;
            tabControl.Size = new Size(405, 258);
            tabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.Transparent;
            tabPage1.Controls.Add(rodParametersControl);
            tabPage1.Controls.Add(button2);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(397, 230);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Стержень";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // rodParametersControl
            // 
            rodParametersControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            rodParametersControl.Location = new Point(0, 0);
            rodParametersControl.Name = "rodParametersControl";
            rodParametersControl.Size = new Size(397, 173);
            rodParametersControl.TabIndex = 11;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Location = new Point(204, 181);
            button2.Margin = new Padding(3, 5, 3, 3);
            button2.Name = "button2";
            button2.Size = new Size(185, 23);
            button2.TabIndex = 10;
            button2.Text = "Показать эскиз с размерами";
            button2.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(disksPanel);
            tabPage2.Controls.Add(button3);
            tabPage2.Controls.Add(label6);
            tabPage2.Controls.Add(numericUpDownDisksPerSide);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(397, 230);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Диски";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // disksPanel
            // 
            disksPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            disksPanel.Controls.Add(disksScrollPanel);
            disksPanel.Location = new Point(6, 69);
            disksPanel.Name = "disksPanel";
            disksPanel.Size = new Size(385, 155);
            disksPanel.TabIndex = 5;
            // 
            // disksScrollPanel
            // 
            disksScrollPanel.AutoScroll = true;
            disksScrollPanel.Controls.Add(tableLayoutDisksPanel);
            disksScrollPanel.Dock = DockStyle.Fill;
            disksScrollPanel.Location = new Point(0, 0);
            disksScrollPanel.Name = "disksScrollPanel";
            disksScrollPanel.Size = new Size(385, 155);
            disksScrollPanel.TabIndex = 5;
            // 
            // tableLayoutDisksPanel
            // 
            tableLayoutDisksPanel.AutoSize = true;
            tableLayoutDisksPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutDisksPanel.ColumnCount = 1;
            tableLayoutDisksPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutDisksPanel.Dock = DockStyle.Top;
            tableLayoutDisksPanel.Location = new Point(0, 0);
            tableLayoutDisksPanel.Name = "tableLayoutDisksPanel";
            tableLayoutDisksPanel.RowCount = 1;
            tableLayoutDisksPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutDisksPanel.Size = new Size(385, 0);
            tableLayoutDisksPanel.TabIndex = 4;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.Location = new Point(215, 45);
            button3.Name = "button3";
            button3.Size = new Size(174, 23);
            button3.TabIndex = 3;
            button3.Text = "Показать эскиз с размерами";
            button3.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(8, 13);
            label6.Margin = new Padding(5, 10, 10, 10);
            label6.Name = "label6";
            label6.Size = new Size(249, 15);
            label6.TabIndex = 1;
            label6.Text = "Количество дисков на одной стороне n, шт:";
            // 
            // numericUpDownDisksPerSide
            // 
            numericUpDownDisksPerSide.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numericUpDownDisksPerSide.Location = new Point(269, 11);
            numericUpDownDisksPerSide.Margin = new Padding(3, 3, 5, 3);
            numericUpDownDisksPerSide.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            numericUpDownDisksPerSide.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownDisksPerSide.Name = "numericUpDownDisksPerSide";
            numericUpDownDisksPerSide.Size = new Size(120, 23);
            numericUpDownDisksPerSide.TabIndex = 0;
            numericUpDownDisksPerSide.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownDisksPerSide.ValueChanged += numericUpDown_ValueChanged;
            // 
            // buttonDesign
            // 
            buttonDesign.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDesign.Location = new Point(299, 276);
            buttonDesign.Name = "buttonDesign";
            buttonDesign.Size = new Size(119, 23);
            buttonDesign.TabIndex = 1;
            buttonDesign.Text = "Спроектировать";
            buttonDesign.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 311);
            Controls.Add(buttonDesign);
            Controls.Add(tabControl);
            MinimumSize = new Size(450, 350);
            Name = "MainForm";
            Text = "Плагин для создания спортивных гантель";
            tabControl.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            disksPanel.ResumeLayout(false);
            disksScrollPanel.ResumeLayout(false);
            disksScrollPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDisksPerSide).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button buttonDesign;
        private Button button2;
        private Label label6;
        private NumericUpDown numericUpDownDisksPerSide;
        private Button button3;
        private TableLayoutPanel tableLayoutDisksPanel;
        private Panel disksPanel;
        private Panel disksScrollPanel;
        private Controls.RodParametersControl rodParametersControl;
    }
}
