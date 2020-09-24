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
    /// Customizable <see cref="System.Windows.Forms.MessageBox"/>.
    /// </summary>
    public partial class HTMsgBox : Form
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://haltroy.com/htalt/HTAlt.WinForms/HTMsgBox");
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

        #endregion HTControls

        /// <summary>
        /// If <c>true</c>, picks text color using <see cref="Tools.AutoWhiteBlack(Color)"/> with BackColor. Otherwise, allows for setting ForeColor.
        /// </summary>
        public bool AutoForeColor = false;

        /// <summary>
        /// Image to display near message. 
        /// </summary>
        public Image Image;

        private HTDialogBoxContext msgbutton = new HTDialogBoxContext(MessageBoxButtons.OK) { };

        /// <summary>
        /// Gets or sets the list of visible buttons.
        /// </summary>
        public HTDialogBoxContext MsgBoxButtons
        {
            get => msgbutton;
            set => msgbutton = value;
        }

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
        /// Text to display on top of buttons.
        /// </summary>
        public string Message = "";



        /// <summary>
        /// Creates new HTMsgBox.
        /// </summary>
        /// <param name="Title">Title of the message.</param>
        /// <param name="Message">Text of message.</param>
        /// <param name="MessageBoxButtons">Buttons to display.</param>
        public HTMsgBox(string Title,
                      string MsgBoxMessage,
                      HTDialogBoxContext MessageBoxButtons)
        {
            msgbutton = MessageBoxButtons;
            InitializeComponent();
            Text = Title;
            Message = MsgBoxMessage;
            pbImage.Visible = (Image != null);
            pbImage.Enabled = (Image != null);
            pbImage.Image = Image;
            label1.Text = Message;

            timer1_Tick(null, null);
        }

        /// <summary>
        /// Creates new HTMsgBox.
        /// </summary>
        /// <param name="message">Text of message.</param>
        public HTMsgBox(string message) : this("", message, new HTDialogBoxContext(MessageBoxButtons.OK) { }) { }

        /// <summary>
        /// Creates new HTMsgBox.
        /// </summary>
        /// <param name="title">Title of the message.</param>
        /// <param name="message">Text to display.</param>
        public HTMsgBox(string message, string title) : this(title, message, new HTDialogBoxContext(MessageBoxButtons.OK) {}) { }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (FormBorderStyle != FormBorderStyle.None || FormBorderStyle != FormBorderStyle.FixedToolWindow)
            {
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
            }
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            label1.Text = Message;
            pbImage.Visible = (Image != null);
            pbImage.Enabled = (Image != null);
            pbImage.Image = Image;
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
            btYes.Text = Yes;
            btNo.Text = No;
            btCancel.Text = Cancel;
            btAbort.Text = Abort;
            btRetry.Text = Retry;
            btIgnore.Text = Ignore;
            btOK.Text = OK;
            bool imagenull = Image == null || Image is null;
            int buttonSize = 50 + flowLayoutPanel1.Height + (!imagenull ? 40 : 0);

            label1.MaximumSize = new Size(Width - (25 + (imagenull ? 0 : 40)), 0);

            label1.Location = new Point(imagenull ? pbImage.Location.X : (pbImage.Location.X + pbImage.Width + 5), label1.Location.Y);

            flowLayoutPanel1.MaximumSize = new Size(Width - 25, 0);
            int locx = Width - (flowLayoutPanel1.Width + 25);
            int locy = imagenull ? (label1.Location.Y + label1.Height) : (pbImage.Height > label1.Height ? (pbImage.Location.Y + pbImage.Height) : (pbImage.Location.Y + label1.Height)) + 2;
            flowLayoutPanel1.Location = new Point(locx, locy);

            MaximumSize = new Size(Width, label1.Height + buttonSize);
            MinimumSize = new Size(Width, label1.Height + buttonSize);
            Height = label1.Height + buttonSize;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            Color bc2 = Tools.ShiftBrightness(BackColor, 20, false);
            ForeColor = AutoForeColor ? Tools.AutoWhiteBlack(BackColor) : ForeColor;
            btCancel.BackColor = bc2;
            btCancel.ForeColor = ForeColor; ;
            btYes.BackColor = bc2;
            btYes.ForeColor = ForeColor; ;
            btNo.BackColor = bc2;
            btNo.ForeColor = ForeColor;
            btOK.ForeColor = ForeColor;
            btOK.BackColor = bc2;
            btAbort.ForeColor = ForeColor;
            btAbort.BackColor = bc2;
            btRetry.ForeColor = ForeColor;
            btRetry.BackColor = bc2;
            btIgnore.ForeColor = ForeColor;
            btIgnore.BackColor = bc2;
            ResumeLayout(true);
            flowLayoutPanel1.ResumeLayout(true);
        }

        private void btYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
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

        private void btOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btAbort_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
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
    }

    /// <summary>
    /// Buttons & other controls to display in a HT Dialog Box.
    /// </summary>
    public class HTDialogBoxContext
    {
        private bool _SetToDefault = false;
        private bool _ProgressBar = false;
        private bool _OK = false;
        private bool _Yes = false;
        private bool _No = false;
        private bool _Cancel = false;
        private bool _Ignore = false;
        private bool _Abort = false;
        private bool _Retry = false;

        public HTDialogBoxContext() : this(null) { }

        public HTDialogBoxContext(MessageBoxButtons? buttons,bool showProgressBar = false, bool showSetToDefaultButton = false)
        {
            ShowProgressBar = showProgressBar;
            ShowSetToDefaultButton = showSetToDefaultButton;
            if (buttons is null)
            {

            }else
            {
                MessageBoxButtons _b = buttons.Value;
                switch (_b)
                {
                    case MessageBoxButtons.OK:
                        ShowOKButton = true;
                        break;
                    case MessageBoxButtons.OKCancel:
                        ShowOKButton = true;
                        ShowCancelButton = true;
                        break;
                    case MessageBoxButtons.AbortRetryIgnore:
                        ShowAbortButton = true;
                        ShowRetryButton = true;
                        ShowIgnoreButton = true;
                        break;
                    case MessageBoxButtons.YesNoCancel:
                        ShowYesButton = true;
                        ShowNoButton = true;
                        ShowCancelButton = true;
                        break;
                    case MessageBoxButtons.YesNo:
                        ShowYesButton = true;
                        ShowNoButton = true;
                        break;
                    case MessageBoxButtons.RetryCancel:
                        ShowRetryButton = true;
                        ShowCancelButton = true;
                        break;
                }
            }
        }

        /// <summary>
        /// "OK" Button.
        /// </summary>
        public bool ShowOKButton
        {
            get => _OK;
            set => _OK = value;
        }

        /// <summary>
        /// Progress Bar.
        /// Only applies to <see cref="HTProgressBox"/>.
        /// </summary>
        public bool ShowProgressBar
        {
            get => _ProgressBar;
            set => _ProgressBar = value;
        }

        /// <summary>
        /// "Set to Default" button..
        /// Only applies to <see cref="HTInputBox"/>.
        /// </summary>
        public bool ShowSetToDefaultButton
        {
            get => _SetToDefault;
            set => _SetToDefault = value;
        }

        /// <summary>
        /// "Yes" Button.
        /// </summary>
        public bool ShowYesButton
        {
            get => _Yes;
            set => _Yes = value;
        }

        /// <summary>
        /// "No" Button.
        /// </summary>
        public bool ShowNoButton
        {
            get => _No;
            set => _No = value;
        }

        /// <summary>
        /// "Cancel" Button.
        /// </summary>
        public bool ShowCancelButton
        {
            get => _Cancel;
            set => _Cancel = value;
        }

        /// <summary>
        /// "Abort" Button.
        /// </summary>
        public bool ShowAbortButton
        {
            get => _Abort;
            set => _Abort = value;
        }

        /// <summary>
        /// "Retry" Button.
        /// </summary>
        public bool ShowRetryButton
        {
            get => _Retry;
            set => _Retry = value;
        }

        /// <summary>
        /// "Ignore" Button.
        /// </summary>
        public bool ShowIgnoreButton
        {
            get => _Ignore;
            set => _Ignore = value;
        }
    }
}