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
using System.Security.Cryptography;
using System.Windows.Forms;

namespace HTAlt
{
    /// <summary>
    /// Customizable Input Box.
    /// </summary>
    public partial class HTInputBox : Form
    {
        #region HTControls
        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://github.com/Haltroy/HTAlt/wiki/HTInputBox-Class");
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
        #endregion
        /// <summary>
        /// Background color of HTInputBox. Foreground color is auto-selected to White or Black.
        /// </summary>
        public Color BackgroundColor;
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
        private HTMsgBoxButtons msgbutton = new HTMsgBoxButtons() { OK = true,Cancel = true, };
        /// <summary>
        /// Gets or sets the list of visible buttons.
        /// </summary>
        public HTMsgBoxButtons MsgBoxButtons
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
        private bool showDefaultButton = true;
        /// <summary>
        /// Gets or sets if "Set to Default" button is visible.
        /// </summary>
        public bool ShowSetToDefaultButton
        {
            get => showDefaultButton;
            set => showDefaultButton = value;
        }
        /// <summary>
        /// Creates a new Input Box.
        /// </summary>
        /// <param name="title">Title of the input box.</param>
        /// <param name="message">Description of the input box.</param>
        /// <param name="MessageBoxButtons">Buttons to display.</param>
        /// <param name="defaultValue">Default value of the input box.</param>
        public HTInputBox(string title,
                               string message,
                               HTMsgBoxButtons MessageBoxButtons,
                               string defaultValue = "")
        {
            
            InitializeComponent();
            defaultString = defaultValue;
            msgbutton = MessageBoxButtons;
            Text = title;
            Message = message;
            label1.Text = Message;
            label1.MaximumSize = new Size(Width - 25, 0);
            int buttonSize = 75;
            int Count = 0;
            Count += msgbutton.Yes ? 1 : 0;
            Count += msgbutton.No ? 1 : 0;
            Count += msgbutton.Cancel ? 1 : 0;
            Count += msgbutton.OK ? 1 : 0;
            Count += msgbutton.Abort ? 1 : 0;
            Count += msgbutton.Retry ? 1 : 0;
            Count += msgbutton.Ignore ? 1 : 0;
            Count += showDefaultButton ? 1 : 0;
            if (Count > 0)
            {
                buttonSize += Count * 25;
            }
            MaximumSize = new Size(Width, label1.Height + buttonSize);
            MinimumSize = new Size(Width, label1.Height + buttonSize);
            Height = label1.Height + buttonSize;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
        }
        /// <summary>
        /// Creates new HTInputBox
        /// </summary>
        /// <param name="title">Title of the input box.</param>
        /// <param name="message">Description of the input box.</param>
        /// <param name="defaultValue">Default value of the input box.</param>
        public HTInputBox(string title,string message,string defaultValue) : this(title,message,new HTMsgBoxButtons() { OK = true, Cancel = true,},defaultValue) { }
        
        /// <summary>
        /// Value inside the textbox in this input box.
        /// </summary>
        public string TextValue => textBox1.Text;


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

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = Message;
            int buttonSize = 75;
            int Count = 0;
            Count += msgbutton.Yes ? 1 : 0;
            Count += msgbutton.No ? 1 : 0;
            Count += msgbutton.Cancel ? 1 : 0;
            Count += msgbutton.OK ? 1 : 0;
            Count += msgbutton.Abort ? 1 : 0;
            Count += msgbutton.Retry ? 1 : 0;
            Count += msgbutton.Ignore ? 1 : 0;
            Count += showDefaultButton ? 1 : 0;
            if (Count > 0)
            {
                buttonSize += Count * 25;
            }
            MaximumSize = new Size(Width, label1.Height + buttonSize);
            MinimumSize = new Size(Width, label1.Height + buttonSize);
            Height = label1.Height + buttonSize;
            MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
            // Set to Default
            btDefault.Visible = showDefaultButton;
            btDefault.Enabled = showDefaultButton;
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
            btOK.ButtonText = OK;
            btCancel.ButtonText = Cancel;
            btDefault.ButtonText = SetToDefault;
            btYes.ButtonText = Yes;
            btNo.ButtonText = No;
            btRetry.ButtonText = Retry;
            btAbort.ButtonText = Abort;
            btIgnore.ButtonText = Ignore;
            ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            BackColor = BackgroundColor;
            btOK.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            btOK.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btCancel.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btDefault.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btCancel.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            btDefault.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            textBox1.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            textBox1.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
        }

        private void haltroyButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text = defaultString;
        }

        private void HTInputBox_Load(object sender, EventArgs e)
        {
            timer1_Tick(sender,e);
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
