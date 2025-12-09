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
            defaultRodButton = new Button();
            rodParametersControl = new RodParametersControl();
            informationRodButton = new Button();
            tabPageDisks = new TabPage();
            disksControl = new DisksControl();
            defaultDisksButton = new Button();
            informationDisksButton = new Button();
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
            tabPageRod.Controls.Add(defaultRodButton);
            tabPageRod.Controls.Add(rodParametersControl);
            tabPageRod.Controls.Add(informationRodButton);
            tabPageRod.Location = new Point(4, 24);
            tabPageRod.Name = "tabPageRod";
            tabPageRod.Padding = new Padding(3);
            tabPageRod.Size = new Size(417, 230);
            tabPageRod.TabIndex = 0;
            tabPageRod.Text = "Стержень";
            tabPageRod.UseVisualStyleBackColor = true;
            // 
            // defaultRodButton
            // 
            defaultRodButton.Location = new Point(6, 8);
            defaultRodButton.Name = "defaultRodButton";
            defaultRodButton.Size = new Size(226, 23);
            defaultRodButton.TabIndex = 12;
            defaultRodButton.Text = "Сбросить до значений по умолчанию";
            defaultRodButton.UseVisualStyleBackColor = true;
            defaultRodButton.Click += SetRodDefault;
            // 
            // rodParametersControl
            // 
            rodParametersControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            rodParametersControl.Location = new Point(3, 36);
            rodParametersControl.Name = "rodParametersControl";
            rodParametersControl.Size = new Size(415, 173);
            rodParametersControl.TabIndex = 11;
            // 
            // informationRodButton
            // 
            informationRodButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            informationRodButton.Location = new Point(237, 8);
            informationRodButton.Margin = new Padding(3, 5, 3, 3);
            informationRodButton.Name = "informationRodButton";
            informationRodButton.Size = new Size(174, 23);
            informationRodButton.TabIndex = 10;
            informationRodButton.Text = "Показать эскиз с размерами";
            informationRodButton.UseVisualStyleBackColor = true;
            informationRodButton.Click += ShowRodInformation;
            // 
            // tabPageDisks
            // 
            tabPageDisks.Controls.Add(disksControl);
            tabPageDisks.Controls.Add(defaultDisksButton);
            tabPageDisks.Controls.Add(informationDisksButton);
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
            // defaultDisksButton
            // 
            defaultDisksButton.Location = new Point(6, 8);
            defaultDisksButton.Name = "defaultDisksButton";
            defaultDisksButton.Size = new Size(226, 23);
            defaultDisksButton.TabIndex = 13;
            defaultDisksButton.Text = "Сбросить до значений по умолчанию";
            defaultDisksButton.UseVisualStyleBackColor = true;
            defaultDisksButton.Click += SetDisksDefault;
            // 
            // informationDisksButton
            // 
            informationDisksButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            informationDisksButton.Location = new Point(237, 8);
            informationDisksButton.Margin = new Padding(3, 5, 3, 3);
            informationDisksButton.Name = "informationDisksButton";
            informationDisksButton.Size = new Size(174, 23);
            informationDisksButton.TabIndex = 3;
            informationDisksButton.Text = "Показать эскиз с размерами";
            informationDisksButton.UseVisualStyleBackColor = true;
            informationDisksButton.Click += ShowDisksInformation;
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
        private Button informationRodButton;
        private Button informationDisksButton;
        private RodParametersControl rodParametersControl;
        private Button defaultRodButton;
        private Button defaultDisksButton;
        private DisksControl disksControl;
    }
}
