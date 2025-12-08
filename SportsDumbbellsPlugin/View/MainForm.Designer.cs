using SportsDumbbellsPlugin.View.Controls;

namespace SportsDumbbellsPlugin.View
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
            tabPageRod = new TabPage();
            button1 = new Button();
            rodParametersControl = new RodParametersControl();
            button2 = new Button();
            tabPageDisks = new TabPage();
            disksControl = new DisksControl();
            button4 = new Button();
            button3 = new Button();
            buttonDesign = new Button();
            tabControl.SuspendLayout();
            tabPageRod.SuspendLayout();
            tabPageDisks.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Controls.Add(tabPageRod);
            tabControl.Controls.Add(tabPageDisks);
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.Location = new Point(12, 12);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.ShowToolTips = true;
            tabControl.Size = new Size(425, 258);
            tabControl.TabIndex = 0;
            tabControl.DrawItem += tabControl_DrawItem;
            // 
            // tabPageRod
            // 
            tabPageRod.BackColor = Color.Transparent;
            tabPageRod.Controls.Add(button1);
            tabPageRod.Controls.Add(rodParametersControl);
            tabPageRod.Controls.Add(button2);
            tabPageRod.Location = new Point(4, 24);
            tabPageRod.Name = "tabPageRod";
            tabPageRod.Padding = new Padding(3);
            tabPageRod.Size = new Size(417, 230);
            tabPageRod.TabIndex = 0;
            tabPageRod.Text = "Стержень";
            tabPageRod.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(6, 8);
            button1.Name = "button1";
            button1.Size = new Size(226, 23);
            button1.TabIndex = 12;
            button1.Text = "Сбросить до значений по умолчанию";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // rodParametersControl
            // 
            rodParametersControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            rodParametersControl.Location = new Point(3, 36);
            rodParametersControl.Name = "rodParametersControl";
            rodParametersControl.Size = new Size(415, 173);
            rodParametersControl.TabIndex = 11;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Location = new Point(237, 8);
            button2.Margin = new Padding(3, 5, 3, 3);
            button2.Name = "button2";
            button2.Size = new Size(174, 23);
            button2.TabIndex = 10;
            button2.Text = "Показать эскиз с размерами";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // tabPageDisks
            // 
            tabPageDisks.Controls.Add(disksControl);
            tabPageDisks.Controls.Add(button4);
            tabPageDisks.Controls.Add(button3);
            tabPageDisks.Location = new Point(4, 24);
            tabPageDisks.Name = "tabPageDisks";
            tabPageDisks.Padding = new Padding(3);
            tabPageDisks.Size = new Size(417, 230);
            tabPageDisks.TabIndex = 1;
            tabPageDisks.Text = "Диски";
            tabPageDisks.UseVisualStyleBackColor = true;
            // 
            // disksControl
            // 
            disksControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            disksControl.DisksPerSide = 1;
            disksControl.Location = new Point(0, 37);
            disksControl.Margin = new Padding(0);
            disksControl.Name = "disksControl";
            disksControl.Size = new Size(417, 193);
            disksControl.TabIndex = 14;
            // 
            // button4
            // 
            button4.Location = new Point(6, 8);
            button4.Name = "button4";
            button4.Size = new Size(226, 23);
            button4.TabIndex = 13;
            button4.Text = "Сбросить до значений по умолчанию";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.Location = new Point(237, 8);
            button3.Margin = new Padding(3, 5, 3, 3);
            button3.Name = "button3";
            button3.Size = new Size(174, 23);
            button3.TabIndex = 3;
            button3.Text = "Показать эскиз с размерами";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // buttonDesign
            // 
            buttonDesign.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDesign.Location = new Point(319, 276);
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
            ClientSize = new Size(454, 311);
            Controls.Add(buttonDesign);
            Controls.Add(tabControl);
            MinimumSize = new Size(470, 350);
            Name = "MainForm";
            Text = "Плагин для создания спортивных гантель";
            tabControl.ResumeLayout(false);
            tabPageRod.ResumeLayout(false);
            tabPageDisks.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabPageRod;
        private TabPage tabPageDisks;
        private Button buttonDesign;
        private Button button2;
        private Button button3;
        private RodParametersControl rodParametersControl;
        private Button button1;
        private Button button4;
        private DisksControl disksControl;
    }
}
