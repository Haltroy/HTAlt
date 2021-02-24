/*

Copyright © 2021 Eren "Haltroy" Kanat

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/HTAlt/blob/master/LICENSE

*/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HTAlt.WinForms
{
    /// <summary>
    /// Custom Switch control that imidates <see cref="System.Windows.Forms.CheckBox"/>.
    /// </summary>
    [DefaultValue("Checked"), DefaultEvent("CheckedChanged"), ToolboxBitmap(typeof(CheckBox))]
    public class HTSwitch : Control
    {
        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://htalt.haltroy.com/api/HTAlt.WinForms/HTSwitch");
        private readonly Version firstHTAltVersion = new Version("0.1.1.0");
        private readonly string description = "Custom Switch control that imidates System.Windows.Forms.CheckBox.";

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

        #region Delegate and Event declarations

        public delegate void CheckedChangedDelegate(object sender, EventArgs e);

        [Description("Raised when the HTSwitch has changed state")]
        public event CheckedChangedDelegate CheckedChanged;

        #endregion Delegate and Event declarations

        #region Private Members

        private readonly Timer _animationTimer = new Timer();
        private bool _checked = false;
        private bool _moving = false;
        private bool _animating = false;
        private bool _animationResult = false;
        private int _animationTarget = 0;
        private bool _useAnimation = true;
        private int _animationInterval = 1;
        private int _animationStep = 10;
        private bool _allowUserChange = true;

        private bool _isLeftFieldHovered = false;
        private bool _isButtonHovered = false;
        private bool _isRightFieldHovered = false;
        private bool _isLeftFieldPressed = false;
        private bool _isButtonPressed = false;
        private bool _isRightFieldPressed = false;

        private int _buttonValue = 0;
        private int _savedButtonValue = 0;
        private int _xOffset = 0;
        private int _xValue = 0;
        private int _thresholdPercentage = 50;
        private bool _grayWhenDisabled = true;
        private bool _toggleOnButtonClick = true;
        private bool _toggleOnSideClick = true;

        private MouseEventArgs _lastMouseEventArgs = null;

        private bool _buttonScaleImage;
        private Color _overlayColor = Color.DodgerBlue;
        private Color _backColor = Color.White;
        private Color _buttonColor = Color.FromArgb(255, 235, 235, 235);
        private Color _buttonHoverColor = Color.FromArgb(255, 215, 215, 215);
        private Color _buttonPressedColor = Color.FromArgb(255, 195, 195, 195);

        #endregion Private Members

        #region Constructor Etc.

        public HTSwitch()
        {
            SetStyle(ControlStyles.ResizeRedraw |
                        ControlStyles.SupportsTransparentBackColor |
                        ControlStyles.AllPaintingInWmPaint |
                        ControlStyles.UserPaint |
                        ControlStyles.OptimizedDoubleBuffer |
                        ControlStyles.DoubleBuffer, true);

            _animationTimer.Enabled = false;
            _animationTimer.Interval = _animationInterval;
            _animationTimer.Tick += AnimationTimer_Tick;
        }

        #endregion Constructor Etc.

        #region Public Properties

        /// <summary>
        /// Gets or sets the Checked value of the HTSwitch
        /// </summary>
        [Bindable(true)]
        [DefaultValue(false)]
        [Category("Data")]
        [Description("Gets or sets the Checked value of the HTSwitch")]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (value != _checked)
                {
                    while (_animating)
                    {
                        Application.DoEvents();
                    }

                    if (value == true)
                    {
                        int buttonWidth = GetButtonWidth();
                        _animationTarget = Width - buttonWidth;
                        BeginAnimation(true);
                    }
                    else
                    {
                        _animationTarget = 0;
                        BeginAnimation(false);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the user can change the value of the button or not.
        /// </summary>
        [Bindable(true)]
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Gets or sets whether the user can change the value of the button or not.")]
        public bool AllowUserChange
        {
            get => _allowUserChange;
            set => _allowUserChange = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ButtonRectangle => GetButtonRectangle();

        /// <summary>
        /// Gets or sets if the HTSwitch should be grayed out when disabled.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(true)]
        [Category("Appearance")]
        [Description("Gets or sets if the HTSwitch should be grayed out when disabled.")]
        public bool GrayWhenDisabled
        {
            get => _grayWhenDisabled;
            set
            {
                if (value != _grayWhenDisabled)
                {
                    _grayWhenDisabled = value;

                    if (!Enabled)
                    {
                        Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets if the HTSwitch should toggle when the button is clicked.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Gets or sets if the HTSwitch should toggle when the button is clicked.")]
        public bool ToggleOnButtonClick
        {
            get => _toggleOnButtonClick;
            set => _toggleOnButtonClick = value;
        }

        /// <summary>
        /// Gets or sets if the HTSwitch should toggle when the track besides the button is clicked.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Gets or sets if the HTSwitch should toggle when the track besides the button is clicked.")]
        public bool ToggleOnSideClick
        {
            get => _toggleOnSideClick;
            set => _toggleOnSideClick = value;
        }

        /// <summary>
        /// Gets or sets how much the button need to be on the other side (in percent) before it snaps.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(50)]
        [Category("Behavior")]
        [Description("Gets or sets how much the button need to be on the other side (in percent) before it snaps.")]
        public int ThresholdPercentage
        {
            get => _thresholdPercentage;
            set => _thresholdPercentage = value;
        }

        /// <summary>
        /// Gets or sets the back color when Checked = true.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        [Category("Appearance")]
        [Description("Gets or sets the back color when Checked = true.")]
        public Color OverlayColor
        {
            get => _overlayColor;
            set
            {
                if (value != _overlayColor)
                {
                    _overlayColor = value;
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets the back color when Checked = false.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(Color), "White")]
        [Category("Appearance")]
        [Description("Gets or sets the back color when Checked = false.")]
        public override Color BackColor
        {
            get => _backColor;
            set
            {
                if (value != _backColor)
                {
                    _backColor = value;
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets the back color of button.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(Color), "255, 235, 235, 235")]
        [Category("Appearance")]
        [Description("Gets or sets the back color of button.")]
        public Color ButtonColor
        {
            get => _buttonColor;
            set
            {
                if (value != _buttonColor)
                {
                    _buttonColor = value;
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets the back color of button when hovered.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(Color), "255, 215, 215, 215")]
        [Category("Appearance")]
        [Description("Gets or sets the back color of button when hovered.")]
        public Color ButtonHoverColor
        {
            get => _buttonHoverColor;
            set
            {
                if (value != _buttonHoverColor)
                {
                    _buttonHoverColor = value;
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets the back color of button when pressed.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(Color), "255, 195, 195, 195")]
        [Category("Appearance")]
        [Description("Gets or sets the back color of button when pressed.")]
        public Color ButtonPressedColor
        {
            get => _buttonPressedColor;
            set
            {
                if (value != _buttonPressedColor)
                {
                    _buttonPressedColor = value;
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the button image should be scaled to fit.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Gets or sets whether the button image should be scaled to fit.")]
        public bool ButtonScaleImageToFit
        {
            get => _buttonScaleImage;
            set
            {
                if (value != _buttonScaleImage)
                {
                    _buttonScaleImage = value;
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the toggle change should be animated or not.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Gets or sets whether the toggle change should be animated or not.")]
        public bool UseAnimation
        {
            get => _useAnimation;
            set => _useAnimation = value;
        }

        /// <summary>
        /// Gets or sets the interval in ms between animation frames.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(1)]
        [Category("Behavior")]
        [Description("Gets or sets the interval in ms between animation frames.")]
        public int AnimationInterval
        {
            get => _animationInterval;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("AnimationInterval must larger than zero!");
                }

                _animationInterval = value;
            }
        }

        /// <summary>
        /// Gets or sets the step in pixels the button should be moved between each animation interval.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(10)]
        [Category("Behavior")]
        [Description("Gets or sets the step in pixels the button should be moved between each animation interval.")]
        public int AnimationStep
        {
            get => _animationStep;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("AnimationStep must larger than zero!");
                }

                _animationStep = value;
            }
        }

        #region Hidden Base Properties

        /// <summary>
        /// Not in use.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text
        {
            get => "";
            set => base.Text = "";
        }

        /// <summary>
        /// Not in use.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color ForeColor
        {
            get => Color.Black;
            set => base.ForeColor = Color.Black;
        }

        /// <summary>
        /// Not in use.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Font Font
        {
            get => base.Font;
            set => base.Font = new Font(base.Font, FontStyle.Regular);
        }

        #endregion Hidden Base Properties

        #endregion Public Properties

        #region Internal Properties

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsButtonHovered => _isButtonHovered && !_isButtonPressed;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsButtonPressed => _isButtonPressed;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsLeftSideHovered => _isLeftFieldHovered && !_isLeftFieldPressed;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsLeftSidePressed => _isLeftFieldPressed;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsRightSideHovered => _isRightFieldHovered && !_isRightFieldPressed;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsRightSidePressed => _isRightFieldPressed;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal int ButtonValue
        {
            get
            {
                if (_animating || _moving)
                {
                    return _buttonValue;
                }
                else if (_checked)
                {
                    return Width - GetButtonWidth();
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value != _buttonValue)
                {
                    _buttonValue = value;
                    Refresh();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsButtonOnLeftSide => (ButtonValue <= 0);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsButtonOnRightSide => (ButtonValue >= (Width - GetButtonWidth()));

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsButtonMovingLeft => (_animating && !IsButtonOnLeftSide && _animationResult == false);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsButtonMovingRight => (_animating && !IsButtonOnRightSide && _animationResult == true);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool AnimationResult => _animationResult;

        #endregion Internal Properties

        #region Overridden Control Methods

        protected override Size DefaultSize => new Size(50, 19);

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            pevent.Graphics.ResetClip();

            base.OnPaintBackground(pevent);
            RenderBackground(pevent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.ResetClip();

            base.OnPaint(e);
            RenderControl(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _lastMouseEventArgs = e;

            int buttonWidth = GetButtonWidth();
            Rectangle buttonRectangle = GetButtonRectangle(buttonWidth);

            if (_moving)
            {
                int val = _xValue + (e.Location.X - _xOffset);

                if (val < 0)
                {
                    val = 0;
                }

                if (val > Width - buttonWidth)
                {
                    val = Width - buttonWidth;
                }

                ButtonValue = val;
                Refresh();
                return;
            }

            if (buttonRectangle.Contains(e.Location))
            {
                _isButtonHovered = true;
                _isLeftFieldHovered = false;
                _isRightFieldHovered = false;
            }
            else
            {
                if (e.Location.X > buttonRectangle.X + buttonRectangle.Width)
                {
                    _isButtonHovered = false;
                    _isLeftFieldHovered = false;
                    _isRightFieldHovered = true;
                }
                else if (e.Location.X < buttonRectangle.X)
                {
                    _isButtonHovered = false;
                    _isLeftFieldHovered = true;
                    _isRightFieldHovered = false;
                }
            }

            Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_animating || !AllowUserChange)
            {
                return;
            }

            int buttonWidth = GetButtonWidth();
            Rectangle buttonRectangle = GetButtonRectangle(buttonWidth);

            _savedButtonValue = ButtonValue;

            if (buttonRectangle.Contains(e.Location))
            {
                _isButtonPressed = true;
                _isLeftFieldPressed = false;
                _isRightFieldPressed = false;

                _moving = true;
                _xOffset = e.Location.X;
                _buttonValue = buttonRectangle.X;
                _xValue = ButtonValue;
            }
            else
            {
                if (e.Location.X > buttonRectangle.X + buttonRectangle.Width)
                {
                    _isButtonPressed = false;
                    _isLeftFieldPressed = false;
                    _isRightFieldPressed = true;
                }
                else if (e.Location.X < buttonRectangle.X)
                {
                    _isButtonPressed = false;
                    _isLeftFieldPressed = true;
                    _isRightFieldPressed = false;
                }
            }

            Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_animating || !AllowUserChange)
            {
                return;
            }

            int buttonWidth = GetButtonWidth();

            bool wasLeftSidePressed = IsLeftSidePressed;
            bool wasRightSidePressed = IsRightSidePressed;

            _isButtonPressed = false;
            _isLeftFieldPressed = false;
            _isRightFieldPressed = false;

            if (_moving)
            {
                int percentage = (int)((100 * (double)ButtonValue) / (Width - (double)buttonWidth));

                if (_checked)
                {
                    if (percentage <= (100 - _thresholdPercentage))
                    {
                        _animationTarget = 0;
                        BeginAnimation(false);
                    }
                    else if (ToggleOnButtonClick && _savedButtonValue == ButtonValue)
                    {
                        _animationTarget = 0;
                        BeginAnimation(false);
                    }
                    else
                    {
                        _animationTarget = Width - buttonWidth;
                        BeginAnimation(true);
                    }
                }
                else
                {
                    if (percentage >= _thresholdPercentage)
                    {
                        _animationTarget = Width - buttonWidth;
                        BeginAnimation(true);
                    }
                    else if (ToggleOnButtonClick && _savedButtonValue == ButtonValue)
                    {
                        _animationTarget = Width - buttonWidth;
                        BeginAnimation(true);
                    }
                    else
                    {
                        _animationTarget = 0;
                        BeginAnimation(false);
                    }
                }

                _moving = false;
                return;
            }

            if (IsButtonOnRightSide)
            {
                _buttonValue = Width - buttonWidth;
                _animationTarget = 0;
            }
            else
            {
                _buttonValue = 0;
                _animationTarget = Width - buttonWidth;
            }

            if (wasLeftSidePressed && ToggleOnSideClick)
            {
                SetValueInternal(false);
            }
            else if (wasRightSidePressed && ToggleOnSideClick)
            {
                SetValueInternal(true);
            }

            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isButtonHovered = false;
            _isLeftFieldHovered = false;
            _isRightFieldHovered = false;
            _isButtonPressed = false;
            _isLeftFieldPressed = false;
            _isRightFieldPressed = false;

            Refresh();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Refresh();
        }

        protected override void OnRegionChanged(EventArgs e)
        {
            base.OnRegionChanged(e);
            Refresh();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (_animationTarget > 0)
            {
                int buttonWidth = GetButtonWidth();
                _animationTarget = Width - buttonWidth;
            }

            base.OnSizeChanged(e);
        }

        #endregion Overridden Control Methods

        #region Private Methods

        private void SetValueInternal(bool checkedValue)
        {
            if (checkedValue == _checked)
            {
                return;
            }

            while (_animating)
            {
                Application.DoEvents();
            }

            BeginAnimation(checkedValue);
        }

        private void BeginAnimation(bool checkedValue)
        {
            _animating = true;
            _animationResult = checkedValue;

            if (_animationTimer != null && _useAnimation)
            {
                _animationTimer.Interval = _animationInterval;
                _animationTimer.Enabled = true;
            }
            else
            {
                AnimationComplete();
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            _animationTimer.Enabled = false;

            bool animationDone = false;
            int newButtonValue;

            if (IsButtonMovingRight)
            {
                newButtonValue = ButtonValue + _animationStep;

                if (newButtonValue > _animationTarget)
                {
                    newButtonValue = _animationTarget;
                }

                ButtonValue = newButtonValue;

                animationDone = ButtonValue >= _animationTarget;
            }
            else
            {
                newButtonValue = ButtonValue - _animationStep;

                if (newButtonValue < _animationTarget)
                {
                    newButtonValue = _animationTarget;
                }

                ButtonValue = newButtonValue;

                animationDone = ButtonValue <= _animationTarget;
            }

            if (animationDone)
            {
                AnimationComplete();
            }
            else
            {
                _animationTimer.Enabled = true;
            }
        }

        private void AnimationComplete()
        {
            _animating = false;
            _moving = false;
            _checked = _animationResult;

            _isButtonHovered = false;
            _isButtonPressed = false;
            _isLeftFieldHovered = false;
            _isLeftFieldPressed = false;
            _isRightFieldHovered = false;
            _isRightFieldPressed = false;

            Refresh();

            if (CheckedChanged != null)
            {
                CheckedChanged(this, new EventArgs());
            }

            if (_lastMouseEventArgs != null)
            {
                OnMouseMove(_lastMouseEventArgs);
            }

            _lastMouseEventArgs = null;
        }

        #endregion Private Methods

        #region "Renderer"

        #region Render Methods

        public void RenderBackground(PaintEventArgs e)
        {
            if (this == null)
            {
                return;
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle controlRectangle = new Rectangle(0, 0, Width, Height);

            FillBackground(e.Graphics, controlRectangle);
        }

        public void RenderControl(PaintEventArgs e)
        {
            if (this == null)
            {
                return;
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle buttonRectangle = GetButtonRectangle();
            int totalToggleFieldWidth = Width - buttonRectangle.Width;

            if (buttonRectangle.X > 0)
            {
                Rectangle leftRectangle = new Rectangle(0, 0, buttonRectangle.X, Height);

                if (leftRectangle.Width > 0)
                {
                    RenderLeftToggleField(e.Graphics, leftRectangle, totalToggleFieldWidth);
                }
            }

            if (buttonRectangle.X + buttonRectangle.Width < e.ClipRectangle.Width)
            {
                Rectangle rightRectangle = new Rectangle(buttonRectangle.X + buttonRectangle.Width, 0, Width - buttonRectangle.X - buttonRectangle.Width, Height);

                if (rightRectangle.Width > 0)
                {
                    RenderRightToggleField(e.Graphics, rightRectangle, totalToggleFieldWidth);
                }
            }

            RenderButton(e.Graphics, buttonRectangle);
        }

        public void FillBackground(Graphics g, Rectangle controlRectangle)
        {
            Color backColor = (!Enabled && GrayWhenDisabled) ? BackColor : BackColor;

            using (Brush backBrush = new SolidBrush(backColor))
            {
                g.FillRectangle(backBrush, controlRectangle);
            }
        }

        public void RenderLeftToggleField(Graphics g, Rectangle leftRectangle, int totalToggleFieldWidth)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int buttonWidth = GetButtonWidth();

            //Draw upper gradient field
            int gradientRectWidth = leftRectangle.Width + buttonWidth / 2;
            int upperGradientRectHeight = (int)(0.8 * (leftRectangle.Height - 2));

            Rectangle controlRectangle = new Rectangle(0, 0, Width, Height);
            GraphicsPath controlClipPath = GetControlClipPath(controlRectangle);

            Rectangle upperGradientRectangle = new Rectangle(leftRectangle.X, leftRectangle.Y + 1, gradientRectWidth, upperGradientRectHeight - 1);

            g.SetClip(controlClipPath);
            g.IntersectClip(upperGradientRectangle);

            using (GraphicsPath upperGradientPath = new GraphicsPath())
            {
                upperGradientPath.AddArc(upperGradientRectangle.X, upperGradientRectangle.Y, Height, Height, 135, 135);
                upperGradientPath.AddLine(upperGradientRectangle.X, upperGradientRectangle.Y, upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y);
                upperGradientPath.AddLine(upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y, upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y + upperGradientRectangle.Height);
                upperGradientPath.AddLine(upperGradientRectangle.X, upperGradientRectangle.Y + upperGradientRectangle.Height, upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y + upperGradientRectangle.Height);

                Color upperColor1 = (!Enabled && GrayWhenDisabled) ? OverlayColor : OverlayColor;

                using (Brush upperGradientBrush = new LinearGradientBrush(upperGradientRectangle, upperColor1, upperColor1, LinearGradientMode.Vertical))
                {
                    g.FillPath(upperGradientBrush, upperGradientPath);
                }
            }

            g.ResetClip();

            //Draw lower gradient field
            int lowerGradientRectHeight = (int)Math.Ceiling(0.5 * (leftRectangle.Height - 2));

            Rectangle lowerGradientRectangle = new Rectangle(leftRectangle.X, leftRectangle.Y + (leftRectangle.Height / 2), gradientRectWidth, lowerGradientRectHeight);

            g.SetClip(controlClipPath);
            g.IntersectClip(lowerGradientRectangle);

            using (GraphicsPath lowerGradientPath = new GraphicsPath())
            {
                lowerGradientPath.AddArc(1, lowerGradientRectangle.Y, (int)(0.75 * (Height - 1)), Height - 1, 215, 45); //Arc from side to top
                lowerGradientPath.AddLine(lowerGradientRectangle.X + buttonWidth / 2, lowerGradientRectangle.Y, lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y);
                lowerGradientPath.AddLine(lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y, lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y + lowerGradientRectangle.Height);
                lowerGradientPath.AddLine(lowerGradientRectangle.X + buttonWidth / 4, lowerGradientRectangle.Y + lowerGradientRectangle.Height, lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y + lowerGradientRectangle.Height);
                lowerGradientPath.AddArc(1, 1, Height - 1, Height - 1, 90, 70); //Arc from side to bottom

                Color lowerColor1 = (!Enabled && GrayWhenDisabled) ? OverlayColor : OverlayColor;

                using (Brush lowerGradientBrush = new LinearGradientBrush(lowerGradientRectangle, lowerColor1, lowerColor1, LinearGradientMode.Vertical))
                {
                    g.FillPath(lowerGradientBrush, lowerGradientPath);
                }
            }

            g.ResetClip();

            controlRectangle = new Rectangle(0, 0, Width, Height);
            controlClipPath = GetControlClipPath(controlRectangle);

            g.SetClip(controlClipPath);

            //Draw upper inside border
            Color upperBorderColor = Tools.AutoWhiteBlack(BackColor);

            //  if (!this.Enabled && this.GrayWhenDisabled)
            //    upperthis.BorderColor = upperthis.BorderColor;

            using (Pen upperBorderPen = new Pen(upperBorderColor))
            {
                g.DrawLine(upperBorderPen, leftRectangle.X, leftRectangle.Y + 1, leftRectangle.X + leftRectangle.Width + (buttonWidth / 2), leftRectangle.Y + 1);
            }

            //Draw lower inside border
            Color lowerBorderColor = Tools.AutoWhiteBlack(BackColor);

            //    if (!this.Enabled && this.GrayWhenDisabled)
            //      lowerthis.BorderColor = lowerthis.BorderColor;

            using (Pen lowerBorderPen = new Pen(lowerBorderColor))
            {
                g.DrawLine(lowerBorderPen, leftRectangle.X, leftRectangle.Y + leftRectangle.Height - 1, leftRectangle.X + leftRectangle.Width + (buttonWidth / 2), leftRectangle.Y + leftRectangle.Height - 1);
            }

            g.ResetClip();
        }

        public void RenderRightToggleField(Graphics g, Rectangle rightRectangle, int totalToggleFieldWidth)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle buttonRectangle = GetButtonRectangle();

            Rectangle controlRectangle = new Rectangle(0, 0, Width, Height);
            GraphicsPath controlClipPath = GetControlClipPath(controlRectangle);

            //Draw upper gradient field
            int gradientRectWidth = rightRectangle.Width + buttonRectangle.Width / 2;
            int upperGradientRectHeight = (int)(0.8 * (rightRectangle.Height - 2));

            Rectangle upperGradientRectangle = new Rectangle(rightRectangle.X - buttonRectangle.Width / 2, rightRectangle.Y + 1, gradientRectWidth - 1, upperGradientRectHeight - 1);

            g.SetClip(controlClipPath);
            g.IntersectClip(upperGradientRectangle);

            using (GraphicsPath upperGradientPath = new GraphicsPath())
            {
                upperGradientPath.AddLine(upperGradientRectangle.X, upperGradientRectangle.Y, upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y);
                upperGradientPath.AddArc(upperGradientRectangle.X + upperGradientRectangle.Width - Height + 1, upperGradientRectangle.Y - 1, Height, Height, 270, 115);
                upperGradientPath.AddLine(upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y + upperGradientRectangle.Height, upperGradientRectangle.X, upperGradientRectangle.Y + upperGradientRectangle.Height);
                upperGradientPath.AddLine(upperGradientRectangle.X, upperGradientRectangle.Y + upperGradientRectangle.Height, upperGradientRectangle.X, upperGradientRectangle.Y);

                Color upperColor1 = (!Enabled && GrayWhenDisabled) ? BackColor : BackColor;

                using (Brush upperGradientBrush = new LinearGradientBrush(upperGradientRectangle, upperColor1, upperColor1, LinearGradientMode.Vertical))
                {
                    g.FillPath(upperGradientBrush, upperGradientPath);
                }
            }

            g.ResetClip();

            //Draw lower gradient field
            int lowerGradientRectHeight = (int)Math.Ceiling(0.5 * (rightRectangle.Height - 2));

            Rectangle lowerGradientRectangle = new Rectangle(rightRectangle.X - buttonRectangle.Width / 2, rightRectangle.Y + (rightRectangle.Height / 2), gradientRectWidth - 1, lowerGradientRectHeight);

            g.SetClip(controlClipPath);
            g.IntersectClip(lowerGradientRectangle);

            using (GraphicsPath lowerGradientPath = new GraphicsPath())
            {
                lowerGradientPath.AddLine(lowerGradientRectangle.X, lowerGradientRectangle.Y, lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y);
                lowerGradientPath.AddArc(lowerGradientRectangle.X + lowerGradientRectangle.Width - (int)(0.75 * (Height - 1)), lowerGradientRectangle.Y, (int)(0.75 * (Height - 1)), Height - 1, 270, 45);  //Arc from top to side
                lowerGradientPath.AddArc(Width - Height, 0, Height, Height, 20, 70); //Arc from side to bottom
                lowerGradientPath.AddLine(lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y + lowerGradientRectangle.Height, lowerGradientRectangle.X, lowerGradientRectangle.Y + lowerGradientRectangle.Height);
                lowerGradientPath.AddLine(lowerGradientRectangle.X, lowerGradientRectangle.Y + lowerGradientRectangle.Height, lowerGradientRectangle.X, lowerGradientRectangle.Y);

                Color lowerColor1 = (!Enabled && GrayWhenDisabled) ? BackColor : BackColor;

                using (Brush lowerGradientBrush = new LinearGradientBrush(lowerGradientRectangle, lowerColor1, lowerColor1, LinearGradientMode.Vertical))
                {
                    g.FillPath(lowerGradientBrush, lowerGradientPath);
                }
            }

            g.ResetClip();

            controlRectangle = new Rectangle(0, 0, Width, Height);
            controlClipPath = GetControlClipPath(controlRectangle);

            g.SetClip(controlClipPath);

            //Draw upper inside border
            Color upperBorderColor = Tools.AutoWhiteBlack(BackColor);

            // if (!this.Enabled && this.GrayWhenDisabled)
            //upperthis.BorderColor = upperthis.BorderColor;

            using (Pen upperBorderPen = new Pen(upperBorderColor))
            {
                g.DrawLine(upperBorderPen, rightRectangle.X - (buttonRectangle.Width / 2), rightRectangle.Y + 1, rightRectangle.X + rightRectangle.Width, rightRectangle.Y + 1);
            }

            //Draw lower inside border
            Color lowerBorderColor = Tools.AutoWhiteBlack(BackColor);

            //  if (!this.Enabled && this.GrayWhenDisabled)
            //    lowerthis.BorderColor = lowerthis.BorderColor;

            using (Pen lowerBorderPen = new Pen(lowerBorderColor))
            {
                g.DrawLine(lowerBorderPen, rightRectangle.X - (buttonRectangle.Width / 2), rightRectangle.Y + rightRectangle.Height - 1, rightRectangle.X + rightRectangle.Width, rightRectangle.Y + rightRectangle.Height - 1);
            }
            g.ResetClip();
        }

        public void RenderButton(Graphics g, Rectangle buttonRectangle)
        {
            if (IsButtonOnLeftSide)
            {
                buttonRectangle.X += 1;
            }
            else if (IsButtonOnRightSide)
            {
                buttonRectangle.X -= 1;
            }

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //Draw button shadow
            buttonRectangle.Inflate(1, 1);

            Rectangle shadowClipRectangle = new Rectangle(buttonRectangle.Location, buttonRectangle.Size);
            shadowClipRectangle.Inflate(0, -1);

            if (IsButtonOnLeftSide)
            {
                shadowClipRectangle.X += shadowClipRectangle.Width / 2;
                shadowClipRectangle.Width = shadowClipRectangle.Width / 2;
            }
            else if (IsButtonOnRightSide)
            {
                shadowClipRectangle.Width = shadowClipRectangle.Width / 2;
            }

            g.SetClip(shadowClipRectangle);

            g.ResetClip();

            buttonRectangle.Inflate(-1, -1);

            //Draw outer button border
            Color buttonOuterBorderColor = Tools.AutoWhiteBlack(BackColor);

            if (IsButtonPressed)
            {
                buttonOuterBorderColor = Tools.AutoWhiteBlack(BackColor);
            }
            else if (IsButtonHovered)
            {
                buttonOuterBorderColor = Tools.AutoWhiteBlack(BackColor);
            }

            // if (!this.Enabled && this.GrayWhenDisabled)
            //    buttonOuterthis.BorderColor = buttonOuterthis.BorderColor;

            using (Brush outerBorderBrush = new SolidBrush(buttonOuterBorderColor))
            {
                g.FillEllipse(outerBorderBrush, buttonRectangle);
            }

            //Draw inner button border
            buttonRectangle.Inflate(-1, -1);

            Color buttonInnerBorderColor = Tools.AutoWhiteBlack(BackColor);

            if (IsButtonPressed)
            {
                buttonInnerBorderColor = Tools.AutoWhiteBlack(BackColor);
            }
            else if (IsButtonHovered)
            {
                buttonInnerBorderColor = Tools.AutoWhiteBlack(BackColor);
            }

            //   if (!this.Enabled && this.GrayWhenDisabled)
            //     buttonInnerthis.BorderColor = buttonInnerthis.BorderColor;

            using (Brush innerBorderBrush = new SolidBrush(buttonInnerBorderColor))
            {
                g.FillEllipse(innerBorderBrush, buttonRectangle);
            }

            //Draw button surface
            buttonRectangle.Inflate(-1, -1);

            Color buttonUpperSurfaceColor = ButtonColor;

            if (IsButtonPressed)
            {
                buttonUpperSurfaceColor = ButtonPressedColor;
            }
            else if (IsButtonHovered)
            {
                buttonUpperSurfaceColor = ButtonHoverColor;
            }

            //      if (!this.Enabled && this.GrayWhenDisabled)
            //        buttonUpperSurfaceColor = buttonUpperSurfaceColor;

            Color buttonLowerSurfaceColor = ButtonColor;

            if (IsButtonPressed)
            {
                buttonLowerSurfaceColor = ButtonPressedColor;
            }
            else if (IsButtonHovered)
            {
                buttonLowerSurfaceColor = ButtonHoverColor;
            }

            //     if (!this.Enabled && this.GrayWhenDisabled)
            //       buttonLowerSurfaceColor = buttonLowerSurfaceColor;

            using (Brush buttonSurfaceBrush = new LinearGradientBrush(buttonRectangle, buttonUpperSurfaceColor, buttonLowerSurfaceColor, LinearGradientMode.Vertical))
            {
                g.FillEllipse(buttonSurfaceBrush, buttonRectangle);
            }

            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;

            //Draw outer control border
            Rectangle controlRectangle = new Rectangle(0, 0, Width, Height);

            using (GraphicsPath borderPath = GetControlClipPath(controlRectangle))
            {
                Color controlBorderColor = (!Enabled && GrayWhenDisabled) ? Tools.AutoWhiteBlack(BackColor) : Tools.AutoWhiteBlack(BackColor);

                using (Pen borderPen = new Pen(controlBorderColor))
                {
                    g.DrawPath(borderPen, borderPath);
                }
            }

            g.ResetClip();
        }

        #endregion Render Methods

        #region Helper Methods

        public GraphicsPath GetButtonClipPath()
        {
            Rectangle buttonRectangle = GetButtonRectangle();

            GraphicsPath buttonPath = new GraphicsPath();

            buttonPath.AddArc(buttonRectangle.X, buttonRectangle.Y, buttonRectangle.Height, buttonRectangle.Height, 0, 360);

            return buttonPath;
        }

        public GraphicsPath GetControlClipPath(Rectangle controlRectangle)
        {
            GraphicsPath borderPath = new GraphicsPath();
            borderPath.AddArc(controlRectangle.X, controlRectangle.Y, controlRectangle.Height, controlRectangle.Height, 90, 180);
            borderPath.AddArc(controlRectangle.Width - controlRectangle.Height, controlRectangle.Y, controlRectangle.Height, controlRectangle.Height, 270, 180);
            borderPath.CloseFigure();

            return borderPath;
        }

        public int GetButtonWidth()
        {
            return Height - 2;
        }

        public Rectangle GetButtonRectangle()
        {
            int buttonWidth = GetButtonWidth();
            return GetButtonRectangle(buttonWidth);
        }

        public Rectangle GetButtonRectangle(int buttonWidth)
        {
            Rectangle buttonRect = new Rectangle(ButtonValue, 1, buttonWidth, buttonWidth);
            return buttonRect;
        }

        #endregion Helper Methods

        #endregion "Renderer"
    }
}