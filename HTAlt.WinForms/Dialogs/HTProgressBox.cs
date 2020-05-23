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
using HTAlt.Standart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Documents;
using System.Windows.Forms;
using Win32Interop.Methods;

namespace HTAlt.WinForms
{
    /// <summary>
    /// Customizable dialog box for showing progress.
    /// </summary>
    public partial class HTProgressBox : Form
    {
        #region HTControls
        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://github.com/Haltroy/HTAlt/wiki/HTProgressBox-Class");
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
        #endregion
        /// <summary>
        /// Background color of HTProgressBox. Foreground color is auto-selected to White or Black.
        /// </summary>
        public Color BackgroundColor;

        /// <summary>
        /// Gets or sets the loading bar color.
        /// </summary>
        public Color OverlayColor;
        /// <summary>
        /// True to show a progress bar.
        /// </summary>
        public bool ShowProgressBar = true;
        /// <summary>
        /// True to show Abort button.
        /// </summary>
        public bool ShowAbortButton = true;
        /// <summary>
        /// Text to display on "Abort" button.
        /// </summary>
        public string Abort = "Abort";
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
        /// <param name="BoxMessage">Text of message.</param>
        public HTProgressBox(string Title,
                      string BoxMessage)
        {
            InitializeComponent();
            
            Text = Title;
            Message = BoxMessage;
            label1.Text = Message;
            label1.MaximumSize = new Size(Width - 25, 0);
            int buttonSize = 75;
            int Count = 0;
            Count += ShowProgressBar ? 1 : 0;
            Count += ShowAbortButton ? 1 : 0;
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
        /// <param name="message">Text of message.</param>
        public HTProgressBox(string message) : this("", message) { }

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
            int buttonSize = 75;
            int Count = 0;
            Count += ShowProgressBar ? 1 : 0;
            Count += ShowAbortButton ? 1 : 0;
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
            htProgressBar1.Visible = ShowProgressBar;
            htProgressBar1.Enabled = ShowProgressBar;
            // Abort
            btAbort.Visible = ShowAbortButton;
            btAbort.Enabled = ShowAbortButton;
            btAbort.ButtonText = Abort;
            ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            BackColor = BackgroundColor;
            btAbort.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btAbort.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
        }
        private void btAbort_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }
    }
}
