//MIT License
//
//Copyright (c) 2020 Eren "Haltroy" Kanat
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
    partial class HTProgressBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HTProgressBox));
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.htProgressBar1 = new HTAlt.WinForms.HTProgressBar();
            this.btAbort = new HTAlt.WinForms.HTButton();
            this.btIgnore = new HTAlt.WinForms.HTButton();
            this.btRetry = new HTAlt.WinForms.HTButton();
            this.btOK = new HTAlt.WinForms.HTButton();
            this.btYes = new HTAlt.WinForms.HTButton();
            this.btCancel = new HTAlt.WinForms.HTButton();
            this.btNo = new HTAlt.WinForms.HTButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "message";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // htProgressBar1
            // 
            this.htProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.htProgressBar1.BorderThickness = 2;
            this.htProgressBar1.DrawBorder = true;
            this.htProgressBar1.Location = new System.Drawing.Point(15, 36);
            this.htProgressBar1.Name = "htProgressBar1";
            this.htProgressBar1.Size = new System.Drawing.Size(335, 20);
            this.htProgressBar1.TabIndex = 3;
            this.htProgressBar1.Text = "htProgressBar1";
            // 
            // btAbort
            // 
            this.btAbort.AutoSize = true;
            this.btAbort.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btAbort.DrawImage = true;
            this.btAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAbort.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btAbort.Location = new System.Drawing.Point(233, 3);
            this.btAbort.Name = "btAbort";
            this.btAbort.Size = new System.Drawing.Size(44, 25);
            this.btAbort.TabIndex = 2;
            this.btAbort.Text = "Abort";
            this.btAbort.UseVisualStyleBackColor = true;
            this.btAbort.Visible = false;
            this.btAbort.Click += new System.EventHandler(this.btAbort_Click);
            // 
            // btIgnore
            // 
            this.btIgnore.AutoSize = true;
            this.btIgnore.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btIgnore.DrawImage = true;
            this.btIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btIgnore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btIgnore.Location = new System.Drawing.Point(283, 3);
            this.btIgnore.Name = "btIgnore";
            this.btIgnore.Size = new System.Drawing.Size(49, 25);
            this.btIgnore.TabIndex = 10;
            this.btIgnore.Text = "Ignore";
            this.btIgnore.UseVisualStyleBackColor = true;
            this.btIgnore.Visible = false;
            this.btIgnore.Click += new System.EventHandler(this.btIgnore_Click);
            // 
            // btRetry
            // 
            this.btRetry.AutoSize = true;
            this.btRetry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btRetry.DrawImage = true;
            this.btRetry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRetry.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btRetry.Location = new System.Drawing.Point(3, 3);
            this.btRetry.Name = "btRetry";
            this.btRetry.Size = new System.Drawing.Size(44, 25);
            this.btRetry.TabIndex = 9;
            this.btRetry.Text = "Retry";
            this.btRetry.UseVisualStyleBackColor = true;
            this.btRetry.Visible = false;
            this.btRetry.Click += new System.EventHandler(this.btRetry_Click);
            // 
            // btOK
            // 
            this.btOK.AutoSize = true;
            this.btOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btOK.DrawImage = true;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btOK.Location = new System.Drawing.Point(53, 3);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(34, 25);
            this.btOK.TabIndex = 8;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Visible = false;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btYes
            // 
            this.btYes.AutoSize = true;
            this.btYes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btYes.DrawImage = true;
            this.btYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btYes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btYes.Location = new System.Drawing.Point(93, 3);
            this.btYes.Name = "btYes";
            this.btYes.Size = new System.Drawing.Size(37, 25);
            this.btYes.TabIndex = 5;
            this.btYes.Text = "Yes";
            this.btYes.UseVisualStyleBackColor = true;
            this.btYes.Visible = false;
            this.btYes.Click += new System.EventHandler(this.btYes_Click);
            // 
            // btCancel
            // 
            this.btCancel.AutoSize = true;
            this.btCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.DrawImage = true;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btCancel.Location = new System.Drawing.Point(136, 3);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(52, 25);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Visible = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btNo
            // 
            this.btNo.AutoSize = true;
            this.btNo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btNo.DrawImage = true;
            this.btNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btNo.Location = new System.Drawing.Point(194, 3);
            this.btNo.Name = "btNo";
            this.btNo.Size = new System.Drawing.Size(33, 25);
            this.btNo.TabIndex = 7;
            this.btNo.Text = "No";
            this.btNo.UseVisualStyleBackColor = true;
            this.btNo.Visible = false;
            this.btNo.Click += new System.EventHandler(this.btNo_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btIgnore);
            this.flowLayoutPanel1.Controls.Add(this.btAbort);
            this.flowLayoutPanel1.Controls.Add(this.btNo);
            this.flowLayoutPanel1.Controls.Add(this.btCancel);
            this.flowLayoutPanel1.Controls.Add(this.btYes);
            this.flowLayoutPanel1.Controls.Add(this.btOK);
            this.flowLayoutPanel1.Controls.Add(this.btRetry);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(15, 71);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(335, 31);
            this.flowLayoutPanel1.TabIndex = 11;
            // 
            // HTProgressBox
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(370, 114);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.htProgressBar1);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(386, 50);
            this.Name = "HTProgressBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "title";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        protected internal System.Windows.Forms.Timer timer1;
        internal HTButton btAbort;
        private HTProgressBar htProgressBar1;
        internal HTButton btIgnore;
        internal HTButton btRetry;
        internal HTButton btOK;
        internal HTButton btYes;
        internal HTButton btCancel;
        internal HTButton btNo;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}