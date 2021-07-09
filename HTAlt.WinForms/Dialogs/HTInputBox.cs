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
    /// Customizable Input Box.
    /// </summary>
    public partial class HTInputBox : Form
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://htalt.haltroy.com/api/HTAlt.WinForms/HTInputBox");
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
        /// If <c>true</c>, picks text color using <see cref="Tools.AutoWhiteBlack(Color)"/> with BackColor. Otherwise, allows for setting ForeColor.
        /// </summary>
        public bool AutoForeColor = false;

        /// <summary>
        /// Image to display near message.
        /// </summary>
        public Image Image;

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
            bool imagenull = Image == null || Image is null;
            int buttonSize = 75 + flowLayoutPanel1.Height + (msgbutton.ShowSetToDefaultButton ? btDefault.Height + 5 : 0) + (imagenull ? 0 : 40);

            label1.MaximumSize = new Size(Width - (25 + (imagenull ? 0 : 40)), 0);

            label1.Location = new Point(imagenull ? pbImage.Location.X : (pbImage.Location.X + pbImage.Width + 5), label1.Location.Y);

            flowLayoutPanel1.MaximumSize = new Size(Width - 25, 0);

            int locx = Width - (flowLayoutPanel1.Width + 25);
            int locy = imagenull ? (label1.Location.Y + label1.Height) : (pbImage.Height > label1.Height ? (pbImage.Location.Y + pbImage.Height) : (pbImage.Location.Y + label1.Height)) + 2;

            textBox1.Width = Width - 40;
            textBox1.Location = new Point(pbImage.Location.X, locy);

            btDefault.Width = Width - 40;
            btDefault.Location = new Point(textBox1.Location.X, textBox1.Location.Y + textBox1.Height + 2);

            flowLayoutPanel1.Location = new Point(locx, msgbutton.ShowSetToDefaultButton ? (btDefault.Location.Y + btDefault.Height) : (textBox1.Location.Y + textBox1.Height) + 2);

            MaximumSize = new Size(Width, label1.Height + buttonSize);
            MinimumSize = new Size(Width, label1.Height + buttonSize);
            Height = label1.Height + buttonSize;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;

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
            btDefault.BackColor = bc2;
            btDefault.ForeColor = ForeColor;
            textBox1.ForeColor = ForeColor;
            textBox1.BackColor = bc2;
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