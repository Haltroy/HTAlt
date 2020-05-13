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
namespace HTAlt
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
            this.btYes = new HTAlt.HTButton();
            this.btCancel = new HTAlt.HTButton();
            this.btNo = new HTAlt.HTButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(346, 19);
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
            this.btYes.ButtonText = "Yes";
            this.btYes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btYes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btYes.Location = new System.Drawing.Point(0, 31);
            this.btYes.Name = "btYes";
            this.btYes.Size = new System.Drawing.Size(370, 25);
            this.btYes.TabIndex = 0;
            this.btYes.TextImageRelation = HTAlt.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.btYes.UseVisualStyleBackColor = true;
            this.btYes.Visible = false;
            this.btYes.Click += new System.EventHandler(this.btYes_Click);
            // 
            // btCancel
            // 
            this.btCancel.ButtonText = "Cancel";
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btCancel.Location = new System.Drawing.Point(0, 56);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(370, 25);
            this.btCancel.TabIndex = 0;
            this.btCancel.TextImageRelation = HTAlt.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Visible = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btNo
            // 
            this.btNo.ButtonText = "No";
            this.btNo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btNo.Location = new System.Drawing.Point(0, 81);
            this.btNo.Name = "btNo";
            this.btNo.Size = new System.Drawing.Size(370, 25);
            this.btNo.TabIndex = 0;
            this.btNo.TextImageRelation = HTAlt.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.btNo.UseVisualStyleBackColor = true;
            this.btNo.Visible = false;
            this.btNo.Click += new System.EventHandler(this.btNo_Click);
            // 
            // HTMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(370, 106);
            this.Controls.Add(this.btYes);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btNo);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(386, 145);
            this.Name = "HTMsgBox";
            this.Text = "title";
            this.Load += new System.EventHandler(this.msgkts_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal HTAlt.HTButton btNo;
        internal HTAlt.HTButton btCancel;
        internal HTAlt.HTButton btYes;
        protected internal System.Windows.Forms.Timer timer1;
    }
}