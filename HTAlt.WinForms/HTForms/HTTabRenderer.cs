using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HTAlt.WinForms
{
    /// <summary>Renderer that produces tabs that mimic the appearance of the Korot desktop Client.</summary>
    public class HTTabRenderer : HTBaseTabRenderer
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://haltroy.com/htalt/HTAlt.WinForms/HTTabRenderer");
        private readonly Version firstHTAltVersion = new Version("0.1.4.0");
        private readonly string originProjectName = "EasyTabs";
        private readonly Uri originProject = new Uri("https://github.com/lstratman/EasyTabs");
        private readonly string description = "Renderer that produces tabs that mimic the appearance of the Korot desktop Client.";

        /// <summary>
        /// This control's wiki link.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's wiki link.")]
        public new Uri WikiLink => wikiLink;

        /// <summary>
        /// This control's origin project name.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's origin project name.")]
        public new string OriginProjectName => originProjectName;

        /// <summary>
        /// This control's origin project link.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's origin project link.")]
        public new Uri OriginProjectLink => originProject;

        /// <summary>
        /// This control's first appearance version for HTAlt.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's first appearance version for HTAlt.")]
        public new Version FirstHTAltVersion => firstHTAltVersion;

        /// <summary>
        /// This control's description.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's description.")]
        public new string Description => description;

        /// <summary>
        /// Information about this control's project.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("Information about this control's project.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public new HTInfo ProjectInfo => info;

        #endregion HTControls

        /// <summary>
        /// Overlay background color.
        /// </summary>
        protected Color OverlayBackgroundColor = Color.FromArgb(255, 255, 255, 255);

        /// <summary>
        /// Overall background color.
        /// </summary>
        protected Color BackgroundColor = Color.FromArgb(255, 235, 235, 235);

        /// <summary>
        /// Overall foreground color.
        /// </summary>
        protected Color ForegroundColor = Color.FromArgb(255, 0, 0, 0);

        /// <summary>
        /// Overall overlay color.
        /// </summary>
        protected Color SecondaryColor = Color.FromArgb(255, 20, 157, 204);

        /// <summary>Constructor that initializes the various resources that we use in rendering.</summary>
        /// <param name="parentWindow">Parent window that this renderer belongs to.</param>
        public HTTabRenderer(HTTitleTabs parentWindow)
            : base(parentWindow)
        {
            // Set the colors
            OverlayBackColor = OverlayBackgroundColor;
            BackColor = BackgroundColor;
            ForeColor = ForegroundColor;
            OverlayColor = SecondaryColor;

            // Set the various positioning properties
            CloseButtonMarginTop = 6;
            CloseButtonMarginLeft = 2;
            AddButtonMarginTop = 7;
            AddButtonMarginLeft = -1;
            CaptionMarginTop = 6;
            IconMarginTop = 7;
            IconMarginRight = 5;
            AddButtonMarginRight = 5;
        }

        /// <summary>
        /// Refreshes colors.
        /// </summary>
        /// <param name="ReRender">True to re-render the overlay.</param>
        public void RefreshColors(bool ReRender = false)
        {
            BackColor = BackgroundColor;
            ForeColor = ForegroundColor;
            OverlayColor = SecondaryColor;
            OverlayBackColor = OverlayBackgroundColor;
            if (ReRender) { _parentWindow._overlay.Render(true); }
        }

        /// <summary>
        /// Changes colors if one of them is different than the applied. Then re-renders the overlay.
        /// </summary>
        /// <param name="_BackColor">Background color.</param>
        /// <param name="_ForeColor">Foreground color.</param>
        /// <param name="_OverlayColor">Overlay color.</param>
        /// <param name="_OverlayBackColor">Background color of overlay.</param>
        public void ApplyColors(Color _BackColor, Color _ForeColor, Color _OverlayColor, Color _OverlayBackColor)
        {
            bool Apply = false;
            if (BackgroundColor != _BackColor)
            {
                BackgroundColor = _BackColor;
                Apply = true;
            }
            if (OverlayBackgroundColor != _OverlayBackColor)
            {
                OverlayBackgroundColor = _OverlayBackColor;
                Apply = true;
            }
            if (ForegroundColor != _ForeColor)
            {
                ForegroundColor = _ForeColor;
                Apply = true;
            }
            if (SecondaryColor != _OverlayColor)
            {
                SecondaryColor = _OverlayColor;
                Apply = true;
            }
            if (Apply) { RefreshColors(); }
            if (_parentWindow != null)
            {
                if (_parentWindow._overlay != null)
                {
                    _parentWindow._overlay.Render(true);
                }
            }
        }

        /// <summary>Since Chrome tabs overlap, we set this property to the amount that they overlap by.</summary>
        public override int OverlapWidth => 15;
    }

    /// <summary>
    /// Provides the base functionality for any tab renderer, taking care of actually rendering and detecting whether the cursor is over a tab.  Any custom
    /// tab renderer needs to inherit from this class, just as <see cref="HTTabRenderer" /> does.
    /// </summary>
    public abstract class HTBaseTabRenderer
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://haltroy.com/htalt/HTAlt.WinForms/HTBaseTabRenderer");
        private readonly Version firstHTAltVersion = new Version("0.1.4.0");
        private readonly string originProjectName = "EasyTabs";
        private readonly Uri originProject = new Uri("https://github.com/lstratman/EasyTabs");
        private readonly string description = "Provides the base functionality for any tab renderer, taking care of actually rendering and detecting whether the cursor is over a tab.  Any custom tab renderer needs to inherit from this class, just as HTTabRenderer does.";

        /// <summary>
        /// This control's wiki link.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's wiki link.")]
        public Uri WikiLink => wikiLink;

        /// <summary>
        /// This control's origin project name.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's origin project name.")]
        public string OriginProjectName => originProjectName;

        /// <summary>
        /// This control's origin project link.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's origin project link.")]
        public Uri OriginProjectLink => originProject;

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

        /// <summary>Area on the screen where the add button is located.</summary>
        protected Rectangle _addButtonArea;

        /// <summary>When the user is dragging a tab, this represents the point where the user first clicked to start the drag operation.</summary>
        protected Point? _dragStart = null;

        /// <summary>Flag indicating whether or not a tab is being repositioned.</summary>
        protected bool _isTabRepositioning = false;

        /// <summary>Maximum area on the screen that tabs may take up for this application.</summary>
        protected Rectangle _maxTabArea = new Rectangle();

        /// <summary>The parent window that this renderer instance belongs to.</summary>
        protected HTTitleTabs _parentWindow;

        /// <summary>The number of tabs that were present when we last rendered; used to determine whether or not we need to redraw tab instances.</summary>
        protected int _previousTabCount;

        /// <summary>Flag indicating whether or not rendering has been suspended while we perform some operation.</summary>
        protected bool _suspendRendering = false;

        /// <summary>When the user is dragging a tab, this represents the horizontal offset within the tab where the user clicked to start the drag operation.</summary>
        protected int? _tabClickOffset = null;

        /// <summary>The width of the content area that we should use for each tab.</summary>
        protected int _tabContentWidth;

        /// <summary>Flag indicating whether or not a tab was being repositioned.</summary>
        protected bool _wasTabRepositioning = false;

        /// <summary>
        /// Overall background color.
        /// </summary>
        protected Color BackColor = Color.FromArgb(255, 235, 235, 235);

        /// <summary>
        /// Overall foreground color.
        /// </summary>
        protected Color ForeColor = Color.FromArgb(255, 0, 0, 0);

        /// <summary>
        /// Overall overlay color.
        /// </summary>
        protected Color OverlayColor = Color.FromArgb(255, 20, 157, 204);

        /// <summary>
        /// Background color of the control box.
        /// </summary>
        protected Color OverlayBackColor = Color.FromArgb(255, 255, 255, 255);

        /// <summary>
        /// Color to use for New Tab button.
        /// </summary>
        protected TabColors NewTabButton = TabColors.OverlayColor;

        /// <summary>
        /// Color to use for Close Tab button.
        /// </summary>
        protected TabColors CloseTabButton = TabColors.OverlayColor;

        /// <summary>
        /// True to draw background.
        /// </summary>
        protected bool DrawBackground = false;

        /// <summary>Default constructor that initializes the <see cref="_parentWindow" /> and <see cref="ShowAddButton" /> properties.</summary>
        /// <param name="parentWindow">The parent window that this renderer instance belongs to.</param>
        protected HTBaseTabRenderer(HTTitleTabs parentWindow)
        {
            _parentWindow = parentWindow;
            ShowAddButton = true;
            TabRepositionDragDistance = 10;
            TabTearDragDistance = 10;

            parentWindow.Tabs.CollectionModified += Tabs_CollectionModified;

            if (parentWindow._overlay != null)
            {
                parentWindow._overlay.MouseMove += Overlay_MouseMove;
                parentWindow._overlay.MouseUp += Overlay_MouseUp;
                parentWindow._overlay.MouseDown += Overlay_MouseDown;
            }
        }

        private readonly int _TabHeight = 29;

        /// <summary>
        /// Width of tab.
        /// </summary>
        protected int TabWidth = 175;

        /// <summary>Height of the tab content area.</summary>
        public virtual int TabHeight => _TabHeight;

        /// <summary>Flag indicating whether or not we should display the add button.</summary>
        public bool ShowAddButton
        {
            get;
            set;
        }

        /// <summary>Amount of space we should put to the left of the caption when rendering the content area of the tab.</summary>
        public int CaptionMarginLeft
        {
            get;
            set;
        }

        /// <summary>Amount of space that we should leave to the right of the caption when rendering the content area of the tab.</summary>
        public int CaptionMarginRight
        {
            get;
            set;
        }

        /// <summary>Amount of space that we should leave between the top of the content area and the top of the caption text.</summary>
        public int CaptionMarginTop
        {
            get;
            set;
        }

        /// <summary>Amount of space we should put to the left of the tab icon when rendering the content area of the tab.</summary>
        public int IconMarginLeft
        {
            get;
            set;
        }

        /// <summary>Amount of space that we should leave to the right of the icon when rendering the content area of the tab.</summary>
        public int IconMarginRight
        {
            get;
            set;
        }

        /// <summary>Amount of space that we should leave between the top of the content area and the top of the icon.</summary>
        public int IconMarginTop
        {
            get;
            set;
        }

        /// <summary>Amount of space that we should put to the left of the close button when rendering the content area of the tab.</summary>
        public int CloseButtonMarginLeft
        {
            get;
            set;
        }

        /// <summary>Amount of space that we should leave to the right of the close button when rendering the content area of the tab.</summary>
        public int CloseButtonMarginRight
        {
            get;
            set;
        }

        /// <summary>Amount of space that we should leave between the top of the content area and the top of the close button.</summary>
        public int CloseButtonMarginTop
        {
            get;
            set;
        }

        /// <summary>Amount of space that we should put to the left of the add tab button when rendering the content area of the tab.</summary>
        public int AddButtonMarginLeft
        {
            get;
            set;
        }

        /// <summary>Amount of space that we should leave to the right of the add tab button when rendering the content area of the tab.</summary>
        public int AddButtonMarginRight
        {
            get;
            set;
        }

        /// <summary>Amount of space that we should leave between the top of the content area and the top of the add tab button.</summary>
        public int AddButtonMarginTop
        {
            get;
            set;
        }

        /// <summary>
        /// If the renderer overlaps the tabs (like Chrome), this is the width that the tabs should overlap by.  For renderers that do not overlap tabs (like
        /// Firefox), this should be left at 0.
        /// </summary>
        public virtual int OverlapWidth => 0;

        /// <summary>Horizontal distance that a tab must be dragged before it starts to be repositioned.</summary>
        public int TabRepositionDragDistance
        {
            get;
            set;
        }

        /// <summary>Distance that a user must drag a tab outside of the tab area before it shows up as "torn" from its parent window.</summary>
        public int TabTearDragDistance
        {
            get;
            set;
        }

        /// <summary>Flag indicating whether or not a tab is being repositioned.</summary>
        public bool IsTabRepositioning
        {
            get => _isTabRepositioning;

            internal set
            {
                _isTabRepositioning = value;

                if (!_isTabRepositioning)
                {
                    _dragStart = null;
                }
            }
        }

        /// <summary>Width of the content area of the tabs.</summary>
        public int TabContentWidth => _tabContentWidth;

        /// <summary>Maximum area that the tabs can occupy.  Excludes the add button.</summary>
        public Rectangle MaxTabArea => _maxTabArea;

        /// <summary>Initialize the <see cref="_dragStart" /> and <see cref="_tabClickOffset" /> fields in case the user starts dragging a tab.</summary>
        /// <param name="sender">Object from which this event originated.</param>
        /// <param name="e">Arguments associated with the event.</param>
        protected internal virtual void Overlay_MouseDown(object sender, MouseEventArgs e)
        {
            _wasTabRepositioning = false;
            _dragStart = e.Location;
            _tabClickOffset = _parentWindow._overlay.GetRelativeCursorPosition(e.Location).X - _parentWindow.SelectedTab.Area.Location.X;
        }

        /// <summary>
        /// End the drag operation by resetting the <see cref="_dragStart" /> and <see cref="_tabClickOffset" /> fields and setting
        /// <see cref="IsTabRepositioning" /> to false.
        /// </summary>
        /// <param name="sender">Object from which this event originated.</param>
        /// <param name="e">Arguments associated with the event.</param>
        protected internal virtual void Overlay_MouseUp(object sender, MouseEventArgs e)
        {
            _dragStart = null;
            _tabClickOffset = null;

            _wasTabRepositioning = IsTabRepositioning;

            IsTabRepositioning = false;

            if (_wasTabRepositioning)
            {
                _parentWindow._overlay.Render(true);
            }
        }

        /// <summary>
        /// If the user is dragging the mouse, see if they have passed the <see cref="TabRepositionDragDistance" /> threshold and, if so, officially begin the
        /// tab drag operation.
        /// </summary>
        /// <param name="sender">Object from which this event originated.</param>
        /// <param name="e">Arguments associated with the event.</param>
        protected internal virtual void Overlay_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragStart != null && !IsTabRepositioning &&
                (Math.Abs(e.X - _dragStart.Value.X) > TabRepositionDragDistance || Math.Abs(e.Y - _dragStart.Value.Y) > TabRepositionDragDistance))
            {
                IsTabRepositioning = true;
            }
        }

        /// <summary>
        /// Width of New Tab button.
        /// </summary>
        protected int addButtonWidth = 34;

        /// <summary>
        /// Height of New Tab button.
        /// </summary>
        protected int addButtonHeight = 18;

        /// <summary>
        /// When items are added to the tabs collection, we need to ensure that the <see cref="_parentWindow" />'s minimum width is set so that we can display at
        /// least each tab and its close buttons.
        /// </summary>
        /// <param name="sender">List of tabs in the <see cref="_parentWindow" />.</param>
        /// <param name="e">Arguments associated with the event.</param>
        private void Tabs_CollectionModified(object sender, ListModificationEventArgs e)
        {
            ListWithEvents<HTTitleTab> tabs = (ListWithEvents<HTTitleTab>)sender;

            if (tabs.Count == 0)
            {
                return;
            }

            int minimumWidth = tabs.Sum(
                tab => (LeftRightWidth * 2) + (tab.ShowCloseButton
                           ? tab.CloseButtonArea.Width + CloseButtonMarginLeft
                           : 0));

            minimumWidth += OverlapWidth;

            minimumWidth += (_parentWindow.ControlBox
                                ? SystemInformation.CaptionButtonSize.Width
                                : 0) -
                            (_parentWindow.MinimizeBox
                                ? SystemInformation.CaptionButtonSize.Width
                                : 0) -
                            (_parentWindow.MaximizeBox
                                ? SystemInformation.CaptionButtonSize.Width
                                : 0) + (ShowAddButton
                                ? addButtonWidth + AddButtonMarginLeft +
                                  AddButtonMarginRight
                                : 0);

            _parentWindow.MinimumSize = new Size(minimumWidth, 0);
        }

        /// <summary>
        /// Called from the <see cref="_parentWindow" /> to determine which, if any, of the <paramref name="tabs" /> the <paramref name="cursor" /> is
        /// over.
        /// </summary>
        /// <param name="tabs">The list of tabs that we should check.</param>
        /// <param name="cursor">The relative position of the cursor within the window.</param>
        /// <returns>The tab within <paramref name="tabs" /> that the <paramref name="cursor" /> is over; if none, then null is returned.</returns>
        public virtual HTTitleTab OverTab(IEnumerable<HTTitleTab> tabs, Point cursor)
        {
            HTTitleTab overTab = null;

            foreach (HTTitleTab tab in tabs.Where(tab => tab.TabImage != null))
            {
                // We have to loop through each of the tabs in turn and check their status; if the tabs overlap, then their areas overlap as well, which means
                // that we may find see that the cursor is over an inactive tab, but we need to check the active tabs as well, since they may overlap their
                // areas and take precedence.
                if (tab.Active && IsOverTab(tab, cursor))
                {
                    overTab = tab;
                    break;
                }

                if (IsOverTab(tab, cursor))
                {
                    overTab = tab;
                }
            }

            return overTab;
        }

        /// <summary>
        /// Helper method to detect whether the <paramref name="cursor" /> is within the given <paramref name="area" /> and, if it is, whether it is over a
        /// non-transparent pixel in the given <paramref name="image" />.
        /// </summary>
        /// <param name="area">Screen area that we should check to see if the <paramref name="cursor" /> is within.</param>
        /// <param name="image">
        /// Image contained within <paramref name="area" /> that we should check to see if the <paramref name="cursor" /> is over a non-
        /// transparent pixel.
        /// </param>
        /// <param name="cursor">Current location of the cursor.</param>
        /// <returns>
        /// True if the <paramref name="cursor" /> is within the given <paramref name="area" /> and is over a non-transparent pixel in the
        /// <paramref name="image" />.
        /// </returns>
        protected bool IsOverNonTransparentArea(Rectangle area, Bitmap image, Point cursor)
        {
            if (!area.Contains(cursor))
            {
                return false;
            }

            // Get the relative location of the cursor within the image and then get the RGBA value of that pixel
            Point relativePoint = new Point(cursor.X - area.Location.X, cursor.Y - area.Location.Y);
            Color pixel = image.GetPixel(relativePoint.X, relativePoint.Y);

            // If the alpha channel of the pixel is greater than 0, then we're considered "over" the image
            return pixel.A > 0;
        }

        /// <summary>
        /// Helper method to detect whether the <paramref name="cursor" /> is within the given <paramref name="area" />.
        /// </summary>
        /// <param name="area">Screen area that we should check to see if the <paramref name="cursor" /> is within.</param>
        /// <param name="cursor">Current location of the cursor.</param>
        /// <returns>
        /// True if the <paramref name="cursor" /> is within the given <paramref name="area" />.
        /// </returns>
        protected bool IsOverArea(Rectangle area, Point cursor)
        {
            return area.Contains(cursor);
        }

        /// <summary>Tests whether the <paramref name="cursor" /> is hovering over the add tab button.</summary>
        /// <param name="cursor">Current location of the cursor.</param>
        /// <returns>
        /// True if the <paramref name="cursor" /> is within <see cref="_addButtonArea" /> and is over a non-transparent pixel, false otherwise.
        /// </returns>
        public virtual bool IsOverAddButton(Point cursor)
        {
            return !_wasTabRepositioning && IsOverArea(_addButtonArea, cursor);
        }

        /// <summary>Tests whether the <paramref name="cursor" /> is hovering over the given <paramref name="tab" />.</summary>
        /// <param name="tab">Tab that we are to see if the cursor is hovering over.</param>
        /// <param name="cursor">Current location of the cursor.</param>
        /// <returns>
        /// True if the <paramref name="cursor" /> is within the <see cref="HTTitleTab.Area" /> of the <paramref name="tab" /> and is over a non- transparent
        /// pixel of <see cref="HTTitleTab.TabImage" />, false otherwise.
        /// </returns>
        protected virtual bool IsOverTab(HTTitleTab tab, Point cursor)
        {
            return IsOverNonTransparentArea(tab.Area, tab.TabImage, cursor);
        }

        /// <summary>Checks to see if the <paramref name="cursor" /> is over the <see cref="HTTitleTab.CloseButtonArea" /> of the given <paramref name="tab" />.</summary>
        /// <param name="tab">The tab whose <see cref="HTTitleTab.CloseButtonArea" /> we are to check to see if it contains <paramref name="cursor" />.</param>
        /// <param name="cursor">Current position of the cursor.</param>
        /// <returns>True if the <paramref name="tab" />'s <see cref="HTTitleTab.CloseButtonArea" /> contains <paramref name="cursor" />, false otherwise.</returns>
        public virtual bool IsOverCloseButton(HTTitleTab tab, Point cursor)
        {
            if (!tab.ShowCloseButton || _wasTabRepositioning)
            {
                return false;
            }

            Rectangle absoluteCloseButtonArea = new Rectangle(
                tab.Area.X + tab.CloseButtonArea.X, tab.Area.Y + tab.CloseButtonArea.Y, tab.CloseButtonArea.Width, tab.CloseButtonArea.Height);

            return absoluteCloseButtonArea.Contains(cursor);
        }

        /// <summary>
        /// Width of Left/Right image.
        /// </summary>
        protected int LeftRightWidth = 15;

        /// <summary>Renders the list of <paramref name="tabs" /> to the screen using the given <paramref name="graphicsContext" />.</summary>
        /// <param name="tabs">List of tabs that we are to render.</param>
        /// <param name="graphicsContext">Graphics context that we should use while rendering.</param>
        /// <param name="cursor">Current location of the cursor on the screen.</param>
        /// <param name="forceRedraw">Flag indicating whether or not the redraw should be forced.</param>
        /// <param name="offset">Offset within <paramref name="graphicsContext" /> that the tabs should be rendered.</param>
        public virtual void Render(List<HTTitleTab> tabs, Graphics graphicsContext, Point offset, Point cursor, bool forceRedraw = false)
        {
            if (_suspendRendering)
            {
                return;
            }

            if (tabs == null || tabs.Count == 0)
            {
                return;
            }

            Point screenCoordinates = _parentWindow.PointToScreen(_parentWindow.ClientRectangle.Location);

            // Calculate the maximum tab area, excluding the add button and any minimize/maximize/close buttons in the window
            _maxTabArea.Location = new Point(SystemInformation.BorderSize.Width + offset.X + screenCoordinates.X, offset.Y + screenCoordinates.Y);
            _maxTabArea.Width = _parentWindow.ClientRectangle.Width - offset.X -
                                (ShowAddButton
                                    ? addButtonWidth + AddButtonMarginLeft +
                                      AddButtonMarginRight
                                    : 0) - (tabs.Count() * OverlapWidth) -
                                (_parentWindow.ControlBox
                                    ? SystemInformation.CaptionButtonSize.Width
                                    : 0) -
                                (_parentWindow.MinimizeBox
                                    ? SystemInformation.CaptionButtonSize.Width
                                    : 0) -
                                (_parentWindow.MaximizeBox
                                    ? SystemInformation.CaptionButtonSize.Width
                                    : 0);
            _maxTabArea.Height = TabHeight;

            // Get the width of the content area for each tab by taking the parent window's client width, subtracting the left and right border widths and the
            // add button area (if applicable) and then dividing by the number of tabs
            int tabContentWidth = Math.Min(TabWidth, Convert.ToInt32(Math.Floor(Convert.ToDouble(_maxTabArea.Width / tabs.Count))));

            // Determine if we need to redraw the TabImage properties for each tab by seeing if the content width that we calculated above is equal to content
            // width we had in the previous rendering pass
            bool redraw = tabContentWidth != _tabContentWidth || forceRedraw;

            if (redraw)
            {
                _tabContentWidth = tabContentWidth;
            }

            int i = tabs.Count - 1;
            List<Tuple<HTTitleTab, Rectangle>> activeTabs = new List<Tuple<HTTitleTab, Rectangle>>();
            // Render the background image
            if (DrawBackground)
            {
                int s = SystemInformation.CaptionButtonSize.Width * 5;
                graphicsContext.FillRectangle(new SolidBrush(OverlayBackColor), new Rectangle(offset.X, offset.Y, _parentWindow.Width - s, TabHeight));
            }

            int selectedIndex = tabs.FindIndex(t => t.Active);

            if (selectedIndex != -1)
            {
                HTTitleTab selectedTab = tabs[selectedIndex];
                Rectangle tabArea = new Rectangle(
                    SystemInformation.BorderSize.Width + offset.X +
                    selectedIndex * (tabContentWidth + (LeftRightWidth * 2) - OverlapWidth),
                    offset.Y, tabContentWidth + (LeftRightWidth * 2),
                    TabHeight);

                if (IsTabRepositioning && _tabClickOffset != null)
                {
                    // Make sure that the user doesn't move the tab past the beginning of the list or the outside of the window
                    tabArea.X = cursor.X - _tabClickOffset.Value;
                    tabArea.X = Math.Max(SystemInformation.BorderSize.Width + offset.X, tabArea.X);
                    tabArea.X =
                        Math.Min(
                            SystemInformation.BorderSize.Width + (_parentWindow.WindowState == FormWindowState.Maximized
                                ? _parentWindow.ClientRectangle.Width - (_parentWindow.ControlBox
                                    ? SystemInformation.CaptionButtonSize.Width
                                    : 0) -
                                  (_parentWindow.MinimizeBox
                                      ? SystemInformation.CaptionButtonSize.Width
                                      : 0) -
                                  (_parentWindow.MaximizeBox
                                      ? SystemInformation.CaptionButtonSize.Width
                                      : 0)
                                : _parentWindow.ClientRectangle.Width) - tabArea.Width, tabArea.X);

                    int dropIndex = 0;

                    // Figure out which slot the active tab is being "dropped" over
                    if (tabArea.X - SystemInformation.BorderSize.Width - offset.X - TabRepositionDragDistance > 0)
                    {
                        dropIndex =
                            Math.Min(
                                Convert.ToInt32(
                                    Math.Round(
                                        Convert.ToDouble(tabArea.X - SystemInformation.BorderSize.Width - offset.X - TabRepositionDragDistance) /
                                        Convert.ToDouble(tabArea.Width - OverlapWidth))), tabs.Count - 1);
                    }

                    // If the tab has been moved over another slot, move the tab object in the window's tab list
                    if (dropIndex != selectedIndex)
                    {
                        HTTitleTab tab = tabs[selectedIndex];

                        _parentWindow.Tabs.SuppressEvents();
                        _parentWindow.Tabs.Remove(tab);
                        _parentWindow.Tabs.Insert(dropIndex, tab);
                        _parentWindow.Tabs.ResumeEvents();
                    }
                }

                activeTabs.Add(new Tuple<HTTitleTab, Rectangle>(tabs[selectedIndex], tabArea));
            }

            // Loop through the tabs in reverse order since we need the ones farthest on the left to overlap those to their right
            foreach (HTTitleTab tab in ((IEnumerable<HTTitleTab>)tabs).Reverse())
            {
                Rectangle tabArea =
                    new Rectangle(
                        SystemInformation.BorderSize.Width + offset.X +
                        (i * (tabContentWidth + (LeftRightWidth * 2) - OverlapWidth)),
                        offset.Y, tabContentWidth + (LeftRightWidth * 2),
                        TabHeight);

                // If we need to redraw the tab image, null out the property so that it will be recreated in the call to Render() below
                if (redraw)
                {
                    tab.TabImage = null;
                }

                // In this first pass, we only render the inactive tabs since we need the active tabs to show up on top of everything else
                if (!tab.Active)
                {
                    Render(graphicsContext, tab, tabArea, cursor);
                }

                i--;
            }

            // In the second pass, render all of the active tabs identified in the previous pass
            foreach (Tuple<HTTitleTab, Rectangle> tab in activeTabs)
            {
                Render(graphicsContext, tab.Item1, tab.Item2, cursor);
            }

            _previousTabCount = tabs.Count;

            // Render the add tab button to the screen
            if (ShowAddButton && !IsTabRepositioning)
            {
                _addButtonArea =
                    new Rectangle(
                        (_previousTabCount *
                         (tabContentWidth + (LeftRightWidth * 2) - OverlapWidth)) +
                        LeftRightWidth + AddButtonMarginLeft + offset.X,
                        AddButtonMarginTop + offset.Y, addButtonWidth, addButtonHeight);

                bool cursorOverAddButton = IsOverAddButton(cursor);
                Color newTabColor = OverlayColor;
                if (NewTabButton == TabColors.BackColor)
                {
                    newTabColor = cursorOverAddButton ? HTAlt.Tools.ShiftBrightness(BackColor, 20, false) : BackColor;
                }
                else if (NewTabButton == TabColors.ForeColor)
                {
                    newTabColor = cursorOverAddButton ? HTAlt.Tools.ShiftBrightness(ForeColor, 20, false) : ForeColor;
                }
                else if (NewTabButton == TabColors.OverlayColor)
                {
                    newTabColor = cursorOverAddButton ? HTAlt.Tools.ShiftBrightness(OverlayColor, 20, false) : OverlayColor;
                }
                else if (NewTabButton == TabColors.OverlayBackColor)
                {
                    newTabColor = cursorOverAddButton ? HTAlt.Tools.ShiftBrightness(OverlayBackColor, 20, false) : OverlayBackColor;
                }
                graphicsContext.DrawString("+", new Font(_parentWindow.Font.FontFamily, 25, FontStyle.Bold), new SolidBrush(newTabColor), _addButtonArea.X, _addButtonArea.Y - (_addButtonArea.Height / 2));
            }
        }

        /// <summary>Internal method for rendering an individual <paramref name="tab" /> to the screen.</summary>
        /// <param name="graphicsContext">Graphics context to use when rendering the tab.</param>
        /// <param name="tab">Individual tab that we are to render.</param>
        /// <param name="area">Area of the screen that the tab should be rendered to.</param>
        /// <param name="cursor">Current position of the cursor.</param>
        protected virtual void Render(Graphics graphicsContext, HTTitleTab tab, Rectangle area, Point cursor)
        {
            if (_suspendRendering)
            {
                return;
            }

            // If we need to redraw the tab image
            if (tab.TabImage == null)
            {
                // We render the tab to an internal property so that we don't necessarily have to redraw it in every rendering pass, only if its width or
                // status have changed
                tab.TabImage = new Bitmap(area.Width <= 0 ? 1 : area.Width, TabHeight <= 0 ? 1 : TabHeight);

                using (Graphics tabGraphicsContext = Graphics.FromImage(tab.TabImage))
                {
                    // Draw the left, center, and right portions of the tab
                    Color backBrush = BackColor;
                    if (!tab.UseDefaultBackColor)
                    {
                        backBrush = tab.Active ? tab.BackColor : HTAlt.Tools.ShiftBrightness(tab.BackColor, 20, false);
                    }
                    else
                    {
                        backBrush = tab.Active ? BackColor : HTAlt.Tools.ShiftBrightness(BackColor, 20, false);
                    }
                    tabGraphicsContext.FillRectangle(new SolidBrush(backBrush), new Rectangle(0, 0, LeftRightWidth, TabHeight));
                    tabGraphicsContext.FillRectangle(new SolidBrush(backBrush), new Rectangle(LeftRightWidth, 0, _tabContentWidth, TabHeight));
                    tabGraphicsContext.FillRectangle(new SolidBrush(backBrush), new Rectangle(LeftRightWidth + _tabContentWidth, 0, LeftRightWidth, TabHeight));
                    // Draw the close button
                    if (tab.ShowCloseButton)
                    {
                        tab.CloseButtonArea = new Rectangle(
                        area.Width - LeftRightWidth - CloseButtonMarginRight - closeButtonWH, CloseButtonMarginTop, closeButtonWH,
                        closeButtonWH);
                        Color closeButtonColor = OverlayColor;
                        if (CloseTabButton == TabColors.BackColor)
                        {
                            closeButtonColor = tab.UseDefaultBackColor ? (IsOverCloseButton(tab, cursor) ? HTAlt.Tools.ShiftBrightness(BackColor, 20, false) : BackColor) : (IsOverCloseButton(tab, cursor) ? HTAlt.Tools.ShiftBrightness(tab.BackColor, 20, false) : tab.BackColor);
                        }
                        else if (CloseTabButton == TabColors.ForeColor)
                        {
                            closeButtonColor = tab.UseDefaultForeColor ? (IsOverCloseButton(tab, cursor) ? HTAlt.Tools.ShiftBrightness(ForeColor, 20, false) : ForeColor) : (IsOverCloseButton(tab, cursor) ? HTAlt.Tools.ShiftBrightness(tab.ForeColor, 20, false) : tab.ForeColor);
                        }
                        else if (CloseTabButton == TabColors.OverlayColor)
                        {
                            closeButtonColor = tab.UseDefaultOverlayColor ? (IsOverCloseButton(tab, cursor) ? HTAlt.Tools.ShiftBrightness(OverlayColor, 20, false) : OverlayColor) : (IsOverCloseButton(tab, cursor) ? HTAlt.Tools.ShiftBrightness(tab.OverlayColor, 20, false) : tab.OverlayColor);
                        }
                        else if (CloseTabButton == TabColors.OverlayBackColor)
                        {
                            closeButtonColor = tab.UseDefaultOverlayBackColor ? (IsOverCloseButton(tab, cursor) ? HTAlt.Tools.ShiftBrightness(OverlayBackColor, 20, false) : OverlayBackColor) : (IsOverCloseButton(tab, cursor) ? HTAlt.Tools.ShiftBrightness(tab.OverlayBackColor, 20, false) : tab.OverlayBackColor);
                        }
                        tabGraphicsContext.DrawString("X", new Font(_parentWindow.Font.FontFamily, 12, FontStyle.Bold), new SolidBrush(closeButtonColor), tab.CloseButtonArea);
                    }
                    // Draw secondary image
                    if (tab.RightImage != null)
                    {
                        Rectangle ImageRect;
                        if (tab.ShowCloseButton)
                        {
                            ImageRect = new Rectangle(
                        area.Width - LeftRightWidth - CloseButtonMarginRight - closeButtonWH - 20, CloseButtonMarginTop, closeButtonWH,
                        closeButtonWH);
                        }
                        else
                        {
                            ImageRect = new Rectangle(
                        area.Width - LeftRightWidth - CloseButtonMarginRight - closeButtonWH, CloseButtonMarginTop, closeButtonWH,
                        closeButtonWH);
                        }
                        tabGraphicsContext.DrawImage(tab.RightImage, ImageRect);
                    }
                }

                tab.Area = area;
            }

            // Render the tab's saved image to the screen
            graphicsContext.DrawImage(
                tab.TabImage, area, 0, 0, tab.TabImage.Width, tab.TabImage.Height,
                GraphicsUnit.Pixel);

            // Render the icon for the tab's content, if it exists and there's room for it in the tab's content area
            if (tab.Content.ShowIcon && _tabContentWidth > 16 + IconMarginLeft + (tab.ShowCloseButton
                ? CloseButtonMarginLeft +
                  tab.CloseButtonArea.Width +
                  CloseButtonMarginRight
                : 0))
            {
                graphicsContext.DrawIcon(
                    new Icon(tab.Content.Icon, 16, 16),
                    new Rectangle(area.X + OverlapWidth + IconMarginLeft, IconMarginTop + area.Y, 16, 16));
            }

            // Render the caption for the tab's content if there's room for it in the tab's content area
            if (_tabContentWidth > (tab.Content.ShowIcon
                ? 16 + IconMarginLeft + IconMarginRight
                : 0) + CaptionMarginLeft + CaptionMarginRight + (tab.ShowCloseButton
                    ? CloseButtonMarginLeft +
                      tab.CloseButtonArea.Width +
                      CloseButtonMarginRight
                    : 0))
            {
                graphicsContext.DrawString(
                    tab.Caption, SystemFonts.CaptionFont, new SolidBrush(tab.UseDefaultForeColor ? ForeColor : tab.ForeColor),
                    new Rectangle(
                        area.X + OverlapWidth + CaptionMarginLeft + (tab.Content.ShowIcon
                            ? IconMarginLeft +
                              16 +
                              IconMarginRight
                            : 0),
                        CaptionMarginTop + area.Y,
                        _tabContentWidth - (tab.Content.ShowIcon
                            ? IconMarginLeft + 16 + (tab.RightImage != null ? 20 : 0) + IconMarginRight
                            : 0) - (tab.ShowCloseButton
                                ? closeButtonWH +
                                  CloseButtonMarginRight +
                                  CloseButtonMarginLeft
                                : 0), tab.TabImage.Height),
                    new StringFormat(StringFormatFlags.NoWrap)
                    {
                        Trimming = StringTrimming.EllipsisCharacter
                    });
            }
        }

        /// <summary>
        /// Width of Close Tab button.
        /// </summary>
        protected int closeButtonWH = 18;

        /// <summary>
        /// Called when a torn tab is dragged into the <see cref="HTTitleTabs.TabDropArea" /> of <see cref="_parentWindow" />.  Places the tab in the list and
        /// sets <see cref="IsTabRepositioning" /> to true to simulate the user continuing to drag the tab around in the window.
        /// </summary>
        /// <param name="tab">Tab that was dragged into this window.</param>
        /// <param name="cursorLocation">Location of the user's cursor.</param>
        internal virtual void CombineTab(HTTitleTab tab, Point cursorLocation)
        {
            // Stop rendering to prevent weird stuff from happening like the wrong tab being focused
            _suspendRendering = true;

            // Find out where to insert the tab in the list
            int dropIndex = _parentWindow.Tabs.FindIndex(t => t.Area.Left <= cursorLocation.X && t.Area.Right >= cursorLocation.X);

            // Simulate the user having clicked in the middle of the tab when they started dragging it so that the tab will move correctly within the window
            // when the user continues to move the mouse
            if (_parentWindow.Tabs.Count > 0)
            {
                _tabClickOffset = _parentWindow.Tabs.First().Area.Width / 2;
            }
            else
            {
                _tabClickOffset = 0;
            }
            IsTabRepositioning = true;

            tab.Parent = _parentWindow;

            if (dropIndex == -1)
            {
                _parentWindow.Tabs.Add(tab);
                dropIndex = _parentWindow.Tabs.Count - 1;
            }
            else
            {
                _parentWindow.Tabs.Insert(dropIndex, tab);
            }

            // Resume rendering
            _suspendRendering = false;

            _parentWindow.SelectedTabIndex = dropIndex;
            _parentWindow.ResizeTabContents();
        }
    }

    /// <summary>
    /// Determines which tab color should be used.
    /// </summary>
    public enum TabColors
    {
        /// <summary>
        /// Uses the background color.
        /// </summary>
        BackColor,

        /// <summary>
        /// Uses the foreground color.
        /// </summary>
        ForeColor,

        /// <summary>
        /// Uses the overlay color.
        /// </summary>
        OverlayColor,

        /// <summary>
        /// Uses the background color of overlay.
        /// </summary>
        OverlayBackColor,
    }
}