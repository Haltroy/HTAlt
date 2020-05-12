//MIT License
//
//Copyright (c) 2020 Eren "Haltroy" Kanat
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Forms;

namespace HTAlt
{
    /// <summary>
    /// Flat button. Imidates <see cref="System.Windows.Forms.Button"/>.
    /// </summary>
    public class HTButton : Button
    {
        #region "Enums"
        public enum ButtonTextImageRelation
        {
            None,
            JustImage,
            JustText,
            TextBelowImage,
            TextAboveImage
        }
        public enum ButtonImageSizeMode
        {
            None,
            Center,
            Stretch,
            Tile,
            Zoom
        }
        #endregion
        public HTButton() : base()
        {
            FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.CheckedBackColor = Color.Empty;
            FlatAppearance.MouseOverBackColor = Color.Empty;
            Tools.PrintInfoToConsole();
            CurrentBackColor = BackColor;
        }
        private ButtonImageSizeMode imgSizeMode = ButtonImageSizeMode.None;
        private ButtonTextImageRelation tiRelation = ButtonTextImageRelation.TextBelowImage;
        /// <summary>
        /// This property is not in use.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image BackgroundImage
        {
            get => null;
            set => BackgroundImage = null;
        }

        /// <summary>
        /// This property is not in use.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new TextAlignment TextAlign
        {
            get => TextAlignment.Center;
            set => value = TextAlignment.Center;
        }
        /// <summary>
        /// This property is not in use.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ImageLayout BackgroundImageLayout
        {
            get => ImageLayout.None;
            set => value = ImageLayout.None;
        }
        /// <summary>
        /// This property is not in use.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Text
        {
            get => string.Empty;
            set => value = string.Empty;
        }
        // <summary>
        /// This property is not in use.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image Image
        {
            get => null;
            set => value = null;
        }
        /// <summary>
        /// Determines how to display image and text.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(ButtonTextImageRelation), "Normal")]
        [Category("Appearance")]
        [Description("Determines how to display image and text.")]
        public new ButtonTextImageRelation TextImageRelation
        {
            get => tiRelation;
            set => tiRelation = value;
        }
        /// <summary>
        /// Determines how to display image.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(typeof(ButtonImageSizeMode), "None")]
        [Category("Appearance")]
        [Description("Determines how to display image.")]
        public ButtonImageSizeMode ImageSizeMode
        {
            get => imgSizeMode;
            set => imgSizeMode = value;
        }
        /// <summary>
        /// Determines the display image.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(null)]
        [Category("Appearance")]
        [Description("Determines the display image.")]
        public Image ButtonImage
        {
            get => _Image;
            set => _Image = value;
        }
        /// <summary>
        /// Determines the display text.
        /// </summary>
        [Bindable(false)]
        [DefaultValue(null)]
        [Category("Appearance")]
        [Description("Determines the display text.")]
        public string ButtonText
        {
            get => _Text;
            set => _Text = value;
        }
        private string _Text;
        private Image _Image;
        private Color CurrentBackColor;


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            CurrentBackColor = Tools.IsOpaque(BackColor) ? Tools.ShiftBrightnessIfNeeded(BackColor, 20, false) : Tools.ShiftBrightnessIfNeeded(BackColor, 20, true);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            CurrentBackColor = BackColor;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            CurrentBackColor = Tools.IsOpaque(BackColor) ? Tools.ShiftBrightnessIfNeeded(BackColor, 40, false) : Tools.ShiftBrightnessIfNeeded(BackColor, 40, true);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            CurrentBackColor = BackColor;
            Invalidate();
        }
        #region "Paint"

        #region "Image Draw Modes"
        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        private void DrawImage(PaintEventArgs e)
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
                resizedImage = ResizeImage(_Image, Height, Height);
            }
            else if (Height > Width)
            {
                resizedImage = ResizeImage(_Image, Width, Width);
            }
            else
            {
                resizedImage = ResizeImage(_Image, Width, Height);
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
                            new Rectangle((Width / 2) - (Image.Width / 2),
                                          (Height / 2) - (Image.Height / 2),
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
            FillPattern(g, _Image, Bounds);
        }
        private static void FillPattern(Graphics g, Image image, Rectangle rect)
        {
            Rectangle _ImageRect;
            Rectangle drawRect;

            for (int x = rect.X; x < rect.Right; x += image.Width)
            {
                for (int y = rect.Y; y < rect.Bottom; y += image.Height)
                {
                    drawRect = new Rectangle(x, y, Math.Min(image.Width, rect.Right - x),
                                   Math.Min(image.Height, rect.Bottom - y));
                    _ImageRect = new Rectangle(0, 0, drawRect.Width, drawRect.Height);

                    g.DrawImage(image, drawRect, _ImageRect, GraphicsUnit.Pixel);
                }
            }
        }
        private void DrawStretchImage(PaintEventArgs p)
        {
            if (_Image == null) { return; }
            Graphics g = p.Graphics;
            Image resizedImage = ResizeImage(_Image, Width, Height);
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
        #endregion
        private void DrawText(PaintEventArgs p)
        {
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(p.Graphics, _Text, Font, new System.Drawing.Point(Width + 3, Height / 2), ForeColor, flags);
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            //Paint the base
            base.OnPaint(pevent);
            //Draw text and image
            if (tiRelation == ButtonTextImageRelation.None) { }
            else if (tiRelation == ButtonTextImageRelation.JustText)
            {
                DrawText(pevent);
            }
            else if (tiRelation == ButtonTextImageRelation.JustImage)
            {
                DrawImage(pevent);
            }
            else if (tiRelation == ButtonTextImageRelation.TextAboveImage)
            {
                DrawImage(pevent);
                DrawText(pevent);
            }
            else if (tiRelation == ButtonTextImageRelation.TextBelowImage)
            {
                DrawText(pevent);
                DrawImage(pevent);
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
        #endregion
    }
}