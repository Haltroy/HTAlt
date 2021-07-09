/*

Copyright © 2018 - 2021 haltroy

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/HTAlt/blob/master/LICENSE

*/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HTAlt.WinForms
{
    /// <summary>
    /// Customizable <see cref="System.Windows.Forms.ListView"/> Control.
    /// </summary>
    public class HTListView : System.Windows.Forms.ListView
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://htalt.haltroy.com/api/HTAlt.WinForms/HTListView");
        private readonly Version firstHTAltVersion = new Version("0.1.1.0");
        private readonly string description = "Customizable System.Windows.Forms.ListView Control.";

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

        private bool updating;
        private int itemnumber;

        #region WM - Window Messages

        public enum WM
        {
            WM_NULL = 0x0000,
            WM_CREATE = 0x0001,
            WM_DESTROY = 0x0002,
            WM_MOVE = 0x0003,
            WM_SIZE = 0x0005,
            WM_ACTIVATE = 0x0006,
            WM_SETFOCUS = 0x0007,
            WM_KILLFOCUS = 0x0008,
            WM_ENABLE = 0x000A,
            WM_SETREDRAW = 0x000B,
            WM_SETTEXT = 0x000C,
            WM_GETTEXT = 0x000D,
            WM_GETTEXTLENGTH = 0x000E,
            WM_PAINT = 0x000F,
            WM_CLOSE = 0x0010,
            WM_QUERYENDSESSION = 0x0011,
            WM_QUIT = 0x0012,
            WM_QUERYOPEN = 0x0013,
            WM_ERASEBKGND = 0x0014,
        }

        #endregion WM - Window Messages

        #region RECT

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        #endregion RECT

        #region Imported User32.DLL functions

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool ValidateRect(IntPtr handle, ref RECT rect);

        #endregion Imported User32.DLL functions

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool OwnerDraw
        {
            get => true;
            set => value = true;
        }

        public HTListView()
        {
            DrawItem += this_DrawItem;
            DrawSubItem += this_DrawSubItem;
            DrawColumnHeader += this_DrawColumnHeaders;
            ColumnWidthChanged += this_ColumnWidthChanged;
        }

        /// <summary>
        /// When adding an item in a loop, use this to update the newly added item.
        /// </summary>
        /// <param name="iIndex">Index of the item just added</param>
        public void UpdateItem(int iIndex)
        {
            updating = true;
            itemnumber = iIndex;
            Update();
            updating = false;
        }

        private Color headerBackColor = Color.FromArgb(255, 235, 235, 235);
        private Color headerForeColor = Color.Black;
        private Color overlayColor = Color.DodgerBlue;

        /// <summary>
        /// The back color of the headers.
        /// </summary>
        [Category("Style"), Browsable(true), Description("The back color of the headers.")]
        public Color HeaderBackColor
        {
            get => headerBackColor;

            set => headerBackColor = value;
        }

        /// <summary>
        /// The text color of the headers.
        /// </summary>
        [Category("Style"), Browsable(true), Description("The text color of the headers.")]
        public Color HeaderForeColor
        {
            get => headerForeColor;

            set => headerForeColor = value;
        }

        /// <summary>
        /// The overlay color.
        /// </summary>
        [Category("Style"), Browsable(true), Description("The overlay color.")]
        public Color OverlayColor
        {
            get => overlayColor;

            set => overlayColor = value;
        }

        private int _barThiccness = 2;

        /// <summary>
        /// The thickness of column header border.
        /// </summary>
        [Category("Style"), Browsable(true), Description("The thickness of column header border.")]
        public int HeaderBorderThickness
        {
            get => _barThiccness;
            set => _barThiccness = value;
        }

        private void this_DrawColumnHeaders(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (StringFormat sf = new StringFormat())
            {
                // Store the column text alignment, letting it default
                // to Left if it has not been set to Center or Right.
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        break;

                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        break;
                }

                // Draw the standard header background.
                e.Graphics.FillRectangle(new SolidBrush(headerBackColor), e.Bounds);
                // Draw the header text.
                e.Graphics.DrawString(e.Header.Text, e.Font,
                        new SolidBrush(headerForeColor), e.Bounds, sf);
                // Draw the header lines.
                e.Graphics.DrawLine(new Pen(new SolidBrush(overlayColor), _barThiccness), e.Bounds.X, e.Bounds.Y, e.Bounds.X + e.Bounds.Width, e.Bounds.Y);
                e.Graphics.DrawLine(new Pen(new SolidBrush(overlayColor), _barThiccness), e.Bounds.X, e.Bounds.Y, e.Bounds.X, e.Bounds.Y + e.Bounds.Height);
                e.Graphics.DrawLine(new Pen(new SolidBrush(overlayColor), _barThiccness), e.Bounds.X + e.Bounds.Width, e.Bounds.Y, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height);
                e.Graphics.DrawLine(new Pen(new SolidBrush(overlayColor), _barThiccness), e.Bounds.X, e.Bounds.Y + e.Bounds.Height, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height);
            }
        }

        private void this_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void this_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void this_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            Invalidate();
        }

        protected override void WndProc(ref Message messg)
        {
            if (updating)
            {
                // We do not want to erase the background, turn this message into a null-message
                if ((int)WM.WM_ERASEBKGND == messg.Msg)
                {
                    messg.Msg = (int)WM.WM_NULL;
                }
                else if ((int)WM.WM_PAINT == messg.Msg)
                {
                    RECT vrect = GetWindowRECT();
                    // validate the entire window
                    ValidateRect(Handle, ref vrect);

                    //Invalidate only the new item
                    Invalidate(Items[itemnumber].Bounds);
                }
            }
            base.WndProc(ref messg);
        }

        #region private helperfunctions

        // Get the listview's rectangle and return it as a RECT structure
        private RECT GetWindowRECT()
        {
            RECT rect = new RECT
            {
                left = Left,
                right = Right,
                top = Top,
                bottom = Bottom
            };
            return rect;
        }

        #endregion private helperfunctions
    }
}