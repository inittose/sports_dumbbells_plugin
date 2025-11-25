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
            textBox5 = new TextBox();
            textBox4 = new TextBox();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            tabPage2 = new TabPage();
            button1 = new Button();
            button2 = new Button();
            tabControl.SuspendLayout();
            tabPage1.SuspendLayout();
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
            tabControl.Size = new Size(408, 258);
            tabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.AutoScroll = true;
            tabPage1.BackColor = Color.Transparent;
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(textBox5);
            tabPage1.Controls.Add(textBox4);
            tabPage1.Controls.Add(textBox3);
            tabPage1.Controls.Add(textBox2);
            tabPage1.Controls.Add(textBox1);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(400, 230);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Стержень";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            textBox5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox5.Location = new Point(292, 150);
            textBox5.Margin = new Padding(3, 3, 5, 3);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(100, 23);
            textBox5.TabIndex = 9;
            // 
            // textBox4
            // 
            textBox4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox4.Location = new Point(292, 115);
            textBox4.Margin = new Padding(3, 3, 5, 3);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(100, 23);
            textBox4.TabIndex = 8;
            // 
            // textBox3
            // 
            textBox3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox3.Location = new Point(292, 80);
            textBox3.Margin = new Padding(3, 3, 5, 3);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox2.Location = new Point(292, 45);
            textBox2.Margin = new Padding(3, 3, 5, 3);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 6;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox1.Location = new Point(292, 10);
            textBox1.Margin = new Padding(3, 3, 5, 3);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 153);
            label5.Margin = new Padding(5, 10, 10, 10);
            label5.Name = "label5";
            label5.Size = new Size(208, 15);
            label5.TabIndex = 4;
            label5.Text = "Диаметер посадочной части d2, мм:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 118);
            label4.Margin = new Padding(5, 10, 10, 10);
            label4.Name = "label4";
            label4.Size = new Size(151, 15);
            label4.TabIndex = 3;
            label4.Text = "Диаметер рукояти d1, мм:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(8, 83);
            label3.Margin = new Padding(5, 10, 10, 10);
            label3.Name = "label3";
            label3.Size = new Size(185, 15);
            label3.TabIndex = 2;
            label3.Text = "Длина посадочной части l2, мм:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 48);
            label2.Margin = new Padding(5, 10, 10, 10);
            label2.Name = "label2";
            label2.Size = new Size(190, 15);
            label2.TabIndex = 1;
            label2.Text = "Длина центральной части l1, мм:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 13);
            label1.Margin = new Padding(5, 10, 10, 10);
            label1.Name = "label1";
            label1.Size = new Size(167, 15);
            label1.TabIndex = 0;
            label1.Text = "Общая длина стрежня L, мм:";
            // 
            // tabPage2
            // 
            tabPage2.AutoScroll = true;
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(659, 544);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Диски";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.Location = new Point(302, 276);
            button1.Name = "button1";
            button1.Size = new Size(119, 23);
            button1.TabIndex = 1;
            button1.Text = "Спроектировать";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Location = new Point(207, 181);
            button2.Margin = new Padding(3, 5, 3, 3);
            button2.Name = "button2";
            button2.Size = new Size(185, 23);
            button2.TabIndex = 10;
            button2.Text = "Показать эскиз с размерами";
            button2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(437, 311);
            Controls.Add(button1);
            Controls.Add(tabControl);
            MinimumSize = new Size(400, 300);
            Name = "MainForm";
            Text = "Плагин для создания спортивных гантель";
            tabControl.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button button1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox textBox2;
        private TextBox textBox1;
        private TextBox textBox5;
        private TextBox textBox4;
        private TextBox textBox3;
        private Button button2;
    }
}
