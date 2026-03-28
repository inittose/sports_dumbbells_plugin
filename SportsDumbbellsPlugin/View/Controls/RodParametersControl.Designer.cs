namespace SportsDumbbellsPlugin.View.Controls
{
    partial class RodParametersControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RodParametersControl));
            textBoxSeatDiameter = new TextBox();
            textBoxHandleDiameter = new TextBox();
            textBoxSeatLength = new TextBox();
            textBoxCenterLength = new TextBox();
            textBoxTotalLength = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            errorProvider = new ErrorProvider(components);
            label6 = new Label();
            label7 = new Label();
            textBoxGrooveDepth = new TextBox();
            numericUpDownGrooveCount = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownGrooveCount).BeginInit();
            SuspendLayout();
            // 
            // textBoxSeatDiameter
            // 
            textBoxSeatDiameter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxSeatDiameter.Location = new Point(245, 147);
            textBoxSeatDiameter.Margin = new Padding(3, 3, 5, 3);
            textBoxSeatDiameter.Name = "textBoxSeatDiameter";
            textBoxSeatDiameter.Size = new Size(100, 23);
            textBoxSeatDiameter.TabIndex = 19;
            // 
            // textBoxHandleDiameter
            // 
            textBoxHandleDiameter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxHandleDiameter.Location = new Point(245, 112);
            textBoxHandleDiameter.Margin = new Padding(3, 3, 5, 3);
            textBoxHandleDiameter.Name = "textBoxHandleDiameter";
            textBoxHandleDiameter.Size = new Size(100, 23);
            textBoxHandleDiameter.TabIndex = 18;
            // 
            // textBoxSeatLength
            // 
            textBoxSeatLength.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxSeatLength.Location = new Point(245, 77);
            textBoxSeatLength.Margin = new Padding(3, 3, 5, 3);
            textBoxSeatLength.Name = "textBoxSeatLength";
            textBoxSeatLength.Size = new Size(100, 23);
            textBoxSeatLength.TabIndex = 17;
            // 
            // textBoxCenterLength
            // 
            textBoxCenterLength.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxCenterLength.Location = new Point(245, 42);
            textBoxCenterLength.Margin = new Padding(3, 3, 5, 3);
            textBoxCenterLength.Name = "textBoxCenterLength";
            textBoxCenterLength.Size = new Size(100, 23);
            textBoxCenterLength.TabIndex = 16;
            // 
            // textBoxTotalLength
            // 
            textBoxTotalLength.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxTotalLength.Location = new Point(245, 7);
            textBoxTotalLength.Margin = new Padding(3, 3, 5, 3);
            textBoxTotalLength.Name = "textBoxTotalLength";
            textBoxTotalLength.ReadOnly = true;
            textBoxTotalLength.Size = new Size(100, 23);
            textBoxTotalLength.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(5, 150);
            label5.Margin = new Padding(5, 10, 10, 10);
            label5.Name = "label5";
            label5.Size = new Size(208, 15);
            label5.TabIndex = 14;
            label5.Text = "Диаметер посадочной части d2, мм:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(5, 115);
            label4.Margin = new Padding(5, 10, 10, 10);
            label4.Name = "label4";
            label4.Size = new Size(151, 15);
            label4.TabIndex = 13;
            label4.Text = "Диаметер рукояти d1, мм:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(5, 80);
            label3.Margin = new Padding(5, 10, 10, 10);
            label3.Name = "label3";
            label3.Size = new Size(185, 15);
            label3.TabIndex = 12;
            label3.Text = "Длина посадочной части l2, мм:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(5, 45);
            label2.Margin = new Padding(5, 10, 10, 10);
            label2.Name = "label2";
            label2.Size = new Size(190, 15);
            label2.TabIndex = 11;
            label2.Text = "Длина центральной части l1, мм:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(5, 10);
            label1.Margin = new Padding(5, 10, 10, 10);
            label1.Name = "label1";
            label1.Size = new Size(167, 15);
            label1.TabIndex = 10;
            label1.Text = "Общая длина стрежня L, мм:";
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            errorProvider.Icon = (Icon)resources.GetObject("errorProvider.Icon");
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(5, 185);
            label6.Margin = new Padding(5, 10, 10, 10);
            label6.Name = "label6";
            label6.Size = new Size(199, 15);
            label6.TabIndex = 20;
            label6.Text = "Количество прорезей рукояти, шт:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(5, 220);
            label7.Margin = new Padding(5, 10, 10, 10);
            label7.Name = "label7";
            label7.Size = new Size(135, 15);
            label7.TabIndex = 21;
            label7.Text = "Глубина прорезей, мм:";
            // 
            // textBoxGrooveDepth
            // 
            textBoxGrooveDepth.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxGrooveDepth.Location = new Point(245, 217);
            textBoxGrooveDepth.Margin = new Padding(3, 3, 5, 3);
            textBoxGrooveDepth.Name = "textBoxGrooveDepth";
            textBoxGrooveDepth.Size = new Size(100, 23);
            textBoxGrooveDepth.TabIndex = 23;
            // 
            // numericUpDownGrooveCount
            // 
            numericUpDownGrooveCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numericUpDownGrooveCount.Location = new Point(245, 183);
            numericUpDownGrooveCount.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            numericUpDownGrooveCount.Name = "numericUpDownGrooveCount";
            numericUpDownGrooveCount.Size = new Size(100, 23);
            numericUpDownGrooveCount.TabIndex = 24;
            // 
            // RodParametersControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(numericUpDownGrooveCount);
            Controls.Add(textBoxGrooveDepth);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(textBoxSeatDiameter);
            Controls.Add(textBoxHandleDiameter);
            Controls.Add(textBoxSeatLength);
            Controls.Add(textBoxCenterLength);
            Controls.Add(textBoxTotalLength);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "RodParametersControl";
            Size = new Size(380, 254);
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownGrooveCount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxSeatDiameter;
        private TextBox textBoxHandleDiameter;
        private TextBox textBoxSeatLength;
        private TextBox textBoxCenterLength;
        private TextBox textBoxTotalLength;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private ErrorProvider errorProvider;
        private NumericUpDown numericUpDownGrooveCount;
        private TextBox textBoxGrooveDepth;
        private Label label7;
        private Label label6;
    }
}
