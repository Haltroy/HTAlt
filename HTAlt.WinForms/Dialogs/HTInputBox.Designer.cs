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
            this.btDefault = new HTAlt.WinForms.HTButton();
            this.btOK = new HTAlt.WinForms.HTButton();
            this.btCancel = new HTAlt.WinForms.HTButton();
            this.btYes = new HTAlt.WinForms.HTButton();
            this.btNo = new HTAlt.WinForms.HTButton();
            this.btAbort = new HTAlt.WinForms.HTButton();
            this.btIgnore = new HTAlt.WinForms.HTButton();
            this.btRetry = new HTAlt.WinForms.HTButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "description";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Location = new System.Drawing.Point(0, 34);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(384, 20);
            this.textBox1.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btDefault
            // 
            this.btDefault.ButtonText = "Set to Default";
            this.btDefault.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btDefault.FlatAppearance.BorderSize = 0;
            this.btDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDefault.Location = new System.Drawing.Point(0, 54);
            this.btDefault.Name = "btDefault";
            this.btDefault.Size = new System.Drawing.Size(384, 23);
            this.btDefault.TabIndex = 2;
            this.btDefault.TextImageRelation = HTAlt.WinForms.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.btDefault.UseVisualStyleBackColor = true;
            this.btDefault.Click += new System.EventHandler(this.haltroyButton1_Click);
            // 
            // btOK
            // 
            this.btOK.ButtonText = "OK";
            this.btOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btOK.FlatAppearance.BorderSize = 0;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btOK.Location = new System.Drawing.Point(0, 192);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(384, 23);
            this.btOK.TabIndex = 3;
            this.btOK.TextImageRelation = HTAlt.WinForms.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.Button1_Click);
            // 
            // btCancel
            // 
            this.btCancel.ButtonText = "Cancel";
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btCancel.FlatAppearance.BorderSize = 0;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancel.Location = new System.Drawing.Point(0, 215);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(384, 23);
            this.btCancel.TabIndex = 4;
            this.btCancel.TextImageRelation = HTAlt.WinForms.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.Button2_Click);
            // 
            // btYes
            // 
            this.btYes.ButtonText = "Yes";
            this.btYes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btYes.FlatAppearance.BorderSize = 0;
            this.btYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btYes.Location = new System.Drawing.Point(0, 169);
            this.btYes.Name = "btYes";
            this.btYes.Size = new System.Drawing.Size(384, 23);
            this.btYes.TabIndex = 5;
            this.btYes.TextImageRelation = HTAlt.WinForms.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.btYes.UseVisualStyleBackColor = true;
            this.btYes.Click += new System.EventHandler(this.btYes_Click);
            // 
            // btNo
            // 
            this.btNo.ButtonText = "No";
            this.btNo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btNo.FlatAppearance.BorderSize = 0;
            this.btNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNo.Location = new System.Drawing.Point(0, 146);
            this.btNo.Name = "btNo";
            this.btNo.Size = new System.Drawing.Size(384, 23);
            this.btNo.TabIndex = 6;
            this.btNo.TextImageRelation = HTAlt.WinForms.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.btNo.UseVisualStyleBackColor = true;
            this.btNo.Click += new System.EventHandler(this.btNo_Click);
            // 
            // btAbort
            // 
            this.btAbort.ButtonText = "Abort";
            this.btAbort.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btAbort.FlatAppearance.BorderSize = 0;
            this.btAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAbort.Location = new System.Drawing.Point(0, 123);
            this.btAbort.Name = "btAbort";
            this.btAbort.Size = new System.Drawing.Size(384, 23);
            this.btAbort.TabIndex = 7;
            this.btAbort.TextImageRelation = HTAlt.WinForms.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.btAbort.UseVisualStyleBackColor = true;
            this.btAbort.Click += new System.EventHandler(this.btAbort_Click);
            // 
            // btIgnore
            // 
            this.btIgnore.ButtonText = "Ignore";
            this.btIgnore.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btIgnore.FlatAppearance.BorderSize = 0;
            this.btIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btIgnore.Location = new System.Drawing.Point(0, 100);
            this.btIgnore.Name = "btIgnore";
            this.btIgnore.Size = new System.Drawing.Size(384, 23);
            this.btIgnore.TabIndex = 8;
            this.btIgnore.TextImageRelation = HTAlt.WinForms.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.btIgnore.UseVisualStyleBackColor = true;
            this.btIgnore.Click += new System.EventHandler(this.btIgnore_Click);
            // 
            // btRetry
            // 
            this.btRetry.ButtonText = "Retry";
            this.btRetry.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btRetry.FlatAppearance.BorderSize = 0;
            this.btRetry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRetry.Location = new System.Drawing.Point(0, 77);
            this.btRetry.Name = "btRetry";
            this.btRetry.Size = new System.Drawing.Size(384, 23);
            this.btRetry.TabIndex = 9;
            this.btRetry.TextImageRelation = HTAlt.WinForms.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.btRetry.UseVisualStyleBackColor = true;
            this.btRetry.Click += new System.EventHandler(this.btRetry_Click);
            // 
            // HTInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 238);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btDefault);
            this.Controls.Add(this.btRetry);
            this.Controls.Add(this.btIgnore);
            this.Controls.Add(this.btAbort);
            this.Controls.Add(this.btNo);
            this.Controls.Add(this.btYes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 155);
            this.Name = "HTInputBox";
            this.Text = "<title>";
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
    }
}