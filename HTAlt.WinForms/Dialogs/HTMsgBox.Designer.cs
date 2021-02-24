/*

Copyright © 2021 Eren "Haltroy" Kanat

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/HTAlt/blob/master/LICENSE

*/

namespace HTAlt.WinForms
{
    partial class HTMsgBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HTMsgBox));
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btYes = new HTAlt.WinForms.HTButton();
            this.btCancel = new HTAlt.WinForms.HTButton();
            this.btNo = new HTAlt.WinForms.HTButton();
            this.btOK = new HTAlt.WinForms.HTButton();
            this.btAbort = new HTAlt.WinForms.HTButton();
            this.btRetry = new HTAlt.WinForms.HTButton();
            this.btIgnore = new HTAlt.WinForms.HTButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 9);
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
            // btYes
            // 
            this.btYes.AutoSize = true;
            this.btYes.DrawImage = true;
            this.btYes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btYes.Location = new System.Drawing.Point(198, 3);
            this.btYes.Name = "btYes";
            this.btYes.Size = new System.Drawing.Size(37, 25);
            this.btYes.TabIndex = 0;
            this.btYes.Text = "Yes";
            this.btYes.Visible = false;
            this.btYes.Click += new System.EventHandler(this.btYes_Click);
            // 
            // btCancel
            // 
            this.btCancel.AutoSize = true;
            this.btCancel.DrawImage = true;
            this.btCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btCancel.Location = new System.Drawing.Point(280, 3);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(52, 25);
            this.btCancel.TabIndex = 0;
            this.btCancel.Text = "Cancel";
            this.btCancel.Visible = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btNo
            // 
            this.btNo.AutoSize = true;
            this.btNo.DrawImage = true;
            this.btNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btNo.Location = new System.Drawing.Point(241, 3);
            this.btNo.Name = "btNo";
            this.btNo.Size = new System.Drawing.Size(33, 25);
            this.btNo.TabIndex = 0;
            this.btNo.Text = "No";
            this.btNo.Visible = false;
            this.btNo.Click += new System.EventHandler(this.btNo_Click);
            // 
            // btOK
            // 
            this.btOK.AutoSize = true;
            this.btOK.DrawImage = true;
            this.btOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btOK.Location = new System.Drawing.Point(158, 3);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(34, 25);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "OK";
            this.btOK.Visible = false;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btAbort
            // 
            this.btAbort.AutoSize = true;
            this.btAbort.DrawImage = true;
            this.btAbort.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btAbort.Location = new System.Drawing.Point(108, 3);
            this.btAbort.Name = "btAbort";
            this.btAbort.Size = new System.Drawing.Size(44, 25);
            this.btAbort.TabIndex = 2;
            this.btAbort.Text = "Abort";
            this.btAbort.Visible = false;
            this.btAbort.Click += new System.EventHandler(this.btAbort_Click);
            // 
            // btRetry
            // 
            this.btRetry.AutoSize = true;
            this.btRetry.DrawImage = true;
            this.btRetry.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btRetry.Location = new System.Drawing.Point(58, 3);
            this.btRetry.Name = "btRetry";
            this.btRetry.Size = new System.Drawing.Size(44, 25);
            this.btRetry.TabIndex = 3;
            this.btRetry.Text = "Retry";
            this.btRetry.Visible = false;
            this.btRetry.Click += new System.EventHandler(this.btRetry_Click);
            // 
            // btIgnore
            // 
            this.btIgnore.AutoSize = true;
            this.btIgnore.DrawImage = true;
            this.btIgnore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btIgnore.Location = new System.Drawing.Point(3, 3);
            this.btIgnore.Name = "btIgnore";
            this.btIgnore.Size = new System.Drawing.Size(49, 25);
            this.btIgnore.TabIndex = 4;
            this.btIgnore.Text = "Ignore";
            this.btIgnore.Visible = false;
            this.btIgnore.Click += new System.EventHandler(this.btIgnore_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btCancel);
            this.flowLayoutPanel1.Controls.Add(this.btNo);
            this.flowLayoutPanel1.Controls.Add(this.btYes);
            this.flowLayoutPanel1.Controls.Add(this.btOK);
            this.flowLayoutPanel1.Controls.Add(this.btAbort);
            this.flowLayoutPanel1.Controls.Add(this.btRetry);
            this.flowLayoutPanel1.Controls.Add(this.btIgnore);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(15, 56);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(335, 31);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // pbImage
            // 
            this.pbImage.Location = new System.Drawing.Point(15, 9);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(32, 32);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbImage.TabIndex = 6;
            this.pbImage.TabStop = false;
            // 
            // HTMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(370, 99);
            this.Controls.Add(this.pbImage);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(386, 50);
            this.Name = "HTMsgBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "title";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal HTAlt.WinForms.HTButton btNo;
        internal HTAlt.WinForms.HTButton btCancel;
        internal HTAlt.WinForms.HTButton btYes;
        protected internal System.Windows.Forms.Timer timer1;
        internal HTButton btOK;
        internal HTButton btAbort;
        internal HTButton btRetry;
        internal HTButton btIgnore;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox pbImage;
    }
}