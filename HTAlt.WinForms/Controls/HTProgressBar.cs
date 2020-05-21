using HTAlt.Standart;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HTAlt.WinForms
{
    /// <summary>
    /// Customizable <see cref="System.Windows.Forms.ProgressBar"/>.
    /// </summary>
    public class HTProgressBar : Control
    {
        #region HTControls
        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://github.com/Haltroy/HTAlt/wiki/HTProgressBar-Class");
        private readonly Version firstHTAltVersion = new Version("0.1.2.0");
        private readonly string description = "Customizable System.Windows.Forms.ProgressBar.";
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
        #region Constructor
        public HTProgressBar() : this(100, 0, 0) { }
        public HTProgressBar(int max, int min, int value)
        {
            
            _Value = value;
            _Max = max;
            _Min = min;
        }
        #endregion
        #region Events
        /// <summary>
        /// Used in MaximumChanged, MinimumChanged & ValueChanged events.
        /// </summary>
        public class IntChangedEventArgs : EventArgs
        {
            /// <summary>
            /// Old value of the control's property.
            /// </summary>
            public int oldValue { get; set; }
            /// <summary>
            /// New value of the control's property.
            /// </summary>
            public int newValue { get; set; }
        }
        /// <summary>
        /// This event is called when Maximum value changes.
        /// </summary>
        [Description("This event is called when Maximum value changes."), Category("HTProgressBar")]
        public event EventHandler<IntChangedEventArgs> MaximumChanged;
        /// <summary>
        /// This event is called when Minimum value changes.
        /// </summary>
        [Description("This event is called when Minimum value changes."), Category("HTProgressBar")]
        public event EventHandler<IntChangedEventArgs> MinimumChanged;
        /// <summary>
        /// This event is called when Value changes.
        /// </summary>
        [Description("This event is called when Value changes."), Category("HTProgressBar")]
        public event EventHandler<IntChangedEventArgs> ValueChanged;
        protected virtual void OnValueChanged(IntChangedEventArgs e)
        {
            EventHandler<IntChangedEventArgs> handler = ValueChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnMaximumChanged(IntChangedEventArgs e)
        {
            EventHandler<IntChangedEventArgs> handler = MaximumChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnMinimumChanged(IntChangedEventArgs e)
        {
            EventHandler<IntChangedEventArgs> handler = MinimumChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion
        #region Enums
        public enum ProgressDirection
        {
            LeftToRight,
            BottomToTop,
            RightToLeft,
            TopToBottom
        }
        #endregion
        #region Properties
        /// <summary>
        /// Color of the background.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(Color), "White")]
        [Category("Appearance")]
        [Description("Color of the background.")]
        public override Color BackColor
        {
            get => _BackColor;
            set { _BackColor = value; Update(); }
        }
        /// <summary>
        /// Color of the border.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(Color), "Black")]
        [Category("Appearance")]
        [Description("Color of the border.")]
        public Color BorderColor
        {
            get => _BorderColor;
            set { _BorderColor = value; Update(); }
        }
        /// <summary>
        /// <c>true</c> if a border should be drawn, otherwise <c>false</c>.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(bool), "false")]
        [Category("Appearance")]
        [Description("true if a border should be drawn, otherwise false.")]
        public bool DrawBorder
        {
            get => _DrawBorder;
            set { _DrawBorder = value; Update(); }
        }
        /// <summary>
        /// Thickness of the border.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(int), "1")]
        [Category("Appearance")]
        [Description("Thickness of the border.")]
        public int BorderThickness
        {
            get => _BorderThiccness;
            set { _BorderThiccness = value; Update(); }
        }
        /// <summary>
        /// Color of the loading bar.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        [Category("Appearance")]
        [Description("Color of the loading bar.")]
        public Color BarColor
        {
            get => _Overlay;
            set { _Overlay = value; Update(); }
        }
        /// <summary>
        /// Color of the loading bar.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(ProgressDirection), "LeftToRight")]
        [Category("Appearance")]
        [Description("Direction of the loading bar.")]
        public ProgressDirection Direction
        {
            get => _Direction;
            set { _Direction = value; Update(); }
        }
        /// <summary>
        /// Maximum value of the progress bar.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(int), "100")]
        [Category("Appearance")]
        [Description("Maximum value of the progress bar.")]
        public int Maximum
        {
            get => _Max;
            set
            {
                if (value < _Min)
                {
                    throw new ArgumentOutOfRangeException("Maximum cannot be smaller than Minimum.");
                }
                else
                {
                    IntChangedEventArgs args = new IntChangedEventArgs
                    {
                        oldValue = _Max,
                        newValue = value
                    };
                    OnMaximumChanged(args);
                    _Max = value;
                    Update();
                }
            }
        }
        /// <summary>
        /// Minimum value of the progress bar.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(int), "0")]
        [Category("Appearance")]
        [Description("Minimum value of the progress bar.")]
        public int Minimum
        {
            get => _Min;
            set
            {
                if (_Max < value)
                {
                    throw new ArgumentOutOfRangeException("Minimum cannot be bigger than Maximum.");
                }
                else
                {
                    IntChangedEventArgs args = new IntChangedEventArgs
                    {
                        oldValue = _Min,
                        newValue = value
                    };
                    OnMinimumChanged(args);
                    _Min = value;
                    Update();
                }
            }
        }
        /// <summary>
        /// Value of the progress bar.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(int), "0")]
        [Category("Appearance")]
        [Description("Value of the progress bar.")]
        public int Value
        {
            get => _Value;
            set
            {
                if (_Min <= value && _Max >= value)
                {
                    IntChangedEventArgs args = new IntChangedEventArgs
                    {
                        oldValue = _Value,
                        newValue = value
                    };
                    OnValueChanged(args);
                    _Value = value;
                    Update();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Value must be equal or smaller than Maximum and equal or bigger than Minimum.");
                }
            }
        }
        private ProgressDirection _Direction = ProgressDirection.LeftToRight;
        private int _Min = 0;
        private int _Max = 100;
        private Color _Overlay = Color.DodgerBlue;
        private int _Value = 0;
        private int _BorderThiccness = 0;
        private bool _DrawBorder = false;
        private Color _BorderColor = Color.Black;
        private Color _BackColor = Color.White;
        #endregion
        #region Paint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_BackColor != Color.Transparent) { e.Graphics.FillRectangle(new SolidBrush(_BackColor), Bounds); }
            if (_Direction == ProgressDirection.LeftToRight)
            {
                DPLR(e);
            }
            else if (_Direction == ProgressDirection.RightToLeft)
            {
                DPRL(e);
            }
            else if (_Direction == ProgressDirection.TopToBottom)
            {
                DPTB(e);
            }
            else if (_Direction == ProgressDirection.BottomToTop)
            {
                DPBT(e);
            }
            if (_DrawBorder)
            {
                e.Graphics.FillRectangle(new SolidBrush(_BorderColor), new Rectangle(0, 0, Width, _BorderThiccness));
                e.Graphics.FillRectangle(new SolidBrush(_BorderColor), new Rectangle(0, Height - _BorderThiccness, Width, _BorderThiccness));
                e.Graphics.FillRectangle(new SolidBrush(_BorderColor), new Rectangle(0, 0, _BorderThiccness, Height));
                e.Graphics.FillRectangle(new SolidBrush(_BorderColor), new Rectangle(Width - _BorderThiccness, 0, _BorderThiccness, Height));
            }
            e.Graphics.ResetClip();
        }
        protected void DPLR(PaintEventArgs e)
        {
            if (_Value == _Max)
            {
                Rectangle loadbar = new System.Drawing.Rectangle(0, 0, Width, Height);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
            else if (_Value == _Min)
            {
                Rectangle loadbar = new System.Drawing.Rectangle(0, 0, 0, Height);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
            else
            {
                Rectangle loadbar = new System.Drawing.Rectangle(0, 0, (Width / (_Max - _Min)) * _Value, Height);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
        }
        protected void DPRL(PaintEventArgs e)
        {
            if (_Value == _Max)
            {
                Rectangle loadbar = new System.Drawing.Rectangle(0, 0, Width, Height);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
            else if (_Value == _Min)
            {
                Rectangle loadbar = new System.Drawing.Rectangle(0, 0, 0, Height);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
            else
            {
                int loadstart = (Width / (_Max - _Min)) * _Value;
                Rectangle loadbar = new System.Drawing.Rectangle(Width - loadstart, 0, loadstart, Height);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
        }
        protected void DPBT(PaintEventArgs e)
        {
            if (_Value == _Max)
            {
                Rectangle loadbar = new System.Drawing.Rectangle(0, 0, Width, Height);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
            else if (_Value == _Min)
            {
                Rectangle loadbar = new System.Drawing.Rectangle(0, 0, Width, 0);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
            else
            {
                int loadstart = (Height / (_Max - _Min)) * _Value;
                Rectangle loadbar = new System.Drawing.Rectangle(0, Height - loadstart, Width, loadstart);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
        }
        protected void DPTB(PaintEventArgs e)
        {
            if (_Value == _Max)
            {
                Rectangle loadbar = new System.Drawing.Rectangle(0, 0, Width, Height);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
            else if (_Value == _Min)
            {
                Rectangle loadbar = new System.Drawing.Rectangle(0, 0, Width, 0);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
            else
            {
                int loadstart = (Height / (_Max - _Min)) * _Value;
                Rectangle loadbar = new System.Drawing.Rectangle(0, 0, Width, loadstart);
                e.Graphics.FillRectangle(new SolidBrush(_Overlay), loadbar);
            }
        }
        #endregion
    }
}
