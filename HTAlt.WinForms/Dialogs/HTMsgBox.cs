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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Documents;
using System.Windows.Forms;
using Win32Interop.Methods;

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
        private HTMsgBoxButtons msgbutton = new HTMsgBoxButtons(){ OK = true,};
        /// <summary>
        /// Gets or sets the list of visible buttons.
        /// </summary>
        public HTMsgBoxButtons MsgBoxButtons
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
        /// Creates new HaltroyMsgBox.
        /// </summary>
        /// <param name="Title">Title of the message.</param>
        /// <param name="Message">Text of message.</param>
        /// <param name="MessageBoxButtons">Buttons to display.</param>
        public HTMsgBox(string Title,
                      string MsgBoxMessage,
                      HTMsgBoxButtons MessageBoxButtons)
        {
            msgbutton = MessageBoxButtons;
            InitializeComponent();
            
            Text = Title;
            Message = MsgBoxMessage;
            label1.Text = Message;
            label1.MaximumSize = new Size(Width - 25, 0);
            int buttonSize = 50;
            int Count = 0;
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
        }
        /// <summary>
        /// Creates new HaltroyMsgBox.
        /// </summary>
        /// <param name="message">Text of message.</param>
        public HTMsgBox(string message) : this("", message, new HTMsgBoxButtons() { OK = true, }) { }
        /// <summary>
        /// Creates new HaltroyMsgBox.
        /// </summary>
        /// <param name="title">Title of the message.</param>
        /// <param name="message">Text of message.</param>
        public HTMsgBox(string message,string title) : this(title, message, new HTMsgBoxButtons() { OK = true, }) { }

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (FormBorderStyle != FormBorderStyle.None || FormBorderStyle != FormBorderStyle.FixedToolWindow)
            {
                FormBorderStyle = FormBorderStyle.FixedToolWindow;
            }
            label1.Text = Message;
            label1.MaximumSize = new Size(Width - 25, 0);
            int buttonSize = 50;
            int Count = 0;
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
            btYes.ButtonText = Yes;
            btNo.ButtonText = No;
            btCancel.ButtonText = Cancel;
            btAbort.ButtonText = Abort;
            btRetry.ButtonText = Retry;
            btIgnore.ButtonText = Ignore;
            btOK.ButtonText = OK;
            ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            BackColor = BackgroundColor;
            btCancel.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btCancel.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            btYes.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btYes.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            btNo.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btNo.ForeColor = Tools.AutoWhiteBlack(BackgroundColor);
        }

        private void btOK_Click_1(object sender, EventArgs e)
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
    /// Buttons to display in a HTMsgBox.
    /// </summary>
    public class HTMsgBoxButtons
    {
        private bool _OK = false;
        private bool _Yes = false;
        private bool _No = false;
        private bool _Cancel = false;
        private bool _Ignore = false;
        private bool _Abort = false;
        private bool _Retry = false;
        /// <summary>
        /// OK Button.
        /// </summary>
        public bool OK
        {
            get
            {
                return _OK;
            }
            set 
            { 
                _OK = value; 
            }
        }

        /// <summary>
        /// Yes Button.
        /// </summary>
        public bool Yes
        {
            get
            {
                return _Yes;
            }
            set
            {
                _Yes = value;
            }
        }
        /// <summary>
        /// No Button.
        /// </summary>
        public bool No
        {
            get
            {
                return _No;
            }
            set
            {
                _No = value;
            }
        }
        /// <summary>
        /// Cancel Button.
        /// </summary>
        public bool Cancel
        {
            get
            {
                return _Cancel;
            }
            set
            {
                _Cancel = value;
            }
        }
        /// <summary>
        /// Abort Button.
        /// </summary>
        public bool Abort
        {
            get
            {
                return _Abort;
            }
            set
            {
                _Abort = value;
            }
        }
        /// <summary>
        /// Retry Button.
        /// </summary>
        public bool Retry
        {
            get
            {
                return _Retry;
            }
            set
            {
                _Retry = value;
            }
        }
        /// <summary>
        /// Ignore Button.
        /// </summary>
        public bool Ignore
        {
            get
            {
                return _Ignore;
            }
            set
            {
                _Ignore = value;
            }
        }
    }
}
