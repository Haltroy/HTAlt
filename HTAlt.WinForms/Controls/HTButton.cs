/*

Copyright © 2021 Eren "Haltroy" Kanat

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/HTAlt/blob/master/LICENSE

*/

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HTAlt.WinForms
{
    /// <summary>
    /// Flat button.
    /// </summary>
    [Description("Flat button.")]
    [ToolboxBitmap(typeof(Button))]
    [Designer(typeof(HTButtonDesigner))]
    public class HTButton : Control
    {
        #region Enums

        /// <summary>
        /// Defiys the drawing mode of <see cref="ButtonImage"/>.
        /// </summary>
        public enum ButtonImageSizeMode
        {
            /// <summary>
            /// Draws <see cref="ButtonImage"/> without changin anything.
            /// </summary>
            None,

            /// <summary>
            /// Draws <see cref="ButtonImage"/> in center and resizes it to fit if <see cref="ButtonImage"/> is bigger than <see cref="Control.Bounds"/>.
            /// </summary>
            Center,

            /// <summary>
            /// Resizes and draws <see cref="ButtonImage"/> to fit into <see cref="Control.Bounds"/>.
            /// </summary>
            Stretch,

            /// <summary>
            /// Draws <see cref="ButtonImage"/> over and over until it covers the entire <see cref="Control.Bounds"/>.
            /// </summary>
            Tile,

            /// <summary>
            /// Draws <see cref="ButtonImage"/> in center and resizes it to fit <see cref="Control.Bounds"/>.
            /// </summary>
            Zoom
        }

        private enum _States
        {
            Normal,
            MouseOver,
            Clicked
        }

        /// <summary>
        /// Shapes that can defiy <see cref="HTButton"/>'s shape.
        /// </summary>
        public enum ButtonShapes
        {
            /// <summary>
            /// Default shape of buttons in general.
            /// </summary>
            Rectangle,

            /// <summary>
            /// Gives an ellipse shape (or round chape if <see cref="Control.Width"/> & <see cref="Control.Height"/> are same) to button.
            /// </summary>
            Ellipse
        }

        #endregion Enums

        #region Properties

        #region HTControls

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://htalt.haltroy.com/api/HTAlt.WinForms/HTButton");
        private readonly Version firstHTAltVersion = new Version("0.1.1.0");
        private readonly string description = "Flat button. Imidates System.Windows.Forms.Button.";

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

        private bool _AutoColor = true;

        /// <summary>
        /// <c>true</c> if <see cref="HTButton"/> is auto-colored from shades of <see cref="Control.BackColor"/>, otherwise <c>false</c>.
        /// </summary>
        [Bindable(false), Browsable(true), Description("true if button is auto-colored from shades of BackColor, otherwise false."), EditorBrowsable(EditorBrowsableState.Always), Category("HTButton")]
        public bool AutoColor
        {
            get => _AutoColor;
            set
            {
                if (value)
                {
                    Recolor();
                }
                _AutoColor = value;
            }
        }

        private void Recolor()
        {
            _NormalColor = Tools.ShiftBrightness(BackColor, 20, false);
            _HoverColor = Tools.ShiftBrightness(_NormalColor, 20, false);
            _ClickColor = Tools.ShiftBrightness(_HoverColor, 20, false);
        }

        /// <summary>
        /// <c>true</c> if <see cref="HTButton"/> is auto-sized to <see cref="Text"/> or <see cref="ButtonImage"/>, otherwise <c>false</c>.
        /// </summary>

        [Bindable(false), Browsable(true), Description("true if button is auto-sized to text or image, otherwise false."), EditorBrowsable(EditorBrowsableState.Always), Category("HTButton")]
        public override bool AutoSize { get => base.AutoSize; set => base.AutoSize = value; }

        private bool _drawImage = false;

        /// <summary>
        /// Determines how to display image and text.
        /// </summary>
        [Bindable(false), Description("Determines if the ButtonImage should be displayed or not."), Category("HTButton")]
        public bool DrawImage
        {
            get => _drawImage;
            set { _drawImage = value; Update(); }
        }

        private ButtonImageSizeMode imgSizeMode = ButtonImageSizeMode.None;

        /// <summary>
        /// Determines how to display image.
        /// </summary>
        [Bindable(false), Description("Determines how to display image."), Category("HTButton")]
        public ButtonImageSizeMode ImageSizeMode
        {
            get => imgSizeMode;
            set { imgSizeMode = value; Update(); }
        }

        /// <summary>
        /// Determines the display image.
        /// </summary>
        [Bindable(false), Description("Determines the display image."), Category("HTButton")]
        public Image ButtonImage
        {
            get => _Image;
            set { _Image = value; Update(); }
        }

        // default values
        private _States _State = _States.Normal;

        private ButtonShapes _ButtonShape = ButtonShapes.Rectangle;

        private Color _NormalColor = Color.FromArgb(255, 235, 235, 235);
        private Color _ClickColor = Color.FromArgb(255, 195, 195, 195);
        private Color _HoverColor = Color.FromArgb(255, 215, 215, 215);

        [Bindable(false), Description("Shape of button."), Category("HTButton")]
        public ButtonShapes ButtonShape
        {
            get
            {
                return _ButtonShape;
            }
            set
            {
                _ButtonShape = value;
                this.Invalidate();
            }
        }

        [Bindable(false), Description("Determines the color of button on idle."), Category("HTButton")]
        public Color NormalColor
        {
            get
            {
                if (_AutoColor) { return Tools.ShiftBrightness(BackColor, 20, false); }
                return _NormalColor;
            }
            set
            {
                if (!_AutoColor)
                {
                    _NormalColor = value;
                }
                else
                {
                    _NormalColor = Tools.ShiftBrightness(BackColor, 20, false);
                }
                this.Invalidate();
            }
        }

        [Bindable(false), Description("Determines the color of button on hover."), Category("HTButton")]
        public Color HoverColor
        {
            get
            {
                if (_AutoColor) { return Tools.ShiftBrightness(BackColor, 40, false); }
                return _HoverColor;
            }
            set
            {
                if (!_AutoColor)
                {
                    _HoverColor = value;
                }
                else
                {
                    _HoverColor = Tools.ShiftBrightness(BackColor, 40, false);
                }
                this.Invalidate();
            }
        }

        [Bindable(false), Description("Determines the color of button on click."), Category("HTButton")]
        public Color ClickColor
        {
            get
            {
                if (_AutoColor) { return Tools.ShiftBrightness(BackColor, 60, false); }
                return _ClickColor;
            }
            set
            {
                if (!_AutoColor)
                {
                    _ClickColor = value;
                }
                else
                {
                    _ClickColor = Tools.ShiftBrightness(BackColor, 60, false);
                }
                this.Invalidate();
            }
        }

        // to make sure the control is invalidated(repainted) when the text is changed
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.Invalidate();
            }
        }

        private Image _Image;

        public HTButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.Selectable | ControlStyles.UserMouse, true);
        }

        #endregion Properties

        #region Paint

        #region AutoSize

        /// <summary>
        /// Calculate the required size of the control if in AutoSize.
        /// </summary>
        /// <returns>Size.</returns>
        private Size GetAutoSize()
        {
            switch (_Image)
            {
                case null:
                    {
                        if (!string.IsNullOrEmpty(Text))
                        {
                            Graphics graphics = Graphics.FromHwnd(Handle);
                            SizeF textSize = graphics.MeasureString(Text, base.Font);
                            int textX = (int)(textSize.Width);
                            int textY = (int)(textSize.Height);
                            return new Size((textX * 2) - (textX / 2), (textY * 2) - (textY / 2));
                        }
                        else
                        {
                            return new Size(100, 50);
                        }
                    }

                default:
                    if (imgSizeMode != ButtonImageSizeMode.None)
                    {
                        if (!string.IsNullOrEmpty(Text))
                        {
                            Graphics graphics = Graphics.FromHwnd(Handle);
                            SizeF textSize = graphics.MeasureString(Text, base.Font);
                            int textX = (int)(textSize.Width);
                            int textY = (int)(textSize.Height);
                            return new Size((textX * 2) - (textX / 2), (textY * 2) - (textY / 2));
                        }
                        else
                        {
                            return new Size(100, 50);
                        }
                    }
                    else
                    {
                        return _Image.Size;
                    }
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return GetAutoSize();
        }

        /// <summary>
        /// Performs the work of setting the specified bounds of this control.
        /// </summary>
        protected override void SetBoundsCore(int x, int y, int width, int height,
                BoundsSpecified specified)
        {
            //  Only when the size is affected...
            if (this.AutoSize && (specified & BoundsSpecified.Size) != 0)
            {
                Size size = GetAutoSize();

                width = size.Width;
                height = size.Height;
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        #endregion AutoSize

        #region Mouse Events

        protected override void OnMouseLeave(System.EventArgs e)
        {
            if (Enabled)
            {
                _State = _States.Normal;
                this.Invalidate();
                base.OnMouseLeave(e);
            }
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            if (Enabled)
            {
                _State = _States.MouseOver;
                this.Invalidate();
                base.OnMouseEnter(e);
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            if (Enabled)
            {
                _State = _States.MouseOver;
                this.Invalidate();
                base.OnMouseUp(e);
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (Enabled)
            {
                _State = _States.Clicked;
                this.Invalidate();
                base.OnMouseDown(e);
            }
        }

        protected override void OnClick(System.EventArgs e)
        {
            // prevent click when button is inactive
            if (Enabled)
            {
                if (_State == _States.Clicked)
                {
                    base.OnClick(e);
                }
            }
        }

        #endregion Mouse Events

        #region Image Draw Modes

        private void EDrawImage(PaintEventArgs e)
        {
            if (imgSizeMode == ButtonImageSizeMode.None)
            {
                DrawNoneImage(e);
            }
            else if (imgSizeMode == ButtonImageSizeMode.Center)
            {
                DrawCenterImage(e);
            }
            else if (imgSizeMode == ButtonImageSizeMode.Stretch)
            {
                DrawStretchImage(e);
            }
            else if (imgSizeMode == ButtonImageSizeMode.Tile)
            {
                DrawTileImage(e);
            }
            else if (imgSizeMode == ButtonImageSizeMode.Zoom)
            {
                DrawZoomImage(e);
            }
        }

        private void DrawZoomImage(PaintEventArgs p)
        {
            if (_Image == null) { return; }
            Graphics g = p.Graphics;
            Image resizedImage = _Image;
            if (Width > Height)
            {
                resizedImage = Tools.ResizeImage(_Image, Height, Height);
            }
            else if (Height > Width)
            {
                resizedImage = Tools.ResizeImage(_Image, Width, Width);
            }
            else
            {
                resizedImage = Tools.ResizeImage(_Image, Width, Height);
            }
            g.DrawImage(_Image,
                        new Rectangle((Width / 2) - (resizedImage.Width / 2),
                                      (Height / 2) - (resizedImage.Height / 2),
                                      resizedImage.Width,
                                      resizedImage.Height));
        }

        private void DrawCenterImage(PaintEventArgs p)
        {
            if (_Image == null) { return; }
            Graphics g = p.Graphics;
            if (Width > _Image.Width && Height > _Image.Height)
            {
                g.DrawImage(_Image,
                            new Rectangle((Width / 2) - (_Image.Width / 2),
                                          (Height / 2) - (_Image.Height / 2),
                                          _Image.Width,
                                          _Image.Height));
            }
            else
            {
                DrawZoomImage(p);
            }
        }

        private void DrawTileImage(PaintEventArgs p)
        {
            if (_Image == null) { return; }
            Graphics g = p.Graphics;
            Tools.FillPattern(_Image, Bounds);
        }

        private void DrawStretchImage(PaintEventArgs p)
        {
            if (_Image == null) { return; }
            Graphics g = p.Graphics;
            Image resizedImage = Tools.ResizeImage(_Image, Width, Height);
            g.DrawImage(resizedImage,
                        new Rectangle(0,
                                      0,
                                      Width,
                                      Height));
        }

        private void DrawNoneImage(PaintEventArgs p)
        {
            if (_Image == null) { return; }
            Graphics g = p.Graphics;
            g.DrawImage(_Image,
                        new Rectangle(0,
                                      0,
                                      Width,
                                      Height), new Rectangle(0, 0, Width, Height), GraphicsUnit.Pixel);
        }

        #endregion Image Draw Modes

        #region Main

        protected override void OnPaint(PaintEventArgs pevent)
        {
            SolidBrush brush;
            if (_AutoColor)
            {
                Recolor();
            }
            // set SmoothingMode
            pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            SizeF textSize = pevent.Graphics.MeasureString(this.Text, base.Font);
            int textX = (int)(base.Size.Width / 2) - (int)(textSize.Width / 2);
            int textY = (int)(base.Size.Height / 2) - (int)(textSize.Height / 2);
            Rectangle newRect = new Rectangle(ClientRectangle.X + 1, ClientRectangle.Y + 1, ClientRectangle.Width - 3, ClientRectangle.Height - 3);
            if (Enabled)
            {
                switch (_State)
                {
                    case _States.Normal:
                        brush = new SolidBrush(_NormalColor);
                        switch (_ButtonShape)
                        {
                            case ButtonShapes.Rectangle:
                                pevent.Graphics.FillRectangle(brush, newRect);
                                break;

                            case ButtonShapes.Ellipse:
                                pevent.Graphics.FillEllipse(brush, newRect);
                                break;
                        }
                        pevent.Graphics.DrawString(this.Text, base.Font, new SolidBrush(base.ForeColor), textX, textY);
                        break;

                    case _States.MouseOver:
                        brush = new SolidBrush(_HoverColor);
                        switch (_ButtonShape)
                        {
                            case ButtonShapes.Rectangle:
                                pevent.Graphics.FillRectangle(brush, newRect);
                                break;

                            case ButtonShapes.Ellipse:
                                pevent.Graphics.FillEllipse(brush, newRect);
                                break;
                        }
                        pevent.Graphics.DrawString(this.Text, base.Font, new SolidBrush(base.ForeColor), textX, textY);
                        break;

                    case _States.Clicked:
                        brush = new SolidBrush(_ClickColor);
                        switch (_ButtonShape)
                        {
                            case ButtonShapes.Rectangle:
                                pevent.Graphics.FillRectangle(brush, newRect);
                                break;

                            case ButtonShapes.Ellipse:
                                pevent.Graphics.FillEllipse(brush, newRect);
                                break;
                        }
                        pevent.Graphics.DrawString(this.Text, base.Font, new SolidBrush(base.ForeColor), textX + 1, textY + 1);
                        break;
                }
            }
            else
            {
                brush = new SolidBrush(_NormalColor);
                switch (_ButtonShape)
                {
                    case ButtonShapes.Rectangle:
                        pevent.Graphics.FillRectangle(brush, newRect);
                        break;

                    case ButtonShapes.Ellipse:
                        pevent.Graphics.FillEllipse(brush, newRect);
                        break;
                }
                pevent.Graphics.DrawString(this.Text, base.Font, new SolidBrush(ForeColor), textX, textY);
            }
            if (_drawImage)
            {
                EDrawImage(pevent);
            }
            pevent.Graphics.ResetClip();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            if (this != null)
            {
                Invalidate(Bounds, true);
            }
            base.OnBackColorChanged(e);
        }

        #endregion Main

        #endregion Paint

        #region Designer

        internal class HTButtonDesigner : System.Windows.Forms.Design.ControlDesigner
        {
            public HTButtonDesigner()
            {
            }

            protected override void PostFilterProperties(IDictionary Properties)
            {
                Properties.Remove("AllowDrop");
                Properties.Remove("BackgroundImage");
                Properties.Remove("ContextMenu");
                Properties.Remove("FlatStyle");
                Properties.Remove("Image");
                Properties.Remove("ImageAlign");
                Properties.Remove("ImageIndex");
                Properties.Remove("ImageList");
                Properties.Remove("TextAlign");
            }
        }

        #endregion Designer
    }
}