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
    /// Customizable dialog box for showing progress.
    /// </summary>
    public partial class HTProgressBox : Form
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://haltroy.com/htalt/HTAlt.WinForms/HTProgressBox");
        private readonly Version firstHTAltVersion = new Version("0.1.4.0");
        private readonly string description = "Customizable dialog box for showing progress.";

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
        /// Background color of HTProgressBox. Foreground color is auto-selected to White or Black.
        /// </summary>
        public Color BackgroundColor = Color.FromArgb(255, 255, 255, 255);

        /// <summary>
        /// Gets or sets the loading bar color.
        /// </summary>
        public Color OverlayColor;

        private HTDialogBoxContext msgbutton = new HTDialogBoxContext() { OK = true, };

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
        /// Maximum of progress bar.
        /// </summary>
        public int Max;

        /// <summary>
        /// Minimum of progress bar.
        /// </summary>
        public int Min;

        /// <summary>
        /// Value of progress bar.
        /// </summary>
        public int Value;

        /// <summary>
        /// Thickness of progress bar.
        /// </summary>
        public int BorderThickness;

        /// <summary>
        /// True to show a border for progress bar.
        /// </summary>
        public bool ShowBorder;

        /// <summary>
        /// Creates new HTProgressBox.
        /// </summary>
        /// <param name="Title">Title of the message.</param>
        /// <param name="BoxMessage">Text to display.</param>
        /// <param name="DialogContext">Context to display in this dialog box.</param>
        public HTProgressBox(string Title,
                      string BoxMessage,
                      HTDialogBoxContext DialogContext)
        {
            InitializeComponent();
            msgbutton = DialogContext;
            Text = Title;
            Message = BoxMessage;
            label1.Text = Message;
            label1.MaximumSize = new Size(Width - 25, 0);
            int buttonSize = 75;
            int Count = 0;
            Count += msgbutton.ProgressBar ? 1 : 0;
            Count += msgbutton.Yes ? 1 : 0;
            Count += msgbutton.No ? 1 : 0;
            Count += msgbutton.Cancel ? 1 : 0;
            Count += msgbutton.OK ? 1 : 0;
            Count += msgbutton.Abort ? 1 : 0;
            Count += msgbutton.Retry ? 1 : 0;
            Count += msgbutton.Ignore ? 1 : 0;
            if (Count > 0)
            {
                buttonSize += Count * 25;
            }
            MaximumSize = new Size(Width, label1.Height + buttonSize);
            MinimumSize = new Size(Width, label1.Height + buttonSize);
            Height = label1.Height + buttonSize;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            timer1_Tick(null, null);
        }

        /// <summary>
        /// Creates new HTProgressBox.
        /// </summary>
        /// <param name="message">Text to display.</param>
        public HTProgressBox(string message) : this("", message, new HTDialogBoxContext() { ProgressBar = true, Abort = true, }) { }

        /// <summary>
        /// Creates new HTProgressBox.
        /// </summary>
        /// <param name="title">Title of message.</param>
        /// <param name="message">Text to display.</param>
        public HTProgressBox(string title, string message) : this("", message, new HTDialogBoxContext() { ProgressBar = true, Abort = true, }) { }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (FormBorderStyle != FormBorderStyle.None || FormBorderStyle != FormBorderStyle.FixedToolWindow)
            {
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
            }
            label1.Text = Message;
            label1.MaximumSize = new Size(Width - 25, 0);
            int buttonSize = 75;
            int Count = 0;
            Count += msgbutton.ProgressBar ? 1 : 0;
            Count += msgbutton.Yes ? 1 : 0;
            Count += msgbutton.No ? 1 : 0;
            Count += msgbutton.Cancel ? 1 : 0;
            Count += msgbutton.OK ? 1 : 0;
            Count += msgbutton.Abort ? 1 : 0;
            Count += msgbutton.Retry ? 1 : 0;
            Count += msgbutton.Ignore ? 1 : 0;
            if (Count > 0)
            {
                buttonSize += Count * 25;
            }
            MaximumSize = new Size(Width, label1.Height + buttonSize);
            MinimumSize = new Size(Width, label1.Height + buttonSize);
            Height = label1.Height + buttonSize;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            htProgressBar1.Location = new Point(htProgressBar1.Location.X, label1.Location.X + label1.Height + 20);
            htProgressBar1.Maximum = Max;
            htProgressBar1.Minimum = Min;
            htProgressBar1.Value = Value;
            htProgressBar1.DrawBorder = ShowBorder;
            htProgressBar1.BorderThickness = BorderThickness;
            htProgressBar1.BarColor = OverlayColor;
            /// ProgressBar
            htProgressBar1.Visible = msgbutton.ProgressBar;
            htProgressBar1.Enabled = msgbutton.ProgressBar;
            // Yes
            btYes.Visible = msgbutton.Yes;
            btYes.Enabled = msgbutton.Yes;
            // No
            btNo.Visible = msgbutton.No;
            btNo.Enabled = msgbutton.No;
            // Cancel
            btCancel.Visible = msgbutton.Cancel;
            btCancel.Enabled = msgbutton.Cancel;
            // OK
            btOK.Visible = msgbutton.OK;
            btOK.Enabled = msgbutton.OK;
            // Abort
            btAbort.Visible = msgbutton.Abort;
            btAbort.Enabled = msgbutton.Abort;
            // Retry
            btRetry.Visible = msgbutton.Retry;
            btRetry.Enabled = msgbutton.Retry;
            // Ignore
            btIgnore.Visible = msgbutton.Ignore;
            btIgnore.Enabled = msgbutton.Ignore;
            btYes.Text = Yes;
            btNo.Text = No;
            btCancel.Text = Cancel;
            btAbort.Text = Abort;
            btRetry.Text = Retry;
            btIgnore.Text = Ignore;
            btOK.Text = OK;
            ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
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
}