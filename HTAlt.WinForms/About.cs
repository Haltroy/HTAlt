using System;
using System.Drawing;
using System.Windows.Forms;

namespace HTAlt.WinForms
{
    public static class UI
    {
        /// <summary>
        /// Shows information about this library.
        /// </summary>
        public static void ShowAbout()
        {
            About().Show();
        }

        /// <summary>
        /// FOrm that shows information about this library.
        /// </summary>
        /// <returns><see cref="About"/></returns>
        public static Form About()
        {
            return new About();
        }
    }

    internal class About : Form
    {
        public About()
        {
            InitializeComponent();
            HTInfo info = new HTInfo();
            label2.Text = info.ProjectVersion.ToString() + " [" + info.ProjectCodeName + "]";
            textBox1.Text += "HTAlt.Standart : " + info.ProjectVersion.ToString() + " [" + info.ProjectCodeName + "]" + Environment.NewLine;
            DarkMode_CheckedChanged(this, new EventArgs());
        }

        private void DarkMode_CheckedChanged(object sender, EventArgs e)
        {
            BackColor = htSwitch1.Checked ? Color.Black : Color.White;
            ForeColor = htSwitch1.Checked ? Color.White : Color.Black;
            htSwitch1.BackColor = BackColor;
            htSwitch1.OverlayColor = HTAlt.Tools.ShiftBrightness(BackColor, 20, false);
            htSwitch1.ButtonColor = ForeColor;
            htSwitch1.ButtonHoverColor = ForeColor;
            htSwitch1.ButtonPressedColor = ForeColor;
            textBox1.BackColor = HTAlt.Tools.ShiftBrightness(BackColor, 20, false);
            textBox1.ForeColor = ForeColor;
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.IContainer components = null;

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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new System.Windows.Forms.PictureBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            htSwitch1 = new HTAlt.WinForms.HTSwitch();
            textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            SuspendLayout();
            //
            // pictureBox1
            //
            pictureBox1.Image = global::HTAlt.WinForms.Properties.Resources.logo1;
            pictureBox1.Location = new System.Drawing.Point(13, 13);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(150, 150);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            label1.Location = new System.Drawing.Point(169, 13);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(267, 39);
            label1.TabIndex = 1;
            label1.Text = "HTAlt WinForms";
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F);
            label2.Location = new System.Drawing.Point(442, 9);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(52, 20);
            label2.TabIndex = 1;
            label2.Text = "<ver>";
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            label3.Location = new System.Drawing.Point(170, 52);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(101, 31);
            label3.TabIndex = 1;
            label3.Text = "Haltroy";
            //
            // label5
            //
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            label5.Location = new System.Drawing.Point(171, 95);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(163, 25);
            label5.TabIndex = 2;
            label5.Text = "Loaded modules:";
            //
            // label6
            //
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            label6.Location = new System.Drawing.Point(12, 180);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(91, 20);
            label6.TabIndex = 2;
            label6.Text = "Dark Mode:";
            //
            // htSwitch1
            //
            htSwitch1.Location = new System.Drawing.Point(113, 181);
            htSwitch1.Name = "htSwitch1";
            htSwitch1.Size = new System.Drawing.Size(50, 19);
            htSwitch1.TabIndex = 3;
            htSwitch1.CheckedChanged += new HTSwitch.CheckedChangedDelegate(DarkMode_CheckedChanged);
            //
            // textBox1
            //
            textBox1.Location = new System.Drawing.Point(176, 124);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new System.Drawing.Size(471, 76);
            textBox1.TabIndex = 4;
            //
            // About
            //
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(659, 211);
            Controls.Add(textBox1);
            Controls.Add(htSwitch1);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Icon = Properties.Resources.logo;
            Name = "About";
            Text = "About HTAlt";
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private HTSwitch htSwitch1;
        private System.Windows.Forms.TextBox textBox1;
    }
}