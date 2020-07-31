using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Win32Interop.Enums;
using Win32Interop.Methods;
using Win32Interop.Structs;
using Timer = System.Timers.Timer;

namespace HTAlt.WinForms
{
    /// <summary>Wraps a <see cref="Form" /> instance (<see cref="_content" />), that represents the content that should be displayed within a tab instance.</summary>
    public class HTTitleTab
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://haltroy.com/htalt/HTAlt.WinForms/HTTitleTab");
        private readonly Version firstHTAltVersion = new Version("0.1.4.0");
        private readonly string originProjectName = "EasyTabs";
        private readonly Uri originProject = new Uri("https://github.com/lstratman/EasyTabs");
        private readonly string description = "Wraps a Form instance (_content), that represents the content that should be displayed within a tab instance.";

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

        /// <summary>
        /// Image to display between Close button and text.
        /// </summary>
        public Image RightImage { get; set; }

        /// <summary>
        /// True to use the default color for overlay background.
        /// </summary>
        protected bool useDefaultOverlayBackColor = true;

        /// <summary>
        /// True to use the default color for background.
        /// </summary>
        protected bool useDefaultBackColor = true;

        /// <summary>
        /// True to use the default color for foreground.
        /// </summary>
        protected bool useDefaultForeColor = true;

        /// <summary>
        /// True to use the default color for overlay.
        /// </summary>
        protected bool useDefaultOverlayColor = true;

        /// <summary>
        /// Overlay color of the tab.
        /// </summary>
        protected Color _OverlayColor = Color.FromArgb(255, 20, 157, 204);

        /// <summary>
        /// Foreground color of the tab.
        /// </summary>
        protected Color _ForeColor = Color.FromArgb(255, 0, 0, 0);

        /// <summary>
        /// Overlay Background color of the tab.
        /// </summary>
        protected Color _OverlayBackColor = Color.FromArgb(255, 255, 255, 255);

        /// <summary>
        /// Background color of the tab.
        /// </summary>
        protected Color _BackColor = Color.FromArgb(255, 235, 235, 235);

        /// <summary>
        /// True to use the default color for all.
        /// </summary>
        public bool UseDefaultColor
        {
            get => (useDefaultBackColor && useDefaultForeColor && useDefaultOverlayColor && useDefaultOverlayBackColor);
            set
            {
                useDefaultOverlayBackColor = value;
                useDefaultBackColor = value;
                useDefaultForeColor = value;
                useDefaultOverlayColor = value;
            }
        }

        /// <summary>
        /// True to use the default color for background.
        /// </summary>
        public bool UseDefaultBackColor
        {
            get => useDefaultBackColor;
            set => useDefaultBackColor = value;
        }

        /// <summary>
        /// True to use the default color for overlay background.
        /// </summary>
        public bool UseDefaultOverlayBackColor
        {
            get => useDefaultOverlayBackColor;
            set => useDefaultOverlayBackColor = value;
        }

        /// <summary>
        /// True to use the default color for foreground.
        /// </summary>
        public bool UseDefaultForeColor
        {
            get => useDefaultForeColor;
            set => useDefaultForeColor = value;
        }

        /// <summary>
        /// True to use the default color for overlay.
        /// </summary>
        public bool UseDefaultOverlayColor
        {
            get => useDefaultOverlayColor;
            set => useDefaultOverlayColor = value;
        }

        /// <summary>
        /// Background color of the Title Tab..
        /// </summary>
        public Color BackColor
        {
            get => _BackColor;
            set => _BackColor = value;
        }

        /// <summary>
        /// Overlay background color of the Title Tab..
        /// </summary>
        public Color OverlayBackColor
        {
            get => _OverlayBackColor;
            set => _OverlayBackColor = value;
        }

        /// <summary>
        /// Foreground color of the Title Tab..
        /// </summary>
        public Color ForeColor
        {
            get => _ForeColor;
            set => _ForeColor = value;
        }

        /// <summary>
        /// Overlayground color of the Title Tab..
        /// </summary>
        public Color OverlayColor
        {
            get => _OverlayColor;
            set => _OverlayColor = value;
        }

        /// <summary>Flag indicating whether or not this tab is active.</summary>
        protected bool _active;

        /// <summary>Content that should be displayed within the tab.</summary>
        protected Form _content;

        /// <summary>Parent window that contains this tab.</summary>
        protected HTTitleTabs _parent;

        /// <summary>Default constructor that initializes the various properties.</summary>
        /// <param name="parent">Parent window that contains this tab.</param>
        public HTTitleTab(HTTitleTabs parent)
        {
            ShowCloseButton = true;
            Parent = parent;
        }

        /// <summary>Parent window that contains this tab.</summary>
        public HTTitleTabs Parent
        {
            get => _parent;

            internal set
            {
                _parent = value;

                if (_content != null)
                {
                    _content.Parent = _parent;
                }
            }
        }

        /// <summary>Flag indicating whether or not we should display the close button for this tab.</summary>
        public bool ShowCloseButton
        {
            get;
            set;
        }

        /// <summary>The caption that's displayed in the tab's title (simply uses the <see cref="Form.Text" /> of
        /// <see cref="Content" />).</summary>
        public string Caption
        {
            get => Content.Text;

            set => Content.Text = value;
        }

        /// <summary>Flag indicating whether or not this tab is active.</summary>
        public bool Active
        {
            get => _active;

            internal set
            {
                // When the status of the tab changes, we null out the TabImage property so that it's recreated in the next rendering pass
                _active = value;
                TabImage = null;
                Content.Visible = value;
            }
        }

        /// <summary>The icon that's displayed in the tab's title (simply uses the <see cref="Form.Icon" /> of <see cref="Content" />).</summary>
        public Icon Icon
        {
            get => Content.Icon;

            set => Content.Icon = value;
        }

        /// <summary>The area in which the tab is rendered in the client window.</summary>
        internal Rectangle Area
        {
            get;
            set;
        }

        /// <summary>The area of the close button for this tab in the client window.</summary>
        internal Rectangle CloseButtonArea
        {
            get;
            set;
        }

        /// <summary>Pre-rendered image of the tab's background.</summary>
        internal Bitmap TabImage
        {
            get;
            set;
        }

        /// <summary>The content that should be displayed for this tab.</summary>
        public Form Content
        {
            get => _content;

            set
            {
                if (_content != null)
                {
                    _content.FormClosing -= Content_Closing;
                    _content.TextChanged -= Content_TextChanged;
                }

                _content = value;

                // We set the content form to a non-top-level child of the parent form.
                Content.FormBorderStyle = FormBorderStyle.None;
                Content.TopLevel = false;
                Content.Parent = Parent;
                Content.FormClosing += Content_Closing;
                Content.TextChanged += Content_TextChanged;
            }
        }

        /// <summary>
        /// Called from <see cref="TornTabForm" /> when we need to generate a thumbnail for a tab when it is torn out of its parent window.  We simply call
        /// <see cref="Graphics.CopyFromScreen(System.Drawing.Point,System.Drawing.Point,System.Drawing.Size)" /> to copy the screen contents to a
        /// <see cref="Bitmap" />.
        /// </summary>
        /// <returns>An image of the tab's contents.</returns>
        public virtual Bitmap GetImage()
        {
            Bitmap tabContents = new Bitmap(Content.Size.Width, Content.Size.Height);
            Graphics contentsGraphics = Graphics.FromImage(tabContents);

            contentsGraphics.CopyFromScreen(Content.PointToScreen(Point.Empty).X, Content.PointToScreen(Point.Empty).Y, 0, 0, Content.Size);

            return tabContents;
        }

        /// <summary>Event that is fired when <see cref="Content" />'s <see cref="Form.Closing" /> event is fired.</summary>
        public event CancelEventHandler Closing;

        /// <summary>Event that is fired when <see cref="Content" />'s <see cref="Control.TextChanged" /> event is fired.</summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// Event handler that is invoked when <see cref="Content" />'s <see cref="Control.TextChanged" /> event is fired, which in turn fires this class'
        /// <see cref="TextChanged" /> event.
        /// </summary>
        /// <param name="sender">Object from which this event originated (<see cref="Content" /> in this case).</param>
        /// <param name="e">Arguments associated with the event.</param>
        private void Content_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

        /// <summary>
        /// Event handler that is invoked when <see cref="Content" />'s <see cref="Form.Closing" /> event is fired, which in turn fires this class'
        /// <see cref="Closing" /> event.
        /// </summary>
        /// <param name="sender">Object from which this event originated (<see cref="Content" /> in this case).</param>
        /// <param name="e">Arguments associated with the event.</param>
        protected void Content_Closing(object sender, CancelEventArgs e)
        {
            if (Closing != null)
            {
                Closing(this, e);
            }
        }

