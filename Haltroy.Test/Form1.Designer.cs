
namespace Haltroy.Test
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
            this.haltroySlider1 = new HaltroyFramework.HaltroySlider();
            this.SuspendLayout();
            // 
            // haltroySlider1
            // 
            this.haltroySlider1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.haltroySlider1.BackColor = System.Drawing.Color.White;
            this.haltroySlider1.BorderRoundRectSize = new System.Drawing.Size(8, 8);
            this.haltroySlider1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.haltroySlider1.ForeColor = System.Drawing.Color.Black;
            this.haltroySlider1.LargeChange = ((uint)(5u));
            this.haltroySlider1.Location = new System.Drawing.Point(0, 0);
            this.haltroySlider1.Name = "haltroySlider1";
            this.haltroySlider1.OverlayColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(157)))), ((int)(((byte)(204)))));
            this.haltroySlider1.Size = new System.Drawing.Size(350, 20);
            this.haltroySlider1.SmallChange = ((uint)(1u));
            this.haltroySlider1.TabIndex = 2;
            this.haltroySlider1.Text = "haltroySlider1";
            this.haltroySlider1.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.haltroySlider1.ThumbSize = new System.Drawing.Size(16, 16);
            this.haltroySlider1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HaltroySlider1_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 24);
            this.Controls.Add(this.haltroySlider1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private HaltroyFramework.HaltroySlider haltroySlider1;
    }
}

