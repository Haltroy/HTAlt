using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HaltroyFramework
{
    public partial class HaltroyMsgBox : Form
    {
        Color BackgroundColor;
        public HaltroyMsgBox(string title,
                      string message,
                      Icon icon,
                      MessageBoxButtons msgbutton = MessageBoxButtons.OK,
                      Color? BackColor = null,
                      string YesButtonText = "Yes",
                      string NoButtonText = "No",
                      string OKBUttonText = "OK",
                      string CancelButtonText = "Cancel")
        {
            InitializeComponent();
            Startup startup = new Startup();
            this.Icon = icon;
            BackgroundColor = BackColor ?? Color.White;
            this.Text = title;
            this.label1.Text = message;
            if (msgbutton == MessageBoxButtons.OK)
            {
                btYes.Visible = false;
                btNo.Visible = false;
                btCancel.Visible = false;
                btYes.Enabled = false;
                btNo.Enabled = false;
                btCancel.Enabled = false;
                btOK.Visible = true;
                btOK.Enabled = true;
            }else if (msgbutton == MessageBoxButtons.OKCancel) {
                btYes.Visible = false;
                btNo.Visible = false;
                btCancel.Visible = true;
                btYes.Enabled = false;
                btNo.Enabled = false;
                btCancel.Enabled = true;
                btOK.Visible = true;
                btOK.Enabled = true;
            }
            else if (msgbutton == MessageBoxButtons.YesNo) {
                btYes.Visible = true;
                btNo.Visible = true;
                btCancel.Visible = false;
                btYes.Enabled = true;
                btNo.Enabled = true;
                btCancel.Enabled = false;
                btOK.Visible = false;
                btOK.Enabled = false;
            }
            else if (msgbutton == MessageBoxButtons.YesNoCancel) {
                btYes.Visible = true;
                btNo.Visible = true;
                btCancel.Visible = true;
                btYes.Enabled = true;
                btNo.Enabled = true;
                btCancel.Enabled = true;
                btOK.Visible = false;
                btOK.Enabled = false;
            }
            btYes.Text = YesButtonText;
            btNo.Text = NoButtonText;
            btOK.Text = OKBUttonText;
            btCancel.Text = CancelButtonText;
        }

        private void btYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private static int Brightness(Color c)
        {
            return (int)Math.Sqrt(
               c.R * c.R * .241 +
               c.G * c.G * .691 +
               c.B * c.B * .068);
        }
        private void msgkts_Load(object sender, EventArgs e)
        {
            this.ForeColor = Brightness(BackgroundColor)< 130 ? Color.White : Color.Black;
            this.BackColor = BackgroundColor;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
