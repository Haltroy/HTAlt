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

namespace HTAlt.WinForms
{
    /// <summary>
    /// Customizable Input Box.
    /// </summary>
    public partial class HTInputBox : Form
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://haltroy.com/htalt/HTAlt.WinForms/HTInputBox");
        private readonly Version firstHTAltVersion = new Version("0.1.1.0");
        private readonly string description = "Customizable Input Box.";

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

        #endregion HTControls

        /// <summary>
        /// Background color of HTInputBox. Foreground color is auto-selected to White or Black.
        /// </summary>
        public Color BackgroundColor = Color.FromArgb(255, 255, 255, 255);

        /// <summary>
        /// Text to display on "Yes" button.
        /// </summary>
        public string Yes = "Yes";

        /// <summary>
        /// Text to display on "Retry" button.
        /// </summary>
        public string Retry = "Retry";

        /// <summary>
        /// Text to display on "Abort" button.
        /// </summary>
        public string Abort = "Abort";

        /// <summary>
        /// Text to display on "Ignore" button.
        /// </summary>
        public string Ignore = "Ignore";

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

        /// <summary>
        /// Text to display on "Set to default" button.
        /// </summary>
        public string SetToDefault = "Set to default";

        private string defaultString = "";

        /// <summary>
        /// Gets or sets the default answer.
        /// </summary>
        public string DefaulValue
        {
            get => defaultString;
            set => defaultString = value;
        }

        private HTDialogBoxContext msgbutton = new HTDialogBoxContext(MessageBoxButtons.OKCancel);

        /// <summary>
        /// Gets or sets the list of visible buttons.
        /// </summary>
        public HTDialogBoxContext MsgBoxButtons
        {
            get => msgbutton;
            set => msgbutton = value;
        }

        private string message = "";

        /// <summary>
        /// Text to display on top of buttons and text area.
        /// </summary>
        public string Message
        {
            get => message;
            set => message = value;
        }

        /// <summary>
        /// Creates new HTInputBox.
        /// </summary>
        /// <param name="title">Title of the input box.</param>
        /// <param name="message">Description of the input box.</param>
        /// <param name="MessageBoxButtons">Buttons to display.</param>
        /// <param name="defaultValue">Default value of the input box.</param>
        public HTInputBox(string title,
                               string message,
                               HTDialogBoxContext MessageBoxButtons,
                               string defaultValue = "")
        {
            InitializeComponent();
            defaultString = defaultValue;
            msgbutton = MessageBoxButtons;
            Text = title;
            Message = message;
            label1.Text = Message;
            label1.MaximumSize = new Size(Width - 25, 0);
            flowLayoutPanel1.MaximumSize = new Size(Width - 25, 0);
            int buttonSize = 75 + flowLayoutPanel1.Height + (msgbutton.ShowSetToDefaultButton ? btDefault.Height + 5 : 0);
            MaximumSize = new Size(Width, label1.Height + buttonSize);
            MinimumSize = new Size(Width, label1.Height + buttonSize);
            Height = label1.Height + buttonSize;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            timer1_Tick(null, null);
        }

        /// <summary>
        /// Creates new HTInputBox
        /// </summary>
        /// <param name="title">Title of the input box.</param>
        /// <param name="message">Description of the input box.</param>
        /// <param name="defaultValue">Default value of the input box.</param>
        public HTInputBox(string title, string message, string defaultValue) : this(title, message, new HTDialogBoxContext(MessageBoxButtons.OKCancel), defaultValue) { }

