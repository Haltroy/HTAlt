/*

Copyright © 2018 - 2021 haltroy

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/HTAlt/blob/master/LICENSE

*/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HTAlt.WinForms
{
    /// <summary>
    /// Customizable <see cref="System.Windows.Forms.TabControl"/> with Drag & Close features.
    /// </summary>
    public class HTTabControl : TabControl
    {
        private Color backgroundColor = Color.FromArgb(45, 45, 48);
        private Color selectedTabColor = Color.FromArgb(0, 122, 204);
        private Color unselectedTabColor = Color.FromArgb(63, 63, 70);
        private Color hoverTabColor = Color.FromArgb(28, 151, 234);
        private Color hoverTabButtonColor = Color.FromArgb(82, 176, 239);
        private Color hoverUnselectedTabButtonColor = Color.FromArgb(85, 85, 85);
        private Color selectedTabButtonColor = Color.FromArgb(28, 151, 234);
        private Color unselectedBorderTabLineColor = Color.FromArgb(63, 63, 70);
        private Color borderTabLineColor = Color.FromArgb(0, 122, 204);
        private Color underBorderTabLineColor = Color.FromArgb(67, 67, 70);
        private Color textColor = Color.FromArgb(255, 255, 255);
        private Color upDownBackColor = Color.FromArgb(63, 63, 70);
        private Color upDownTextColor = Color.FromArgb(109, 109, 112);

        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://htalt.haltroy.com/api/HTAlt.WinForms/HTTabControl");
        private readonly Version firstHTAltVersion = new Version("0.1.1.0");
        private readonly string description = "Customizable System.Windows.Forms.TabControl with Drag & Close features.";

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
        /// The back color.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The back color.")]
        public Color BackgroundColor
        {
            get => backgroundColor;

            set => backgroundColor = value;
        }

        /// <summary>
        /// The color of the selected tab.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The color of the selected tab.")]
        public Color SelectedTabColor
        {
            get => selectedTabColor;

            set => selectedTabColor = value;
        }

        /// <summary>
        /// The color of the unselected tab.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The color of the unselected tab.")]
        public Color UnselectedTabColor
        {
            get => unselectedTabColor;

            set => unselectedTabColor = value;
        }

        /// <summary>
        /// The color of the tab when hovered.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The color of the tab when hovered.")]
        public Color HoverTabColor
        {
            get => hoverTabColor;

            set => hoverTabColor = value;
        }

        /// <summary>
        /// The button color of the hovered tab.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The button color of the hovered tab.")]
        public Color HoverTabButtonColor
        {
            get => hoverTabButtonColor;

            set => hoverTabButtonColor = value;
        }

        /// <summary>
        /// The button color of the hovered unselected tab.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The button color of the hovered unselected tab.")]
        public Color HoverUnselectedTabButtonColor
        {
            get => hoverUnselectedTabButtonColor;

            set => hoverUnselectedTabButtonColor = value;
        }

        /// <summary>
        /// The button color of the selected tab.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The button color of the selected tab.")]
        public Color SelectedTabButtonColor
        {
            get => selectedTabButtonColor;

            set => selectedTabButtonColor = value;
        }

        /// <summary>
        /// The border line color of the unselected tab.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The border line color of the unselected tab.")]
        public Color UnselectedBorderTabLineColor
        {
            get => unselectedBorderTabLineColor;

            set => unselectedBorderTabLineColor = value;
        }

        /// <summary>
        /// The border line color of the tab.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The border line color of the tab.")]
        public Color BorderTabLineColor
        {
            get => borderTabLineColor;

            set => borderTabLineColor = value;
        }

        /// <summary>
        /// The border line color of the under tab.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The border line color of the under tab.")]
        public Color UnderBorderTabLineColor
        {
            get => underBorderTabLineColor;

            set => underBorderTabLineColor = value;
        }

        /// <summary>
        /// The text color.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The text color.")]
        public Color TextColor
        {
            get => textColor;

            set => textColor = value;
        }

        /// <summary>
        /// The back color of up down.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The back color of up down.")]
        public Color UpDownBackColor
        {
            get => upDownBackColor;

            set => upDownBackColor = value;
        }

        /// <summary>
        /// The text color of up down.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("The text color of up down.")]
        public Color UpDownTextColor
        {
            get => upDownTextColor;

            set => upDownTextColor = value;
        }

        /// <summary>
        /// This option disables closing tabs.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("This option disables closing tabs.")]
        public bool DisableClose { get; set; }

        /// <summary>
        /// This option disables dragging tabs.
        /// </summary>
        [Category("HTTabControl"), Browsable(true), Description("This option disables dragging tabs.")]
        public bool DisableDragging { get; set; }

        private readonly StringFormat CenterSF;
        private TabPage predraggedTab;
        private int hoveringTabIndex;

        private SubClass scUpDown = null;
        private bool bUpDown = false;
        private bool hasFocus = false;

        public HTTabControl()
        {
            base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            CenterSF = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };

            Padding = new Point(14, 4);
            AllowDrop = true;
            Font = new Font("Segoe UI", 9f, FontStyle.Regular);
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            base.Alignment = TabAlignment.Top;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Rectangle mouseRect = new Rectangle(e.X, e.Y, 1, 1);
            System.Collections.Generic.IEnumerable<int> hoveringTabs = Enumerable.Range(0, TabCount).Where(i => GetTabRect(i).IntersectsWith(mouseRect));

            if (hoveringTabs.Any())
            {
                int tabIndex = hoveringTabs.First();
                Rectangle tabBase = new Rectangle(new Point(base.GetTabRect(tabIndex).Location.X + 2, base.GetTabRect(tabIndex).Location.Y), new Size(base.GetTabRect(tabIndex).Width, base.GetTabRect(tabIndex).Height));
                Rectangle tabExitRectangle = new Rectangle((tabBase.Location.X + tabBase.Width) - (15 + 3), tabBase.Location.Y + 3, 15, 15);

                if (tabExitRectangle.Contains(PointToClient(Cursor.Position)))
                {
                    if (!DisableClose)
                    {
                        TabPages.Remove(TabPages[tabIndex]);
                        return;
                    }
                }
            }

            predraggedTab = getPointedTab();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            predraggedTab = null;
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (SelectedIndex == -1)
            {
                base.OnMouseMove(e);
                return;
            }

            // check whether they are hovering over a tab button
            int tabIndex = SelectedIndex;
            Rectangle tabBase = new Rectangle(new Point(base.GetTabRect(tabIndex).Location.X + 2, base.GetTabRect(tabIndex).Location.Y), new Size(base.GetTabRect(tabIndex).Width, base.GetTabRect(tabIndex).Height));

            Rectangle mouseRect = new Rectangle(e.X, e.Y, 1, 1);
            System.Collections.Generic.IEnumerable<int> hoveringTabs = Enumerable.Range(0, TabCount).Where(i => GetTabRect(i).IntersectsWith(mouseRect));

            if (hoveringTabs.Any())
            {
                hoveringTabIndex = hoveringTabs.First();
            }

            if (e.Button == MouseButtons.Left && predraggedTab != null)
            {
                base.DoDragDrop(predraggedTab, DragDropEffects.Move);
            }

            if (e.Y < 25) // purely for performance reasons, only necessary for hovering button states
            {
                Invalidate();
            }

            base.OnMouseMove(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (hoveringTabIndex != -1)
            {
                hoveringTabIndex = -1;
                Invalidate();
            }

            base.OnLeave(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (hoveringTabIndex != -1)
            {
                hoveringTabIndex = -1;
                Invalidate();
            }

            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.Clear(backgroundColor);

            g.DrawLine(new Pen(new SolidBrush(FindForm() == Form.ActiveForm ? borderTabLineColor : unselectedBorderTabLineColor), 2), new Point(0, 22), new Point(base.Width, 22));
            g.FillRectangle(new SolidBrush(underBorderTabLineColor), 0, 23, base.Width, 1);

            // ugly way to check whether the parent form has focus or not
            if (!hasFocus && FindForm() == Form.ActiveForm)
            {
                Invalidate(new Rectangle(0, 21, base.Width, 24));
                hasFocus = true;
            }
            else if (hasFocus && FindForm() != Form.ActiveForm)
            {
                Invalidate(new Rectangle(0, 21, base.Width, 24));
                hasFocus = false;
            }

            for (int i = 0; i < TabCount; i++)
            {
                Rectangle tabBase = new Rectangle(new Point(base.GetTabRect(i).Location.X + 2, base.GetTabRect(i).Location.Y), new Size(base.GetTabRect(i).Width, base.GetTabRect(i).Height));
                Rectangle tabSize = new Rectangle(tabBase.Location, new Size(tabBase.Width, tabBase.Height - 4));

                // draw tab highlights
                if (FindForm() != Form.ActiveForm && base.SelectedIndex == i) // unselected selected tab
                {
                    g.FillRectangle(new SolidBrush(unselectedTabColor), tabSize);
                }
                else if (base.SelectedIndex == i) // selected tab
                {
                    g.FillRectangle(new SolidBrush(selectedTabColor), tabSize);
                }
                else if (hoveringTabIndex == i) // hovering tab
                {
                    g.FillRectangle(new SolidBrush(hoverTabColor), tabSize);
                }
                else // unselected tab
                {
                    g.FillRectangle(new SolidBrush(backgroundColor), tabSize);
                }

                // if current selected tab
                if (base.SelectedIndex == i)
                {
                    if (!DisableClose)
                    {
                        // hovering over selected tab button
                        if (new Rectangle((tabBase.Location.X + tabBase.Width) - (15 + 3), tabBase.Location.Y + 3, 15, 15).Contains(PointToClient(Cursor.Position)))
                        {
                            g.FillRectangle(new SolidBrush(FindForm() == Form.ActiveForm ? selectedTabButtonColor : hoverUnselectedTabButtonColor),
                                new RectangleF((tabBase.Location.X + tabBase.Width) - (15 + 3), tabBase.Location.Y + 3, 15, 15));
                        }

                        g.TextContrast = 0;

                        g.DrawString("×", new Font(Font.FontFamily, 15f), new SolidBrush(textColor),
                            new Rectangle((tabBase.Location.X + tabBase.Width) - (15 + 5), tabBase.Location.Y - 3, tabBase.Width, tabBase.Height), CenterSF);
                    }
                }
                else
                {
                    // if hovering over a tab
                    if (hoveringTabIndex == i)
                    {
                        if (!DisableClose)
                        {
                            // hovering over hovered tab button
                            if (new Rectangle((tabBase.Location.X + tabBase.Width) - (15 + 3), tabBase.Location.Y + 3, 15, 15).Contains(PointToClient(Cursor.Position)))
                            {
                                g.FillRectangle(new SolidBrush(hoverTabButtonColor),
                                    new RectangleF((tabBase.Location.X + tabBase.Width) - (15 + 3), tabBase.Location.Y + 3, 15, 15));
                            }

                            g.TextContrast = 0;
                            g.DrawString("×", new Font(Font.FontFamily, 15f), new SolidBrush(textColor),
                                new Rectangle((tabBase.Location.X + tabBase.Width) - (15 + 5), tabBase.Location.Y - 3, tabBase.Width, tabBase.Height), CenterSF);
                        }
                    }
                }

                g.TextContrast = 12;
                g.DrawString(base.TabPages[i].Text, new Font(Font.FontFamily, Font.Size), new SolidBrush(textColor),
                    new Rectangle(tabBase.Location.X + 3, tabBase.Location.Y - 1, tabBase.Width, tabBase.Height + 1), CenterSF);
            }

            if (SelectedIndex != -1)
            {
                base.SelectedTab.BorderStyle = BorderStyle.None;
            }
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            TabPage draggedTab = (TabPage)drgevent.Data.GetData(typeof(TabPage));
            TabPage pointedTab = getPointedTab();
            if (!DisableDragging)
            {
                if (draggedTab == predraggedTab && pointedTab != null)
                {
                    drgevent.Effect = DragDropEffects.Move;

                    if (pointedTab != draggedTab)
                    {
                        swapTabPages(draggedTab, pointedTab);
                    }
                }
            }
            base.OnDragOver(drgevent);
        }

        private TabPage getPointedTab()
        {
            checked
            {
                for (int i = 0; i <= base.TabPages.Count - 1; i++)
                {
                    if (base.GetTabRect(i).Contains(base.PointToClient(Cursor.Position)))
                    {
                        return base.TabPages[i];
                    }
                }
                return null;
            }
        }

        private void swapTabPages(TabPage src, TabPage dst)
        {
            if (!DisableDragging)
            {
                int srci = base.TabPages.IndexOf(src);
                int dsti = base.TabPages.IndexOf(dst);
                base.TabPages[dsti] = src;
                base.TabPages[srci] = dst;

                base.SelectedIndex = (base.SelectedIndex == srci) ? dsti : srci;
            }
            Refresh();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            FindUpDown();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();

            base.OnControlAdded(e);
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();

            base.OnControlRemoved(e);
        }

        private void FindUpDown()
        {
            bool bFound = false;
            IntPtr pWnd = Win32.GetWindow(Handle, Win32.GW_CHILD);

            while (pWnd != IntPtr.Zero)
            {
                char[] className = new char[33];
                int length = Win32.GetClassName(pWnd, className, 32);
                string s = new string(className, 0, length);

                if (s == "msctls_updown32")
                {
                    bFound = true;

                    if (!bUpDown)
                    {
                        scUpDown = new SubClass(pWnd, true);
                        scUpDown.SubClassedWndProc += new SubClass.SubClassWndProcEventHandler(scUpDown_SubClassedWndProc);

                        bUpDown = true;
                    }
                    break;
                }

                pWnd = Win32.GetWindow(pWnd, Win32.GW_HWNDNEXT);
            }

            if ((!bFound) && (bUpDown))
            {
                bUpDown = false;
            }
        }

        private void UpdateUpDown()
        {
            if (bUpDown)
            {
                if (Win32.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rect = new Rectangle();

                    Win32.GetClientRect(scUpDown.Handle, ref rect);
                    Win32.InvalidateRect(scUpDown.Handle, ref rect, true);
                }
            }
        }

        private int scUpDown_SubClassedWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_PAINT:
                    {
                        IntPtr hDC = Win32.GetWindowDC(scUpDown.Handle);
                        Graphics g = Graphics.FromHdc(hDC);

                        DrawIcons(g);

                        g.Dispose();
                        Win32.ReleaseDC(scUpDown.Handle, hDC);
                        m.Result = IntPtr.Zero;

                        Rectangle rect = new Rectangle();

                        Win32.GetClientRect(scUpDown.Handle, ref rect);
                        Win32.ValidateRect(scUpDown.Handle, ref rect);
                    }
                    return 1;
            }

            return 0;
        }

        internal void DrawIcons(Graphics g)
        {
            Rectangle TabControlArea = ClientRectangle;
            Rectangle r0 = new Rectangle();
            Win32.GetClientRect(scUpDown.Handle, ref r0);

            Brush br = new SolidBrush(upDownBackColor);
            g.FillRectangle(br, r0);
            br.Dispose();

            g.DrawString("◀", new Font(Font.FontFamily, 12f),
                new SolidBrush(upDownTextColor), r0);

            g.DrawString("▶", new Font(Font.FontFamily, 12f),
                new SolidBrush(upDownTextColor),
                new Rectangle(r0.X + 20, r0.Y, r0.Width, r0.Height));
        }
    }

    internal static class Win32
    {
        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDLAST = 1;
        public const int GW_HWNDNEXT = 2;
        public const int GW_HWNDPREV = 3;
        public const int GW_OWNER = 4;
        public const int GW_CHILD = 5;

        public const int WM_NCCALCSIZE = 0x83;
        public const int WM_WINDOWPOSCHANGING = 0x46;
        public const int WM_PAINT = 0xF;
        public const int WM_CREATE = 0x1;
        public const int WM_NCCREATE = 0x81;
        public const int WM_NCPAINT = 0x85;
        public const int WM_PRINT = 0x317;
        public const int WM_DESTROY = 0x2;
        public const int WM_SHOWWINDOW = 0x18;
        public const int WM_SHARED_MENU = 0x1E2;
        public const int HC_ACTION = 0;
        public const int WH_CALLWNDPROC = 4;
        public const int GWL_WNDPROC = -4;

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr handle);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ReleaseDC(IntPtr handle, IntPtr hDC);

        [DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hwnd, char[] className, int maxCount);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindow(IntPtr hwnd, int uCmd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(IntPtr hwnd);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern int GetClientRect(IntPtr hwnd, ref RECT lpRect);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern int GetClientRect(IntPtr hwnd, [In, Out] ref Rectangle rect);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool UpdateWindow(IntPtr hwnd);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool InvalidateRect(IntPtr hwnd, ref Rectangle rect, bool bErase);

        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern bool ValidateRect(IntPtr hwnd, ref Rectangle rect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool GetWindowRect(IntPtr hWnd, [In, Out] ref Rectangle rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS
        {
            public RECT rgc;
            public WINDOWPOS wndpos;
        }
    }

    internal class SubClass : NativeWindow
    {
        public delegate int SubClassWndProcEventHandler(ref System.Windows.Forms.Message m);

        public event SubClassWndProcEventHandler SubClassedWndProc;

        private bool IsSubClassed = false;

        public SubClass(IntPtr Handle, bool _SubClass)
        {
            base.AssignHandle(Handle);
            IsSubClassed = _SubClass;
        }

        public bool SubClassed
        {
            get => IsSubClassed;
            set => IsSubClassed = value;
        }

        protected override void WndProc(ref Message m)
        {
            if (IsSubClassed)
            {
                if (OnSubClassedWndProc(ref m) != 0)
                {
                    return;
                }
            }
            base.WndProc(ref m);
        }

        public void CallDefaultWndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        public int HiWord(int Number)
        {
            return ((Number >> 16) & 0xffff);
        }

        public int LoWord(int Number)
        {
            return (Number & 0xffff);
        }

        public int MakeLong(int LoWord, int HiWord)
        {
            return (HiWord << 16) | (LoWord & 0xffff);
        }

        public IntPtr MakeLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }

        private int OnSubClassedWndProc(ref Message m)
        {
            if (SubClassedWndProc != null)
            {
                return SubClassedWndProc(ref m);
            }

            return 0;
        }
    }
}