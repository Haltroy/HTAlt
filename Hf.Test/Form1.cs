using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Hf.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog filedlg = new OpenFileDialog();
            filedlg.Title = "Select an Icon";
            filedlg.Filter = "Icon Fİle|*.ico|All Files|*.*";
            filedlg.Multiselect = false;
            if (filedlg.ShowDialog() == DialogResult.OK)
            {
                tbIcon.Text = filedlg.FileName;
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            ColorDialog colordlg = new ColorDialog();
            colordlg.AllowFullOpen = true;
            colordlg.AnyColor = true;
            if (colordlg.ShowDialog() == DialogResult.OK)
            {
                pbBackColor.BackColor = colordlg.Color;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MessageBoxButtons mesajbuton = new MessageBoxButtons();
            if (cbButtons.Text == "Nothing")
            {
                mesajbuton = MessageBoxButtons.AbortRetryIgnore;
            }else if (cbButtons.Text == "YesNo")
            {
                mesajbuton = MessageBoxButtons.YesNo;
            }
            else if (cbButtons.Text == "YesNoCancel")
            {
                mesajbuton = MessageBoxButtons.YesNoCancel;
            }
            else if (cbButtons.Text == "OK")
            {
                mesajbuton = MessageBoxButtons.OK;
            }
            else if (cbButtons.Text == "OKCancel")
            {
                mesajbuton = MessageBoxButtons.OKCancel;
            }
            Icon mesajicon = null;
            if (System.IO.File.Exists(tbIcon.Text))
            {
                mesajicon = new Icon(tbIcon.Text);
            }else
            {
                mesajicon = this.Icon;
            }
            HaltroyFramework.HaltroyMsgBox mesaj = new HaltroyFramework.HaltroyMsgBox(tbTitle.Text,
                                                                                      tbMessage.Text,
                                                                                      mesajicon,
                                                                                      mesajbuton,
                                                                                      pbBackColor.BackColor,
                                                                                      tbYes.Text,
                                                                                      tbNo.Text,
                                                                                      tbOK.Text,
                                                                                      tbCancel.Text,
                                                                                      htsWidth.Value,
                                                                                      htsHeight.Value);
            mesaj.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            MessageBoxButtons mesajbuton = new MessageBoxButtons();
            if (cbButtons.Text == "Nothing")
            {
                mesajbuton = MessageBoxButtons.AbortRetryIgnore;
            }
            else if (cbButtons.Text == "YesNo")
            {
                mesajbuton = MessageBoxButtons.YesNo;
            }
            else if (cbButtons.Text == "YesNoCancel")
            {
                mesajbuton = MessageBoxButtons.YesNoCancel;
            }
            else if (cbButtons.Text == "OK")
            {
                mesajbuton = MessageBoxButtons.OK;
            }
            else if (cbButtons.Text == "OKCancel")
            {
                mesajbuton = MessageBoxButtons.OKCancel;
            }
            Icon mesajicon = null;
            if (System.IO.File.Exists(tbIcon.Text))
            {
                mesajicon = new Icon(tbIcon.Text);
            }
            else
            {
                mesajicon = this.Icon;
            }
            HaltroyFramework.HaltroyMsgBox mesaj = new HaltroyFramework.HaltroyMsgBox(tbTitle.Text,
                                                                                      tbMessage.Text,
                                                                                      mesajicon,
                                                                                      mesajbuton,
                                                                                      pbBackColor.BackColor,
                                                                                      tbYes.Text,
                                                                                      tbNo.Text,
                                                                                      tbOK.Text,
                                                                                      tbCancel.Text); DialogResult diares = mesaj.ShowDialog();
            if (diares == DialogResult.OK)
            {
                lResult.Text = "OK - " + tbOK.Text;
            }
            else if (diares == DialogResult.Cancel)
            {
                lResult.Text = "Cancel - " + tbCancel.Text;
            }
            else if (diares == DialogResult.Yes)
            {
                lResult.Text = "Yes - " + tbYes.Text;
            }
            else if (diares == DialogResult.No)
            {
                lResult.Text = "No - " + tbNo.Text;
            }
            label10.Visible = true;
            lResult.Visible = true;
        }
        private void CbButtons_SelectedIndexChanged(object sender, EventArgs e)
        {
           // if (cbButtons.Text = "Nothing"){button2.Enabled = false;} else { button2.Enabled = true; }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Icon mesajicon = null;
            if (System.IO.File.Exists(ibIcon.Text))
            {
                mesajicon = new Icon(ibIcon.Text);
            }
            HaltroyFramework.HaltroyInputBox inputbox = new HaltroyFramework.HaltroyInputBox(ibTitle.Text,
                                                                                             ibMessage.Text,
                                                                                             mesajicon,
                                                                                             ibDefault.Text,
                                                                                             ibBackColor.BackColor,
                                                                                             ibOverlayColor.BackColor,
                                                                                             ibOK.Text,
                                                                                             ibCancel.Text,
                                                                                             ibWidth.Value,
                                                                                             ibHeight.Value);
            DialogResult diagres = inputbox.ShowDialog();
            if (diagres == DialogResult.OK)
            {
                ibResult.Text = inputbox.TextValue();
            }else
            {
                ibResult.Text = "Cancelled.";
            }
            ibResult.Visible = true;
            ibResultTitle.Visible = true;
        }

        private void İbHeight_Scroll(object sender, ScrollEventArgs e)
        {
            label20.Text = ibHeight.Value.ToString();
        }

        private void İbWidth_Scroll(object sender, ScrollEventArgs e)
        {
            label12.Text = ibWidth.Value.ToString();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Icon mesajicon = null;
            if (System.IO.File.Exists(ibIcon.Text))
            {
                mesajicon = new Icon(ibIcon.Text);
            }
            HaltroyFramework.HaltroyInputBox inputbox = new HaltroyFramework.HaltroyInputBox(ibTitle.Text,
                                                                                             ibMessage.Text,
                                                                                             mesajicon,
                                                                                             ibDefault.Text,
                                                                                             ibBackColor.BackColor,
                                                                                             ibOverlayColor.BackColor,
                                                                                             ibOK.Text,
                                                                                             ibCancel.Text,
                                                                                             ibWidth.Value,
                                                                                             ibHeight.Value);
            DialogResult diagres = inputbox.ShowDialog();
        }

        private void HaltroySlider1_Scroll(object sender, ScrollEventArgs e)
        {
            sliderValue.Text = haltroySlider1.Value.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            htsHeight.Maximum = Screen.FromHandle(this.Handle).WorkingArea.Height;
            htsWidth.Maximum = Screen.FromHandle(this.Handle).WorkingArea.Width;
            ibWidth.Maximum = Screen.FromHandle(this.Handle).WorkingArea.Width;
            ibHeight.Maximum = Screen.FromHandle(this.Handle).WorkingArea.Height;
            htsHeight.Value = 140;
            htsWidth.Value = 390;
            ibWidth.Value = 400;
            ibHeight.Value = 150;
        }
    }
}
