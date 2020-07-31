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
            // btYes
            // 
            this.btYes.Text = "Yes";
            this.btYes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btYes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btYes.Location = new System.Drawing.Point(0, 136);
            this.btYes.Name = "btYes";
            this.btYes.Size = new System.Drawing.Size(370, 25);
            this.btYes.TabIndex = 0;
            this.btYes.DrawImage = true;
            this.btYes.UseVisualStyleBackColor = true;
            this.btYes.Visible = false;
            this.btYes.Click += new System.EventHandler(this.btYes_Click);
            // 
            // btCancel
            // 
            this.btCancel.Text = "Cancel";
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btCancel.Location = new System.Drawing.Point(0, 161);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(370, 25);
            this.btCancel.TabIndex = 0;
            this.btCancel.DrawImage = true;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Visible = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btNo
            // 
            this.btNo.Text = "No";
            this.btNo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btNo.Location = new System.Drawing.Point(0, 186);
            this.btNo.Name = "btNo";
            this.btNo.Size = new System.Drawing.Size(370, 25);
            this.btNo.TabIndex = 0;
            this.btNo.DrawImage = true;
            this.btNo.UseVisualStyleBackColor = true;
            this.btNo.Visible = false;
            this.btNo.Click += new System.EventHandler(this.btNo_Click);
            // 
            // btOK
            // 
            this.btOK.Text = "OK";
            this.btOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btOK.Location = new System.Drawing.Point(0, 111);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(370, 25);
            this.btOK.TabIndex = 1;
            this.btOK.DrawImage = true;
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Visible = false;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btAbort
            // 
            this.btAbort.Text = "Abort";
            this.btAbort.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAbort.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btAbort.Location = new System.Drawing.Point(0, 86);
            this.btAbort.Name = "btAbort";
            this.btAbort.Size = new System.Drawing.Size(370, 25);
            this.btAbort.TabIndex = 2;
            this.btAbort.DrawImage = true;
            this.btAbort.UseVisualStyleBackColor = true;
            this.btAbort.Visible = false;
            this.btAbort.Click += new System.EventHandler(this.btAbort_Click);
            // 
            // btRetry
            // 
            this.btRetry.Text = "Retry";
            this.btRetry.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btRetry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRetry.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btRetry.Location = new System.Drawing.Point(0, 61);
            this.btRetry.Name = "btRetry";
            this.btRetry.Size = new System.Drawing.Size(370, 25);
            this.btRetry.TabIndex = 3;
            this.btRetry.DrawImage = true;
            this.btRetry.UseVisualStyleBackColor = true;
            this.btRetry.Visible = false;
            this.btRetry.Click += new System.EventHandler(this.btRetry_Click);
            // 
            // btIgnore
            // 
            this.btIgnore.Text = "Ignore";
            this.btIgnore.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btIgnore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btIgnore.Location = new System.Drawing.Point(0, 36);
            this.btIgnore.Name = "btIgnore";
            this.btIgnore.Size = new System.Drawing.Size(370, 25);
            this.btIgnore.TabIndex = 4;
            this.btIgnore.DrawImage = true;
            this.btIgnore.UseVisualStyleBackColor = true;
            this.btIgnore.Visible = false;
            this.btIgnore.Click += new System.EventHandler(this.btIgnore_Click);
            // 
            // HTMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(370, 211);
            this.Controls.Add(this.btIgnore);
            this.Controls.Add(this.btRetry);
            this.Controls.Add(this.btAbort);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btYes);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btNo);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(386, 50);
            this.Name = "HTMsgBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "title";
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
    }
}