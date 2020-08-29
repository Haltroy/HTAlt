using System;
using System.Drawing;
using System.Windows.Forms;

namespace HTAlt.WinForms.Example
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
            colordlg.Color = pbBackColor.BackColor;
            colordlg.AllowFullOpen = true;
            colordlg.AnyColor = true;
            if (colordlg.ShowDialog() == DialogResult.OK)
            {
                pbBackColor.BackColor = colordlg.Color;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            HTDialogBoxContext mesajbuton = new HTDialogBoxContext()
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
            HTMsgBox mesaj = new HTMsgBox(tbTitle.Text,
                                          tbMessage.Text,
                                          mesajbuton)
            {
                Icon = mesajicon,
                BackgroundColor = pbBackColor.BackColor,
                Abort = tbAbort.Text,
                Retry = tbRetry.Text,
                Ignore = tbIgnore.Text,
                Yes = tbYes.Text,
                No = tbNo.Text,
                OK = tbOK.Text,
                Cancel = tbCancel.Text,
            };
            mesaj.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            HTDialogBoxContext mesajbuton = new HTDialogBoxContext()
            {
                SetToDefault = hsDefault.Checked,
                ProgressBar = hsPBar.Checked,
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
            HTMsgBox mesaj = new HTMsgBox(tbTitle.Text,
                                          tbMessage.Text,
                                          mesajbuton)
            {
                Icon = mesajicon,
                BackgroundColor = pbBackColor.BackColor,
                Abort = tbAbort.Text,
                Retry = tbRetry.Text,
                Ignore = tbIgnore.Text,
                Yes = tbYes.Text,
                No = tbNo.Text,
                OK = tbOK.Text,
                Cancel = tbCancel.Text,
            };
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
            }
            else
            {
                lResult.Text = "None";
            }
            label10.Visible = true;
            lResult.Visible = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            HTDialogBoxContext mesajbuton = new HTDialogBoxContext()
            {
                SetToDefault = hsDefault.Checked,
                ProgressBar = hsPBar.Checked,
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
            HTInputBox inputbox = new HTInputBox(tbTitle.Text, tbMessage.Text, mesajbuton, ibDefault.Text)
            {
                Icon = mesajicon,
                BackgroundColor = pbBackColor.BackColor,
                Abort = tbAbort.Text,
                Retry = tbRetry.Text,
                Ignore = tbIgnore.Text,
                Yes = tbYes.Text,
                No = tbNo.Text,
                OK = tbOK.Text,
                Cancel = tbCancel.Text,
                SetToDefault = ibSetToDefault.Text,
            };
            DialogResult diagres = inputbox.ShowDialog();
            if (diagres == DialogResult.OK)
            {
                ibResult.Text = inputbox.TextValue;
            }
            else
            {
                ibResult.Text = "Cancelled.";
            }
            ibResult.Visible = true;
            ibResultTitle.Visible = true;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            HTDialogBoxContext mesajbuton = new HTDialogBoxContext()
            {
                SetToDefault = hsDefault.Checked,
                ProgressBar = hsPBar.Checked,
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
            HTInputBox inputbox = new HTInputBox(tbTitle.Text, tbMessage.Text, mesajbuton, ibDefault.Text)
            { Icon = mesajicon, BackgroundColor = pbBackColor.BackColor, Abort = tbAbort.Text, Retry = tbRetry.Text, Ignore = tbIgnore.Text, Yes = tbYes.Text, No = tbNo.Text, OK = tbOK.Text, Cancel = tbCancel.Text, SetToDefault = ibSetToDefault.Text, }; ;
            inputbox.Show();
        }

        private void HTSlider1_Scroll(object sender, ScrollEventArgs e)
        {
            sliderValue.Text = HTSlider1.Value.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmHTF newform = new frmHTF();
            newform.EnableDrag = hsDrag.Checked;
            newform.FullScreenMode = hsFullScreen.Checked;
            newform.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TabForm tabForm = new TabForm();
            tabForm.Tabs.Add(tabForm.CreateTab());
            tabForm.SelectedTabIndex = 0;
            HTTitleTabsApplicationContext appContext = new HTTitleTabsApplicationContext();
            appContext.Start(tabForm);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            HTDialogBoxContext mesajbuton = new HTDialogBoxContext()
            {
                SetToDefault = hsDefault.Checked,
                ProgressBar = hsPBar.Checked,
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
            HTProgressBox inputbox = new HTProgressBox(tbTitle.Text, tbMessage.Text, mesajbuton)
            {
                OverlayColor = pbOverlayColor.BackColor,
                Icon = mesajicon,
                BackgroundColor = pbBackColor.BackColor,
                Abort = tbAbort.Text,
                Min = Convert.ToInt32(nudMin.Value),
                Max = Convert.ToInt32(nudMax.Value),
                Value = Convert.ToInt32(nudVal.Value),
                BorderThickness = Convert.ToInt32(nudBorder.Value),
                ShowBorder = hsBorder.Checked,
            };
            inputbox.Show();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            ColorDialog colordlg = new ColorDialog();
            colordlg.Color = pbOverlayColor.BackColor;
            colordlg.AllowFullOpen = true;
            colordlg.AnyColor = true;
            if (colordlg.ShowDialog() == DialogResult.OK)
            {
                pbOverlayColor.BackColor = colordlg.Color;
            }
        }

        private void hbRandom_Click(object sender, EventArgs e)
        {
            try
            {
                lbRandom.Text = HTAlt.Tools.GenerateRandomText(Convert.ToInt32(nudRandom.Value));
            }
            catch (Exception ex)
            {
                lbRandom.Text = ex.Message;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (Tools.ValidUrl(textBox1.Text, textBox2.Text.Split(';'), checkBox1.Checked))
            {
                pbValidUrl.BackColor = Color.Green;
            }
            else
            {
                pbValidUrl.BackColor = Color.Red;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(sender, e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(sender, e);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox1_TextChanged(sender, e);
            }
        }
    }
}