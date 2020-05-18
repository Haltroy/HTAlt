using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;
using Win32Interop.Enums;
using Win32Interop.Methods;
using Win32Interop.Structs;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace HTAlt
{
	/// <summary>
	/// Base class that contains the functionality to render tabs within a WinForms application's title bar area. This  is done through a borderless overlay
	/// window (<see cref="_overlay" />) rendered on top of the non-client area at the top of this window.  All an implementing class will need to do is set
	/// the <see cref="TabRenderer" /> property and begin adding tabs to <see cref="Tabs" />.
	/// </summary>
	public abstract partial class HTTitleBarTabs : Form
	{
		/// <summary>
		/// Event delegate for <see cref="HTTitleBarTabs.TabDeselecting" /> and <see cref="HTTitleBarTabs.TabSelecting" /> that allows subscribers to cancel the
		/// event and keep it from proceeding.
		/// </summary>
		/// <param name="sender">Object for which this event was raised.</param>
		/// <param name="e">Data associated with the event.</param>
		public delegate void HTTitleBarTabCancelEventHandler(object sender, HTTitleBarTabCancelEventArgs e);

		/// <summary>Event delegate for <see cref="HTTitleBarTabs.TabSelected" /> and <see cref="HTTitleBarTabs.TabDeselected" />.</summary>
		/// <param name="sender">Object for which this event was raised.</param>
		/// <param name="e">Data associated with the event.</param>
		public delegate void HTTitleBarTabEventHandler(object sender, HTTitleBarTabEventArgs e);

		/// <summary>Flag indicating whether or not each tab has an Aero Peek entry allowing the user to switch between tabs from the taskbar.</summary>
		protected bool _aeroPeekEnabled = true;

		/// <summary>Height of the non-client area at the top of the window.</summary>
		protected int _nonClientAreaHeight;

		/// <summary>Borderless window that is rendered over top of the non-client area of this window.</summary>
		protected internal HTTitleBarTabsOverlay _overlay;

		/// <summary>The preview images for each tab used to display each tab when Aero Peek is activated.</summary>
		protected Dictionary<Form, Bitmap> _previews = new Dictionary<Form, Bitmap>();

		/// <summary>
		/// When switching between tabs, this keeps track of the tab that was previously active so that, when it is switched away from, we can generate a fresh
		/// Aero Peek preview image for it.
		/// </summary>
		protected HTTitleBarTab _previousActiveTab = null;

		/// <summary>Maintains the previous window state so that we can respond properly to maximize/restore events in <see cref="OnSizeChanged" />.</summary>
		protected FormWindowState? _previousWindowState;

		/// <summary>Class responsible for actually rendering the tabs in <see cref="_overlay" />.</summary>
		protected HTTabBaseRenderer _tabRenderer;

		/// <summary>List of tabs to display for this window.</summary>
		protected ListWithEvents<HTTitleBarTab> _tabs = new ListWithEvents<HTTitleBarTab>();

		/// <summary>Default constructor.</summary>
		protected HTTitleBarTabs()
		{
			_previousWindowState = null;
			ExitOnLastTabClose = true;
			InitializeComponent();
			SetWindowThemeAttributes(WTNCA.NODRAWCAPTION | WTNCA.NODRAWICON);

			_tabs.CollectionModified += _tabs_CollectionModified;

			// Set the window style so that we take care of painting the non-client area, a redraw is triggered when the size of the window changes, and the 
			// window itself has a transparent background color (otherwise the non-client area will simply be black when the window is maximized)
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

			Tooltip = new ToolTip
			{
				AutoPopDelay = 5000,
				AutomaticDelay = 500
			};

			ShowTooltips = true;
		}

		/// <summary>Flag indicating whether composition is enabled on the desktop.</summary>
		internal bool IsCompositionEnabled
		{
			get
			{
				// This tests that the OS will support what we want to do. Will be false on Windows XP and earlier, as well as on Vista and 7 with Aero Glass 
				// disabled.
				bool hasComposition;
				Dwmapi.DwmIsCompositionEnabled(out hasComposition);

				return hasComposition;
			}
		}

		/// <summary>Flag indicating whether or not each tab has an Aero Peek entry allowing the user to switch between tabs from the taskbar.</summary>
		public bool AeroPeekEnabled
		{
			get
			{
				return _aeroPeekEnabled;
			}

			set
			{
				_aeroPeekEnabled = value;

				// Clear out any previously generate thumbnails if we are no longer enabled
				if (!_aeroPeekEnabled)
				{
					foreach (HTTitleBarTab tab in Tabs)
					{
						TaskbarManager.Instance.TabbedThumbnail.RemoveThumbnailPreview(tab.Content);
					}

					_previews.Clear();
				}

				else
				{
					foreach (HTTitleBarTab tab in Tabs)
					{
						CreateThumbnailPreview(tab);
					}

					if (SelectedTab != null)
					{
						TaskbarManager.Instance.TabbedThumbnail.SetActiveTab(SelectedTab.Content);
					}
				}
			}
		}

        /// <summary>Flag indicating whether a tooltip should be shown when hovering over a tab.</summary>
		public bool ShowTooltips
		{
			get;
			set;
		}

        /// <summary>Tooltip UI element to show when hovering over a tab.</summary>
		public ToolTip Tooltip
		{
			get;
			set;
		}

		/// <summary>List of tabs to display for this window.</summary>
		public ListWithEvents<HTTitleBarTab> Tabs
		{
			get
			{
				return _tabs;
			}
		}

		/// <summary>The renderer to use when drawing the tabs.</summary>
		public HTTabBaseRenderer TabRenderer
		{
			get
			{
				return _tabRenderer;
			}

			set
			{
				_tabRenderer = value;
				SetFrameSize();
			}
		}

		/// <summary>The tab that is currently selected by the user.</summary>
		public HTTitleBarTab SelectedTab
		{
			get
			{
				return Tabs.FirstOrDefault((HTTitleBarTab t) => t.Active);
			}

			set
			{
				SelectedTabIndex = Tabs.IndexOf(value);
			}
		}

		/// <summary>Gets or sets the index of the tab that is currently selected by the user.</summary>
		public int SelectedTabIndex
		{
			get
			{
				return Tabs.FindIndex((HTTitleBarTab t) => t.Active);
			}

			set
			{
				HTTitleBarTab selectedTab = SelectedTab;
				int selectedTabIndex = SelectedTabIndex;

				if (selectedTab != null && selectedTabIndex != value)
				{
					// Raise the TabDeselecting event
					HTTitleBarTabCancelEventArgs e = new HTTitleBarTabCancelEventArgs
					{
						Action = TabControlAction.Deselecting,
						Tab = selectedTab,
						TabIndex = selectedTabIndex
					};

					OnTabDeselecting(e);

					// If the subscribers to the event cancelled it, return before we do anything else
					if (e.Cancel)
					{
						return;
					}

					selectedTab.Active = false;

					// Raise the TabDeselected event
					OnTabDeselected(
						new HTTitleBarTabEventArgs
						{
							Tab = selectedTab,
							TabIndex = selectedTabIndex,
							Action = TabControlAction.Deselected
						});
				}

				if (value != -1)
				{
					// Raise the TabSelecting event
					HTTitleBarTabCancelEventArgs e = new HTTitleBarTabCancelEventArgs
					{
						Action = TabControlAction.Selecting,
						Tab = Tabs[value],
						TabIndex = value
					};

					OnTabSelecting(e);

					// If the subscribers to the event cancelled it, return before we do anything else
					if (e.Cancel)
					{
						return;
					}

					Tabs[value].Active = true;

					// Raise the TabSelected event
					OnTabSelected(
						new HTTitleBarTabEventArgs
						{
							Tab = Tabs[value],
							TabIndex = value,
							Action = TabControlAction.Selected
						});
				}

				if (_overlay != null)
				{
					_overlay.Render();
				}
			}
		}

		/// <summary>Flag indicating whether the application itself should exit when the last tab is closed.</summary>
		public bool ExitOnLastTabClose
		{
			get;
			set;
		}

		/// <summary>Flag indicating whether we are in the process of closing the window.</summary>
		public bool IsClosing
		{
			get;
			set;
		}

		/// <summary>Application context under which this particular window runs.</summary>
		public HTTitleBarTabsApplicationContext ApplicationContext
		{
			get;
			internal set;
		}

		/// <summary>Height of the "glassed" area of the window's non-client area.</summary>
		public int NonClientAreaHeight
		{
			get
			{
				return _nonClientAreaHeight;
			}
		}

		/// <summary>Area of the screen in which tabs can be dropped for this window.</summary>
		public Rectangle TabDropArea
		{
			get
			{
				return _overlay.TabDropArea;
			}
		}

		/// <summary>Calls <see cref="Uxtheme.SetWindowThemeAttribute" /> to set various attributes on the window.</summary>
		/// <param name="attributes">Attributes to set on the window.</param>
		private void SetWindowThemeAttributes(WTNCA attributes)
		{
			WTA_OPTIONS options = new WTA_OPTIONS
			{
				dwFlags = attributes,
				dwMask = WTNCA.VALIDBITS
			};

			// The SetWindowThemeAttribute API call takes care of everything
			Uxtheme.SetWindowThemeAttribute(Handle, WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT, ref options, (uint) Marshal.SizeOf(typeof (WTA_OPTIONS)));
		}

		/// <summary>
		/// Event handler that is invoked when the <see cref="Form.Load" /> event is fired.  Instantiates <see cref="_overlay" /> and clears out the window's
		/// caption.
		/// </summary>
		/// <param name="e">Arguments associated with the event.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			_overlay = HTTitleBarTabsOverlay.GetInstance(this);

			if (TabRenderer != null)
			{
				_overlay.MouseMove += TabRenderer.Overlay_MouseMove;
				_overlay.MouseUp += TabRenderer.Overlay_MouseUp;
				_overlay.MouseDown += TabRenderer.Overlay_MouseDown;
			}
		}

		/// <summary>
		/// When the window's state (maximized, minimized, or restored) changes, this sets the size of the non-client area at the top of the window properly so
		/// that the tabs can be displayed.
		/// </summary>
		protected void SetFrameSize()
		{
			if (TabRenderer == null || WindowState == FormWindowState.Minimized)
			{
				return;
			}

			int topPadding;

			if (WindowState == FormWindowState.Maximized)
			{
				topPadding = TabRenderer.TabHeight - SystemInformation.CaptionHeight;
			}

			else
			{
				topPadding = (TabRenderer.TabHeight + SystemInformation.CaptionButtonSize.Height) - SystemInformation.CaptionHeight;
			}

			Padding = new Padding(
				Padding.Left, topPadding > 0
					? topPadding
					: 0, Padding.Right, Padding.Bottom);

			// Set the margins and extend the frame into the client area
			MARGINS margins = new MARGINS
			{
				cxLeftWidth = 0,
				cxRightWidth = 0,
				cyBottomHeight = 0,
				cyTopHeight = topPadding > 0
									  ? topPadding
									  : 0
			};

			Dwmapi.DwmExtendFrameIntoClientArea(Handle, ref margins);

			_nonClientAreaHeight = SystemInformation.CaptionHeight + (topPadding > 0
				? topPadding
				: 0);

			if (AeroPeekEnabled)
			{
				foreach (
					TabbedThumbnail preview in
						Tabs.Select(tab => TaskbarManager.Instance.TabbedThumbnail.GetThumbnailPreview(tab.Content)).Where(preview => preview != null))
				{
					preview.PeekOffset = new Vector(Padding.Left, Padding.Top);
				}
			}
		}

		/// <summary>Event that is raised immediately prior to a tab being deselected (<see cref="TabDeselected" />).</summary>
		public event HTTitleBarTabCancelEventHandler TabDeselecting;

		/// <summary>Event that is raised after a tab has been deselected.</summary>
		public event HTTitleBarTabEventHandler TabDeselected;

		/// <summary>Event that is raised immediately prior to a tab being selected (<see cref="TabSelected" />).</summary>
		public event HTTitleBarTabCancelEventHandler TabSelecting;

		/// <summary>Event that is raised after a tab has been selected.</summary>
		public event HTTitleBarTabEventHandler TabSelected;

		/// <summary>Event that is raised after a tab has been clicked.</summary>
		public event HTTitleBarTabEventHandler TabClicked;

		/// <summary>
		/// Callback that should be implemented by the inheriting class that will create a new <see cref="HTTitleBarTab" /> object when the add button is
		/// clicked.
		/// </summary>
		/// <returns>A newly created tab.</returns>
		public abstract HTTitleBarTab CreateTab();

		/// <summary>Callback for the <see cref="TabClicked" /> event.</summary>
		/// <param name="e">Arguments associated with the event.</param>
		protected internal void OnTabClicked(HTTitleBarTabEventArgs e)
		{
			if (TabClicked != null)
			{
				TabClicked(this, e);
			}
		}

		/// <summary>
		/// Callback for the <see cref="TabDeselecting" /> event.  Called when a <see cref="HTTitleBarTab" /> is in the process of losing focus.  Grabs an image of
		/// the tab's content to be used when Aero Peek is activated.
		/// </summary>
		/// <param name="e">Arguments associated with the event.</param>
		protected void OnTabDeselecting(HTTitleBarTabCancelEventArgs e)
		{
			if (_previousActiveTab != null && AeroPeekEnabled)
			{
				UpdateTabThumbnail(_previousActiveTab);
			}

			if (TabDeselecting != null)
			{
				TabDeselecting(this, e);
			}
		}

		/// <summary>Generate a new thumbnail image for <paramref name="tab" />.</summary>
		/// <param name="tab">Tab that we need to generate a thumbnail for.</param>
		protected void UpdateTabThumbnail(HTTitleBarTab tab)
		{
			TabbedThumbnail preview = TaskbarManager.Instance.TabbedThumbnail.GetThumbnailPreview(tab.Content);

			if (preview == null)
			{
				return;
			}

			Bitmap bitmap = TabbedThumbnailScreenCapture.GrabWindowBitmap(tab.Content.Handle, tab.Content.Size);

			preview.SetImage(bitmap);

			// If we already had a preview image for the tab, dispose of it
			if (_previews.ContainsKey(tab.Content) && _previews[tab.Content] != null)
			{
				_previews[tab.Content].Dispose();
			}

			_previews[tab.Content] = bitmap;
		}

		/// <summary>Callback for the <see cref="TabDeselected" /> event.</summary>
		/// <param name="e">Arguments associated with the event.</param>
		protected void OnTabDeselected(HTTitleBarTabEventArgs e)
		{
			if (TabDeselected != null)
			{
				TabDeselected(this, e);
			}
		}

		/// <summary>Callback for the <see cref="TabSelecting" /> event.</summary>
		/// <param name="e">Arguments associated with the event.</param>
		protected void OnTabSelecting(HTTitleBarTabCancelEventArgs e)
		{
			ResizeTabContents(e.Tab);

			if (TabSelecting != null)
			{
				TabSelecting(this, e);
			}
		}

		/// <summary>
		/// Callback for the <see cref="TabSelected" /> event.  Called when a <see cref="HTTitleBarTab" /> gains focus.  Sets the active window in Aero Peek via a
		/// call to <see cref="TabbedThumbnailManager.SetActiveTab(Control)" />.
		/// </summary>
		/// <param name="e">Arguments associated with the event.</param>
		protected void OnTabSelected(HTTitleBarTabEventArgs e)
		{
			if (SelectedTabIndex != -1 && _previews.ContainsKey(SelectedTab.Content) && AeroPeekEnabled)
			{
				TaskbarManager.Instance.TabbedThumbnail.SetActiveTab(SelectedTab.Content);
			}

			_previousActiveTab = SelectedTab;

			if (TabSelected != null)
			{
				TabSelected(this, e);
			}
		}

		/// <summary>
		/// Handler method that's called when Aero Peek needs to display a thumbnail for a <see cref="HTTitleBarTab" />; finds the preview bitmap generated in
		/// <see cref="TabDeselecting" /> and returns that.
		/// </summary>
		/// <param name="sender">Object from which this event originated.</param>
		/// <param name="e">Arguments associated with this event.</param>
		private void preview_TabbedThumbnailBitmapRequested(object sender, TabbedThumbnailBitmapRequestedEventArgs e)
		{
			foreach (
				HTTitleBarTab rdcWindow in Tabs.Where(rdcWindow => rdcWindow.Content.Handle == e.WindowHandle && _previews.ContainsKey(rdcWindow.Content)))
			{
				TabbedThumbnail preview = TaskbarManager.Instance.TabbedThumbnail.GetThumbnailPreview(rdcWindow.Content);
				preview.SetImage(_previews[rdcWindow.Content]);

				break;
			}
		}

		/// <summary>
		/// Callback for the <see cref="Control.ClientSizeChanged" /> event that resizes the <see cref="HTTitleBarTab.Content" /> form of the currently selected
		/// tab when the size of the client area for this window changes.
		/// </summary>
		/// <param name="e">Arguments associated with the event.</param>
		protected override void OnClientSizeChanged(EventArgs e)
		{
			base.OnClientSizeChanged(e);

			ResizeTabContents();
		}

		/// <summary>Resizes the <see cref="HTTitleBarTab.Content" /> form of the <paramref name="tab" /> to match the size of the client area for this window.</summary>
		/// <param name="tab">Tab whose <see cref="HTTitleBarTab.Content" /> form we should resize; if not specified, we default to
		/// <see cref="SelectedTab" />.</param>
		public void ResizeTabContents(HTTitleBarTab tab = null)
		{
			if (tab == null)
			{
				tab = SelectedTab;
			}

			if (tab != null)
			{
				tab.Content.Location = new Point(0, Padding.Top - 1);
				tab.Content.Size = new Size(ClientRectangle.Width, ClientRectangle.Height - Padding.Top + 1);
			}
		}

		/// <summary>Override of the handler for the paint background event that is left blank so that code is never executed.</summary>
		/// <param name="e">Arguments associated with the event.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		/// <summary>Forwards a message received by <see cref="HTTitleBarTabsOverlay" /> to the underlying window.</summary>
		/// <param name="m">Message received by the overlay.</param>
		internal void ForwardMessage(ref Message m)
		{
			m.HWnd = Handle;
			WndProc(ref m);
		}

		/// <summary>
		/// Handler method that's called when the user clicks on an Aero Peek preview thumbnail.  Finds the tab associated with the thumbnail and
		/// focuses on it.
		/// </summary>
		/// <param name="sender">Object from which this event originated.</param>
		/// <param name="e">Arguments associated with this event.</param>
		private void preview_TabbedThumbnailActivated(object sender, TabbedThumbnailEventArgs e)
		{
			foreach (HTTitleBarTab tab in Tabs.Where(tab => tab.Content.Handle == e.WindowHandle))
			{
				SelectedTabIndex = Tabs.IndexOf(tab);
				TaskbarManager.Instance.TabbedThumbnail.SetActiveTab(tab.Content);

				break;
			}

			// Restore the window if it was minimized
			if (WindowState == FormWindowState.Minimized)
			{
				User32.ShowWindow(Handle, 3);
			}

			else
			{
				Focus();
			}
		}

		/// <summary>
		/// Handler method that's called when the user clicks the close button in an Aero Peek preview thumbnail.  Finds the window associated with the thumbnail
		/// and calls <see cref="Form.Close" /> on it.
		/// </summary>
		/// <param name="sender">Object from which this event originated.</param>
		/// <param name="e">Arguments associated with this event.</param>
		private void preview_TabbedThumbnailClosed(object sender, TabbedThumbnailEventArgs e)
		{
			foreach (HTTitleBarTab tab in Tabs.Where(tab => tab.Content.Handle == e.WindowHandle))
			{
				CloseTab(tab);

				break;
			}
		}

		/// <summary>Callback that is invoked whenever anything is added or removed from <see cref="Tabs" /> so that we can trigger a redraw of the tabs.</summary>
		/// <param name="sender">Object for which this event was raised.</param>
		/// <param name="e">Arguments associated with the event.</param>
		private void _tabs_CollectionModified(object sender, ListModificationEventArgs e)
		{
			SetFrameSize();

			if (e.Modification == ListModification.ItemAdded || e.Modification == ListModification.RangeAdded)
			{
				for (int i = 0; i < e.Count; i++)
				{
					HTTitleBarTab currentTab = Tabs[i + e.StartIndex];

					currentTab.Content.TextChanged += Content_TextChanged;
					currentTab.Closing += HTTitleBarTabs_Closing;

					if (AeroPeekEnabled)
					{
						TaskbarManager.Instance.TabbedThumbnail.SetActiveTab(CreateThumbnailPreview(currentTab));
					}
				}
			}

			if (_overlay != null)
			{
				_overlay.Render(true);
			}
		}

		/// <summary>
		/// Creates a new thumbnail for <paramref name="tab" /> when the application is initially enabled for AeroPeek or when it is turned on sometime during
		/// execution.
		/// </summary>
		/// <param name="tab">Tab that we are to create the thumbnail for.</param>
		/// <returns>Thumbnail created for <paramref name="tab" />.</returns>
		protected virtual TabbedThumbnail CreateThumbnailPreview(HTTitleBarTab tab)
		{
			TabbedThumbnail preview = TaskbarManager.Instance.TabbedThumbnail.GetThumbnailPreview(tab.Content);

			if (preview != null)
			{
				TaskbarManager.Instance.TabbedThumbnail.RemoveThumbnailPreview(tab.Content);
			}

			preview = new TabbedThumbnail(Handle, tab.Content)
			{
				Title = tab.Content.Text,
				Tooltip = tab.Content.Text
			};

			preview.SetWindowIcon((Icon)tab.Content.Icon.Clone());

			preview.TabbedThumbnailActivated += preview_TabbedThumbnailActivated;
			preview.TabbedThumbnailClosed += preview_TabbedThumbnailClosed;
			preview.TabbedThumbnailBitmapRequested += preview_TabbedThumbnailBitmapRequested;
			preview.PeekOffset = new Vector(Padding.Left, Padding.Top - 1);

			TaskbarManager.Instance.TabbedThumbnail.AddThumbnailPreview(preview);

			return preview;
		}

		/// <summary>
		/// When a child tab updates its <see cref="Form.Icon"/> property, it should call this method to update the icon in the AeroPeek preview.
		/// </summary>
		/// <param name="tab">Tab whose icon was updated.</param>
		/// <param name="icon">The new icon to use.  If this is left as null, we use <see cref="Form.Icon"/> on <paramref name="tab"/>.</param>
		public virtual void UpdateThumbnailPreviewIcon(HTTitleBarTab tab, Icon icon = null)
		{
			if (!AeroPeekEnabled)
			{
				return;
			}

			TabbedThumbnail preview = TaskbarManager.Instance.TabbedThumbnail.GetThumbnailPreview(tab.Content);

			if (preview == null)
			{
				return;
			}

			if (icon == null)
			{
				icon = tab.Content.Icon;
			}

			preview.SetWindowIcon((Icon)icon.Clone());
		}

		/// <summary>
		/// Event handler that is called when a tab's <see cref="Form.Text" /> property is changed, which re-renders the tab text and updates the title of the
		/// Aero Peek preview.
		/// </summary>
		/// <param name="sender">Object from which this event originated (the <see cref="HTTitleBarTab.Content" /> object in this case).</param>
		/// <param name="e">Arguments associated with the event.</param>
		private void Content_TextChanged(object sender, EventArgs e)
		{
			if (AeroPeekEnabled)
			{
				TabbedThumbnail preview = TaskbarManager.Instance.TabbedThumbnail.GetThumbnailPreview((Form) sender);

				if (preview != null)
				{
					preview.Title = (sender as Form).Text;
				}
			}

			if (_overlay != null)
			{
				_overlay.Render(true);
			}
		}

		/// <summary>
		/// Event handler that is called when a tab's <see cref="HTTitleBarTab.Closing" /> event is fired, which removes the tab from <see cref="Tabs" /> and
		/// re-renders <see cref="_overlay" />.
		/// </summary>
		/// <param name="sender">Object from which this event originated (the <see cref="HTTitleBarTab" /> in this case).</param>
		/// <param name="e">Arguments associated with the event.</param>
		private void HTTitleBarTabs_Closing(object sender, CancelEventArgs e)
		{
			if (e.Cancel) return;

			HTTitleBarTab tab = (HTTitleBarTab) sender;
			CloseTab(tab);

			if (!tab.Content.IsDisposed && AeroPeekEnabled)
			{
				TaskbarManager.Instance.TabbedThumbnail.RemoveThumbnailPreview(tab.Content);
			}

			if (_overlay != null)
			{
				_overlay.Render(true);
			}
		}

		/// <summary>
		/// Calls <see cref="HTTitleBarTabsOverlay.Render(bool)"/> on <see cref="_overlay"/> to force a redrawing of the tabs.
		/// </summary>
		public void RedrawTabs()
		{
			if (_overlay != null)
			{
				_overlay.Render(true);
			}
		}

		/// <summary>
		/// Overrides the <see cref="Control.SizeChanged" /> handler so that we can detect when the user has maximized or restored the window and adjust the size
		/// of the non-client area accordingly.
		/// </summary>
		/// <param name="e">Arguments associated with the event.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			// If no tab renderer has been set yet or the window state hasn't changed, don't do anything
			if (_previousWindowState != null && WindowState != _previousWindowState.Value)
			{
				SetFrameSize();
			}

			_previousWindowState = WindowState;

			base.OnSizeChanged(e);
		}

		/// <summary>Overrides the message processor for the window so that we can respond to windows events to render and manipulate the tabs properly.</summary>
		/// <param name="m">Message received by the pump.</param>
		protected override void WndProc(ref Message m)
		{
			bool callDwp = true;

			switch ((WM) m.Msg)
			{
				// When the window is activated, set the size of the non-client area appropriately
				case WM.WM_ACTIVATE:
					if ((m.WParam.ToInt64() & 0x0000FFFF) != 0)
					{
						SetFrameSize();
						ResizeTabContents();
						m.Result = IntPtr.Zero;
					}

					break;

				case WM.WM_NCHITTEST:
					// Call the base message handler to see where the user clicked in the window
					base.WndProc(ref m);

					HT hitResult = (HT) m.Result.ToInt32();

					// If they were over the minimize/maximize/close buttons or the system menu, let the message pass
					if (!(hitResult == HT.HTCLOSE || hitResult == HT.HTMINBUTTON || hitResult == HT.HTMAXBUTTON || hitResult == HT.HTMENU ||
						  hitResult == HT.HTSYSMENU))
					{
						m.Result = new IntPtr((int) HitTest(m));
					}

					callDwp = false;

					break;

				// Catch the case where the user is clicking the minimize button and use this opportunity to update the AeroPeek thumbnail for the current tab
				case WM.WM_NCLBUTTONDOWN:
					if (((HT) m.WParam.ToInt32()) == HT.HTMINBUTTON && AeroPeekEnabled && SelectedTab != null)
					{
						UpdateTabThumbnail(SelectedTab);
					}

					break;
			}

			if (callDwp)
			{
				base.WndProc(ref m);
			}
		}

		/// <summary>Calls <see cref="CreateTab" />, adds the resulting tab to the <see cref="Tabs" /> collection, and activates it.</summary>
		public virtual void AddNewTab()
		{
			HTTitleBarTab newTab = CreateTab();

			Tabs.Add(newTab);
			ResizeTabContents(newTab);

			SelectedTabIndex = _tabs.Count - 1;
		}

		/// <summary>Removes <paramref name="closingTab" /> from <see cref="Tabs" /> and selects the next applicable tab in the list.</summary>
		/// <param name="closingTab">Tab that is being closed.</param>
		protected virtual void CloseTab(HTTitleBarTab closingTab)
		{
			int removeIndex = Tabs.IndexOf(closingTab);
			int selectedTabIndex = SelectedTabIndex;

			Tabs.Remove(closingTab);

			if (selectedTabIndex > removeIndex)
			{
				SelectedTabIndex = selectedTabIndex - 1;
			}

			else if (selectedTabIndex == removeIndex)
			{
				SelectedTabIndex = Math.Min(selectedTabIndex, Tabs.Count - 1);
			}

			else
			{
				SelectedTabIndex = selectedTabIndex;
			}

			if (_previews.ContainsKey(closingTab.Content))
			{
				_previews[closingTab.Content].Dispose();
				_previews.Remove(closingTab.Content);
			}

			if (_previousActiveTab != null && closingTab.Content == _previousActiveTab.Content)
			{
				_previousActiveTab = null;
			}

			if (Tabs.Count == 0 && ExitOnLastTabClose)
			{
				Close();
			}
		}

		private HT HitTest(Message m)
		{
			// Get the point that the user clicked
			int lParam = (int) m.LParam;
			Point point = new Point(lParam & 0xffff, lParam >> 16);

			return HitTest(point, m.HWnd);
		}

		/// <summary>Called when a <see cref="WM.WM_NCHITTEST" /> message is received to see where in the non-client area the user clicked.</summary>
		/// <param name="point">Screen location that we are to test.</param>
		/// <param name="windowHandle">Handle to the window for which we are performing the test.</param>
		/// <returns>One of the <see cref="HT" /> values, depending on where the user clicked.</returns>
		private HT HitTest(Point point, IntPtr windowHandle)
		{
			RECT rect;

			User32.GetWindowRect(windowHandle, out rect);
			Rectangle area = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);

			int row = 1;
			int column = 1;
			bool onResizeBorder = false;

			// Determine if we are on the top or bottom border
			if (point.Y >= area.Top && point.Y < area.Top + SystemInformation.VerticalResizeBorderThickness + _nonClientAreaHeight - 2)
			{
				onResizeBorder = point.Y < (area.Top + SystemInformation.VerticalResizeBorderThickness);
				row = 0;
			}

			else if (point.Y < area.Bottom && point.Y > area.Bottom - SystemInformation.VerticalResizeBorderThickness)
			{
				row = 2;
			}

			// Determine if we are on the left border or the right border
			if (point.X >= area.Left && point.X < area.Left + SystemInformation.HorizontalResizeBorderThickness)
			{
				column = 0;
			}

			else if (point.X < area.Right && point.X >= area.Right - SystemInformation.HorizontalResizeBorderThickness)
			{
				column = 2;
			}

			HT[,] hitTests =
			{
				{
					onResizeBorder
						? HT.HTTOPLEFT
						: HT.HTLEFT,
					onResizeBorder
						? HT.HTTOP
						: HT.HTCAPTION,
					onResizeBorder
						? HT.HTTOPRIGHT
						: HT.HTRIGHT
				},
				{
					HT.HTLEFT, HT.HTNOWHERE, HT.HTRIGHT
				},
				{
					HT.HTBOTTOMLEFT, HT.HTBOTTOM,
					HT.HTBOTTOMRIGHT
				}
			};

			return hitTests[row, column];
		}
	}
	/// <summary>Wraps a <see cref="Form" /> instance (<see cref="_content" />), that represents the content that should be displayed within a tab instance.</summary>
	public class HTTitleBarTab
	{
		protected Color _BackColor = Color.White;
		protected bool isLocked = false;
		/// <summary>
		/// True to disable dragging for this tab.
		/// Note that other draggable tabs can still change position of this tab.
		/// </summary>
		public bool IsLocked
		{
			get => isLocked;
			set => isLocked = value;
		}
		/// <summary>
		/// Gets the BackgroundColor for this tab.
		/// </summary>
		public Color BackColor
		{
			get => _BackColor;
			set => _BackColor = value;
		}
		protected bool UseDefaultColor = true;
		/// <summary>
		/// This will tell to renderer that this tab uses default colors.
		/// </summary>
		public bool useDefaultBackColor
		{
			get => UseDefaultColor;
			set => UseDefaultColor = value;
		}
		/// <summary>Flag indicating whether or not this tab is active.</summary>
		protected bool _active;

		/// <summary>Content that should be displayed within the tab.</summary>
		protected Form _content;

		/// <summary>Parent window that contains this tab.</summary>
		protected HTTitleBarTabs _parent;

		/// <summary>Default constructor that initializes the various properties.</summary>
		/// <param name="parent">Parent window that contains this tab.</param>
		public HTTitleBarTab(HTTitleBarTabs parent)
		{
			ShowCloseButton = true;
			Parent = parent;
		}

		/// <summary>Parent window that contains this tab.</summary>
		public HTTitleBarTabs Parent
		{
			get
			{
				return _parent;
			}

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
			get
			{
				return Content.Text;
			}

			set
			{
				Content.Text = value;
			}
		}

		/// <summary>Flag indicating whether or not this tab is active.</summary>
		public bool Active
		{
			get
			{
				return _active;
			}

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
			get
			{
				return Content.Icon;
			}

			set
			{
				Content.Icon = value;
			}
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
			get
			{
				return _content;
			}

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
				Content.Dock = DockStyle.Fill;
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
	/// <summary>Event arguments class for an event that occurs on a collection of collection of <see cref="HTTitleBarTab" />s.</summary>
	public class HTTitleBarTabEventArgs : EventArgs
	{
		/// <summary>Action that is being performed.</summary>
		public TabControlAction? Action
		{
			get;
			set;
		}

		/// <summary>The tab that the <see cref="Action" /> is being performed on.</summary>
		public HTTitleBarTab Tab
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
	/// <summary>Event arguments class for a cancelable event that occurs on a collection of collection of <see cref="HTTitleBarTab" />s.</summary>
	public class HTTitleBarTabCancelEventArgs : CancelEventArgs
	{
		/// <summary>Action that is being performed.</summary>
		public TabControlAction Action
		{
			get;
			set;
		}

		/// <summary>The tab that the <see cref="Action" /> is being performed on.</summary>
		public HTTitleBarTab Tab
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
		protected bool EventsSuppressed
		{
			get
			{
				return _suppressEvents;
			}
		}

		/// <summary>Overloads <see cref="List{T}.this" />.</summary>
		public new virtual T this[int index]
		{
			get
			{
				return base[index];
			}
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
		public object SyncRoot
		{
			get
			{
				return _syncRoot;
			}
		}

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
		public int StartIndex
		{
			get
			{
				return _startIndex;
			}
		}

		/// <summary>Gets the number of items in the range.</summary>
		public int Count
		{
			get
			{
				return _count;
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
		public int ItemIndex
		{
			get
			{
				return _itemIndex;
			}
		}
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
		public ListModification Modification
		{
			get
			{
				return _modification;
			}
		}
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
	/// Application context to use when starting a <see cref="HTTitleBarTabs" /> application via <see cref="Application.Run(ApplicationContext)" />.  Used to
	/// track open windows so that the entire application doesn't quit when the first-opened window is closed.
	/// </summary>
	public class HTTitleBarTabsApplicationContext : ApplicationContext
	{
		/// <summary>List of all opened windows.</summary>
		protected List<HTTitleBarTabs> _openWindows = new List<HTTitleBarTabs>();

		/// <summary>List of all opened windows.</summary>
		public List<HTTitleBarTabs> OpenWindows
		{
			get
			{
				return _openWindows;
			}
		}

		/// <summary>Constructor; takes the initial window to display and, if it's not closing, opens it and shows it.</summary>
		/// <param name="initialFormInstance">Initial window to display.</param>
		public void Start(HTTitleBarTabs initialFormInstance)
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
		public void OpenWindow(HTTitleBarTabs window)
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
			_openWindows.Remove((HTTitleBarTabs)sender);

			if (_openWindows.Count == 0)
			{
				ExitThread();
			}
		}
	}
}