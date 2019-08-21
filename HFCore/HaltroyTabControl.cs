using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace HaltroyFramework
{

    public class HaltroyTabControl : TabControl
    {
        /// <summary>
        ///     Format of the title of the TabPage
        /// </summary>
        private readonly StringFormat CenterSringFormat = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Center
        };

        private bool enableReposition = false;
        private TabPage donotmoveorremovethistab1 = null;
        private TabPage donotmoveorremovethistab2 = null;
        /// <summary>
        ///     The color of the active tab header
        /// </summary>
        private Color activeColor = Color.FromArgb(0, 122, 204);

        /// <summary>
        ///     The color of the background of the Tab
        /// </summary>
        private Color backTabColor = Color.FromArgb(28, 28, 28);

        /// <summary>
        ///     The color of the border of the control
        /// </summary>
        private Color borderColor = Color.FromArgb(30, 30, 30);

        /// <summary>
        ///     Message for the user before losing
        /// </summary>
        private string closingMessage;

        /// <summary>
        ///     The color of the tab header
        /// </summary>
        private Color headerColor = Color.FromArgb(45, 45, 48);

        /// <summary>
        ///     The color of the horizontal line which is under the headers of the tab pages
        /// </summary>
        private Color horizLineColor = Color.FromArgb(0, 122, 204);

        /// <summary>
        ///     A random page will be used to store a tab that will be deplaced in the run-time
        /// </summary>
        private TabPage predraggedTab;

        /// <summary>
        ///     The color of the text
        /// </summary>
        private Color textColor = Color.FromArgb(255, 255, 255);
        
        ///<summary>
        /// Shows closing buttons
        /// </summary> 
        public bool ShowClosingButton { get; set; }

        /// <summary>
        /// Selected tab text color
        /// </summary>
        public Color theselectedTextColor = Color.FromArgb(255, 255, 255);
        /// <summary>
        ///     Init
        /// </summary>
        public HaltroyTabControl()
        {
            Startup startup = new Startup();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw
                | ControlStyles.OptimizedDoubleBuffer,
                true);
            DoubleBuffered = true;
            SizeMode = TabSizeMode.Normal;
            ItemSize = new Size(240, 16);
            AllowDrop = true;
        }
        [Category("Style"), Browsable(true), Description("The color of the selected page")]
        public Color ActiveColor
        {
            get
            {
                return this.activeColor;
            }

            set
            {
                this.activeColor = value;
            }
        }

        [Category("Style"), Browsable(true), Description("The color of the background of the tab")]
        public Color BackTabColor
        {
            get
            {
                return this.backTabColor;
            }

            set
            {
                this.backTabColor = value;
            }
        }

        [Category("Frozen Tab"), Browsable(true), Description("Tab that cannot be removed or moved.")]
        public TabPage DoNotRemoveThisTab1
        {
            get
            {
                return this.donotmoveorremovethistab1;
            }

            set
            {
                this.donotmoveorremovethistab1 = value;
            }
        }

        [Category("Misc"),Browsable(true),Description("Enables repositioning the tab pages on runtime.")]
        public bool EnableRepositioning
        {
            get
            {
                return this.enableReposition;
            }
            set
            {
                this.enableReposition = value;
            }
        }

        [Category("Frozen Tab"), Browsable(true), Description("Tab that cannot be removed or moved.")]
        public TabPage DoNotRemoveThisTab2
        {
            get
            {
                return this.donotmoveorremovethistab2;
            }

            set
            {
                this.donotmoveorremovethistab2 = value;
            }
        }

        [Category("Style"), Browsable(true), Description("The color of the border of the control")]
        public Color BorderColor
        {
            get
            {
                return this.borderColor;
            }

            set
            {
                this.borderColor = value;
            }
        }

        /// <summary>
        ///     The message that will be shown before closing.
        /// </summary>
        [Category("Options"), Browsable(true), Description("The message that will be shown before closing.")]
        public string ClosingMessage
        {
            get
            {
                return this.closingMessage;
            }

            set
            {
                this.closingMessage = value;
            }
        }

        [Category("Style"), Browsable(true), Description("The color of the header.")]
        public Color HeaderColor
        {
            get
            {
                return this.headerColor;
            }

            set
            {
                this.headerColor = value;
            }
        }
        [Category("Style"), Browsable(true),
         Description("The color of the horizontal line which is located under the headers of the pages.")]
        public Color HorizontalLineColor
        {
            get
            {
                return this.horizLineColor;
            }

            set
            {
                this.horizLineColor = value;
            }
        }

        /// <summary>
        ///     Show a Yes/No message before closing?
        /// </summary>
        [Category("Options"), Browsable(true), Description("Show a Yes/No message before closing?")]
        public bool ShowClosingMessage { get; set; }

        [Category("Style"), Browsable(true), Description("The color of the title of the page")]
        public Color SelectedTextColor 
        {
            get
            {
                return this.theselectedTextColor;
            }

            set
            {
                this.theselectedTextColor = value;
            }
        }

        [Category("Style"), Browsable(true), Description("The color of the title of the page")]
        public Color TextColor
        {
            get
            {
                return this.textColor;
            }

            set
            {
                this.textColor = value;
            }
        }

        /// <summary>
        ///     Sets the Tabs on the top
        /// </summary>
        protected override void CreateHandle()
        {
            base.CreateHandle();
            Alignment = TabAlignment.Top;
        }

        /// <summary>
        ///     Drags the selected tab
        /// </summary>
        /// <param name="drgevent"></param>
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            var draggedTab = (TabPage)drgevent.Data.GetData(typeof(TabPage));
            var pointedTab = getPointedTab();

            if (ReferenceEquals(draggedTab, predraggedTab) && pointedTab != null)
            {
                drgevent.Effect = DragDropEffects.Move;

                if (!ReferenceEquals(pointedTab, draggedTab))
                {
                    if (draggedTab == donotmoveorremovethistab1|| pointedTab == donotmoveorremovethistab1) { }
                    else if (draggedTab == donotmoveorremovethistab2 || pointedTab == donotmoveorremovethistab2) { }
                    else
                    {
                        this.ReplaceTabPages(draggedTab, pointedTab);
                    }
                }
            }

            base.OnDragOver(drgevent);
        }

        /// <summary>
        ///     Handles the selected tab|closes the selected page if wanted.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            predraggedTab = getPointedTab();
            var p = e.Location;
            if (!this.ShowClosingButton)
            {
            }
            else
            {
                for (var i = 0; i < this.TabCount; i++)
                {
                    var r = this.GetTabRect(i);
                    r.Offset(r.Width - 15, 2);
                    r.Width = 10;
                    r.Height = 10;
                    if (!r.Contains(p))
                    {
                        continue;
                    }

                    if (this.ShowClosingMessage)
                    {
                        HaltroyMsgBox mesaj = new HaltroyMsgBox(this.Parent.Text, this.ClosingMessage, null, MessageBoxButtons.YesNo, this.backTabColor);
                        if (DialogResult.Yes == mesaj.ShowDialog())
                        {
                            if (this.SelectedTab == donotmoveorremovethistab1 || this.SelectedTab == donotmoveorremovethistab2) { }
                            else {
                               
                                this.TabPages[i].Controls.Clear();
                                this.TabPages[i].Dispose();
                                try
                                {
                                    this.TabPages.RemoveAt(i);
                                    if (i == 0) { this.SelectedIndex = i; }
                                    else
                                    {
                                        this.SelectedIndex = i - 1;
                                    }
                                }
                                catch { } //ignored , possibly double click
                            }
                        }
                    }
                    else
                    {
                        if (i == this.TabPages.IndexOf(donotmoveorremovethistab1) || i == this.TabPages.IndexOf(donotmoveorremovethistab2)) { }
                        else
                        {
                            if (this.SelectedTab == donotmoveorremovethistab1) { } else if(this.SelectedTab == donotmoveorremovethistab2) { }
                            else{
                                this.TabPages.RemoveAt(i);
                                if(i == 0) { this.SelectedIndex = i; } else { 
                                this.SelectedIndex = i - 1;
                                }
                            }
                        }
                    }
                }
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        ///     Holds the selected page until it sets down
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && predraggedTab != null)
            {
                if (enableReposition)
                {
                    this.DoDragDrop(predraggedTab, DragDropEffects.Move);
                }
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        ///     Abandons the selected tab
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            predraggedTab = null;
            base.OnMouseUp(e);
        }

        Color InvertMeAColour(Color ColourToInvert)
        {
            return Color.FromArgb(255 - ColourToInvert.R,
              255 - ColourToInvert.G, 255 - ColourToInvert.B);
        }
        /// <summary>
        ///     Draws the control
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var Drawer = g;

            Drawer.SmoothingMode = SmoothingMode.HighQuality;
            Drawer.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Drawer.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                Drawer.Clear(this.headerColor);
            try
            {
                SelectedTab.BorderStyle = BorderStyle.None;
            }
            catch
            {
                // ignored
            }

            for (var i = 0; i <= TabCount - 1; i++)
            {
                var Header = new Rectangle(
                    new Point(GetTabRect(i).Location.X + 2, GetTabRect(i).Location.Y),
                    new Size(GetTabRect(i).Width, GetTabRect(i).Height));
                var HeaderSize = new Rectangle(Header.Location, new Size(Header.Width, Header.Height));
                Brush ClosingColorBrush = new SolidBrush(InvertMeAColour(this.activeColor));

                if (i == SelectedIndex)
                {
                        Drawer.FillRectangle(new SolidBrush(this.headerColor), HeaderSize);

                    // Draws the back of the color when it is selected
                    Drawer.FillRectangle(
                        new SolidBrush(this.activeColor),
                        new Rectangle(Header.X - 5, Header.Y - 3, Header.Width, Header.Height + 5));

                    //Draws the Icon of the tab(if exists) and draws the text right next to it
                    //If image is null then draw text alone without shifthing.
                    if (ImageList == null || ImageList.Images[TabPages[i].ImageKey] == null)
                    {
                        Drawer.DrawString(
                        TabPages[i].Text,
                        Font,
                        new SolidBrush(this.theselectedTextColor),
                      HeaderSize,
                        this.CenterSringFormat);
                    }else
                    {
                        Drawer.DrawImage(
                            ImageList.Images[TabPages[i].ImageKey],
                            new Rectangle(HeaderSize.X, HeaderSize.Y, HeaderSize.Height, HeaderSize.Height));
                        Drawer.DrawString(
                        TabPages[i].Text,
                        Font,
                        new SolidBrush(this.theselectedTextColor),
                      new Rectangle(HeaderSize.X + HeaderSize.Height, HeaderSize.Y, HeaderSize.Width + HeaderSize.Height, HeaderSize.Height),
                        this.CenterSringFormat);
                    }
                    
                    // Draws the closing button
                    if (this.ShowClosingButton)
                    {
                        if (this.SelectedTab == donotmoveorremovethistab1) { }
                        else if (this.SelectedTab == donotmoveorremovethistab2) { }
                        else
                        {
                            e.Graphics.DrawString("X", Font, ClosingColorBrush, HeaderSize.Right - 17, 3);
                        }
                        }
                }
                else
                {
 
                    Drawer.FillRectangle(new SolidBrush(this.headerColor), HeaderSize);
                    if (ImageList == null || ImageList.Images[TabPages[i].ImageKey] == null)
                    {
                        Drawer.DrawString(
   TabPages[i].Text,
   Font,
   new SolidBrush(this.textColor),
   HeaderSize,
   this.CenterSringFormat);
                    }
                       else
                    {
                        Drawer.DrawImage(
                            ImageList.Images[TabPages[i].ImageKey],
                            new Rectangle(HeaderSize.X,HeaderSize.Y,HeaderSize.Height,HeaderSize.Height));
                                    
                        Drawer.DrawString(
                        TabPages[i].Text,
                        Font,
                        new SolidBrush(this.textColor),
                        new Rectangle(HeaderSize.X + HeaderSize.Height, HeaderSize.Y, HeaderSize.Width + HeaderSize.Height, HeaderSize.Height),
                        this.CenterSringFormat);
                    }
                    
                }
            }

            // Draws the horizontal line
            Drawer.DrawLine(new Pen(this.horizLineColor, 2), new Point(0, 19), new Point(Width, 19));

            // Draws the background of the tab control
                Brush backbrush = new SolidBrush(this.backTabColor);
                Drawer.FillRectangle(backbrush, new Rectangle(0, 20, Width, Height - 20));

            // Draws the border of the TabControl
            Drawer.DrawRectangle(new Pen(this.borderColor, 2), new Rectangle(0, 0, Width, Height));
            Drawer.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        /// <summary>
        ///     Gets the pointed tab
        /// </summary>
        /// <returns></returns>
        private TabPage getPointedTab()
        {
            for (var i = 0; i <= this.TabPages.Count - 1; i++)
            {
                if (this.GetTabRect(i).Contains(this.PointToClient(Cursor.Position)))
                {
                    return this.TabPages[i];
                }
            }

            return null;
        }

        /// <summary>
        ///     Swaps the two tabs
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Destination"></param>
        private void ReplaceTabPages(TabPage Source, TabPage Destination)
        {
            if (enableReposition)
            {
                var SourceIndex = this.TabPages.IndexOf(Source);
                var DestinationIndex = this.TabPages.IndexOf(Destination);

                this.TabPages[DestinationIndex] = Source;
                this.TabPages[SourceIndex] = Destination;
                if (Source == donotmoveorremovethistab1 || DestinationIndex == 0)
                {
                    Source.TabIndex = 0;
                }
                else if (Source == donotmoveorremovethistab2 || DestinationIndex == this.TabCount - 1)
                {
                    Source.TabIndex = this.TabCount - 1;
                }

                else
                {
                    if (this.SelectedIndex == SourceIndex)
                    {
                        this.SelectedIndex = DestinationIndex;
                    }
                    else if (this.SelectedIndex == DestinationIndex)
                    {
                        this.SelectedIndex = SourceIndex;
                    }
                }
                this.Refresh();
            }
        }

      
    }
}
