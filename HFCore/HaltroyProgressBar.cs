using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTAlt
{
    /// <summary>
    /// Customizable <see cref="System.Windows.Forms.ProgressBar"/>.
    /// </summary>
    public class HTProgressBar : Control
    {
        #region "Enums"
        public enum ProgressDirection
        {
            LeftToRight,
            BottomToTop,
            RightToLeft,
            TopToBottom
        }
        #endregion
        #region "Properties"
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
            set { _BackColor = value; Refresh(); }
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
            set { _BorderColor = value; Refresh(); }
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
            set { _DrawBorder = value; Refresh(); }
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
            set { _BorderThiccness = value; Refresh(); }
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
            set { _Overlay = value; Refresh();}
        }
        /// <summary>
        /// Color of the loading bar.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(ProgressDirection), "LeftToRight")]
        [Category("HaltroyProgressBar")]
        [Description("Direction of the loading bar.")]
        public ProgressDirection Direction
        {
            get => _Direction;
            set { _Direction = value; Refresh(); }
            }
        /// <summary>
        /// Maximum value of the progress bar.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(int), "100")]
        [Category("HaltroyProgressBar")]
        [Description("Maximum value of the progress bar.")]
        public int Maximum
        {
            get => _Max;
            set { _Max = value; Refresh(); }
        }
        /// <summary>
        /// Minimum value of the progress bar.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(int), "0")]
        [Category("HaltroyProgressBar")]
        [Description("Minimum value of the progress bar.")]
        public int Minimum
        {
            get => _Min;
            set { _Min = value; Refresh(); }
        }
        /// <summary>
        /// Value of the progress bar.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(int), "0")]
        [Category("HaltroyProgressBar")]
        [Description("Value of the progress bar.")]
        public int Value
        {
            get => _Value;
            set { _Value = value; Refresh(); }
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
        #region "Paint"
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_BackColor != Color.Transparent) { e.Graphics.FillRectangle(new SolidBrush(_BackColor), Bounds); }
            if (_Direction == ProgressDirection.LeftToRight)
            {
                DPLR(e);
            }else if (_Direction == ProgressDirection.RightToLeft)
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
                e.Graphics.FillRectangle(new SolidBrush(_BorderColor), new Rectangle(0,0,Width,_BorderThiccness));
                e.Graphics.FillRectangle(new SolidBrush(_BorderColor), new Rectangle(0, Height - _BorderThiccness, Width, _BorderThiccness));
                e.Graphics.FillRectangle(new SolidBrush(_BorderColor), new Rectangle(0, 0, _BorderThiccness,Height));
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
                Rectangle loadbar = new System.Drawing.Rectangle(0, Height - loadstart, Width,  loadstart);
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
