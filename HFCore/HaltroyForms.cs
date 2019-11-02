using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HaltroyFramework
{
    public partial class HaltroyForms : Form
    {
        bool useFullScreen = false;
        bool draggable = true;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
                const Int32 WS_CHILDWINDOW = 0x40000000;
                // The following two styles are used to clip child
                // and sibling windows in Paint events.
                const Int32 WS_CLIPCHILDREN = 0x2000000;
                const Int32 WS_CLIPSIBLINGS = 0x4000000;
                // Add a Minimize button (or Minimize option in Window Menu).
                const Int32 WS_MINIMIZEBOX = 0x20000;
                // Add a Maximum/Restore Button (or Options in Window Menu).
                const Int32 WS_MAXIMIZEBOX = 0x10000;
                // Window can be resized.
                const Int32 WS_THICKFRAME = 0x40000;
                // Add A Window Menu
                const Int32 WS_SYSMENU = 0x80000;

                // Detect Double Clicks
                const int CS_DBLCLKS = 0x8;
                // Add a DropShadow (WinXP or greater).
                const int CS_DROPSHADOW = 0x20000;

                CreateParams cp = base.CreateParams;

                cp.Style = WS_CLIPCHILDREN | WS_CLIPSIBLINGS
            | WS_MAXIMIZEBOX | WS_MINIMIZEBOX
            | WS_SYSMENU | WS_THICKFRAME;

                if (this.DesignMode)
                    cp.Style = cp.Style | WS_CHILDWINDOW;

                int ClassFlags = CS_DBLCLKS;
                int OSVER = Environment.OSVersion.Version.Major * 10;
                OSVER += Environment.OSVersion.Version.Minor;

                if (OSVER >= 51)
                    ClassFlags = ClassFlags | CS_DROPSHADOW;
                cp.ClassStyle = ClassFlags;

                return cp;
            }
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        protected void This_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left & draggable)
            {
                if (e.Clicks > 1)
                {
                    this_MouseDoubleClick(sender, e);
                }
                else
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                    ReleaseCapture();
                }
            }
        }
        protected void this_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                if (useFullScreen)
                {
                    this.MaximizedBounds = Screen.FromHandle(this.Handle).Bounds;
                } else
                {
                    this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                }
                this.WindowState = FormWindowState.Maximized;
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MouseDown += This_MouseDown;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "HaltroyForms";

        }
        [Category("HaltroyForms"), Browsable(true), Description("Maximizes to entire screen on duble-click if enabled.Miximizes to Working Area if disabled.")]
        public bool FullScreenMode
        {
            get
            {
                return this.useFullScreen;
            }

            set
            {
                this.useFullScreen = value;
            }
        }
        [Category("HaltroyForms"), Browsable(true), Description("Enables dragging.")]
        public bool EnableDrag
        {
            get
            {
                return this.draggable;
            }

            set
            {
                this.draggable = value;
            }
        }
        public HaltroyForms()
        {
            Startup start = new Startup();
            InitializeComponent();
        }
    }
}
