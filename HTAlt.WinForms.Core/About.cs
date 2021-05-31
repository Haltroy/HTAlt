/*

Copyright © 2021 Eren "Haltroy" Kanat

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/HTAlt/blob/master/LICENSE

*/

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
            pictureBox1 = new System.Windows.Forms.PictureBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            htSwitch1 = new HTAlt.WinForms.HTSwitch();
            textBox1 = new System.Windows.Forms.TextBox();
            HTInfo info = new HTInfo();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            SuspendLayout();
            //
            // About
            //
            Size = new System.Drawing.Size(690, 285);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Icon = Properties.Resources.HTAlt;
            Name = "About";
            Text = "About HTAlt";
            //
            // pictureBox1
            //
            pictureBox1.Image = global::HTAlt.WinForms.Properties.Resources.logo;
            pictureBox1.Location = new System.Drawing.Point(10, 10);
            pictureBox1.Size = new System.Drawing.Size(150, 150);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            //
            // label1
            //
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            label1.Location = new System.Drawing.Point(pictureBox1.Location.X + pictureBox1.Width + 5, pictureBox1.Location.Y);
            label1.Text = "HTAlt.WinForms";
            label1.AutoSize = true;
            //
            // label2
            //
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F);
            label2.Location = new System.Drawing.Point(label1.Location.X, label1.Location.Y + label1.Height + 20);
            label2.Text = info.ProjectVersion.ToString() + " [" + info.ProjectCodeName + "] (" + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription + ")";
            label2.AutoSize = true;
            //
            // label3
            //
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            label3.Location = new System.Drawing.Point(label1.Location.X, label2.Location.Y + label2.Height);
            label3.Text = "Haltroy";
            label3.AutoSize = true;
            //
            // label5
            //
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            label5.Location = new System.Drawing.Point(label1.Location.X, label3.Location.Y + label3.Height + 10);
            label5.Text = "Loaded modules:";
            label5.AutoSize = true;
            //
            // textBox1
            //
            textBox1.Location = new System.Drawing.Point(label1.Location.X, label5.Location.Y + label5.Height + 5);
            textBox1.Multiline = true;
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(500, 100);
            textBox1.Text += "HTAlt.Standart : " + info.ProjectVersion.ToString() + " [" + info.ProjectCodeName + "]" + Environment.NewLine;
            //
            // label6
            //
            label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            label6.Location = new System.Drawing.Point(pictureBox1.Location.X + 5, pictureBox1.Location.Y + pictureBox1.Height + 15);
            label6.Text = "Dark Mode:";
            label6.AutoSize = true;
            //
            // htSwitch1
            //
            htSwitch1.Size = new System.Drawing.Size(50, 20);
            htSwitch1.Location = new System.Drawing.Point((pictureBox1.Location.X + pictureBox1.Width) - (htSwitch1.Width + 10), label6.Location.Y - 1);
            htSwitch1.CheckedChanged += new HTSwitch.CheckedChangedDelegate(DarkMode_CheckedChanged);
            //
            // About
            //
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(textBox1);
            Controls.Add(htSwitch1);
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            ResumeLayout(false);
            PerformLayout();
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

        private readonly System.Windows.Forms.PictureBox pictureBox1;
        private readonly System.Windows.Forms.Label label1;
        private readonly System.Windows.Forms.Label label2;
        private readonly System.Windows.Forms.Label label3;
        private readonly System.Windows.Forms.Label label5;
        private readonly System.Windows.Forms.Label label6;
        private readonly HTSwitch htSwitch1;
        private readonly System.Windows.Forms.TextBox textBox1;
    }
}