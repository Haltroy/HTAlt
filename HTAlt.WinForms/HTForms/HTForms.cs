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
using System.Windows.Forms;

namespace HTAlt
{
    /// <summary>
    /// A <see cref="System.Windows.Forms.Form"/> alternative if you want a borderless resizable form.
    /// </summary>
    public partial class HTForms : Form
    {
        #region HTControls
        private readonly HTAlt.Standart.HTInfo info = new HTAlt.Standart.HTInfo();
        private readonly Uri wikiLink = new Uri("https://github.com/Haltroy/HTAlt/wiki/HTForms-Class");
        private readonly Version firstHTAltVersion = new Version("0.1.1.0");
        private readonly string description = "A System.Windows.Forms.Form alternative if you want a borderless resizable form.";
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
        public HTAlt.Standart.HTInfo ProjectInfo => info;
        #endregion
        private bool useFullScreen = false;
        private bool draggable = true;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                // If this form is inherited, the IDE needs this style
                // set so that it's coordinate system is correct.
                const int WS_CHILDWINDOW = 0x40000000;
                // The following two styles are used to clip child
                // and sibling windows in Paint events.
                const int WS_CLIPCHILDREN = 0x2000000;
                const int WS_CLIPSIBLINGS = 0x4000000;
                // Add a Minimize button (or Minimize option in Window Menu).
                const int WS_MINIMIZEBOX = 0x20000;
                // Add a Maximum/Restore Button (or Options in Window Menu).
                const int WS_MAXIMIZEBOX = 0x10000;
                // Window can be resized.
                const int WS_THICKFRAME = 0x40000;
                // Add A Window Menu
                const int WS_SYSMENU = 0x80000;

                // Detect Double Clicks
                const int CS_DBLCLKS = 0x8;
                // Add a DropShadow (WinXP or greater).
                const int CS_DROPSHADOW = 0x20000;

                CreateParams cp = base.CreateParams;

                cp.Style = WS_CLIPCHILDREN | WS_CLIPSIBLINGS
            | WS_MAXIMIZEBOX | WS_MINIMIZEBOX
            | WS_SYSMENU | WS_THICKFRAME;

                if (DesignMode)
                {
                    cp.Style = cp.Style | WS_CHILDWINDOW;
                }

                int ClassFlags = CS_DBLCLKS;
                int OSVER = Environment.OSVersion.Version.Major * 10;
                OSVER += Environment.OSVersion.Version.Minor;

                if (OSVER >= 51)
                {
                    ClassFlags = ClassFlags | CS_DROPSHADOW;
                }

                cp.ClassStyle = ClassFlags;

                return cp;
            }
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left & draggable)
            {
                if (e.Clicks > 1)
                {
                    OnMouseDoubleClick(e);
                }
                else
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                    ReleaseCapture();
                }
            }
            Invalidate();
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                if (useFullScreen)
                {
                    MaximizedBounds = Screen.FromHandle(Handle).Bounds;
                }
                else
                {
                    MaximizedBounds = Screen.FromHandle(Handle).WorkingArea;
                }
                WindowState = FormWindowState.Maximized;
            }
            Invalidate();
        }
        /// <summary>
        /// Maximizes to entire screen on double-click if enabled. Maximizes to Working Area if disabled.
        /// </summary>
        [Category("HTForms"), Browsable(true), Description("Maximizes to entire screen on double-click if enabled. Maximizes to Working Area if disabled.")]
        public bool FullScreenMode
        {
            get => useFullScreen;

            set => useFullScreen = value;
        }
        /// <summary>
        /// Enables dragging.
        /// </summary>
        [Category("HTForms"), Browsable(true), Description("Enables dragging.")]
        public bool EnableDrag
        {
            get => draggable;

            set => draggable = value;
        }
        public HTForms()
        {
            
        }
    }
}