        /// <summary>
        /// Value inside the textbox in this input box.
        /// </summary>
        public string TextValue => textBox1.Text;

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = Message;
            flowLayoutPanel1.SuspendLayout();
            // Set to Default
            btDefault.Visible = msgbutton.ShowSetToDefaultButton;
            btDefault.Enabled = msgbutton.ShowSetToDefaultButton;
            // Yes
            btYes.Visible = msgbutton.ShowYesButton;
            btYes.Enabled = msgbutton.ShowYesButton;
            // No
            btNo.Visible = msgbutton.ShowNoButton;
            btNo.Enabled = msgbutton.ShowNoButton;
            // Cancel
            btCancel.Visible = msgbutton.ShowCancelButton;
            btCancel.Enabled = msgbutton.ShowCancelButton;
            // OK
            btOK.Visible = msgbutton.ShowOKButton;
            btOK.Enabled = msgbutton.ShowOKButton;
            // Abort
            btAbort.Visible = msgbutton.ShowAbortButton;
            btAbort.Enabled = msgbutton.ShowAbortButton;
            // Retry
            btRetry.Visible = msgbutton.ShowRetryButton;
            btRetry.Enabled = msgbutton.ShowRetryButton;
            // Ignore
            btIgnore.Visible = msgbutton.ShowIgnoreButton;
            btIgnore.Enabled = msgbutton.ShowIgnoreButton;
            btOK.Text = OK;
            btCancel.Text = Cancel;
            btDefault.Text = SetToDefault;
            btYes.Text = Yes;
            btNo.Text = No;
            btRetry.Text = Retry;
            btAbort.Text = Abort;
            btIgnore.Text = Ignore;

            int buttonSize = 75 + flowLayoutPanel1.Height + (msgbutton.ShowSetToDefaultButton ? btDefault.Height + 5 : 0);

            textBox1.Width = Width - 25;
            textBox1.Location = new Point(label1.Location.X, label1.Location.Y + label1.Height + 2);

            btDefault.Width = Width - 25;
            btDefault.Location = new Point(textBox1.Location.X, textBox1.Location.Y + textBox1.Height + 2);

            flowLayoutPanel1.Width = Width - 25;
            flowLayoutPanel1.Location = new Point((Width - (flowLayoutPanel1.Width + 25)), msgbutton.ShowSetToDefaultButton ? (btDefault.Location.Y + btDefault.Height) : (textBox1.Location.Y + textBox1.Height) + 2);



            MaximumSize = new Size(Width, label1.Height + buttonSize);
            MinimumSize = new Size(Width, label1.Height + buttonSize);
            Height = label1.Height + buttonSize;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;

            ForeColor = Tools.AutoWhiteBlack(BackgroundColor);
            BackColor = BackgroundColor;
            btCancel.BackColor = Tools.ShiftBrightness(BackgroundColor, 20, false);
            btCancel.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            btYes.BackColor = Tools.ShiftBrightness(BackgroundColor, 20, false);
            btYes.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            btNo.BackColor = Tools.ShiftBrightness(BackgroundColor, 20, false);
            btNo.ForeColor = Tools.AutoWhiteBlack(BackgroundColor);
            btOK.ForeColor = Tools.AutoWhiteBlack(BackgroundColor);
            btOK.BackColor = Tools.ShiftBrightness(BackgroundColor, 20, false);
            btAbort.ForeColor = Tools.AutoWhiteBlack(BackgroundColor);
            btAbort.BackColor = Tools.ShiftBrightness(BackgroundColor, 20, false);
            btRetry.ForeColor = Tools.AutoWhiteBlack(BackgroundColor);
            btRetry.BackColor = Tools.ShiftBrightness(BackgroundColor, 20, false);
            btIgnore.ForeColor = Tools.AutoWhiteBlack(BackgroundColor);
            btIgnore.BackColor = Tools.ShiftBrightness(BackgroundColor, 20, false);
            btDefault.BackColor = Tools.ShiftBrightness(BackgroundColor, 20, false);
            btDefault.ForeColor = Tools.AutoWhiteBlack(BackgroundColor);
            textBox1.ForeColor = Tools.AutoWhiteBlack(BackgroundColor);
            textBox1.BackColor = Tools.ShiftBrightness(BackgroundColor, 20, false);
            flowLayoutPanel1.ResumeLayout(true);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void haltroyButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text = defaultString;
        }

        private void btRetry_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
            Close();
        }

        private void btIgnore_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Ignore;
            Close();
        }

        private void btAbort_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void btNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void btYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }
    }
}