//MIT License
//
//Copyright (c) 2020-2021 Eren "Haltroy" Kanat
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
namespace HTAlt.WinForms
{
    partial class HTInputBox
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HTInputBox));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btRetry = new HTAlt.WinForms.HTButton();
            this.btCancel = new HTAlt.WinForms.HTButton();
            this.btOK = new HTAlt.WinForms.HTButton();
            this.btYes = new HTAlt.WinForms.HTButton();
            this.btNo = new HTAlt.WinForms.HTButton();
            this.btAbort = new HTAlt.WinForms.HTButton();
            this.btIgnore = new HTAlt.WinForms.HTButton();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.btDefault = new HTAlt.WinForms.HTButton();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "description";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(12, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(321, 20);
            this.textBox1.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btRetry);
            this.flowLayoutPanel1.Controls.Add(this.btCancel);
            this.flowLayoutPanel1.Controls.Add(this.btOK);
            this.flowLayoutPanel1.Controls.Add(this.btYes);
            this.flowLayoutPanel1.Controls.Add(this.btNo);
            this.flowLayoutPanel1.Controls.Add(this.btAbort);
            this.flowLayoutPanel1.Controls.Add(this.btIgnore);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 114);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(275, 52);
            this.flowLayoutPanel1.TabIndex = 10;
            // 
            // btRetry
            // 
            this.btRetry.AutoColor = true;
            this.btRetry.ButtonImage = null;
            this.btRetry.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btRetry.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btRetry.DrawImage = true;
            this.btRetry.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(215)))));
            this.btRetry.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btRetry.Location = new System.Drawing.Point(227, 3);
            this.btRetry.Name = "btRetry";
            this.btRetry.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btRetry.Size = new System.Drawing.Size(45, 20);
            this.btRetry.TabIndex = 9;
            this.btRetry.Text = "Retry";
            this.btRetry.Click += new System.EventHandler(this.btRetry_Click);
            // 
            // btCancel
            // 
            this.btCancel.AutoColor = true;
            this.btCancel.ButtonImage = null;
            this.btCancel.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btCancel.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btCancel.DrawImage = true;
            this.btCancel.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(215)))));
            this.btCancel.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btCancel.Location = new System.Drawing.Point(164, 3);
            this.btCancel.Name = "btCancel";
            this.btCancel.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btCancel.Size = new System.Drawing.Size(57, 20);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Cancel";
            this.btCancel.Click += new System.EventHandler(this.Button2_Click);
            // 
            // btOK
            // 
            this.btOK.AutoColor = true;
            this.btOK.ButtonImage = null;
            this.btOK.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btOK.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btOK.DrawImage = true;
            this.btOK.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(215)))));
            this.btOK.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btOK.Location = new System.Drawing.Point(128, 3);
            this.btOK.Name = "btOK";
            this.btOK.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btOK.Size = new System.Drawing.Size(30, 20);
            this.btOK.TabIndex = 3;
            this.btOK.Text = "OK";
            this.btOK.Click += new System.EventHandler(this.Button1_Click);
            // 
            // btYes
            // 
            this.btYes.AutoColor = true;
            this.btYes.ButtonImage = null;
            this.btYes.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btYes.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btYes.DrawImage = true;
            this.btYes.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(215)))));
            this.btYes.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btYes.Location = new System.Drawing.Point(87, 3);
            this.btYes.Name = "btYes";
            this.btYes.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btYes.Size = new System.Drawing.Size(35, 20);
            this.btYes.TabIndex = 5;
            this.btYes.Text = "Yes";
            this.btYes.Click += new System.EventHandler(this.btYes_Click);
            // 
            // btNo
            // 
            this.btNo.AutoColor = true;
            this.btNo.ButtonImage = null;
            this.btNo.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btNo.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btNo.DrawImage = true;
            this.btNo.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(215)))));
            this.btNo.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btNo.Location = new System.Drawing.Point(54, 3);
            this.btNo.Name = "btNo";
            this.btNo.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btNo.Size = new System.Drawing.Size(27, 20);
            this.btNo.TabIndex = 6;
            this.btNo.Text = "No";
            this.btNo.Click += new System.EventHandler(this.btNo_Click);
            // 
            // btAbort
            // 
            this.btAbort.AutoColor = true;
            this.btAbort.ButtonImage = null;
            this.btAbort.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btAbort.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btAbort.DrawImage = true;
            this.btAbort.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(215)))));
            this.btAbort.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btAbort.Location = new System.Drawing.Point(3, 3);
            this.btAbort.Name = "btAbort";
            this.btAbort.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btAbort.Size = new System.Drawing.Size(45, 20);
            this.btAbort.TabIndex = 7;
            this.btAbort.Text = "Abort";
            this.btAbort.Click += new System.EventHandler(this.btAbort_Click);
            // 
            // btIgnore
            // 
            this.btIgnore.AutoColor = true;
            this.btIgnore.ButtonImage = null;
            this.btIgnore.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btIgnore.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btIgnore.DrawImage = true;
            this.btIgnore.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(215)))));
            this.btIgnore.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btIgnore.Location = new System.Drawing.Point(219, 29);
            this.btIgnore.Name = "btIgnore";
            this.btIgnore.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btIgnore.Size = new System.Drawing.Size(53, 20);
            this.btIgnore.TabIndex = 8;
            this.btIgnore.Text = "Ignore";
            this.btIgnore.Click += new System.EventHandler(this.btIgnore_Click);
            // 
            // pbImage
            // 
            this.pbImage.Location = new System.Drawing.Point(11, 10);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(32, 32);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImage.TabIndex = 11;
            this.pbImage.TabStop = false;
            // 
            // btDefault
            // 
            this.btDefault.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btDefault.AutoColor = true;
            this.btDefault.ButtonImage = null;
            this.btDefault.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btDefault.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btDefault.DrawImage = true;
            this.btDefault.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(215)))));
            this.btDefault.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btDefault.Location = new System.Drawing.Point(12, 73);
            this.btDefault.Name = "btDefault";
            this.btDefault.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.btDefault.Size = new System.Drawing.Size(321, 23);
            this.btDefault.TabIndex = 2;
            this.btDefault.Text = "Set to Default";
            this.btDefault.Click += new System.EventHandler(this.haltroyButton1_Click);
            // 
            // HTInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(344, 180);
            this.Controls.Add(this.pbImage);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btDefault);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(360, 160);
            this.Name = "HTInputBox";
            this.Text = "<title>";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected internal System.Windows.Forms.Label label1;
        protected internal HTAlt.WinForms.HTButton btOK;
        protected internal HTAlt.WinForms.HTButton btCancel;
        protected internal System.Windows.Forms.TextBox textBox1;
        protected internal System.Windows.Forms.Timer timer1;
        protected internal HTButton btDefault;
        protected internal HTButton btYes;
        protected internal HTButton btNo;
        protected internal HTButton btAbort;
        protected internal HTButton btIgnore;
        protected internal HTButton btRetry;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox pbImage;
    }
}