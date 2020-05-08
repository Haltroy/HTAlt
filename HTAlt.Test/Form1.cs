using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HTAlt.Test
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
            HTAlt.HTMsgBox mesaj = new HTAlt.HTMsgBox(tbTitle.Text,
                                                                                      tbMessage.Text,
                                                                                      mesajbuton)
            {Icon = mesajicon,BackgroundColor = pbBackColor.BackColor,Yes = tbYes.Text,No = tbNo.Text,OK = tbOK.Text,Cancel = tbCancel.Text, };
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
            HTAlt.HTMsgBox mesaj = new HTAlt.HTMsgBox(tbTitle.Text,
                                                                          tbMessage.Text,
                                                                          mesajbuton)
            { Icon = mesajicon, BackgroundColor = pbBackColor.BackColor, Yes = tbYes.Text, No = tbNo.Text, OK = tbOK.Text, Cancel = tbCancel.Text, };
            DialogResult diares = mesaj.ShowDialog();
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
            HTAlt.HTInputBox inputbox = new HTAlt.HTInputBox(ibTitle.Text,
                                                                                             ibMessage.Text,
                                                                                             ibDefault.Text)
            { Icon = mesajicon, BackgroundColor = ibBackColor.BackColor,  OK = ibOK.Text, Cancel = ibCancel.Text, SetToDefault = ibSetToDefault.Text, };;
            DialogResult diagres = inputbox.ShowDialog();
            if (diagres == DialogResult.OK)
            {
                ibResult.Text = inputbox.TextValue;
            }else
            {
                ibResult.Text = "Cancelled.";
            }
            ibResult.Visible = true;
            ibResultTitle.Visible = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Icon mesajicon = null;
            if (System.IO.File.Exists(ibIcon.Text))
            {
                mesajicon = new Icon(ibIcon.Text);
            }
            HTAlt.HTInputBox inputbox = new HTAlt.HTInputBox(ibTitle.Text,
                                                                                             ibMessage.Text,
                                                                                             ibDefault.Text)
            { Icon = mesajicon, BackgroundColor = ibBackColor.BackColor, OK = ibOK.Text, Cancel = ibCancel.Text, SetToDefault = ibSetToDefault.Text, }; ;
            DialogResult diagres = inputbox.ShowDialog();
        }

        private void HTSlider1_Scroll(object sender, ScrollEventArgs e)
        {
            sliderValue.Text = HTSlider1.Value.ToString();
        }
        bool draggable;
        bool fullscreenmode;
        private void button7_Click(object sender, EventArgs e)
        {
            frmHTF newform = new frmHTF();
            newform.EnableDrag = draggable;
            newform.FullScreenMode = fullscreenmode;
            newform.Show();
        }

        private void HTSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            draggable = HTSwitch2.Checked;
        }

        private void HTSwitch3_CheckedChanged(object sender, EventArgs e)
        {
            fullscreenmode = HTSwitch3.Checked;
        }
    }
}
