namespace SportsDumbbellsPlugin.View.Controls
{
    partial class DisksControl
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
            label6 = new Label();
            numericUpDownDisksPerSide = new NumericUpDown();
            disksPanel = new Panel();
            disksScrollPanel = new Panel();
            tableLayoutDisksPanel = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)numericUpDownDisksPerSide).BeginInit();
            disksPanel.SuspendLayout();
            disksScrollPanel.SuspendLayout();
            SuspendLayout();
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(8, 10);
            label6.Margin = new Padding(8, 10, 5, 5);
            label6.Name = "label6";
            label6.Size = new Size(249, 15);
            label6.TabIndex = 3;
            label6.Text = "Количество дисков на одной стороне n, шт:";
            // 
            // numericUpDownDisksPerSide
            // 
            numericUpDownDisksPerSide.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numericUpDownDisksPerSide.Location = new Point(265, 8);
            numericUpDownDisksPerSide.Margin = new Padding(3, 3, 5, 3);
            numericUpDownDisksPerSide.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            numericUpDownDisksPerSide.Name = "numericUpDownDisksPerSide";
            numericUpDownDisksPerSide.Size = new Size(120, 23);
            numericUpDownDisksPerSide.TabIndex = 2;
            numericUpDownDisksPerSide.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // disksPanel
            // 
            disksPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            disksPanel.Controls.Add(disksScrollPanel);
            disksPanel.Location = new Point(0, 37);
            disksPanel.Margin = new Padding(0, 3, 0, 0);
            disksPanel.Name = "disksPanel";
            disksPanel.Size = new Size(390, 165);
            disksPanel.TabIndex = 6;
            // 
            // disksScrollPanel
            // 
            disksScrollPanel.AutoScroll = true;
            disksScrollPanel.AutoSize = true;
            disksScrollPanel.Controls.Add(tableLayoutDisksPanel);
            disksScrollPanel.Dock = DockStyle.Fill;
            disksScrollPanel.Location = new Point(0, 0);
            disksScrollPanel.Name = "disksScrollPanel";
            disksScrollPanel.Size = new Size(390, 165);
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
            tableLayoutDisksPanel.Size = new Size(390, 0);
            tableLayoutDisksPanel.TabIndex = 4;
            // 
            // DisksControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(disksPanel);
            Controls.Add(label6);
            Controls.Add(numericUpDownDisksPerSide);
            Name = "DisksControl";
            Size = new Size(390, 202);
            ((System.ComponentModel.ISupportInitialize)numericUpDownDisksPerSide).EndInit();
            disksPanel.ResumeLayout(false);
            disksPanel.PerformLayout();
            disksScrollPanel.ResumeLayout(false);
            disksScrollPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label6;
        private NumericUpDown numericUpDownDisksPerSide;
        private Panel disksPanel;
        private Panel disksScrollPanel;
        private TableLayoutPanel tableLayoutDisksPanel;
    }
}
