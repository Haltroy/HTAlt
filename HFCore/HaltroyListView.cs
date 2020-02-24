using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HaltroyFramework
{
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
    #endregion

    #region RECT
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
    #endregion



    public class HaltroyListView : System.Windows.Forms.ListView
    {
        bool updating;
        int itemnumber;

        #region Imported User32.DLL functions
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern bool ValidateRect(IntPtr handle, ref RECT rect);
        #endregion

        public HaltroyListView()
        {
            Startup startup = new Startup();
            OwnerDraw = true;
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
            OwnerDraw = true;
            updating = true;
            itemnumber = iIndex;
            this.Update();
            updating = false;
        }
        private Color headerBackColor = Color.FromArgb(255, 235, 235, 235);
        private Color headerForeColor = Color.Black;

        [Category("Style"), Browsable(true), Description("The back color of the headers.")]
        public Color HeaderBackColor
        {
            get
            {
                OwnerDraw = true;
                return this.headerBackColor;
            }

            set
            {
                OwnerDraw = true;
                this.headerBackColor = value;
            }
        }
        [Category("Style"), Browsable(true), Description("The text color of the headers.")]
        public Color HeaderForeColor
        {
            get
            {
                OwnerDraw = true;
                return this.headerForeColor;
            }

            set
            {
                OwnerDraw = true;
                this.headerForeColor = value;
            }
        }
        private void this_DrawColumnHeaders(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            OwnerDraw = true;
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
                e.Graphics.DrawLine(new Pen(new SolidBrush(headerForeColor), 2), e.Bounds.X, e.Bounds.Y, e.Bounds.X + e.Bounds.Width, e.Bounds.Y);
                e.Graphics.DrawLine(new Pen(new SolidBrush(headerForeColor), 2), e.Bounds.X, e.Bounds.Y, e.Bounds.X, e.Bounds.Y + e.Bounds.Height);
                e.Graphics.DrawLine(new Pen(new SolidBrush(headerForeColor), 2), e.Bounds.X + e.Bounds.Width, e.Bounds.Y, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height);
                e.Graphics.DrawLine(new Pen(new SolidBrush(headerForeColor), 2), e.Bounds.X, e.Bounds.Y + e.Bounds.Height, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height);
            }
        }
        private void this_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            OwnerDraw = true;
            e.DrawDefault = true;
        }
        private void this_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            OwnerDraw = true;
            e.DrawDefault = true;
        }
        private void this_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            OwnerDraw = true;
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
                    RECT vrect = this.GetWindowRECT();
                    // validate the entire window				
                    ValidateRect(this.Handle, ref vrect);

                    //Invalidate only the new item
                    Invalidate(this.Items[itemnumber].Bounds);
                }

            }
            base.WndProc(ref messg);
        }


        #region private helperfunctions

        // Get the listview's rectangle and return it as a RECT structure
        private RECT GetWindowRECT()
        {
            RECT rect = new RECT();
            rect.left = this.Left;
            rect.right = this.Right;
            rect.top = this.Top;
            rect.bottom = this.Bottom;
            return rect;
        }

        #endregion
    }


}
