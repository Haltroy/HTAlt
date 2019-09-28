namespace HaltroyFramework
{
    partial class HaltroyMsgBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HaltroyMsgBox));
            this.label1 = new System.Windows.Forms.Label();
            this.btNo = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btYes = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(346, 54);
            this.label1.TabIndex = 0;
            this.label1.Text = "message";
            // 
            // btNo
            // 
            this.btNo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btNo.Location = new System.Drawing.Point(223, 66);
            this.btNo.Name = "btNo";
            this.btNo.Size = new System.Drawing.Size(75, 23);
            this.btNo.TabIndex = 0;
            this.btNo.Text = "No";
            this.btNo.UseVisualStyleBackColor = true;
            this.btNo.Visible = false;
            this.btNo.Click += new System.EventHandler(this.btNo_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btCancel.Location = new System.Drawing.Point(142, 66);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 0;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Visible = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btYes
            // 
            this.btYes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btYes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btYes.Location = new System.Drawing.Point(61, 66);
            this.btYes.Name = "btYes";
            this.btYes.Size = new System.Drawing.Size(75, 23);
            this.btYes.TabIndex = 0;
            this.btYes.Text = "Yes";
            this.btYes.UseVisualStyleBackColor = true;
            this.btYes.Visible = false;
            this.btYes.Click += new System.EventHandler(this.btYes_Click);
            // 
            // btOK
            // 
            this.btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btOK.Location = new System.Drawing.Point(61, 66);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // HaltroyMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(370, 99);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btNo);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btYes);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(386, 138);
            this.Name = "HaltroyMsgBox";
            this.Text = "title";
            this.Load += new System.EventHandler(this.msgkts_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btNo;
        internal System.Windows.Forms.Button btCancel;
        internal System.Windows.Forms.Button btYes;
        internal System.Windows.Forms.Button btOK;
    }
}