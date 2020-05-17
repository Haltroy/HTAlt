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
            HTMsgBoxButtons mesajbuton = new HTMsgBoxButtons()
            {
                OK = hsOK.Checked,
                Cancel = hsCancel.Checked,
                Yes = hsYes.Checked,
                No = hsNo.Checked,
                Abort = hsAbort.Checked,
                Ignore = hsIgnore.Checked,
                Retry = hsRetry.Checked,
            };
            Icon mesajicon = null;
            if (System.IO.File.Exists(tbIcon.Text))
            {
                mesajicon = new Icon(tbIcon.Text);
            } else
            {
                mesajicon = this.Icon;
            }
            HTMsgBox mesaj = new HTMsgBox(tbTitle.Text,
                                                                                      tbMessage.Text,
                                                                                      mesajbuton)
            { Icon = mesajicon, BackgroundColor = pbBackColor.BackColor, Abort = tbAbort.Text, Retry = tbRetry.Text, Ignore = tbIgnore.Text, Yes = tbYes.Text, No = tbNo.Text, OK = tbOK.Text, Cancel = tbCancel.Text, };
            mesaj.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            HTMsgBoxButtons mesajbuton = new HTMsgBoxButtons()
            {
                OK = hsOK.Checked,
                Cancel = hsCancel.Checked,
                Yes = hsYes.Checked,
                No = hsNo.Checked,
                Abort = hsAbort.Checked,
                Ignore = hsIgnore.Checked,
                Retry = hsRetry.Checked,
            };
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
            { Icon = mesajicon, BackgroundColor = pbBackColor.BackColor,Abort = tbAbort.Text, Retry = tbRetry.Text, Ignore = tbIgnore.Text, Yes = tbYes.Text, No = tbNo.Text, OK = tbOK.Text, Cancel = tbCancel.Text, };
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
            else if (diares == DialogResult.Abort)
            {
                lResult.Text = "Abort - " + tbAbort.Text;
            }
            else if (diares == DialogResult.Retry)
            {
                lResult.Text = "Retry - " + tbRetry.Text;
            }
            else if (diares == DialogResult.Ignore)
            {
                lResult.Text = "Ignore - " + tbIgnore.Text;
            }else
            {
                lResult.Text = "None";
            }
            label10.Visible = true;
            lResult.Visible = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Icon mesajicon = null;
            if (System.IO.File.Exists(tbIcon.Text))
            {
                mesajicon = new Icon(tbIcon.Text);
            }
            HTInputBox inputbox = new HTInputBox(tbTitle.Text, tbMessage.Text, ibDefault.Text)
            { Icon = mesajicon, BackgroundColor = pbBackColor.BackColor, OK = tbOK.Text, Cancel = tbCancel.Text, SetToDefault = ibSetToDefault.Text, }; ;
            DialogResult diagres = inputbox.ShowDialog();
            if (diagres == DialogResult.OK)
            {
                ibResult.Text = inputbox.TextValue;
            } else
            {
                ibResult.Text = "Cancelled.";
            }
            ibResult.Visible = true;
            ibResultTitle.Visible = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Icon mesajicon = null;
            if (System.IO.File.Exists(tbIcon.Text))
            {
                mesajicon = new Icon(tbIcon.Text);
            }
            HTInputBox inputbox = new HTInputBox(tbTitle.Text, tbMessage.Text, ibDefault.Text)
            { Icon = mesajicon, BackgroundColor = pbBackColor.BackColor, OK = tbOK.Text, Cancel = tbCancel.Text, SetToDefault = ibSetToDefault.Text, }; ;
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
