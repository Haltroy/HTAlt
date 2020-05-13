//MIT License
//
//Copyright (c) 2020 Eren "Haltroy" Kanat
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HTAlt
{
    /// <summary>
    /// Customizable <see cref="System.Windows.Forms.MessageBox"/>.
    /// </summary>
    public partial class HTMsgBox : Form
    {
        #region HTControls
        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://github.com/Haltroy/HTAlt/wiki/HTMsgBox-Class");
        private readonly Version firstHTAltVersion = new Version("0.1.1.0");
        private readonly string description = "Customizable System.Windows.Forms.MessageBox.";
        /// <summary>
        /// This control's wiki link.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's wiki link.")]
        public Uri WikiLink => wikiLink;
        /// <summary>
        /// This control's first appearance version for HTAlt.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's first appearance version for HTAlt.")]
        public Version FirstHTAltVersion => firstHTAltVersion;
        /// <summary>
        /// This control's description.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's description.")]
        public string Description => description;
        /// <summary>
        /// Information about this control's project.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("Information about this control's project.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public HTInfo ProjectInfo => info;
        #endregion
        /// <summary>
        /// Background color of HTMsgBox. Foreground color is auto-selected to White or Black.
        /// </summary>
        public Color BackgroundColor;
        private bool useOK = false;
        private readonly MessageBoxButtons msgbutton = MessageBoxButtons.OK;
        /// <summary>
        /// Text to display on "Yes" button.
        /// </summary>
        public string Yes = "Yes";
        /// <summary>
        /// Text to display on "No" button.
        /// </summary>
        public string No = "No";
        /// <summary>
        /// Text to display on "OK" button.
        /// </summary>
        public string OK = "OK";
        /// <summary>
        /// Text to display on "Cancel" button.
        /// </summary>
        public string Cancel = "Cancel";

        private static int LinesCountIndexOf(string s)
        {
            int count = 0;
            int position = 0;
            while ((position = s.IndexOf('\n', position)) != -1)
            {
                count++;
                position++;
            }
            return count;
        }
        /// <summary>
        /// Creates new HaltroyMsgBox.
        /// </summary>
        /// <param name="title">Title of the message.</param>
        /// <param name="message">Text of message.</param>
        /// <param name="messageBoxButtons">Buttons to display.</param>
        public HTMsgBox(string title,
                      string message,
                      MessageBoxButtons messageBoxButtons)
        {
            msgbutton = messageBoxButtons;
            InitializeComponent();
            Tools.PrintInfoToConsole();
            Text = title;
            label1.Text = message;
            Height = (15 * LinesCountIndexOf(message)) + 123;
            MaximumSize = new Size(Screen.FromHandle(Handle).WorkingArea.Width, Screen.FromHandle(Handle).WorkingArea.Height);
        }

        private void btYes_Click(object sender, EventArgs e)
        {
            DialogResult = useOK ? DialogResult.OK : DialogResult.Yes;
            Close();
        }

        private void btNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void msgkts_Load(object sender, EventArgs e)
        {
            if (msgbutton == MessageBoxButtons.OK)
            {
                btYes.Visible = true;
                btNo.Visible = false;
                btCancel.Visible = false;
                btYes.Enabled = true;
                btNo.Enabled = false;
                btCancel.Enabled = false;
                useOK = true;
            }
            else if (msgbutton == MessageBoxButtons.OKCancel)
            {
                btYes.Visible = true;
                btNo.Visible = false;
                btCancel.Visible = true;
                btYes.Enabled = true;
                btNo.Enabled = false;
                btCancel.Enabled = true;
                useOK = true;
            }
            else if (msgbutton == MessageBoxButtons.YesNo)
            {
                btYes.Visible = true;
                btNo.Visible = true;
                btCancel.Visible = false;
                btYes.Enabled = true;
                btNo.Enabled = true;
                btCancel.Enabled = false;
            }
            else if (msgbutton == MessageBoxButtons.YesNoCancel)
            {
                btYes.Visible = true;
                btNo.Visible = true;
                btCancel.Visible = true;
                btYes.Enabled = true;
                btNo.Enabled = true;
                btCancel.Enabled = true;
            }
            else
            {
                btYes.Visible = false;
                btNo.Visible = false;
                btCancel.Visible = false;
                btYes.Enabled = false;
                btNo.Enabled = false;
                btCancel.Enabled = false;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btYes.Text = useOK ? OK : Yes;
            btNo.Text = No;
            btCancel.Text = Cancel;
            ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            BackColor = BackgroundColor;
            btCancel.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btCancel.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            btYes.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btYes.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            btNo.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btNo.ForeColor = Tools.AutoWhiteBlack(BackgroundColor);
            flowLayoutPanel1.BackColor = BackgroundColor;
        }
    }
}
