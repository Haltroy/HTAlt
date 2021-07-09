/*

Copyright © 2018 - 2021 haltroy

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/HTAlt/blob/master/LICENSE

*/

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
        private readonly Uri wikiLink = new Uri("https://htalt.haltroy.com/api/HTAlt.WinForms/HTProgressBox");
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
        /// If <c>true</c>, picks text color using <see cref="Tools.AutoWhiteBlack(Color)"/> with BackColor. Otherwise, allows for setting ForeColor.
        /// </summary>
        public bool AutoForeColor = false;

        /// <summary>
        /// Image to display near message.
        /// </summary>
        public Image Image;

        /// <summary>
        /// Gets or sets the loading bar color.
        /// </summary>
        public Color OverlayColor;

        private HTDialogBoxContext msgbutton = new HTDialogBoxContext(MessageBoxButtons.OK, true, false) { };

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

            timer1_Tick(null, null);
        }

        /// <summary>
        /// Creates new HTProgressBox.
        /// </summary>
        /// <param name="message">Text to display.</param>
        public HTProgressBox(string message) : this("", message, new HTDialogBoxContext(MessageBoxButtons.OK, true, false) { }) { }

        /// <summary>
        /// Creates new HTProgressBox.
        /// </summary>
        /// <param name="title">Title of message.</param>
        /// <param name="message">Text to display.</param>
        public HTProgressBox(string title, string message) : this("", message, new HTDialogBoxContext(MessageBoxButtons.OK, true, false) { }) { }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (FormBorderStyle != FormBorderStyle.None || FormBorderStyle != FormBorderStyle.FixedToolWindow)
            {
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
            }
            if (Image is null)
            {
                pbImage.Visible = false;
                pbImage.Enabled = false;
                pbImage.Image = null;
            }
            else
            {
                pbImage.Visible = true;
                pbImage.Enabled = true;
                pbImage.Image = Image;
            }
            flowLayoutPanel1.SuspendLayout();
            label1.Text = Message;
            flowLayoutPanel1.MaximumSize = new Size(Width - 25, 0);

            /// ProgressBar
            htProgressBar1.Visible = msgbutton.ShowProgressBar;
            htProgressBar1.Enabled = msgbutton.ShowProgressBar;
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
            int buttonSize = 75 + flowLayoutPanel1.Height + (msgbutton.ShowProgressBar ? htProgressBar1.Height + 5 : 0) + (imagenull ? 0 : 40);

            label1.MaximumSize = new Size(Width - (25 + (imagenull ? 0 : 40)), 0);

            label1.Location = new Point(imagenull ? pbImage.Location.X : (pbImage.Location.X + pbImage.Width + 5), label1.Location.Y);

            MaximumSize = new Size(Width, label1.Height + buttonSize);
            MinimumSize = new Size(Width, label1.Height + buttonSize);
            Height = label1.Height + buttonSize;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            int locx = Width - (flowLayoutPanel1.Width + 25);
            int locy = imagenull ? (label1.Location.Y + label1.Height) : (pbImage.Height > label1.Height ? (pbImage.Location.Y + pbImage.Height) : (pbImage.Location.Y + label1.Height)) + 2;

            htProgressBar1.Width = Width - 50;
            htProgressBar1.Location = new Point(htProgressBar1.Location.X, locy + 10);

            flowLayoutPanel1.Location = new Point(locx, msgbutton.ShowProgressBar ? (htProgressBar1.Location.Y + htProgressBar1.Height + 2) : locy + 10);
            htProgressBar1.Maximum = Max;
            htProgressBar1.Minimum = Min;
            htProgressBar1.Value = Value;
            htProgressBar1.DrawBorder = ShowBorder;
            htProgressBar1.BackColor = Tools.ShiftBrightness(BackColor, 20, false);
            htProgressBar1.BorderThickness = BorderThickness;
            htProgressBar1.BarColor = OverlayColor;
            ForeColor = AutoForeColor ? Tools.AutoWhiteBlack(BackColor) : ForeColor;
            Color bc2 = Tools.ShiftBrightness(BackColor, 20, false);
            btCancel.BackColor = bc2;
            btCancel.ForeColor = ForeColor;
            btYes.BackColor = bc2;
            btYes.ForeColor = ForeColor;
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
}