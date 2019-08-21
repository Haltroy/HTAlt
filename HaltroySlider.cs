using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HaltroyFramework
{
    
    /// <summary>
    /// Encapsulates control that visualy displays certain integer value and allows user to change it within desired range. It imitates <see cref="System.Windows.Forms.TrackBar"/> as far as mouse usage is concerned.
    /// </summary>
    [ToolboxBitmap(typeof(TrackBar))]
    [DefaultEvent("Scroll"), DefaultProperty("BarInnerColor")]
    public partial class HaltroySlider : Control
    {
        #region Events

        /// <summary>
        /// Fires when Slider position has changed
        /// </summary>
        [Description("Event fires when the Value property changes")]
        [Category("Action")]
        public event EventHandler ValueChanged;

        /// <summary>
        /// Fires when user scrolls the Slider
        /// </summary>
        [Description("Event fires when the Slider position is changed")]
        [Category("Behavior")]
        public event ScrollEventHandler Scroll;

        #endregion


        #region Properties

        private Rectangle barRect; //bounding rectangle of bar area
        private Rectangle barHalfRect;
        private Rectangle thumbHalfRect;
        private Rectangle elapsedRect; //bounding rectangle of elapsed area

        #region thumb

        private Rectangle thumbRect; //bounding rectangle of thumb area
        /// <summary>
        /// Gets the thumb rect. Usefull to determine bounding rectangle when creating custom thumb shape.
        /// </summary>
        /// <value>The thumb rect.</value>
        [Browsable(false)]
        public Rectangle ThumbRect
        {
            get { return thumbRect; }
        }

        private Size _thumbSize = new Size(16, 16);

        /// <summary>
        /// Gets or sets the size of the thumb.
        /// </summary>
        /// <value>The size of the thumb.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when value is lower than zero or grather than half of appropiate dimension</exception>
        [Description("Set Slider thumb size")]
        [Category("HaltroySlider")]
        [DefaultValue(16)]
        public Size ThumbSize
        {
            get { return _thumbSize; }
            set
            {
                int h = value.Height;
                int w = value.Width;
                if (h > 0 && w > 0)
                {
                    _thumbSize = new Size(w, h);
                }
                else
                    throw new ArgumentOutOfRangeException(
                        "TrackSize has to be greather than zero and lower than half of Slider width");                 

                Invalidate();
            }
        }

        private GraphicsPath _thumbCustomShape = null;
        /// <summary>
        /// Gets or sets the thumb custom shape. Use ThumbRect property to determine bounding rectangle.
        /// </summary>
        /// <value>The thumb custom shape. null means default shape</value>
        [Description("Set Slider's thumb's custom shape")]
        [Category("HaltroySlider")]
        [Browsable(false)]
        [DefaultValue(typeof(GraphicsPath), "null")]
        public GraphicsPath ThumbCustomShape
        {
            get { return _thumbCustomShape; }
            set
            {
                _thumbCustomShape = value;
                //_thumbSize = (int) (_barOrientation == Orientation.Horizontal ? value.GetBounds().Width : value.GetBounds().Height) + 1;
                _thumbSize = new Size((int)value.GetBounds().Width, (int)value.GetBounds().Height);

                Invalidate();
            }
        }

        private Size _thumbRoundRectSize = new Size(16, 16);
        /// <summary>
        /// Gets or sets the size of the thumb round rectangle edges.
        /// </summary>
        /// <value>The size of the thumb round rectangle edges.</value>
        [Description("Set Slider's thumb round rect size")]
        [Category("HaltroySlider")]
        [DefaultValue(typeof(Size), "16; 16")]
        public Size ThumbRoundRectSize
        {
            get { return _thumbRoundRectSize; }
            set
            {
                int h = value.Height, w = value.Width;
                if (h <= 0) h = 1;
                if (w <= 0) w = 1;
                _thumbRoundRectSize = new Size(w, h);
                Invalidate();
            }
        }

        private Size _borderRoundRectSize = new Size(8, 8);
        /// <summary>
        /// Gets or sets the size of the border round rect.
        /// </summary>
        /// <value>The size of the border round rect.</value>
        [Description("Set Slider's border round rect size")]
        [Category("HaltroySlider")]
        [DefaultValue(typeof(Size), "8; 8")]
        public Size BorderRoundRectSize
        {
            get { return _borderRoundRectSize; }
            set
            {
                int h = value.Height, w = value.Width;
                if (h <= 0) h = 1;
                if (w <= 0) w = 1;
                _borderRoundRectSize = new Size(w, h);
                Invalidate();
            }
        }

        private bool _drawSemitransparentThumb = true;
        /// <summary>
        /// Gets or sets a value indicating whether to draw semitransparent thumb.
        /// </summary>
        /// <value><c>true</c> if semitransparent thumb should be drawn; otherwise, <c>false</c>.</value>
        [Description("Set whether to draw semitransparent thumb")]
        [Category("HaltroySlider")]
        [DefaultValue(true)]
        public bool DrawSemitransparentThumb
        {
            get { return _drawSemitransparentThumb; }
            set
            {
                _drawSemitransparentThumb = value;
                Invalidate();
            }
        }

        private Image _thumbImage = null;
        //private Image _thumbImage = Properties.Resources.BTN_Thumb_Blue;
        /// <summary>
        /// Gets or sets the Image used to render the thumb.
        /// </summary>
        /// <value>the thumb Image</value> 
        [Description("Set to use a specific Image for the thumb")]
        [Category("HaltroySlider")]
        [DefaultValue(null)]
        public Image ThumbImage
        {
            get { return _thumbImage; }
            set
            {
                if (value != null)
                    _thumbImage = value;
                else
                    _thumbImage = null;
                Invalidate();
            }
        }

        #endregion


        #region Appearance

        private Orientation _barOrientation = Orientation.Horizontal;
        /// <summary>
        /// Gets or sets the orientation of Slider.
        /// </summary>
        /// <value>The orientation.</value>
        [Description("Set Slider orientation")]
        [Category("HaltroySlider")]
        [DefaultValue(Orientation.Horizontal)]
        public Orientation Orientation
        {
            get { return _barOrientation; }
            set
            {
                if (_barOrientation != value)
                {
                    _barOrientation = value;
                    // Switch from horizontal to vertical (design mode)
					// Comment these lines if problems in Run mode
                    if (this.DesignMode)
                    {
                        int temp = Width;
                        Width = Height;
                        Height = temp;
                    }

                    Invalidate();
                }
            }
        }

        private bool _drawFocusRectangle = false;
        /// <summary>
        /// Gets or sets a value indicating whether to draw focus rectangle.
        /// </summary>
        /// <value><c>true</c> if focus rectangle should be drawn; otherwise, <c>false</c>.</value>
        [Description("Set whether to draw focus rectangle")]
        [Category("HaltroySlider")]
        [DefaultValue(false)]
        public bool DrawFocusRectangle
        {
            get { return _drawFocusRectangle; }
            set
            {
                _drawFocusRectangle = value;
                Invalidate();
            }
        }

        private bool _mouseEffects = true;
        /// <summary>
        /// Gets or sets whether mouse entry and exit actions have impact on how control look.
        /// </summary>
        /// <value><c>true</c> if mouse entry and exit actions have impact on how control look; otherwise, <c>false</c>.</value>
        [Description("Set whether mouse entry and exit actions have impact on how control look")]
        [Category("HaltroySlider")]
        [DefaultValue(true)]
        public bool MouseEffects
        {
            get { return _mouseEffects; }
            set
            {
                _mouseEffects = value;
                Invalidate();
            }
        }

        #endregion


        #region values

        private int _trackerValue = 30;
        /// <summary>
        /// Gets or sets the value of Slider.
        /// </summary>
        /// <value>The value.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when value is outside appropriate range (min, max)</exception>
        [Description("Set Slider value")]
        [Category("HaltroySlider")]
        [DefaultValue(30)]
        public int Value
        {
            get { return _trackerValue; }
            set
            {
                if (value >= _minimum & value <= _maximum)
                {
                    _trackerValue = value;
                    if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    Invalidate();
                }
                else throw new ArgumentOutOfRangeException("Value is outside appropriate range (min, max)");
            }
        }

        private int _minimum = 0;
        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when minimal value is greather than maximal one</exception>
        [Description("Set Slider minimal point")]
        [Category("HaltroySlider")]
        [DefaultValue(0)]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                if (value < _maximum)
                {
                    _minimum = value;
                    if (_trackerValue < _minimum)
                    {
                        _trackerValue = _minimum;
                        if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    }
                    Invalidate();
                }
                else throw new ArgumentOutOfRangeException("Minimal value is greather than maximal one");
            }
        }

        private int _maximum = 100;
        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when maximal value is lower than minimal one</exception>
        [Description("Set Slider maximal point")]
        [Category("HaltroySlider")]
        [DefaultValue(100)]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                if (value > _minimum)
                {
                    _maximum = value;
                    if (_trackerValue > _maximum)
                    {
                        _trackerValue = _maximum;
                        if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    }
                    Invalidate();
                }
                //else throw new ArgumentOutOfRangeException("Maximal value is lower than minimal one");
            }
        }

        private uint _smallChange = 1;
        /// <summary>
        /// Gets or sets trackbar's small change. It affects how to behave when directional keys are pressed
        /// </summary>
        /// <value>The small change value.</value>
        [Description("Set trackbar's small change")]
        [Category("HaltroySlider")]
        [DefaultValue(1)]
        public uint SmallChange
        {
            get { return _smallChange; }
            set { _smallChange = value; }
        }

        private uint _largeChange = 5;
        /// <summary>
        /// Gets or sets trackbar's large change. It affects how to behave when PageUp/PageDown keys are pressed
        /// </summary>
        /// <value>The large change value.</value>
        [Description("Set trackbar's large change")]
        [Category("HaltroySlider")]
        [DefaultValue(5)]
        public uint LargeChange
        {
            get { return _largeChange; }
            set { _largeChange = value; }
        }
           
        private int _mouseWheelBarPartitions = 10;
        /// <summary>
        /// Gets or sets the mouse wheel bar partitions.
        /// </summary>
        /// <value>The mouse wheel bar partitions.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when value isn't greather than zero</exception>
        [Description("Set to how many parts is bar divided when using mouse wheel")]
        [Category("HaltroySlider")]
        [DefaultValue(10)]
        public int MouseWheelBarPartitions
        {
            get { return _mouseWheelBarPartitions; }
            set
            {
                if (value > 0)
                    _mouseWheelBarPartitions = value;
                else throw new ArgumentOutOfRangeException("MouseWheelBarPartitions has to be greather than zero");
            }
        }

        #endregion


        #region colors



        private Color _overlayColor = Color.FromArgb(255, 20, 157, 204);
        /// <summary>
        /// Gets or sets the bottom color of the elapsed
        /// </summary>
        [Description("Gets or sets the overlay color")]
        [Category("HaltroySlider")]
        public Color OverlayColor
        {
            get { return _overlayColor; }
            set
            {
                _overlayColor = value;
                Invalidate();
            }
        }




        #endregion


        


        

        #endregion





        


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HaltroySlider"/> class.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="value">The current value.</param>
        public HaltroySlider(int min, int max, int value)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.Selectable |
                     ControlStyles.SupportsTransparentBackColor | ControlStyles.UserMouse |
                     ControlStyles.UserPaint, true);
            
            // Default backcolor
            BackColor = Color.White;
            ForeColor = Color.Black;
            _overlayColor = Color.FromArgb(255, 20, 157, 204);

            // Font
            //this.Font = new Font("Tahoma", 6.75f);
            this.Font = new Font("Microsoft Sans Serif", 6f);

            Minimum = min;
            Maximum = max;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HaltroySlider"/> class.
        /// </summary>
        public HaltroySlider() : this(0, 100, 30) { }

        #endregion

        #region Paint

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Enabled)
            {
                Color[] desaturatedColors = DesaturateColors(BackColor,BackColor, _overlayColor,
                                                             ForeColor, 
                                                             _overlayColor, _overlayColor, 
                                                             ForeColor, ForeColor,
                                                             _overlayColor);
                DrawHaltroySlider(e, 
                                    desaturatedColors[0], desaturatedColors[1], desaturatedColors[2], 
                                    desaturatedColors[3], 
                                    desaturatedColors[4], desaturatedColors[5], 
                                    desaturatedColors[6], desaturatedColors[7], 
                                    desaturatedColors[8]);
            }
            else
            {
                if (_mouseEffects && mouseInRegion)
                {
                    Color[] lightenedColors = LightenColors(BackColor,BackColor, _overlayColor,
                                                            ForeColor,
                                                            _overlayColor, _overlayColor, 
                                                            ForeColor, ForeColor,
                                                            _overlayColor);
                    DrawHaltroySlider(e, 
                        lightenedColors[0], lightenedColors[1], lightenedColors[2], 
                        lightenedColors[3], 
                        lightenedColors[4], lightenedColors[5], 
                        lightenedColors[6], lightenedColors[7], 
                        lightenedColors[8]);
                }
                else
                {
                    DrawHaltroySlider(e, 
                                    BackColor,BackColor, _overlayColor,
                                    ForeColor,
                                    _overlayColor, _overlayColor, 
                                    ForeColor, ForeColor,
                                    _overlayColor);
                }
            }
        }

        /// <summary>
        /// Draws the colorslider control using passed colors.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        /// <param name="thumbOuterColorPaint">The thumb outer color paint.</param>
        /// <param name="thumbInnerColorPaint">The thumb inner color paint.</param>
        /// <param name="thumbPenColorPaint">The thumb pen color paint.</param>
        /// <param name="barInnerColorPaint">The bar inner color paint.</param>
        /// <param name="barPenColorPaint">The bar pen color paint.</param>
        /// <param name="elapsedInnerColorPaint">The elapsed inner color paint.</param>
        private void DrawHaltroySlider(PaintEventArgs e, 
            Color thumbOuterColorPaint, Color thumbInnerColorPaint, Color thumbPenColorPaint, 
            Color barInnerColorPaint,
            Color ElapsedTopPenColorPaint, Color ElapsedBottomPenColorPaint, 
            Color barTopPenColorPaint, Color barBottomPenColorPaint,
            Color elapsedInnerColorPaint)
        {
            try
            {
                //set up thumbRect approprietly
                if (_barOrientation == Orientation.Horizontal)
                {
                    #region horizontal
                    if (_thumbImage != null)
                    {
                        int TrackX = (((_trackerValue - _minimum) * (ClientRectangle.Width - _thumbImage.Width)) / (_maximum - _minimum));
                        thumbRect = new Rectangle(TrackX, ClientRectangle.Height/2 - _thumbImage.Height/2, _thumbImage.Width, _thumbImage.Height);
                    }
                    else
                    {
                        int TrackX = (((_trackerValue - _minimum) * (ClientRectangle.Width - _thumbSize.Width)) / (_maximum - _minimum));                        
                        thumbRect = new Rectangle(TrackX, ClientRectangle.Y + ClientRectangle.Height/2 - _thumbSize.Height/2 , _thumbSize.Width, _thumbSize.Height);
                    }
                    #endregion
                }
                else
                {
                    #region vertical
                    if (_thumbImage != null)
                    {                        
                        int TrackY = (((_maximum - (_trackerValue)) * (ClientRectangle.Height - _thumbImage.Height)) / (_maximum - _minimum));
                        thumbRect = new Rectangle(ClientRectangle.Width/2 - _thumbImage.Width/2, TrackY, _thumbImage.Width, _thumbImage.Height);
                    }
                    else
                    {                        
                        int TrackY = (((_maximum - (_trackerValue)) * (ClientRectangle.Height - _thumbSize.Height)) / (_maximum - _minimum));
                        thumbRect = new Rectangle(ClientRectangle.X + ClientRectangle.Width/2 - _thumbSize.Width/2, TrackY, _thumbSize.Width, _thumbSize.Height);
                    }
                    #endregion
                }


                //adjust drawing rects
                barRect = ClientRectangle;
                // TODO : make barRect rectangle smaller than Control rectangle  
                // barRect = new Rectangle(ClientRectangle.X + 5, ClientRectangle.Y + 5, ClientRectangle.Width - 10, ClientRectangle.Height - 10);
                thumbHalfRect = thumbRect;
                LinearGradientMode gradientOrientation;


                if (_barOrientation == Orientation.Horizontal)
                {
                    #region horizontal
                    barRect.Inflate(-1, -barRect.Height / 3);
                    barHalfRect = barRect;
                    barHalfRect.Height /= 2;

                    gradientOrientation = LinearGradientMode.Vertical;
                    

                    thumbHalfRect.Height /= 2;
                    elapsedRect = barRect;
                    elapsedRect.Width = thumbRect.Left + _thumbSize.Width / 2;
                    #endregion
                }
                else
                {
                    #region vertical
                    barRect.Inflate(-barRect.Width / 3, -1);
                    barHalfRect = barRect;
                    barHalfRect.Width /= 2;
                   
                    gradientOrientation = LinearGradientMode.Vertical;

                    thumbHalfRect.Width /= 2;
                    elapsedRect = barRect;                    
                    elapsedRect.Height = barRect.Height - (thumbRect.Top + ThumbSize.Height / 2);
                    elapsedRect.Y = 1 + thumbRect.Top + ThumbSize.Height/2;

                    #endregion
                }
                
                //get thumb shape path 
                GraphicsPath thumbPath;
                if (_thumbCustomShape == null)
                    thumbPath = CreateRoundRectPath(thumbRect, _thumbRoundRectSize);
                else
                {
                    thumbPath = _thumbCustomShape;
                    Matrix m = new Matrix();
                    m.Translate(thumbRect.Left - thumbPath.GetBounds().Left, thumbRect.Top - thumbPath.GetBounds().Top);
                    thumbPath.Transform(m);
                }


                //draw bar

                #region draw inner bar

                // inner bar is a single line 
                // draw the line on the whole lenght of the control
                if (_barOrientation == Orientation.Horizontal)
                {
                    e.Graphics.DrawLine(new Pen(barInnerColorPaint, 1f), barRect.X, barRect.Y + barRect.Height/2, barRect.X + barRect.Width, barRect.Y + barRect.Height / 2);
                }
                else
                {
                    e.Graphics.DrawLine(new Pen(barInnerColorPaint, 1f), barRect.X + barRect.Width/2, barRect.Y, barRect.X + barRect.Width/2 , barRect.Y + barRect.Height);
                }
                #endregion


                #region draw elapsed bar

                //draw elapsed inner bar (single line too)                               
                if (_barOrientation == Orientation.Horizontal)
                {
                    e.Graphics.DrawLine(new Pen(elapsedInnerColorPaint, 1f), barRect.X, barRect.Y + barRect.Height / 2, barRect.X + elapsedRect.Width, barRect.Y + barRect.Height / 2);
                }
                else
                {
                    e.Graphics.DrawLine(new Pen(elapsedInnerColorPaint, 1f), barRect.X + barRect.Width / 2, barRect.Y + (barRect.Height - elapsedRect.Height), barRect.X + barRect.Width / 2, barRect.Y + barRect.Height);
                }

                #endregion draw elapsed bar


                #region draw external contours
                
                //draw external bar band 
                // 2 lines: top and bottom
                if (_barOrientation == Orientation.Horizontal)
                {
                    #region horizontal
                    // Elapsed top
                    e.Graphics.DrawLine(new Pen(ElapsedTopPenColorPaint, 1f), barRect.X, barRect.Y - 1 + barRect.Height / 2, barRect.X + elapsedRect.Width, barRect.Y - 1 + barRect.Height / 2);
                    // Elapsed bottom
                    e.Graphics.DrawLine(new Pen(ElapsedBottomPenColorPaint, 1f), barRect.X, barRect.Y + 1 + barRect.Height / 2, barRect.X + elapsedRect.Width, barRect.Y + 1 + barRect.Height / 2);


                    // Remain top
                    e.Graphics.DrawLine(new Pen(barTopPenColorPaint, 1f), barRect.X + elapsedRect.Width, barRect.Y - 1 + barRect.Height / 2, barRect.X + barRect.Width, barRect.Y - 1 + barRect.Height / 2);
                    // Remain bottom
                    e.Graphics.DrawLine(new Pen(barBottomPenColorPaint, 1f), barRect.X + elapsedRect.Width, barRect.Y + 1 + barRect.Height / 2, barRect.X + barRect.Width, barRect.Y + 1 + barRect.Height / 2);


                    // Left vertical (dark)
                    e.Graphics.DrawLine(new Pen(barTopPenColorPaint, 1f), barRect.X, barRect.Y -1 + barRect.Height/2, barRect.X, barRect.Y + barRect.Height/2 + 1);

                    // Right vertical (light)                        
                    e.Graphics.DrawLine(new Pen(barBottomPenColorPaint, 1f), barRect.X + barRect.Width, barRect.Y - 1 + barRect.Height/2, barRect.X + barRect.Width, barRect.Y + 1 + barRect.Height/2);
                    #endregion
                }
                else
                {
                    #region vertical
                    // Elapsed top
                    e.Graphics.DrawLine(new Pen(ElapsedTopPenColorPaint, 1f), barRect.X -1 + barRect.Width/2, barRect.Y + (barRect.Height - elapsedRect.Height), barRect.X - 1 + barRect.Width / 2, barRect.Y + barRect.Height);

                    // Elapsed bottom
                    e.Graphics.DrawLine(new Pen(ElapsedBottomPenColorPaint, 1f), barRect.X + 1 + barRect.Width / 2, barRect.Y + (barRect.Height - elapsedRect.Height), barRect.X + 1 + barRect.Width/2, barRect.Y + barRect.Height);


                    // Remain top
                    e.Graphics.DrawLine(new Pen(barTopPenColorPaint, 1f), barRect.X - 1 + barRect.Width / 2, barRect.Y, barRect.X - 1 + barRect.Width / 2, barRect.Y + barRect.Height - elapsedRect.Height);


                    // Remain bottom
                    e.Graphics.DrawLine(new Pen(barBottomPenColorPaint, 1f), barRect.X + 1 + barRect.Width / 2, barRect.Y, barRect.X + 1 + barRect.Width / 2, barRect.Y + barRect.Height - elapsedRect.Height);


                    // top horizontal (dark) 
                    e.Graphics.DrawLine(new Pen(barTopPenColorPaint, 1f), barRect.X - 1 + barRect.Width/2, barRect.Y, barRect.X + 1 + barRect.Width/2, barRect.Y);

                    // bottom horizontal (light)
                    e.Graphics.DrawLine(new Pen(barBottomPenColorPaint, 1f), barRect.X - 1 + barRect.Width/2, barRect.Y + barRect.Height, barRect.X + 1 + barRect.Width/2, barRect.Y + barRect.Height);
                    #endregion

                }
                    
                #endregion draw contours

                

                #region draw thumb

                //draw thumb
                Color newthumbOuterColorPaint = thumbOuterColorPaint, newthumbInnerColorPaint = thumbInnerColorPaint;
                if (Capture && _drawSemitransparentThumb)
                {
                    newthumbOuterColorPaint = Color.FromArgb(175, thumbOuterColorPaint);
                    newthumbInnerColorPaint = Color.FromArgb(175, thumbInnerColorPaint);
                }

                LinearGradientBrush lgbThumb;
                if (_barOrientation == Orientation.Horizontal)
                {
                    lgbThumb = new LinearGradientBrush(thumbRect, newthumbOuterColorPaint, newthumbInnerColorPaint, gradientOrientation);
                }
                else
                {
                    lgbThumb = new LinearGradientBrush(thumbHalfRect, newthumbOuterColorPaint, newthumbInnerColorPaint, gradientOrientation);                    
                }
                using (lgbThumb)
                {
                    lgbThumb.WrapMode = WrapMode.TileFlipXY;
                    
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(lgbThumb, thumbPath);

                    //draw thumb band
                    Color newThumbPenColor = thumbPenColorPaint;

                    if (_mouseEffects && (Capture || mouseInThumbRegion))
                        newThumbPenColor = ControlPaint.Dark(newThumbPenColor);
                    using (Pen thumbPen = new Pen(newThumbPenColor))
                    {

                        if (_thumbImage != null)
                        {
                            Bitmap bmp = new Bitmap(_thumbImage);
                            bmp.MakeTransparent(Color.FromArgb(255, 0, 255));
                            Rectangle srceRect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                            e.Graphics.DrawImage(bmp, thumbRect, srceRect, GraphicsUnit.Pixel);
                            bmp.Dispose();
                            
                        }
                        else
                        {
                            e.Graphics.DrawPath(thumbPen, thumbPath);
                        }
                    }

                }
                #endregion draw thumb


                #region draw focusing rectangle
                //draw focusing rectangle
                if (Focused & _drawFocusRectangle)
                    using (Pen p = new Pen(Color.FromArgb(200, ElapsedTopPenColorPaint)))
                    {
                        p.DashStyle = DashStyle.Dot;
                        Rectangle r = ClientRectangle;
                        r.Width -= 2;
                        r.Height--;
                        r.X++;
                                               
                        using (GraphicsPath gpBorder = CreateRoundRectPath(r, _borderRoundRectSize))
                        {
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            e.Graphics.DrawPath(p, gpBorder);
                        }
                    }
                #endregion draw focusing rectangle


                


            }
            catch (Exception Err)
            {
                Console.WriteLine("DrawBackGround Error in " + Name + ":" + Err.Message);
            }
            finally
            {
            }
        }

        #endregion

        #region Overided events

        private bool mouseInRegion = false;
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mouseInRegion = true;
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseInRegion = false;
            mouseInThumbRegion = false;
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                Capture = true;
                if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.ThumbTrack, _trackerValue));
                if (ValueChanged != null) ValueChanged(this, new EventArgs());
                OnMouseMove(e);
            }
        }

        private bool mouseInThumbRegion = false;

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mouseInThumbRegion = IsPointInRect(e.Location, thumbRect);
            if (Capture & e.Button == MouseButtons.Left)
            {
                ScrollEventType set = ScrollEventType.ThumbPosition;
                Point pt = e.Location;
                int p = _barOrientation == Orientation.Horizontal ? pt.X : pt.Y;
                int margin = _thumbSize.Height >> 1;
                p -= margin;
                float coef = (float)(_maximum - _minimum) /
                             (float)
                             ((_barOrientation == Orientation.Horizontal ? ClientSize.Width : ClientSize.Height) - 2 * margin);

                
                _trackerValue = _barOrientation == Orientation.Horizontal ? (int)(p * coef + _minimum) : (_maximum - (int)(p * coef));


                if (_trackerValue <= _minimum)
                {
                    _trackerValue = _minimum;
                    set = ScrollEventType.First;
                }
                else if (_trackerValue >= _maximum)
                {
                    _trackerValue = _maximum;
                    set = ScrollEventType.Last;
                }

                if (Scroll != null) Scroll(this, new ScrollEventArgs(set, _trackerValue));
                if (ValueChanged != null) ValueChanged(this, new EventArgs());
            }
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Capture = false;
            mouseInThumbRegion = IsPointInRect(e.Location, thumbRect);
            if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.EndScroll, _trackerValue));
            if (ValueChanged != null) ValueChanged(this, new EventArgs());
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (mouseInRegion)
            {
                int v = e.Delta / 120 * (_maximum - _minimum) / _mouseWheelBarPartitions;
                SetProperValue(Value + v);

                // Avoid to send MouseWheel event to the parent container
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyUp"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"></see> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Left:
                    SetProperValue(Value - (int)_smallChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.SmallDecrement, Value));
                    break;
                case Keys.Up:
                case Keys.Right:
                    SetProperValue(Value + (int)_smallChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.SmallIncrement, Value));
                    break;
                case Keys.Home:
                    Value = _minimum;
                    break;
                case Keys.End:
                    Value = _maximum;
                    break;
                case Keys.PageDown:
                    SetProperValue(Value - (int)_largeChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.LargeDecrement, Value));
                    break;
                case Keys.PageUp:
                    SetProperValue(Value + (int)_largeChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.LargeIncrement, Value));
                    break;
            }
            if (Scroll != null && Value == _minimum) Scroll(this, new ScrollEventArgs(ScrollEventType.First, Value));
            if (Scroll != null && Value == _maximum) Scroll(this, new ScrollEventArgs(ScrollEventType.Last, Value));
            Point pt = PointToClient(Cursor.Position);
            OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, pt.X, pt.Y, 0));
        }

        /// <summary>
        /// Processes a dialog key.
        /// </summary>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"></see> values that represents the key to process.</param>
        /// <returns>
        /// true if the key was processed by the control; otherwise, false.
        /// </returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab | ModifierKeys == Keys.Shift)
                return base.ProcessDialogKey(keyData);
            else
            {
                OnKeyDown(new KeyEventArgs(keyData));
                return true;
            }
        }

        #endregion

        #region Help routines

        /// <summary>
        /// Creates the round rect path.
        /// </summary>
        /// <param name="rect">The rectangle on which graphics path will be spanned.</param>
        /// <param name="size">The size of rounded rectangle edges.</param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundRectPath(Rectangle rect, Size size)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(rect.Left + size.Width / 2, rect.Top, rect.Right - size.Width / 2, rect.Top);
            gp.AddArc(rect.Right - size.Width, rect.Top, size.Width, size.Height, 270, 90);

            gp.AddLine(rect.Right, rect.Top + size.Height / 2, rect.Right, rect.Bottom - size.Width / 2);
            gp.AddArc(rect.Right - size.Width, rect.Bottom - size.Height, size.Width, size.Height, 0, 90);

            gp.AddLine(rect.Right - size.Width / 2, rect.Bottom, rect.Left + size.Width / 2, rect.Bottom);
            gp.AddArc(rect.Left, rect.Bottom - size.Height, size.Width, size.Height, 90, 90);

            gp.AddLine(rect.Left, rect.Bottom - size.Height / 2, rect.Left, rect.Top + size.Height / 2);
            gp.AddArc(rect.Left, rect.Top, size.Width, size.Height, 180, 90);
            return gp;
        }

        /// <summary>
        /// Desaturates colors from given array.
        /// </summary>
        /// <param name="colorsToDesaturate">The colors to be desaturated.</param>
        /// <returns></returns>
        public static Color[] DesaturateColors(params Color[] colorsToDesaturate)
        {
            Color[] colorsToReturn = new Color[colorsToDesaturate.Length];
            for (int i = 0; i < colorsToDesaturate.Length; i++)
            {
                //use NTSC weighted avarage
                int gray =
                    (int)(colorsToDesaturate[i].R * 0.3 + colorsToDesaturate[i].G * 0.6 + colorsToDesaturate[i].B * 0.1);
                colorsToReturn[i] = Color.FromArgb(-0x010101 * (255 - gray) - 1);
            }
            return colorsToReturn;
        }

        /// <summary>
        /// Lightens colors from given array.
        /// </summary>
        /// <param name="colorsToLighten">The colors to lighten.</param>
        /// <returns></returns>
        public static Color[] LightenColors(params Color[] colorsToLighten)
        {
            Color[] colorsToReturn = new Color[colorsToLighten.Length];
            for (int i = 0; i < colorsToLighten.Length; i++)
            {
                colorsToReturn[i] = ControlPaint.Light(colorsToLighten[i]);
            }
            return colorsToReturn;
        }

        /// <summary>
        /// Sets the trackbar value so that it wont exceed allowed range.
        /// </summary>
        /// <param name="val">The value.</param>
        private void SetProperValue(int val)
        {
            if (val < _minimum) Value = _minimum;
            else if (val > _maximum) Value = _maximum;
            else Value = val;
        }

        /// <summary>
        /// Determines whether rectangle contains given point.
        /// </summary>
        /// <param name="pt">The point to test.</param>
        /// <param name="rect">The base rectangle.</param>
        /// <returns>
        /// 	<c>true</c> if rectangle contains given point; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPointInRect(Point pt, Rectangle rect)
        {
            if (pt.X > rect.Left & pt.X < rect.Right & pt.Y > rect.Top & pt.Y < rect.Bottom)
                return true;
            else return false;
        }

        #endregion
    }
}