        /// <summary>Unsubscribes the tab from any event handlers that may have been attached to its <see cref="Closing" /> or <see cref="TextChanged" /> events.</summary>
        public void ClearSubscriptions()
        {
            Closing = null;
            TextChanged = null;
        }
    }

    /// <summary>
    /// Borderless overlay window that is moved with and rendered on top of the non-client area of a  <see cref="HTTitleTabs" /> instance that's responsible
    /// for rendering the actual tab content and responding to click events for those tabs.
    /// </summary>
    public class HTTitleTabsOverlay : Form
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://haltroy.com/htalt/HTAlt.WinForms/HTTitleTabsOverlay");
        private readonly Version firstHTAltVersion = new Version("0.1.4.0");
        private readonly string originProjectName = "EasyTabs";
        private readonly Uri originProject = new Uri("https://github.com/lstratman/EasyTabs");
        private readonly string description = "Borderless overlay window that is moved with and rendered on top of the non-client area of a HTTitleTabs instance that's responsible for rendering the actual tab content and responding to click events for those tabs.";

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

        /// <summary>
        /// Tooltip's timer.
        /// </summary>
        protected Timer showTooltipTimer;

        /// <summary>All of the parent forms and their overlays so that we don't create duplicate overlays across the application domain.</summary>
        protected static Dictionary<HTTitleTabs, HTTitleTabsOverlay> _parents = new Dictionary<HTTitleTabs, HTTitleTabsOverlay>();

        /// <summary>Tab that has been torn off from this window and is being dragged.</summary>
        protected static HTTitleTab _tornTab;

        /// <summary>Thumbnail representation of <see cref="_tornTab" /> used when dragging.</summary>
        protected static TornTabForm _tornTabForm;

        /// <summary>
        /// Flag used in <see cref="WndProc" /> and <see cref="MouseHookCallback" /> to track whether the user was click/dragging when a particular event
        /// occurred.
        /// </summary>
        protected static bool _wasDragging = false;

        /// <summary>Flag indicating whether or not <see cref="_hookproc" /> has been installed as a hook.</summary>
        protected static bool _hookProcInstalled;

        /// <summary>Semaphore to control access to <see cref="_tornTab" />.</summary>
        protected static object _tornTabLock = new object();

        /// <summary>Flag indicating whether or not the underlying window is active.</summary>
        protected bool _active = false;

        /// <summary>Flag indicating whether we should draw the titlebar background (i.e. we are in a non-Aero environment).</summary>
        protected bool _aeroEnabled = false;

        /// <summary>
        /// When a tab is torn from the window, this is where we store the areas on all open windows where tabs can be dropped to combine the tab with that
        /// window.
        /// </summary>
        protected Tuple<HTTitleTabs, Rectangle>[] _dropAreas = null;

        /// <summary>Pointer to the low-level mouse hook callback (<see cref="MouseHookCallback" />).</summary>
        protected IntPtr _hookId;

        /// <summary>Delegate of <see cref="MouseHookCallback" />; declared as a member variable to keep it from being garbage collected.</summary>
        protected HOOKPROC _hookproc = null;

        /// <summary>Index of the tab, if any, whose close button is being hovered over.</summary>
        protected int _isOverCloseButtonForTab = -1;

        /// <summary>Queue of mouse events reported by <see cref="_hookproc" /> that need to be processed.</summary>
        protected BlockingCollection<MouseEvent> _mouseEvents = new BlockingCollection<MouseEvent>();

        /// <summary>Consumer thread for processing events in <see cref="_mouseEvents" />.</summary>
        protected Thread _mouseEventsThread = null;

        /// <summary>Parent form for the overlay.</summary>
        protected HTTitleTabs _parentForm;

        /// <summary>Blank default constructor to ensure that the overlays are only initialized through <see cref="GetInstance" />.</summary>
        protected HTTitleTabsOverlay()
        {
        }

        /// <summary>Creates the overlay window and attaches it to <paramref name="parentForm" />.</summary>
        /// <param name="parentForm">Parent form that the overlay should be rendered on top of.</param>
        protected HTTitleTabsOverlay(HTTitleTabs parentForm)
        {
            _parentForm = parentForm;

            // We don't want this window visible in the taskbar
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = false;
            MaximizeBox = false;
            _aeroEnabled = _parentForm.IsCompositionEnabled;

            Show(_parentForm);
            AttachHandlers();

            showTooltipTimer = new Timer
            {
                AutoReset = false
            };

            showTooltipTimer.Elapsed += ShowTooltipTimer_Elapsed;
        }

        /// <summary>
        /// Makes sure that the window is created with an <see cref="WS_EX.WS_EX_LAYERED" /> flag set so that it can be alpha-blended properly with the content (
        /// <see cref="_parentForm" />) underneath the overlay.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= (int)(WS_EX.WS_EX_LAYERED | WS_EX.WS_EX_NOACTIVATE);

                return createParams;
            }
        }

        /// <summary>Primary color for the titlebar background.</summary>
        protected Color TitleBarColor
        {
            get
            {
                if (Application.RenderWithVisualStyles && Environment.OSVersion.Version.Major >= 6)
                {
                    return _active
                        ? SystemColors.GradientActiveCaption
                        : SystemColors.GradientInactiveCaption;
                }

                return _active
                    ? SystemColors.ActiveCaption
                    : SystemColors.InactiveCaption;
            }
        }

        /// <summary>Type of theme being used by the OS to render the desktop.</summary>
        protected DisplayType DisplayType
        {
            get
            {
                if (_aeroEnabled)
                {
                    return DisplayType.Aero;
                }

                if (Application.RenderWithVisualStyles && Environment.OSVersion.Version.Major >= 6)
                {
                    return DisplayType.Basic;
                }

                return DisplayType.Classic;
            }
        }

        /// <summary>Gradient color for the titlebar background.</summary>
        protected Color TitleBarGradientColor => _active
                    ? SystemInformation.IsTitleBarGradientEnabled
                        ? SystemColors.GradientActiveCaption
                        : SystemColors.ActiveCaption
                    : SystemInformation.IsTitleBarGradientEnabled
                        ? SystemColors.GradientInactiveCaption
                        : SystemColors.InactiveCaption;

        /// <summary>Screen area in which tabs can be dragged to and dropped for this window.</summary>
        public Rectangle TabDropArea
        {
            get
            {
                User32.GetWindowRect(_parentForm.Handle, out RECT windowRectangle);

                return new Rectangle(
                    windowRectangle.left + SystemInformation.HorizontalResizeBorderThickness, windowRectangle.top + SystemInformation.VerticalResizeBorderThickness,
                    ClientRectangle.Width, _parentForm.NonClientAreaHeight - SystemInformation.VerticalResizeBorderThickness);
            }
        }

        /// <summary>Retrieves or creates the overlay for <paramref name="parentForm" />.</summary>
        /// <param name="parentForm">Parent form that we are to create the overlay for.</param>
        /// <returns>Newly-created or previously existing overlay for <paramref name="parentForm" />.</returns>
        public static HTTitleTabsOverlay GetInstance(HTTitleTabs parentForm)
        {
            if (!_parents.ContainsKey(parentForm))
            {
                _parents.Add(parentForm, new HTTitleTabsOverlay(parentForm));
            }

            return _parents[parentForm];
        }

        /// <summary>
        /// Attaches the various event handlers to <see cref="_parentForm" /> so that the overlay is moved in synchronization to
        /// <see cref="_parentForm" />.
        /// </summary>
        protected void AttachHandlers()
        {
            _parentForm.Closing += _parentForm_Closing;
            _parentForm.Disposed += _parentForm_Disposed;
            _parentForm.Deactivate += _parentForm_Deactivate;
            _parentForm.Activated += _parentForm_Activated;
            _parentForm.SizeChanged += _parentForm_Refresh;
            _parentForm.Shown += _parentForm_Refresh;
            _parentForm.VisibleChanged += _parentForm_Refresh;
            _parentForm.Move += _parentForm_Refresh;
            _parentForm.SystemColorsChanged += _parentForm_SystemColorsChanged;

            if (_hookproc == null)
            {
                // Spin up a consumer thread to process mouse events from _mouseEvents
                _mouseEventsThread = new Thread(InterpretMouseEvents)
                {
                    Name = "Low level mouse hooks processing thread"
                };
                _mouseEventsThread.Priority = ThreadPriority.Highest;
                _mouseEventsThread.Start();

                using (Process curProcess = Process.GetCurrentProcess())
                {
                    using (ProcessModule curModule = curProcess.MainModule)
                    {
                        // Install the low level mouse hook that will put events into _mouseEvents
                        _hookproc = MouseHookCallback;
                        _hookId = User32.SetWindowsHookEx(WH.WH_MOUSE_LL, _hookproc, Kernel32.GetModuleHandle(curModule.ModuleName), 0);
                    }
                }
            }
        }

        /// <summary>
        /// Event handler that is called when <see cref="_parentForm" /> is in the process of closing.  This uninstalls <see cref="_hookproc" /> from the low-
        /// level hooks list and stops the consumer thread that processes those events.
        /// </summary>
        /// <param name="sender">Object from which this event originated, <see cref="_parentForm" /> in this case.</param>
        /// <param name="e">Arguments associated with this event.</param>
        private void _parentForm_Closing(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
            {
                return;
            }

            HTTitleTabs form = (HTTitleTabs)sender;

            if (form == null)
            {
                return;
            }

            if (_parents.ContainsKey(form))
            {
                _parents.Remove(form);
            }

            // Uninstall the mouse hook
            User32.UnhookWindowsHookEx(_hookId);

            // Kill the mouse events processing thread
            _mouseEvents.CompleteAdding();
            _mouseEventsThread.Abort();
        }

        private void HideTooltip()
        {
            showTooltipTimer.Stop();

            if (_parentForm.InvokeRequired)
            {
                _parentForm.Invoke(new Action(() =>
                {
                    _parentForm.Tooltip.Hide(_parentForm);
                }));
            }
            else
            {
                _parentForm.Tooltip.Hide(_parentForm);
            }
        }

        private void ShowTooltip(HTTitleTabs tabsForm, string caption)
        {
            Point tooltipLocation = new Point(Cursor.Position.X + 7, Cursor.Position.Y + 55);
            tabsForm.Tooltip.Show(caption, tabsForm, tabsForm.PointToClient(tooltipLocation), tabsForm.Tooltip.AutoPopDelay);
        }

        private void ShowTooltipTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_parentForm.ShowTooltips)
            {
                return;
            }

            Point relativeCursorPosition = GetRelativeCursorPosition(Cursor.Position);
            HTTitleTab hoverTab = _parentForm.TabRenderer.OverTab(_parentForm.Tabs, relativeCursorPosition);

            if (hoverTab != null)
            {
                HTTitleTabs hoverTabForm = hoverTab.Parent;

                if (hoverTabForm.InvokeRequired)
                {
                    hoverTabForm.Invoke(new Action(() =>
                    {
                        ShowTooltip(hoverTabForm, hoverTab.Caption);
                    }));
                }
                else
                {
                    ShowTooltip(hoverTabForm, hoverTab.Caption);
                }
            }
        }

        private void StartTooltipTimer()
        {
            if (!_parentForm.ShowTooltips)
            {
                return;
            }

            Point relativeCursorPosition = GetRelativeCursorPosition(Cursor.Position);
            HTTitleTab hoverTab = _parentForm.TabRenderer.OverTab(_parentForm.Tabs, relativeCursorPosition);

            if (hoverTab != null)
            {
                showTooltipTimer.Interval = hoverTab.Parent.Tooltip.AutomaticDelay;
                showTooltipTimer.Start();
            }
        }

        /// <summary>Consumer method that processes mouse events in <see cref="_mouseEvents" /> that are recorded by <see cref="MouseHookCallback" />.</summary>
        protected void InterpretMouseEvents()
        {
            foreach (MouseEvent mouseEvent in _mouseEvents.GetConsumingEnumerable())
            {
                int nCode = mouseEvent.nCode;
                IntPtr wParam = mouseEvent.wParam;
                MSLLHOOKSTRUCT? hookStruct = mouseEvent.MouseData;

                if (nCode >= 0 && (int)WM.WM_MOUSEMOVE == (int)wParam)
                {
                    HideTooltip();

                    // ReSharper disable PossibleInvalidOperationException
                    Point cursorPosition = new Point(hookStruct.Value.pt.x, hookStruct.Value.pt.y);
                    // ReSharper restore PossibleInvalidOperationException
                    bool reRender = false;

                    if (_tornTab != null && _dropAreas != null)
                    {
                        // ReSharper disable ForCanBeConvertedToForeach
                        for (int i = 0; i < _dropAreas.Length; i++)
                        // ReSharper restore ForCanBeConvertedToForeach
                        {
                            // If the cursor is within the drop area, combine the tab for the window that belongs to that drop area
                            if (_dropAreas[i].Item2.Contains(cursorPosition))
                            {
                                HTTitleTab tabToCombine = null;

                                lock (_tornTabLock)
                                {
                                    if (_tornTab != null)
                                    {
                                        tabToCombine = _tornTab;
                                        _tornTab = null;
                                    }
                                }

                                if (tabToCombine != null)
                                {
                                    int i1 = i;

                                    // In all cases where we need to affect the UI, we call Invoke so that those changes are made on the main UI thread since
                                    // we are on a separate processing thread in this case
                                    Invoke(
                                        new Action(
                                            () =>
                                            {
                                                _dropAreas[i1].Item1.TabRenderer.CombineTab(tabToCombine, cursorPosition);

                                                tabToCombine = null;
                                                _tornTabForm.Close();
                                                _tornTabForm = null;

                                                if (_parentForm.Tabs.Count == 0)
                                                {
                                                    _parentForm.Close();
                                                }
                                            }));
                                }
                            }
                        }
                    }
                    else if (!_parentForm.TabRenderer.IsTabRepositioning)
                    {
                        StartTooltipTimer();

                        // If we were over a close button previously, check to see if the cursor is still over that tab's
                        // close button; if not, re-render
                        if (_isOverCloseButtonForTab != -1 &&
                            (_isOverCloseButtonForTab >= _parentForm.Tabs.Count ||
                            !_parentForm.TabRenderer.IsOverCloseButton(_parentForm.Tabs[_isOverCloseButtonForTab], GetRelativeCursorPosition(cursorPosition))))
                        {
                            reRender = true;
                            _isOverCloseButtonForTab = -1;
                        }

                        // Otherwise, see if any tabs' close button is being hovered over
                        else
                        {
                            // ReSharper disable ForCanBeConvertedToForeach
                            for (int i = 0; i < _parentForm.Tabs.Count; i++)
                            // ReSharper restore ForCanBeConvertedToForeach
                            {
                                if (_parentForm.TabRenderer.IsOverCloseButton(_parentForm.Tabs[i], GetRelativeCursorPosition(cursorPosition)))
                                {
                                    _isOverCloseButtonForTab = i;
                                    reRender = true;

                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Invoke(
                            new Action(
                                () =>
                                {
                                    _wasDragging = true;

                                    // When determining if a tab has been torn from the window while dragging, we take the drop area for this window and inflate it by the
                                    // TabTearDragDistance setting
                                    Rectangle dragArea = TabDropArea;
                                    dragArea.Inflate(_parentForm.TabRenderer.TabTearDragDistance, _parentForm.TabRenderer.TabTearDragDistance);

                                    // If the cursor is outside the tear area, tear it away from the current window
                                    if (!dragArea.Contains(cursorPosition) && _tornTab == null)
                                    {
                                        lock (_tornTabLock)
                                        {
                                            if (_tornTab == null)
                                            {
                                                _parentForm.TabRenderer.IsTabRepositioning = false;

                                                // Clear the event handler subscriptions from the tab and then create a thumbnail representation of it to use when dragging
                                                _tornTab = _parentForm.SelectedTab;
                                                _tornTab.ClearSubscriptions();
                                                _tornTabForm = new TornTabForm(_tornTab, _parentForm.TabRenderer);
                                            }
                                        }

                                        if (_tornTab != null)
                                        {
                                            _parentForm.SelectedTabIndex = (_parentForm.SelectedTabIndex == _parentForm.Tabs.Count - 1
                                                ? _parentForm.SelectedTabIndex - 1
                                                : _parentForm.SelectedTabIndex + 1);
                                            _parentForm.Tabs.Remove(_tornTab);

                                            // If this tab was the only tab in the window, hide the parent window
                                            if (_parentForm.Tabs.Count == 0)
                                            {
                                                _parentForm.Hide();
                                            }

                                            _tornTabForm.Show();
                                            _dropAreas = (from window in _parentForm.ApplicationContext.OpenWindows.Where(w => w.Tabs.Count > 0)
                                                          select new Tuple<HTTitleTabs, Rectangle>(window, window.TabDropArea)).ToArray();
                                        }
                                    }
                                }));
                    }

                    Invoke(new Action(() => OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, cursorPosition.X, cursorPosition.Y, 0))));

                    if (_parentForm.TabRenderer.IsTabRepositioning)
                    {
                        reRender = true;
                    }

                    if (reRender)
                    {
                        Invoke(new Action(() => Render(cursorPosition, true)));
                    }
                }
                else if (nCode >= 0 && (int)WM.WM_LBUTTONDOWN == (int)wParam)
                {
                    _wasDragging = false;
                }
                else if (nCode >= 0 && (int)WM.WM_LBUTTONUP == (int)wParam)
                {
                    // If we released the mouse button while we were dragging a torn tab, put that tab into a new window
                    if (_tornTab != null)
                    {
                        HTTitleTab tabToRelease = null;

                        lock (_tornTabLock)
                        {
                            if (_tornTab != null)
                            {
                                tabToRelease = _tornTab;
                                _tornTab = null;
                            }
                        }

                        if (tabToRelease != null)
                        {
                            Invoke(
                                new Action(
                                    () =>
                                    {
                                        HTTitleTabs newWindow = (HTTitleTabs)Activator.CreateInstance(_parentForm.GetType());

                                        // Set the initial window position and state properly
                                        if (newWindow.WindowState == FormWindowState.Maximized)
                                        {
                                            Screen screen = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position));

                                            newWindow.StartPosition = FormStartPosition.Manual;
                                            newWindow.WindowState = FormWindowState.Normal;
                                            newWindow.Left = screen.WorkingArea.Left;
                                            newWindow.Top = screen.WorkingArea.Top;
                                            newWindow.Width = screen.WorkingArea.Width;
                                            newWindow.Height = screen.WorkingArea.Height;
                                        }
                                        else
                                        {
                                            newWindow.Left = Cursor.Position.X;
                                            newWindow.Top = Cursor.Position.Y;
                                        }

                                        tabToRelease.Parent = newWindow;
                                        _parentForm.ApplicationContext.OpenWindow(newWindow);

                                        newWindow.Show();
                                        newWindow.Tabs.Add(tabToRelease);
                                        newWindow.SelectedTabIndex = 0;
                                        newWindow.ResizeTabContents();

                                        _tornTabForm.Close();
                                        _tornTabForm = null;

                                        if (_parentForm.Tabs.Count == 0)
                                        {
                                            _parentForm.Close();
                                        }
                                    }));
                        }
                    }

                    Invoke(new Action(() => OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X, Cursor.Position.Y, 0))));
                }
            }
        }

        /// <summary>Hook callback to process <see cref="WM.WM_MOUSEMOVE" /> messages to highlight/un-highlight the close button on each tab.</summary>
        /// <param name="nCode">The message being received.</param>
        /// <param name="wParam">Additional information about the message.</param>
        /// <param name="lParam">Additional information about the message.</param>
        /// <returns>A zero value if the procedure processes the message; a nonzero value if the procedure ignores the message.</returns>
        protected IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            MouseEvent mouseEvent = new MouseEvent
            {
                nCode = nCode,
                wParam = wParam,
                lParam = lParam
            };

            if (nCode >= 0 && (int)WM.WM_MOUSEMOVE == (int)wParam)
            {
                mouseEvent.MouseData = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            }

            _mouseEvents.Add(mouseEvent);

            return User32.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        /// <summary>Draws the titlebar background behind the tabs if Aero glass is not enabled.</summary>
        /// <param name="graphics">Graphics context with which to draw the background.</param>
        protected virtual void DrawTitleBarBackground(Graphics graphics)
        {
            if (DisplayType == DisplayType.Aero)
            {
                return;
            }

            Rectangle fillArea;

            if (DisplayType == DisplayType.Basic)
            {
                fillArea = new Rectangle(
                    new Point(
                        1, Top == 0
                            ? SystemInformation.CaptionHeight - 1
                            : (SystemInformation.CaptionHeight + SystemInformation.VerticalResizeBorderThickness) - (Top - _parentForm.Top) - 1),
                    new Size(Width - 2, _parentForm.Padding.Top));
            }
            else
            {
                fillArea = new Rectangle(new Point(1, 0), new Size(Width - 2, Height - 1));
            }

            if (fillArea.Height <= 0)
            {
                return;
            }

            // Adjust the margin so that the gradient stops immediately prior to the control box in the titlebar
            int rightMargin = 3;

            if (_parentForm.ControlBox && _parentForm.MinimizeBox)
            {
                rightMargin += SystemInformation.CaptionButtonSize.Width;
            }

            if (_parentForm.ControlBox && _parentForm.MaximizeBox)
            {
                rightMargin += SystemInformation.CaptionButtonSize.Width;
            }

            if (_parentForm.ControlBox)
            {
                rightMargin += SystemInformation.CaptionButtonSize.Width;
            }

            LinearGradientBrush gradient = new LinearGradientBrush(
                new Point(24, 0), new Point(fillArea.Width - rightMargin + 1, 0), TitleBarColor, TitleBarGradientColor);

            using (BufferedGraphics bufferedGraphics = BufferedGraphicsManager.Current.Allocate(graphics, fillArea))
            {
                bufferedGraphics.Graphics.FillRectangle(new SolidBrush(TitleBarColor), fillArea);
                bufferedGraphics.Graphics.FillRectangle(
                    new SolidBrush(TitleBarGradientColor),
                    new Rectangle(new Point(fillArea.Location.X + fillArea.Width - rightMargin, fillArea.Location.Y), new Size(rightMargin, fillArea.Height)));
                bufferedGraphics.Graphics.FillRectangle(
                    gradient, new Rectangle(fillArea.Location, new Size(fillArea.Width - rightMargin, fillArea.Height)));
                bufferedGraphics.Graphics.FillRectangle(new SolidBrush(TitleBarColor), new Rectangle(fillArea.Location, new Size(24, fillArea.Height)));

                bufferedGraphics.Render(graphics);
            }
        }

        /// <summary>
        /// Event handler that is called when <see cref="_parentForm" />'s <see cref="Control.SystemColorsChanged" /> event is fired which re-renders
        /// the tabs.
        /// </summary>
        /// <param name="sender">Object from which the event originated.</param>
        /// <param name="e">Arguments associated with the event.</param>
        private void _parentForm_SystemColorsChanged(object sender, EventArgs e)
        {
            _aeroEnabled = _parentForm.IsCompositionEnabled;
            OnPosition();
        }

        /// <summary>
        /// Event handler that is called when <see cref="_parentForm" />'s <see cref="Control.SizeChanged" />, <see cref="Control.VisibleChanged" />, or
        /// <see cref="Control.Move" /> events are fired which re-renders the tabs.
        /// </summary>
        /// <param name="sender">Object from which the event originated.</param>
        /// <param name="e">Arguments associated with the event.</param>
        private void _parentForm_Refresh(object sender, EventArgs e)
        {
            if (_parentForm.WindowState == FormWindowState.Minimized)
            {
                Visible = false;
            }
            else
            {
                OnPosition();
            }
        }

        /// <summary>Sets the position of the overlay window to match that of <see cref="_parentForm" /> so that it moves in tandem with it.</summary>
        protected void OnPosition()
        {
            if (!IsDisposed)
            {
                // 92 is SM_CXPADDEDBORDER, which returns the amount of extra border padding around captioned windows
                int borderPadding = DisplayType == DisplayType.Classic
                    ? 0
                    : User32.GetSystemMetrics(92);

                // If the form is in a non-maximized state, we position the tabs below the minimize/maximize/close
                // buttons
                Top = _parentForm.Top + (DisplayType == DisplayType.Classic
                    ? SystemInformation.VerticalResizeBorderThickness
                    : _parentForm.WindowState == FormWindowState.Maximized
                        ? SystemInformation.VerticalResizeBorderThickness + borderPadding
                        : SystemInformation.CaptionHeight + borderPadding);
                Left = _parentForm.Left + SystemInformation.HorizontalResizeBorderThickness - SystemInformation.BorderSize.Width + borderPadding;
                Width = _parentForm.Width - ((SystemInformation.VerticalResizeBorderThickness + borderPadding) * 2) + (SystemInformation.BorderSize.Width * 2);
                Height = _parentForm.TabRenderer.TabHeight + (DisplayType == DisplayType.Classic && _parentForm.WindowState != FormWindowState.Maximized
                    ? SystemInformation.CaptionButtonSize.Height
                    : 0);

                Render();
            }
        }

        /// <summary>
        /// Renders the tabs and then calls <see cref="User32.UpdateLayeredWindow" /> to blend the tab content with the underlying window (
        /// <see cref="_parentForm" />).
        /// </summary>
        /// <param name="forceRedraw">Flag indicating whether a full render should be forced.</param>
        public void Render(bool forceRedraw = false)
        {
            Render(Cursor.Position, forceRedraw);
        }

        /// <summary>
        /// Renders the tabs and then calls <see cref="User32.UpdateLayeredWindow" /> to blend the tab content with the underlying window (
        /// <see cref="_parentForm" />).
        /// </summary>
        /// <param name="cursorPosition">Current position of the cursor.</param>
        /// <param name="forceRedraw">Flag indicating whether a full render should be forced.</param>
        public void Render(Point cursorPosition, bool forceRedraw = false)
        {
            if (!IsDisposed && _parentForm.TabRenderer != null && _parentForm.WindowState != FormWindowState.Minimized && _parentForm.ClientRectangle.Width > 0)
            {
                cursorPosition = GetRelativeCursorPosition(cursorPosition);

                using (Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        DrawTitleBarBackground(graphics);

                        // Since classic mode themes draw over the *entire* titlebar, not just the area immediately behind the tabs, we have to offset the tabs
                        // when rendering in the window
                        Point offset = _parentForm.WindowState != FormWindowState.Maximized && DisplayType == DisplayType.Classic
                            ? new Point(0, SystemInformation.CaptionButtonSize.Height)
                            : _parentForm.WindowState != FormWindowState.Maximized
                                ? new Point(0, SystemInformation.VerticalResizeBorderThickness - SystemInformation.BorderSize.Height)
                                : new Point(0, 0);

                        // Render the tabs into the bitmap
                        _parentForm.TabRenderer.Render(_parentForm.Tabs, graphics, offset, cursorPosition, forceRedraw);

                        // Cut out a hole in the background so that the control box on the underlying window can be shown
                        if (DisplayType == DisplayType.Classic && (_parentForm.ControlBox || _parentForm.MaximizeBox || _parentForm.MinimizeBox))
                        {
                            int boxWidth = 0;

                            if (_parentForm.ControlBox)
                            {
                                boxWidth += SystemInformation.CaptionButtonSize.Width;
                            }

                            if (_parentForm.MinimizeBox)
                            {
                                boxWidth += SystemInformation.CaptionButtonSize.Width;
                            }

                            if (_parentForm.MaximizeBox)
                            {
                                boxWidth += SystemInformation.CaptionButtonSize.Width;
                            }

                            CompositingMode oldCompositingMode = graphics.CompositingMode;

                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.FillRectangle(
                                new SolidBrush(Color.Transparent), Width - boxWidth, 0, boxWidth, SystemInformation.CaptionButtonSize.Height);
                            graphics.CompositingMode = oldCompositingMode;
                        }

                        IntPtr screenDc = User32.GetDC(IntPtr.Zero);
                        IntPtr memDc = Gdi32.CreateCompatibleDC(screenDc);
                        IntPtr oldBitmap = IntPtr.Zero;
                        IntPtr bitmapHandle = IntPtr.Zero;

                        try
                        {
                            // Copy the contents of the bitmap into memDc
                            bitmapHandle = bitmap.GetHbitmap(Color.FromArgb(0));
                            oldBitmap = Gdi32.SelectObject(memDc, bitmapHandle);

                            SIZE size = new SIZE
                            {
                                cx = bitmap.Width,
                                cy = bitmap.Height
                            };

                            POINT pointSource = new POINT
                            {
                                x = 0,
                                y = 0
                            };
                            POINT topPos = new POINT
                            {
                                x = Left,
                                y = Top
                            };
                            BLENDFUNCTION blend = new BLENDFUNCTION
                            {
                                // We want to blend the bitmap's content with the screen content under it
                                BlendOp = Convert.ToByte((int)AC.AC_SRC_OVER),
                                BlendFlags = 0,
                                // Follow the parent forms' opacity level
                                SourceConstantAlpha = (byte)(_parentForm.Opacity * 255),
                                // We use the bitmap's alpha channel for blending instead of a pre-defined transparency key
                                AlphaFormat = Convert.ToByte((int)AC.AC_SRC_ALPHA)
                            };

                            // Blend the tab content with the underlying content
                            if (!User32.UpdateLayeredWindow(
                                Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, ULW.ULW_ALPHA))
                            {
                                int error = Marshal.GetLastWin32Error();
                                throw new Win32Exception(error, "Error while calling UpdateLayeredWindow().");
                            }
                        }

                        // Clean up after ourselves
                        finally
                        {
                            User32.ReleaseDC(IntPtr.Zero, screenDc);

                            if (bitmapHandle != IntPtr.Zero)
                            {
                                Gdi32.SelectObject(memDc, oldBitmap);
                                Gdi32.DeleteObject(bitmapHandle);
                            }

                            Gdi32.DeleteDC(memDc);
                        }
                    }
                }
            }
        }

        /// <summary>Gets the relative location of the cursor within the overlay.</summary>
        /// <param name="cursorPosition">Cursor position that represents the absolute position of the cursor on the screen.</param>
        /// <returns>The relative location of the cursor within the overlay.</returns>
        public Point GetRelativeCursorPosition(Point cursorPosition)
        {
            return new Point(cursorPosition.X - Location.X, cursorPosition.Y - Location.Y);
        }

        /// <summary>Overrides the message pump for the window so that we can respond to click events on the tabs themselves.</summary>
        /// <param name="m">Message received by the pump.</param>
        protected override void WndProc(ref Message m)
        {
            switch ((WM)m.Msg)
            {
                case WM.WM_NCLBUTTONDOWN:
                case WM.WM_LBUTTONDOWN:
                    Point relativeCursorPosition = GetRelativeCursorPosition(Cursor.Position);

                    // If we were over a tab, set the capture state for the window so that we'll actually receive a WM_LBUTTONUP message
                    if (_parentForm.TabRenderer.OverTab(_parentForm.Tabs, relativeCursorPosition) == null &&
                        !_parentForm.TabRenderer.IsOverAddButton(relativeCursorPosition))
                    {
                        _parentForm.ForwardMessage(ref m);
                    }
                    else
                    {
                        // When the user clicks a mouse button, save the tab that the user was over so we can respond properly when the mouse button is released
                        HTTitleTab clickedTab = _parentForm.TabRenderer.OverTab(_parentForm.Tabs, relativeCursorPosition);

                        if (clickedTab != null)
                        {
                            // If the user clicked the close button, remove the tab from the list
                            if (!_parentForm.TabRenderer.IsOverCloseButton(clickedTab, relativeCursorPosition))
                            {
                                _parentForm.ResizeTabContents(clickedTab);
                                _parentForm.SelectedTabIndex = _parentForm.Tabs.IndexOf(clickedTab);

                                Render();
                            }

                            OnMouseDown(new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X, Cursor.Position.Y, 0));
                        }

                        _parentForm.Activate();
                    }

                    break;

                case WM.WM_LBUTTONDBLCLK:
                    _parentForm.ForwardMessage(ref m);
                    break;

                // We always return HTCAPTION for the hit test message so that the underlying window doesn't have its focus removed
                case WM.WM_NCHITTEST:
                    m.Result = new IntPtr((int)HT.HTCAPTION);
                    break;

                case WM.WM_LBUTTONUP:
                case WM.WM_NCLBUTTONUP:
                case WM.WM_MBUTTONUP:
                case WM.WM_NCMBUTTONUP:
                    Point relativeCursorPosition2 = GetRelativeCursorPosition(Cursor.Position);

                    if (_parentForm.TabRenderer.OverTab(_parentForm.Tabs, relativeCursorPosition2) == null &&
                        !_parentForm.TabRenderer.IsOverAddButton(relativeCursorPosition2))
                    {
                        _parentForm.ForwardMessage(ref m);
                    }
                    else
                    {
                        // When the user clicks a mouse button, save the tab that the user was over so we can respond properly when the mouse button is released
                        HTTitleTab clickedTab = _parentForm.TabRenderer.OverTab(_parentForm.Tabs, relativeCursorPosition2);

                        if (clickedTab != null)
                        {
                            // If the user clicks the middle button/scroll wheel over a tab, close it
                            if ((WM)m.Msg == WM.WM_MBUTTONUP || (WM)m.Msg == WM.WM_NCMBUTTONUP)
                            {
                                clickedTab.Content.Close();
                                Render();
                            }
                            else
                            {
                                // If the user clicked the close button, remove the tab from the list
                                if (_parentForm.TabRenderer.IsOverCloseButton(clickedTab, relativeCursorPosition2))
                                {
                                    clickedTab.Content.Close();
                                    Render();
                                }
                                else
                                {
                                    _parentForm.OnTabClicked(
                                        new HTTitleTabEventArgs
                                        {
                                            Tab = clickedTab,
                                            TabIndex = _parentForm.SelectedTabIndex,
                                            Action = TabControlAction.Selected,
                                            WasDragging = _wasDragging
                                        });
                                }
                            }
                        }

                        // Otherwise, if the user clicked the add button, call CreateTab to add a new tab to the list and select it
                        else if (_parentForm.TabRenderer.IsOverAddButton(relativeCursorPosition2))
                        {
                            _parentForm.AddNewTab();
                        }

                        if ((WM)m.Msg == WM.WM_LBUTTONUP || (WM)m.Msg == WM.WM_NCLBUTTONUP)
                        {
                            OnMouseUp(new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X, Cursor.Position.Y, 0));
                        }
                    }

                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>Event handler that is called when <see cref="_parentForm" />'s <see cref="Form.Activated" /> event is fired.</summary>
        /// <param name="sender">Object from which this event originated.</param>
        /// <param name="e">Arguments associated with the event.</param>
        private void _parentForm_Activated(object sender, EventArgs e)
        {
            _active = true;
            Render();
        }

        /// <summary>Event handler that is called when <see cref="_parentForm" />'s <see cref="Form.Deactivate" /> event is fired.</summary>
        /// <param name="sender">Object from which this event originated.</param>
        /// <param name="e">Arguments associated with the event.</param>
        private void _parentForm_Deactivate(object sender, EventArgs e)
        {
            _active = false;
            Render();
        }

        /// <summary>Event handler that is called when <see cref="_parentForm" />'s <see cref="Component.Disposed" /> event is fired.</summary>
        /// <param name="sender">Object from which this event originated.</param>
        /// <param name="e">Arguments associated with the event.</param>
        private void _parentForm_Disposed(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Contains information on mouse events captured by <see cref="HTTitleTabsOverlay.MouseHookCallback" /> and processed by
        /// <see cref="HTTitleTabsOverlay.InterpretMouseEvents" />.
        /// </summary>
        protected class MouseEvent
        {
            /// <summary>Code for the event.</summary>
            // ReSharper disable InconsistentNaming
            public int nCode
            {
                get;
                set;
            }

            /// <summary>wParam value associated with the event.</summary>
            public IntPtr wParam
            {
                get;
                set;
            }

            /// <summary>lParam value associated with the event.</summary>
            public IntPtr lParam
            {
                get;
                set;
            }

            // ReSharper restore InconsistentNaming

            /// <summary>Data associated with the mouse event.</summary>
            public MSLLHOOKSTRUCT? MouseData
            {
                get;
                set;
            }
        }
    }

    /// <summary>List of possible <see cref="ListWithEvents{T}" /> modifications.</summary>
    public enum ListModification
    {
        /// <summary>The list has been cleared.</summary>
        Cleared = 0,

        /// <summary>A new item has been added.</summary>
        ItemAdded,

        /// <summary>An item has been modified.</summary>
        ItemModified,

        /// <summary>An item has been removed.</summary>
        ItemRemoved,

        /// <summary>A range of items has been added.</summary>
        RangeAdded,

        /// <summary>A range of items has been removed.</summary>
        RangeRemoved
    }

    /// <summary>Provides data for the <see cref="ListWithEvents{T}.CollectionModified" /> events.</summary>
    [Serializable]
    public class ListModificationEventArgs : ListRangeEventArgs
    {
        /// <summary>Modification being made to the list.</summary>
        private readonly ListModification _modification;

        /// <summary>Initializes a new instance of the <see cref="ListModificationEventArgs" /> class.</summary>
        /// <param name="modification">Modification being made to the list.</param>
        /// <param name="startIndex">Index from which the modifications start.</param>
        /// <param name="count">Number of modifications being made.</param>
        public ListModificationEventArgs(ListModification modification, int startIndex, int count)
            : base(startIndex, count)
        {
            _modification = modification;
        }

        /// <summary>Gets the type of list modification.</summary>
        public ListModification Modification => _modification;
    }

    /// <summary>Provides data for the <see cref="ListWithEvents{T}.RangeAdded" /> events.</summary>
    [Serializable]
    public class ListRangeEventArgs : EventArgs
    {
        /// <summary>Number of items in the range.</summary>
        private readonly int _count;

        /// <summary>Index of the first item in the range.</summary>
        private readonly int _startIndex;

        /// <summary>Initializes a new instance of the <see cref="ListRangeEventArgs" /> class.</summary>
        /// <param name="startIndex">Index of the first item in the range.</param>
        /// <param name="count">Number of items in the range.</param>
        public ListRangeEventArgs(int startIndex, int count)
        {
            _startIndex = startIndex;
            _count = count;
        }

        /// <summary>Gets the index of the first item in the range.</summary>
        public int StartIndex => _startIndex;

        /// <summary>Gets the number of items in the range.</summary>
        public int Count => _count;
    }

    /// <summary>Represents a strongly typed list of objects with events.</summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    public class ListWithEvents<T> : List<T>, IList
    {
        /// <summary>Synchronization root for thread safety.</summary>
        private readonly object _syncRoot = new object();

        /// <summary>Flag indicating whether events are being suppressed during an operation.</summary>
        private bool _suppressEvents;

        /// <summary>Initializes a new instance of the <see cref="ListWithEvents{T}" /> class that is empty and has the default initial capacity.</summary>
        public ListWithEvents()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListWithEvents{T}" /> class that contains elements copied from the specified collection and has
        /// sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="ArgumentNullException">The collection is null.</exception>
        public ListWithEvents(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ListWithEvents{T}" /> class that is empty and has the specified initial capacity.</summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException">The capacity is less than 0.</exception>
        public ListWithEvents(int capacity)
            : base(capacity)
        {
        }

        /// <summary>Gets whether the events are currently being suppressed.</summary>
        protected bool EventsSuppressed => _suppressEvents;

        /// <summary>Overloads <see cref="List{T}.this" />.</summary>
        public new virtual T this[int index]
        {
            get => base[index];
            set
            {
                lock (_syncRoot)
                {
                    bool equal = false;

                    // ReSharper disable CompareNonConstrainedGenericWithNull
                    if (base[index] != null)
                    {
                        equal = base[index].Equals(value);
                    }
                    else if (base[index] == null && value == null)
                    {
                        equal = true;
                    }
                    // ReSharper restore CompareNonConstrainedGenericWithNull

                    if (!equal)
                    {
                        base[index] = value;
                        OnItemModified(new ListItemEventArgs(index));
                    }
                }
            }
        }

        /// <summary>Gets an object that can be used to synchronize access to the <see cref="ListWithEvents{T}" />.</summary>
        public object SyncRoot => _syncRoot;

        /// <summary>Adds an item to the end of the list.</summary>
        /// <param name="value">Item to add to the list.</param>
        /// <returns>Index of the new item in the list.</returns>
        int IList.Add(object value)
        {
            if (value is T)
            {
                Add((T)value);
                return Count - 1;
            }

            return -1;
        }

        /// <summary>Overloads <see cref="List{T}.Clear" />.</summary>
        /// <remarks>This operation is thread-safe.</remarks>
        public new virtual void Clear()
        {
            lock (_syncRoot)
            {
                base.Clear();
            }

            OnCleared(EventArgs.Empty);
        }

        /// <summary>Overloads <see cref="List{T}.RemoveAt" />.</summary>
        /// <remarks>This operation is thread-safe.</remarks>
        public new virtual void RemoveAt(int index)
        {
            lock (_syncRoot)
            {
                base.RemoveAt(index);
            }

            OnItemRemoved(EventArgs.Empty);
        }

        /// <summary>Occurs whenever the list's content is modified.</summary>
        public event EventHandler<ListModificationEventArgs> CollectionModified;

        /// <summary>Occurs whenever the list is cleared.</summary>
        public event EventHandler Cleared;

        /// <summary>Occurs whenever a new item is added to the list.</summary>
        public event EventHandler<ListItemEventArgs> ItemAdded;

        /// <summary>Occurs whenever a item is modified.</summary>
        public event EventHandler<ListItemEventArgs> ItemModified;

        /// <summary>Occurs whenever an  item is removed from the list.</summary>
        public event EventHandler ItemRemoved;

        /// <summary>Occurs whenever a range of items is added to the list.</summary>
        public event EventHandler<ListRangeEventArgs> RangeAdded;

        /// <summary>Occurs whenever a range of items is removed from the list.</summary>
        public event EventHandler RangeRemoved;

        /// <summary>Overloads <see cref="List{T}.Add" />.</summary>
        /// <remarks>This operation is thread-safe.</remarks>
        public new virtual void Add(T item)
        {
            int count;

            lock (_syncRoot)
            {
                base.Add(item);
                count = Count - 1;
            }

            OnItemAdded(new ListItemEventArgs(count));
        }

        /// <summary>Overloads <see cref="List{T}.AddRange" />.</summary>
        /// <remarks>This operation is thread-safe.</remarks>
        public new virtual void AddRange(IEnumerable<T> collection)
        {
            lock (_syncRoot)
            {
                InsertRange(Count, collection);
            }
        }

        /// <summary>Overloads <see cref="List{T}.Insert" />.</summary>
        /// <remarks>This operation is thread-safe.</remarks>
        public new virtual void Insert(int index, T item)
        {
            lock (_syncRoot)
            {
                base.Insert(index, item);
            }

            OnItemAdded(new ListItemEventArgs(index));
        }

        /// <summary>Overloads <see cref="List{T}.InsertRange" />.</summary>
        /// <remarks>This operation is thread-safe.</remarks>
        public new virtual void InsertRange(int index, IEnumerable<T> collection)
        {
            int count;

            lock (_syncRoot)
            {
                base.InsertRange(index, collection);
                count = Count - index;
            }

            OnRangeAdded(new ListRangeEventArgs(index, count));
        }

        /// <summary>Overloads <see cref="List{T}.Remove" />.</summary>
        /// <remarks>This operation is thread-safe.</remarks>
        public new virtual bool Remove(T item)
        {
            bool result;

            lock (_syncRoot)
            {
                result = base.Remove(item);
            }

            // Raise the event only if the removal was successful
            if (result)
            {
                OnItemRemoved(EventArgs.Empty);
            }

            return result;
        }

        /// <summary>Overloads <see cref="List{T}.RemoveAll" />.</summary>
        /// <remarks>This operation is thread-safe.</remarks>
        public new virtual int RemoveAll(Predicate<T> match)
        {
            int count;

            lock (_syncRoot)
            {
                count = base.RemoveAll(match);
            }

            // Raise the event only if the removal was successful
            if (count > 0)
            {
                OnRangeRemoved(EventArgs.Empty);
            }

            return count;
        }

        /// <summary>Overloads <see cref="List{T}.RemoveRange" />.</summary>
        /// <remarks>This operation is thread-safe.</remarks>
        public new virtual void RemoveRange(int index, int count)
        {
            int listCountOld, listCountNew;

            lock (_syncRoot)
            {
                listCountOld = Count;
                base.RemoveRange(index, count);
                listCountNew = Count;
            }

            // Raise the event only if the removal was successful
            if (listCountOld != listCountNew)
            {
                OnRangeRemoved(EventArgs.Empty);
            }
        }

        /// <summary>Removes the specified list of entries from the collection.</summary>
        /// <param name="collection">Collection to be removed from the list.</param>
        /// <remarks>
        /// This operation employs <see cref="Remove" /> method for removing each individual item which is thread-safe.  However overall operation isn't atomic,
        /// and hence does not guarantee thread-safety.
        /// </remarks>
        public virtual void RemoveRange(List<T> collection)
        {
            // ReSharper disable ForCanBeConvertedToForeach
            for (int i = 0; i < collection.Count; i++)
            {
                // ReSharper restore ForCanBeConvertedToForeach
                Remove(collection[i]);
            }
        }

        /// <summary>Stops raising events until <see cref="ResumeEvents" /> is called.</summary>
        public void SuppressEvents()
        {
            _suppressEvents = true;
        }

        /// <summary>Resumes raising events after <see cref="SuppressEvents" /> call.</summary>
        public void ResumeEvents()
        {
            _suppressEvents = false;
        }

        /// <summary>Raises <see cref="CollectionModified" /> and <see cref="Cleared" /> events.</summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected virtual void OnCleared(EventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }

            if (Cleared != null)
            {
                Cleared(this, e);
            }

            OnCollectionModified(new ListModificationEventArgs(ListModification.Cleared, -1, -1));
        }

        /// <summary>Raises <see cref="CollectionModified" /> events.</summary>
        /// <param name="e">An <see cref="ListModificationEventArgs" /> that contains the event data.</param>
        protected virtual void OnCollectionModified(ListModificationEventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }

            if (CollectionModified != null)
            {
                CollectionModified(this, e);
            }
        }

        /// <summary>Raises <see cref="CollectionModified" /> and <see cref="ItemAdded" /> events.</summary>
        /// <param name="e">An <see cref="ListItemEventArgs" /> that contains the event data.</param>
        protected virtual void OnItemAdded(ListItemEventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }

            if (ItemAdded != null)
            {
                ItemAdded(this, e);
            }

            OnCollectionModified(new ListModificationEventArgs(ListModification.ItemAdded, e.ItemIndex, 1));
        }

        /// <summary>Raises <see cref="CollectionModified" /> and <see cref="ItemModified" /> events.</summary>
        /// <param name="e">An <see cref="ListItemEventArgs" /> that contains the event data.</param>
        protected virtual void OnItemModified(ListItemEventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }

            if (ItemModified != null)
            {
                ItemModified(this, e);
            }

            OnCollectionModified(new ListModificationEventArgs(ListModification.ItemModified, e.ItemIndex, 1));
        }

        /// <summary>Raises <see cref="CollectionModified" /> and <see cref="ItemRemoved" /> events.</summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected virtual void OnItemRemoved(EventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }

            if (ItemRemoved != null)
            {
                ItemRemoved(this, e);
            }

            OnCollectionModified(new ListModificationEventArgs(ListModification.ItemRemoved, -1, 1));
        }

        /// <summary>Raises <see cref="CollectionModified" /> and <see cref="RangeAdded" /> events.</summary>
        /// <param name="e">An <see cref="ListRangeEventArgs" /> that contains the event data.</param>
        protected virtual void OnRangeAdded(ListRangeEventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }

            if (RangeAdded != null)
            {
                RangeAdded(this, e);
            }

            OnCollectionModified(new ListModificationEventArgs(ListModification.RangeAdded, e.StartIndex, e.Count));
        }

        /// <summary>Raises <see cref="CollectionModified" /> and <see cref="RangeRemoved" /> events.</summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected virtual void OnRangeRemoved(EventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }

            if (RangeRemoved != null)
            {
                RangeRemoved(this, e);
            }

            OnCollectionModified(new ListModificationEventArgs(ListModification.RangeRemoved, -1, -1));
        }
    }

    /// <summary>Event arguments class for an event that occurs on a collection of collection of <see cref="HTTitleTab" />s.</summary>
    public class HTTitleTabEventArgs : EventArgs
    {
        /// <summary>Action that is being performed.</summary>
        public TabControlAction? Action
        {
            get;
            set;
        }

        /// <summary>The tab that the <see cref="Action" /> is being performed on.</summary>
        public HTTitleTab Tab
        {
            get;
            set;
        }

        /// <summary>Index of the tab within the collection.</summary>
        public int TabIndex
        {
            get;
            set;
        }

        /// <summary>Flag indicating if the user was dragging the tab when the event occurred.</summary>
        public bool WasDragging
        {
            get;
            set;
        }
    }

    /// <summary>Event arguments class for a cancelable event that occurs on a collection of collection of <see cref="HTTitleTab" />s.</summary>
    public class HTTitleTabCancelEventArgs : CancelEventArgs
    {
        /// <summary>Action that is being performed.</summary>
        public TabControlAction Action
        {
            get;
            set;
        }

        /// <summary>The tab that the <see cref="Action" /> is being performed on.</summary>
        public HTTitleTab Tab
        {
            get;
            set;
        }

        /// <summary>Index of the tab within the collection.</summary>
        public int TabIndex
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Application context to use when starting a <see cref="HTTitleTabs" /> application via <see cref="Application.Run(ApplicationContext)" />.  Used to
    /// track open windows so that the entire application doesn't quit when the first-opened window is closed.
    /// </summary>
    public class HTTitleTabsApplicationContext : ApplicationContext
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://haltroy.com/htalt/HTAlt.WinForms/HTTitleTabsApplicationContext");
        private readonly Version firstHTAltVersion = new Version("0.1.4.0");
        private readonly string originProjectName = "EasyTabs";
        private readonly Uri originProject = new Uri("https://github.com/lstratman/EasyTabs");
        private readonly string description = "Application context to use when starting a HTTitleTabs application via Application.Run(ApplicationContext).  Used to track open windows so that the entire application doesn't quit when the first-opened window is closed.";

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

        /// <summary>List of all opened windows.</summary>
        protected List<HTTitleTabs> _openWindows = new List<HTTitleTabs>();

        /// <summary>List of all opened windows.</summary>
        public List<HTTitleTabs> OpenWindows => _openWindows;

        /// <summary>Constructor; takes the initial window to display and, if it's not closing, opens it and shows it.</summary>
        /// <param name="initialFormInstance">Initial window to display.</param>
        public void Start(HTTitleTabs initialFormInstance)
        {
            if (initialFormInstance.IsClosing)
            {
                ExitThread();
            }
            else
            {
                OpenWindow(initialFormInstance);
                initialFormInstance.Show();
            }
        }

        /// <summary>
        /// Adds <paramref name="window" /> to <see cref="_openWindows" /> and attaches event handlers to its <see cref="Form.FormClosed" /> event to keep track
        /// of it.
        /// </summary>
        /// <param name="window">Window that we're opening.</param>
        public void OpenWindow(HTTitleTabs window)
        {
            if (!_openWindows.Contains(window))
            {
                window.ApplicationContext = this;

                _openWindows.Add(window);
                window.FormClosed += window_FormClosed;
            }
        }

        /// <summary>
        /// Handler method that's called when an item in <see cref="_openWindows" /> has its <see cref="Form.FormClosed" /> event invoked.  Removes the window
        /// from <see cref="_openWindows" /> and, if there are no more windows open, calls <see cref="ApplicationContext.ExitThread" />.
        /// </summary>
        /// <param name="sender">Object from which this event originated.</param>
        /// <param name="e">Arguments associated with the event.</param>
        protected void window_FormClosed(object sender, FormClosedEventArgs e)
        {
            _openWindows.Remove((HTTitleTabs)sender);

            if (_openWindows.Count == 0)
            {
                ExitThread();
            }
        }
    }

    /// <summary>Provides data for the <see cref="ListWithEvents{T}.ItemAdded" /> events.</summary>
    [Serializable]
    public class ListItemEventArgs : EventArgs
    {
        /// <summary>Index of the item being changed.</summary>
        private readonly int _itemIndex;

        /// <summary>Initializes a new instance of the <see cref="ListItemEventArgs" /> class.</summary>
        /// <param name="itemIndex">Index of the item being changed.</param>
        public ListItemEventArgs(int itemIndex)
        {
            _itemIndex = itemIndex;
        }

        /// <summary>Gets the index of the item changed.</summary>
        public int ItemIndex => _itemIndex;
    }

    /// <summary>The type of theme being used to render the desktop.</summary>
    public enum DisplayType
    {
        /// <summary>Windows 2000-esque theme.</summary>
        Classic,

        /// <summary>Contemporary theme, but without Aero enabled.</summary>
        Basic,

        /// <summary>Full compositing enabled in the theme.</summary>
        Aero
    }

    /// <summary>
    /// Contains a semi-transparent window with a thumbnail of a tab that has been torn away from its parent window.  This thumbnail will follow the cursor
    /// around as it's dragged around the screen.
    /// </summary>
    public class TornTabForm : Form
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://haltroy.com/htalt/HTAlt.WinForms/TornTabForm");
        private readonly Version firstHTAltVersion = new Version("0.1.4.0");
        private readonly string originProjectName = "EasyTabs";
        private readonly Uri originProject = new Uri("https://github.com/lstratman/EasyTabs");
        private readonly string description = "Contains a semi-transparent window with a thumbnail of a tab that has been torn away from its parent window.  This thumbnail will follow the cursor around as it's dragged around the screen.";

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

        /// <summary>Window that contains the actual thumbnail image data.</summary>
        private readonly LayeredWindow _layeredWindow;

        /// <summary>Offset of the cursor within the torn tab representation while dragging.</summary>
        protected Point _cursorOffset;

        /// <summary>Pointer to the low-level mouse hook callback (<see cref="MouseHookCallback" />).</summary>
        protected IntPtr _hookId;

        /// <summary>Flag indicating whether <see cref="_hookproc" /> is installed.</summary>
        protected bool _hookInstalled = false;

        /// <summary>Delegate of <see cref="MouseHookCallback" />; declared as a member variable to keep it from being garbage collected.</summary>
        protected HOOKPROC _hookproc = null;

        /// <summary>Flag indicating whether or not the constructor has finished running.</summary>
        private bool _initialized;

        /// <summary>Thumbnail of the tab we are dragging.</summary>
        protected Bitmap _tabThumbnail;

        /// <summary>Constructor; initializes the window and constructs the tab thumbnail image to use when dragging.</summary>
        /// <param name="tab">Tab that was torn out of its parent window.</param>
        /// <param name="tabRenderer">Renderer instance to use when drawing the actual tab.</param>
        public TornTabForm(HTTitleTab tab, HTBaseTabRenderer tabRenderer)
        {
            _layeredWindow = new LayeredWindow();
            _initialized = false;

            // Set drawing styles
            SetStyle(ControlStyles.DoubleBuffer, true);

            // This should show up as a semi-transparent borderless window
            Opacity = 0.70;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            BackColor = Color.Fuchsia;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
            TransparencyKey = Color.Fuchsia;
            AllowTransparency = true;

            Disposed += TornTabForm_Disposed;

            // Get the tab thumbnail (full size) and then draw the actual representation of the tab onto it as well
            Bitmap tabContents = tab.GetImage();
            Bitmap contentsAndTab = new Bitmap(tabContents.Width, tabContents.Height + tabRenderer.TabHeight, tabContents.PixelFormat);
            Graphics tabGraphics = Graphics.FromImage(contentsAndTab);

            tabGraphics.DrawImage(tabContents, 0, tabRenderer.TabHeight);

            bool oldShowAddButton = tabRenderer.ShowAddButton;

            tabRenderer.ShowAddButton = false;
            tabRenderer.Render(
                new List<HTTitleTab>
                {
                    tab
                }, tabGraphics, new Point(0, 0), new Point(0, 0), true);
            tabRenderer.ShowAddButton = oldShowAddButton;

            // Scale the thumbnail down to half size
            _tabThumbnail = new Bitmap(contentsAndTab.Width / 2, contentsAndTab.Height / 2, contentsAndTab.PixelFormat);
            Graphics thumbnailGraphics = Graphics.FromImage(_tabThumbnail);

            thumbnailGraphics.InterpolationMode = InterpolationMode.High;
            thumbnailGraphics.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            thumbnailGraphics.DrawImage(contentsAndTab, 0, 0, _tabThumbnail.Width, _tabThumbnail.Height);

            Width = _tabThumbnail.Width - 1;
            Height = _tabThumbnail.Height - 1;

            _cursorOffset = new Point(tabRenderer.TabContentWidth / 4, tabRenderer.TabHeight / 4);

            SetWindowPosition(Cursor.Position);
        }

        /// <summary>
        /// Event handler that's called from <see cref="IDisposable.Dispose" />; calls <see cref="User32.UnhookWindowsHookEx" /> to unsubscribe from the mouse
        /// hook.
        /// </summary>
        /// <param name="sender">Object from which this event originated.</param>
        /// <param name="e">Arguments associated with this event.</param>
        private void TornTabForm_Disposed(object sender, EventArgs e)
        {
            User32.UnhookWindowsHookEx(_hookId);
        }

        /// <summary>Hook callback to process <see cref="WM.WM_MOUSEMOVE" /> messages to move the thumbnail along with the cursor.</summary>
        /// <param name="nCode">The message being received.</param>
        /// <param name="wParam">Additional information about the message.</param>
        /// <param name="lParam">Additional information about the message.</param>
        /// <returns>A zero value if the procedure processes the message; a nonzero value if the procedure ignores the message.</returns>
        protected IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (int)WM.WM_MOUSEMOVE == (int)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                Point cursorPosition = new Point(hookStruct.pt.x, hookStruct.pt.y);

                SetWindowPosition(cursorPosition);
            }

            return User32.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        /// <summary>Updates the window position to keep up with the cursor's movement.</summary>
        /// <param name="cursorPosition">Current position of the cursor.</param>
        protected void SetWindowPosition(Point cursorPosition)
        {
            Left = cursorPosition.X - _cursorOffset.X;
            Top = cursorPosition.Y - _cursorOffset.Y;

            UpdateLayeredBackground();
        }

        /// <summary>
        /// Event handler that's called when the window is loaded; shows <see cref="_layeredWindow" /> and installs the mouse hook via
        /// <see cref="User32.SetWindowsHookEx" />.
        /// </summary>
        /// <param name="e">Arguments associated with this event.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _initialized = true;

            UpdateLayeredBackground();

            _layeredWindow.Show();
            _layeredWindow.Enabled = false;

            // Installs the mouse hook
            if (!_hookInstalled)
            {
                using (Process curProcess = Process.GetCurrentProcess())
                {
                    using (ProcessModule curModule = curProcess.MainModule)
                    {
                        _hookproc = MouseHookCallback;
                        _hookId = User32.SetWindowsHookEx(WH.WH_MOUSE_LL, _hookproc, Kernel32.GetModuleHandle(curModule.ModuleName), 0);
                    }
                }

                _hookInstalled = true;
            }
        }

        /// <summary>Event handler that is called when the window is closing; closes <see cref="_layeredWindow" /> as well.</summary>
        /// <param name="e">Arguments associated with this event.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _layeredWindow.Close();
        }

        /// <summary>
        /// Calls <see cref="LayeredWindow.UpdateWindow" /> to update the position of the thumbnail and blend it properly with the underlying desktop
        /// elements.
        /// </summary>
        public void UpdateLayeredBackground()
        {
            if (_tabThumbnail == null || !_initialized)
            {
                return;
            }

            byte opacity = (byte)(Opacity * 255);
            _layeredWindow.UpdateWindow(
                _tabThumbnail, opacity, Width, Height, new POINT
                {
                    x = Location.X,
                    y = Location.Y
                });
        }
    }

    /// <summary>Form that actually displays the thumbnail content for <see cref="TornTabForm" />.</summary>
    internal class LayeredWindow : Form
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://haltroy.com/htalt/HTAlt.WinForms/LayeredWindow");
        private readonly Version firstHTAltVersion = new Version("0.1.4.0");
        private readonly string originProjectName = "EasyTabs";
        private readonly Uri originProject = new Uri("https://github.com/lstratman/EasyTabs");
        private readonly string description = "Form that actually displays the thumbnail content for TornTabForm.";

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

        /// <summary>Default constructor.</summary>
        public LayeredWindow()
        {
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
        }

        /// <summary>
        /// Makes sure that the window is created with an <see cref="WS_EX.WS_EX_LAYERED" /> flag set so that it can be alpha-blended properly with the desktop
        /// contents underneath it.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= (int)WS_EX.WS_EX_LAYERED;

                return createParams;
            }
        }

        /// <summary>
        /// Renders the tab thumbnail (<paramref name="image" />) using the given dimensions and coordinates and blends it properly with the underlying desktop
        /// elements.
        /// </summary>
        /// <param name="image">Thumbnail to display.</param>
        /// <param name="opacity">Opacity that <paramref name="image" /> should be displayed with.</param>
        /// <param name="width">Width of <paramref name="image" />.</param>
        /// <param name="height">Height of <paramref name="image" />.</param>
        /// <param name="position">Screen position that <paramref name="image" /> should be displayed at.</param>
        public void UpdateWindow(Bitmap image, byte opacity, int width, int height, POINT position)
        {
            IntPtr windowHandle = User32.GetWindowDC(Handle);
            IntPtr deviceContextHandle = Gdi32.CreateCompatibleDC(windowHandle);
            IntPtr bitmapHandle = image.GetHbitmap(Color.FromArgb(0));
            IntPtr oldBitmapHandle = Gdi32.SelectObject(deviceContextHandle, bitmapHandle);
            SIZE size = new SIZE
            {
                cx = 0,
                cy = 0
            };
            POINT destinationPosition = new POINT
            {
                x = 0,
                y = 0
            };

            if (width == -1 || height == -1)
            {
                // No width and height specified, use the size of the image
                size.cx = image.Width;
                size.cy = image.Height;
            }
            else
            {
                // Use whichever size is smallest, so that the image will be clipped if necessary
                size.cx = Math.Min(image.Width, width);
                size.cy = Math.Min(image.Height, height);
            }

            // Set the opacity and blend the image with the underlying desktop elements using User32.UpdateLayeredWindow
            BLENDFUNCTION blendFunction = new BLENDFUNCTION
            {
                BlendOp = Convert.ToByte((int)AC.AC_SRC_OVER),
                SourceConstantAlpha = opacity,
                AlphaFormat = Convert.ToByte((int)AC.AC_SRC_ALPHA),
                BlendFlags = 0
            };

            User32.UpdateLayeredWindow(Handle, windowHandle, ref position, ref size, deviceContextHandle, ref destinationPosition, 0, ref blendFunction, ULW.ULW_ALPHA);

            Gdi32.SelectObject(deviceContextHandle, oldBitmapHandle);
            Gdi32.DeleteObject(bitmapHandle);
            Gdi32.DeleteDC(deviceContextHandle);
            User32.ReleaseDC(Handle, windowHandle);
        }
    }
}