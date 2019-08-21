namespace Hf.Test
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.haltroyTabControl1 = new HaltroyFramework.HaltroyTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.haltroyListView1 = new HaltroyFramework.HaltroyListView();
            this.haltroySlider1 = new HaltroyFramework.HaltroySlider();
            this.haltroySwitch1 = new HaltroyFramework.HaltroySwitch();
            this.haltroyTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // haltroyTabControl1
            // 
            this.haltroyTabControl1.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.haltroyTabControl1.AllowDrop = true;
            this.haltroyTabControl1.BackTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.haltroyTabControl1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.haltroyTabControl1.ClosingMessage = null;
            this.haltroyTabControl1.Controls.Add(this.tabPage1);
            this.haltroyTabControl1.Controls.Add(this.tabPage2);
            this.haltroyTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.haltroyTabControl1.DoNotRemoveThisTab1 = null;
            this.haltroyTabControl1.DoNotRemoveThisTab2 = null;
            this.haltroyTabControl1.EnableRepositioning = false;
            this.haltroyTabControl1.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.haltroyTabControl1.HorizontalLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.haltroyTabControl1.ItemSize = new System.Drawing.Size(240, 16);
            this.haltroyTabControl1.Location = new System.Drawing.Point(0, 0);
            this.haltroyTabControl1.Name = "haltroyTabControl1";
            this.haltroyTabControl1.SelectedIndex = 0;
            this.haltroyTabControl1.SelectedTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.haltroyTabControl1.ShowClosingButton = false;
            this.haltroyTabControl1.ShowClosingMessage = false;
            this.haltroyTabControl1.Size = new System.Drawing.Size(800, 450);
            this.haltroyTabControl1.TabIndex = 0;
            this.haltroyTabControl1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.haltroyListView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 20);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 426);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.haltroySwitch1);
            this.tabPage2.Controls.Add(this.haltroySlider1);
            this.tabPage2.Location = new System.Drawing.Point(4, 20);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 426);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // haltroyListView1
            // 
            this.haltroyListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.haltroyListView1.HideSelection = false;
            this.haltroyListView1.Location = new System.Drawing.Point(3, 3);
            this.haltroyListView1.Name = "haltroyListView1";
            this.haltroyListView1.Size = new System.Drawing.Size(786, 420);
            this.haltroyListView1.TabIndex = 0;
            this.haltroyListView1.UseCompatibleStateImageBehavior = false;
            // 
            // haltroySlider1
            // 
            this.haltroySlider1.BackColor = System.Drawing.Color.White;
            this.haltroySlider1.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.haltroySlider1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.haltroySlider1.ForeColor = System.Drawing.Color.Black;
            this.haltroySlider1.LargeChange = ((uint)(5u));
            this.haltroySlider1.Location = new System.Drawing.Point(50, 45);
            this.haltroySlider1.Name = "haltroySlider1";
            this.haltroySlider1.OverlayColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(157)))), ((int)(((byte)(204)))));
            this.haltroySlider1.Size = new System.Drawing.Size(200, 48);
            this.haltroySlider1.SmallChange = ((uint)(1u));
            this.haltroySlider1.TabIndex = 0;
            this.haltroySlider1.Text = "haltroySlider1";
            this.haltroySlider1.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.haltroySlider1.ThumbSize = new System.Drawing.Size(16, 16);
            // 
            // haltroySwitch1
            // 
            this.haltroySwitch1.Location = new System.Drawing.Point(50, 100);
            this.haltroySwitch1.Name = "haltroySwitch1";
            this.haltroySwitch1.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.haltroySwitch1.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.haltroySwitch1.Size = new System.Drawing.Size(50, 19);
            this.haltroySwitch1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.haltroyTabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.haltroyTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HaltroyFramework.HaltroyTabControl haltroyTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private HaltroyFramework.HaltroyListView haltroyListView1;
        private HaltroyFramework.HaltroySwitch haltroySwitch1;
        private HaltroyFramework.HaltroySlider haltroySlider1;
    }
}

