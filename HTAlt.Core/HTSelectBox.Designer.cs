namespace HTAlt
{
    partial class HTSelectBox
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
            this.htListView1 = new HTAlt.HTListView();
            this.label1 = new System.Windows.Forms.Label();
            this.htButton1 = new HTAlt.HTButton();
            this.htButton2 = new HTAlt.HTButton();
            this.SuspendLayout();
            // 
            // htListView1
            // 
            this.htListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.htListView1.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.htListView1.HeaderForeColor = System.Drawing.Color.Black;
            this.htListView1.HideSelection = false;
            this.htListView1.Location = new System.Drawing.Point(0, 67);
            this.htListView1.Name = "htListView1";
            this.htListView1.OverlayColor = System.Drawing.Color.DodgerBlue;
            this.htListView1.OwnerDraw = true;
            this.htListView1.Size = new System.Drawing.Size(430, 203);
            this.htListView1.TabIndex = 0;
            this.htListView1.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(0, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(430, 61);
            this.label1.TabIndex = 1;
            this.label1.Text = "message";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // htButton1
            // 
            this.htButton1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.htButton1.Location = new System.Drawing.Point(0, 299);
            this.htButton1.Name = "htButton1";
            this.htButton1.Size = new System.Drawing.Size(430, 23);
            this.htButton1.TabIndex = 2;
            this.htButton1.Text = "OK";
            this.htButton1.TextImageRelation = HTAlt.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.htButton1.UseVisualStyleBackColor = true;
            // 
            // htButton2
            // 
            this.htButton2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.htButton2.Location = new System.Drawing.Point(0, 276);
            this.htButton2.Name = "htButton2";
            this.htButton2.Size = new System.Drawing.Size(430, 23);
            this.htButton2.TabIndex = 3;
            this.htButton2.Text = "Reset to default";
            this.htButton2.TextImageRelation = HTAlt.HTButton.ButtonTextImageRelation.TextBelowImage;
            this.htButton2.UseVisualStyleBackColor = true;
            // 
            // HTSelectBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 322);
            this.Controls.Add(this.htButton2);
            this.Controls.Add(this.htButton1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.htListView1);
            this.Name = "HTSelectBox";
            this.Text = "HTSelectBox";
            this.ResumeLayout(false);

        }

        #endregion

        private HTListView htListView1;
        private System.Windows.Forms.Label label1;
        private HTButton htButton1;
        private HTButton htButton2;
    }
}