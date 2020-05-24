namespace HTAlt.WinForms.Example
{
    partial class TabInnerForm
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
            this.noshitsherlock = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // noshitsherlock
            // 
            this.noshitsherlock.AutoSize = true;
            this.noshitsherlock.Location = new System.Drawing.Point(13, 27);
            this.noshitsherlock.Name = "noshitsherlock";
            this.noshitsherlock.Size = new System.Drawing.Size(67, 13);
            this.noshitsherlock.TabIndex = 0;
            this.noshitsherlock.Text = "This is a tab.";
            this.noshitsherlock.Click += new System.EventHandler(this.label1_Click);
            // 
            // TabInnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.noshitsherlock);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TabInnerForm";
            this.Text = "TabInnerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label noshitsherlock;
    }
}