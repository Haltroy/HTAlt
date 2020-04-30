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
using System.Windows.Forms;

namespace HaltroyFramework
{
    public class HaltroyButton : Button
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
        #endregion
        public HaltroyButton()
        {
            Startup startup = new Startup();
            CurrentBackColor = BackColor;
        }
        
        private ButtonTextImageRelation tiRelation = ButtonTextImageRelation.TextBelowImage;
        
        [Bindable(false)]
        [DefaultValue(typeof(ButtonTextImageRelation), "Normal")]
        [Category("Appearance")]
        [Description("Determines how to display image.")]
        public ButtonTextImageRelation TextImageRelation
        {
            get { return tiRelation; }
            set { tiRelation = value; }
        }

        private Color CurrentBackColor;

        #region "MathBox"
        private static int Brightness(Color c)
        {
            return (int)Math.Sqrt(
               c.R * c.R * .241 +
               c.G * c.G * .691 +
               c.B * c.B * .068);
        }
        public static bool isBright(Color c)
        {
            return Brightness(c) > 130;
        }
        public static int GerekiyorsaAzalt(int defaultint, int azaltma)
        {
            return defaultint > azaltma ? defaultint - azaltma : defaultint;
        }
        public static int GerekiyorsaArttýr(int defaultint, int arttýrma, int sýnýr)
        {
            return defaultint + arttýrma > sýnýr ? defaultint : defaultint + arttýrma;
        }
        public static Color ShiftBrightnessIfNeeded(Color baseColor, int value, bool shiftAlpha)
        {
            if (isBright(baseColor))
            {
                return Color.FromArgb(shiftAlpha ? GerekiyorsaAzalt(baseColor.A, value) : baseColor.A,
                                      GerekiyorsaAzalt(baseColor.R, value),
                                      GerekiyorsaAzalt(baseColor.G, value),
                                      GerekiyorsaAzalt(baseColor.B, value));
            }
            else
            {
                return Color.FromArgb(shiftAlpha ? GerekiyorsaArttýr(baseColor.A, value, 255) : baseColor.A,
                      GerekiyorsaArttýr(baseColor.R, value, 255),
                      GerekiyorsaArttýr(baseColor.G, value, 255),
                      GerekiyorsaArttýr(baseColor.B, value, 255));
            }
        }
        #endregion
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            CurrentBackColor = ShiftBrightnessIfNeeded(BackColor,20,false);
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
            CurrentBackColor = ShiftBrightnessIfNeeded(BackColor, 40, false);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            CurrentBackColor = BackColor;
            Invalidate();
        }
        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        private void DrawImage(PaintEventArgs p)
        {
            if (Image == null) { return; }
            Graphics g = p.Graphics;
            Image resizedImage = Image;
            if (Width > Height)
            {
                resizedImage = ResizeImage(Image, Height, Height);
            }
            else if ( Height > Width)
            {
                resizedImage = ResizeImage(Image, Width, Width);
            }
            else
            {
                resizedImage = ResizeImage(Image, Width,Height);
            }
            g.DrawImage(Image, 
                        new Rectangle((Width / 2) - (resizedImage.Width / 2),
                                      (Height / 2) - (resizedImage.Height / 2),
                                      resizedImage.Width,
                                      resizedImage.Height));

        }
        private void DrawText(PaintEventArgs p)
        {
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(p.Graphics, Text, Font, new Point(Width + 3, Height / 2), ForeColor, flags);
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            ButtonRenderer.DrawParentBackground(pevent.Graphics, ClientRectangle, this);
            if (BackColor != Color.Transparent)
            {
                pevent.Graphics.FillRectangle(new SolidBrush(CurrentBackColor), 0, 0, Width, Height);
            }
            if (tiRelation == ButtonTextImageRelation.None) { return; }
            else if ( tiRelation == ButtonTextImageRelation.JustText)
            {
                DrawText(pevent);
            }
            else if (tiRelation == ButtonTextImageRelation.JustImage)
            {
                DrawImage(pevent);
            }else if (tiRelation == ButtonTextImageRelation.TextAboveImage)
            {
                DrawImage(pevent);
                DrawText(pevent);
            }
            else if (tiRelation == ButtonTextImageRelation.TextBelowImage)
            {
                DrawText(pevent);
                DrawImage(pevent);
            }
        }
    }
}