using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Haltroyify;

namespace Haltroyify
{
    public class WebtroySwitchIphoneRenderer : WebtroySwitchRendererBase, IDisposable
    {
        #region Constructor

        private GraphicsPath _innerControlPath = null;

        public WebtroySwitchIphoneRenderer()
        {
            OuterBorderColor = Color.FromArgb(255, 205, 205, 207);
            InnerBorderColor1 = Color.FromArgb(200, 205, 205, 207);
            InnerBorderColor2 = Color.FromArgb(200, 205, 205, 207);
            LeftSideBackColor1 = Color.FromArgb(255, 50, 101, 161);
            LeftSideBackColor2 = Color.FromArgb(255, 123, 174, 229);
            RightSideBackColor1 = Color.FromArgb(255, 161, 161, 161);
            RightSideBackColor2 = Color.FromArgb(255, 250, 250, 250);
            ButtonNormalBorderColor1 = Color.FromArgb(255, 172, 172, 172);
            ButtonNormalBorderColor2 = Color.FromArgb(255, 196, 196, 196);
            ButtonNormalSurfaceColor1 = Color.FromArgb(255, 216, 215, 216);
            ButtonNormalSurfaceColor2 = Color.FromArgb(255, 251, 250, 251);
            ButtonHoverBorderColor1 = Color.FromArgb(255, 163, 163, 163);
            ButtonHoverBorderColor2 = Color.FromArgb(255, 185, 185, 185);
            ButtonHoverSurfaceColor1 = Color.FromArgb(255, 205, 204, 205);
            ButtonHoverSurfaceColor2 = Color.FromArgb(255, 239, 238, 239);
            ButtonPressedBorderColor1 = Color.FromArgb(255, 129, 129, 129);
            ButtonPressedBorderColor2 = Color.FromArgb(255, 146, 146, 146);
            ButtonPressedSurfaceColor1 = Color.FromArgb(255, 162, 161, 162);
            ButtonPressedSurfaceColor2 = Color.FromArgb(255, 188, 187, 188);
            ButtonShadowColor1 = Color.FromArgb(50, 0, 0, 0);
            ButtonShadowColor2 = Color.FromArgb(0, 0, 0, 0);

            ButtonShadowWidth = 7;
            CornerRadius = 6;
            ButtonCornerRadius = 9;
        }

        public void Dispose()
        {
            if (_innerControlPath != null)
                _innerControlPath.Dispose();
        }

        #endregion Constructor

        #region Public Properties

        public Color OuterBorderColor { get; set; }
        public Color InnerBorderColor1 { get; set; }
        public Color InnerBorderColor2 { get; set; }
        public Color LeftSideBackColor1 { get; set; }
        public Color LeftSideBackColor2 { get; set; }
        public Color RightSideBackColor1 { get; set; }
        public Color RightSideBackColor2 { get; set; }
        public Color ButtonNormalBorderColor1 { get; set; }
        public Color ButtonNormalBorderColor2 { get; set; }
        public Color ButtonNormalSurfaceColor1 { get; set; }
        public Color ButtonNormalSurfaceColor2 { get; set; }
        public Color ButtonHoverBorderColor1 { get; set; }
        public Color ButtonHoverBorderColor2 { get; set; }
        public Color ButtonHoverSurfaceColor1 { get; set; }
        public Color ButtonHoverSurfaceColor2 { get; set; }
        public Color ButtonPressedBorderColor1 { get; set; }
        public Color ButtonPressedBorderColor2 { get; set; }
        public Color ButtonPressedSurfaceColor1 { get; set; }
        public Color ButtonPressedSurfaceColor2 { get; set; }
        public Color ButtonShadowColor1 { get; set; }
        public Color ButtonShadowColor2 { get; set; }
        
        public int ButtonShadowWidth { get; set; }
        public int CornerRadius { get; set; }
        public int ButtonCornerRadius { get; set; }

        #endregion Public Properties

        #region Render Method Implementations

        public override void RenderBorder(Graphics g, Rectangle borderRectangle)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            //Draw outer border
            using (GraphicsPath outerBorderPath = GetRoundedRectanglePath(borderRectangle, CornerRadius))
            {
                g.SetClip(outerBorderPath);

                Color outerBorderColor = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? OuterBorderColor : OuterBorderColor;

                using (Brush outerBorderBrush = new SolidBrush(outerBorderColor))
                {
                    g.FillPath(outerBorderBrush, outerBorderPath);
                }

                g.ResetClip();    
            }

            //Draw inner border
            Rectangle innerborderRectangle = new Rectangle(borderRectangle.X + 1, borderRectangle.Y + 1, borderRectangle.Width - 2, borderRectangle.Height - 2);

            using (GraphicsPath innerBorderPath = GetRoundedRectanglePath(innerborderRectangle, CornerRadius))
            {
                g.SetClip(innerBorderPath);

                Color borderColor1 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? InnerBorderColor1 : InnerBorderColor1;
                Color borderColor2 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? InnerBorderColor2 : InnerBorderColor2;

                using (Brush borderBrush = new LinearGradientBrush(borderRectangle, borderColor1, borderColor2, LinearGradientMode.Vertical))
                {
                    g.FillPath(borderBrush, innerBorderPath);
                }

                g.ResetClip();    
            }

            Rectangle backgroundRectangle = new Rectangle(borderRectangle.X + 2, borderRectangle.Y + 2, borderRectangle.Width - 4, borderRectangle.Height - 4);
            _innerControlPath = GetRoundedRectanglePath(backgroundRectangle, CornerRadius);
        }

        public override void RenderLeftToggleField(Graphics g, Rectangle leftRectangle, int totalToggleFieldWidth)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            int buttonWidth = GetButtonWidth();

            //Draw inner background
            int gradientRectWidth = leftRectangle.Width + buttonWidth / 2;
            Rectangle gradientRectangle = new Rectangle(leftRectangle.X, leftRectangle.Y, gradientRectWidth, leftRectangle.Height);

            Color leftSideBackColor1 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? LeftSideBackColor1 : LeftSideBackColor1;
            Color leftSideBackColor2 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? LeftSideBackColor2 : LeftSideBackColor2;

            if (_innerControlPath != null)
            {
                g.SetClip(_innerControlPath);
                g.IntersectClip(gradientRectangle);
            }
            else
            {
                g.SetClip(gradientRectangle);
            }

            using (Brush backgroundBrush = new LinearGradientBrush(gradientRectangle, leftSideBackColor1, leftSideBackColor2, LinearGradientMode.Vertical))
            {
                g.FillRectangle(backgroundBrush, gradientRectangle);
            }

            g.ResetClip();

            Rectangle leftShadowRectangle = new Rectangle();
            leftShadowRectangle.X = leftRectangle.X + leftRectangle.Width - ButtonShadowWidth;
            leftShadowRectangle.Y = leftRectangle.Y;
            leftShadowRectangle.Width = ButtonShadowWidth + CornerRadius;
            leftShadowRectangle.Height = leftRectangle.Height;

            if (_innerControlPath != null)
            {
                g.SetClip(_innerControlPath);
                g.IntersectClip(leftShadowRectangle);
            }
            else
            {
                g.SetClip(leftShadowRectangle);
            }

            using (Brush buttonShadowBrush = new LinearGradientBrush(leftShadowRectangle, ButtonShadowColor2, ButtonShadowColor1, LinearGradientMode.Horizontal))
            {
                g.FillRectangle(buttonShadowBrush, leftShadowRectangle);
            }

            g.ResetClip();

            //Draw image or text
            if (WebtroySwitch.OnSideImage != null || !string.IsNullOrEmpty(WebtroySwitch.OnText))
            {
                RectangleF fullRectangle = new RectangleF(leftRectangle.X + 1 - (totalToggleFieldWidth - leftRectangle.Width), 1, totalToggleFieldWidth - 1, WebtroySwitch.Height - 2);

                if (_innerControlPath != null)
                {
                    g.SetClip(_innerControlPath);
                    g.IntersectClip(fullRectangle);
                }
                else
                {
                    g.SetClip(fullRectangle);
                }

                if (WebtroySwitch.OnSideImage != null)
                {
                    Size imageSize = WebtroySwitch.OnSideImage.Size;
                    Rectangle imageRectangle;

                    int imageXPos = (int)fullRectangle.X;

                    if (WebtroySwitch.OnSideScaleImageToFit)
                    {
                        Size canvasSize = new Size((int)fullRectangle.Width, (int)fullRectangle.Height);
                        Size resizedImageSize = ImageHelper.RescaleImageToFit(imageSize, canvasSize);

                        if (WebtroySwitch.OnSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Center)
                        {
                            imageXPos = (int)((float)fullRectangle.X + (((float)fullRectangle.Width - (float)resizedImageSize.Width) / 2));
                        }
                        else if (WebtroySwitch.OnSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Near)
                        {
                            imageXPos = (int)((float)fullRectangle.X + (float)fullRectangle.Width - (float)resizedImageSize.Width);
                        }

                        imageRectangle = new Rectangle(imageXPos, (int)((float)fullRectangle.Y + (((float)fullRectangle.Height - (float)resizedImageSize.Height) / 2)), resizedImageSize.Width, resizedImageSize.Height);

                        if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                            g.DrawImage(WebtroySwitch.OnSideImage, imageRectangle, 0, 0, WebtroySwitch.OnSideImage.Width, WebtroySwitch.OnSideImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                        else
                            g.DrawImage(WebtroySwitch.OnSideImage, imageRectangle);
                    }
                    else
                    {
                        if (WebtroySwitch.OnSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Center)
                        {
                            imageXPos = (int)((float)fullRectangle.X + (((float)fullRectangle.Width - (float)imageSize.Width) / 2));
                        }
                        else if (WebtroySwitch.OnSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Near)
                        {
                            imageXPos = (int)((float)fullRectangle.X + (float)fullRectangle.Width - (float)imageSize.Width);
                        }

                        imageRectangle = new Rectangle(imageXPos, (int)((float)fullRectangle.Y + (((float)fullRectangle.Height - (float)imageSize.Height) / 2)), imageSize.Width, imageSize.Height);

                        if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                            g.DrawImage(WebtroySwitch.OnSideImage, imageRectangle, 0, 0, WebtroySwitch.OnSideImage.Width, WebtroySwitch.OnSideImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                        else
                            g.DrawImageUnscaled(WebtroySwitch.OnSideImage, imageRectangle);
                    }
                }
                else if (!string.IsNullOrEmpty(WebtroySwitch.OnText))
                {
                    SizeF textSize = g.MeasureString(WebtroySwitch.OnText, WebtroySwitch.OnFont);

                    float textXPos = fullRectangle.X;

                    if (WebtroySwitch.OnSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Center)
                    {
                        textXPos = (float)fullRectangle.X + (((float)fullRectangle.Width - (float)textSize.Width) / 2);
                    }
                    else if (WebtroySwitch.OnSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Near)
                    {
                        textXPos = (float)fullRectangle.X + (float)fullRectangle.Width - (float)textSize.Width;
                    }

                    RectangleF textRectangle = new RectangleF(textXPos, (float)fullRectangle.Y + (((float)fullRectangle.Height - (float)textSize.Height) / 2), textSize.Width, textSize.Height);

                    Color textForeColor = WebtroySwitch.OnForeColor;

                    if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                        textForeColor = textForeColor;

                    using (Brush textBrush = new SolidBrush(textForeColor))
                    {
                        g.DrawString(WebtroySwitch.OnText, WebtroySwitch.OnFont, textBrush, textRectangle);
                    }
                }

                g.ResetClip();
            }
        }

        public override void RenderRightToggleField(Graphics g, Rectangle rightRectangle, int totalToggleFieldWidth)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            int buttonWidth = GetButtonWidth();

            //Draw inner background
            int gradientRectWidth = rightRectangle.Width + buttonWidth / 2;
            Rectangle gradientRectangle = new Rectangle(rightRectangle.X - buttonWidth / 2, rightRectangle.Y, gradientRectWidth, rightRectangle.Height);

            Color rightSideBackColor1 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? RightSideBackColor1 : RightSideBackColor1;
            Color rightSideBackColor2 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? RightSideBackColor2 : RightSideBackColor2;

            if (_innerControlPath != null)
            {
                g.SetClip(_innerControlPath);
                g.IntersectClip(gradientRectangle);
            }
            else
            {
                g.SetClip(gradientRectangle);
            }

            using (Brush backgroundBrush = new LinearGradientBrush(gradientRectangle, rightSideBackColor1, rightSideBackColor2, LinearGradientMode.Vertical))
            {
                g.FillRectangle(backgroundBrush, gradientRectangle);
            }

            g.ResetClip();

            Rectangle rightShadowRectangle = new Rectangle();
            rightShadowRectangle.X = rightRectangle.X - CornerRadius;
            rightShadowRectangle.Y = rightRectangle.Y;
            rightShadowRectangle.Width = ButtonShadowWidth + CornerRadius;
            rightShadowRectangle.Height = rightRectangle.Height;

            if (_innerControlPath != null)
            {
                g.SetClip(_innerControlPath);
                g.IntersectClip(rightShadowRectangle);
            }
            else
            {
                g.SetClip(rightShadowRectangle);
            }

            using (Brush buttonShadowBrush = new LinearGradientBrush(rightShadowRectangle, ButtonShadowColor1, ButtonShadowColor2, LinearGradientMode.Horizontal))
            {
                g.FillRectangle(buttonShadowBrush, rightShadowRectangle);
            }

            g.ResetClip();

            //Draw image or text
            if (WebtroySwitch.OffSideImage != null || !string.IsNullOrEmpty(WebtroySwitch.OffText))
            {
                RectangleF fullRectangle = new RectangleF(rightRectangle.X, 1, totalToggleFieldWidth - 1, WebtroySwitch.Height - 2);

                if (_innerControlPath != null)
                {
                    g.SetClip(_innerControlPath);
                    g.IntersectClip(fullRectangle);
                }
                else
                {
                    g.SetClip(fullRectangle);
                }

                if (WebtroySwitch.OffSideImage != null)
                {
                    Size imageSize = WebtroySwitch.OffSideImage.Size;
                    Rectangle imageRectangle;

                    int imageXPos = (int)fullRectangle.X;

                    if (WebtroySwitch.OffSideScaleImageToFit)
                    {
                        Size canvasSize = new Size((int)fullRectangle.Width, (int)fullRectangle.Height);
                        Size resizedImageSize = ImageHelper.RescaleImageToFit(imageSize, canvasSize);

                        if (WebtroySwitch.OffSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Center)
                        {
                            imageXPos = (int)((float)fullRectangle.X + (((float)fullRectangle.Width - (float)resizedImageSize.Width) / 2));
                        }
                        else if (WebtroySwitch.OffSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Far)
                        {
                            imageXPos = (int)((float)fullRectangle.X + (float)fullRectangle.Width - (float)resizedImageSize.Width);
                        }

                        imageRectangle = new Rectangle(imageXPos, (int)((float)fullRectangle.Y + (((float)fullRectangle.Height - (float)resizedImageSize.Height) / 2)), resizedImageSize.Width, resizedImageSize.Height);

                        if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                            g.DrawImage(WebtroySwitch.OnSideImage, imageRectangle, 0, 0, WebtroySwitch.OnSideImage.Width, WebtroySwitch.OnSideImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                        else
                            g.DrawImage(WebtroySwitch.OnSideImage, imageRectangle);
                    }
                    else
                    {
                        if (WebtroySwitch.OffSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Center)
                        {
                            imageXPos = (int)((float)fullRectangle.X + (((float)fullRectangle.Width - (float)imageSize.Width) / 2));
                        }
                        else if (WebtroySwitch.OffSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Far)
                        {
                            imageXPos = (int)((float)fullRectangle.X + (float)fullRectangle.Width - (float)imageSize.Width);
                        }

                        imageRectangle = new Rectangle(imageXPos, (int)((float)fullRectangle.Y + (((float)fullRectangle.Height - (float)imageSize.Height) / 2)), imageSize.Width, imageSize.Height);

                        if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                            g.DrawImage(WebtroySwitch.OnSideImage, imageRectangle, 0, 0, WebtroySwitch.OnSideImage.Width, WebtroySwitch.OnSideImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                        else
                            g.DrawImageUnscaled(WebtroySwitch.OffSideImage, imageRectangle);
                    }
                }
                else if (!string.IsNullOrEmpty(WebtroySwitch.OffText))
                {
                    SizeF textSize = g.MeasureString(WebtroySwitch.OffText, WebtroySwitch.OffFont);

                    float textXPos = fullRectangle.X;

                    if (WebtroySwitch.OffSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Center)
                    {
                        textXPos = (float)fullRectangle.X + (((float)fullRectangle.Width - (float)textSize.Width) / 2);
                    }
                    else if (WebtroySwitch.OffSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Far)
                    {
                        textXPos = (float)fullRectangle.X + (float)fullRectangle.Width - (float)textSize.Width;
                    }

                    RectangleF textRectangle = new RectangleF(textXPos, (float)fullRectangle.Y + (((float)fullRectangle.Height - (float)textSize.Height) / 2), textSize.Width, textSize.Height);

                    Color textForeColor = WebtroySwitch.OffForeColor;

                    if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                        textForeColor = textForeColor;

                    using (Brush textBrush = new SolidBrush(textForeColor))
                    {
                        g.DrawString(WebtroySwitch.OffText, WebtroySwitch.OffFont, textBrush, textRectangle);
                    }
                }

                g.ResetClip();
            }
        }

        public override void RenderButton(Graphics g, Rectangle buttonRectangle)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            using (GraphicsPath buttonPath = GetRoundedRectanglePath(buttonRectangle, ButtonCornerRadius))
            {
                if (_innerControlPath != null)
                {
                    g.SetClip(_innerControlPath);
                    g.IntersectClip(buttonRectangle);
                }
                else
                {
                    g.SetClip(buttonRectangle);
                }

                //Draw button surface
                Color buttonSurfaceColor1 = ButtonNormalSurfaceColor1;
                Color buttonSurfaceColor2 = ButtonNormalSurfaceColor2;

                if (WebtroySwitch.IsButtonPressed)
                {
                    buttonSurfaceColor1 = ButtonPressedSurfaceColor1;
                    buttonSurfaceColor2 = ButtonPressedSurfaceColor2;
                }
                else if (WebtroySwitch.IsButtonHovered)
                {
                    buttonSurfaceColor1 = ButtonHoverSurfaceColor1;
                    buttonSurfaceColor2 = ButtonHoverSurfaceColor2;
                }

                if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                {
                    buttonSurfaceColor1 = buttonSurfaceColor1;
                    buttonSurfaceColor2 = buttonSurfaceColor2;
                }

                using (Brush buttonSurfaceBrush = new LinearGradientBrush(buttonRectangle, buttonSurfaceColor1, buttonSurfaceColor2, LinearGradientMode.Vertical))
                {
                    g.FillPath(buttonSurfaceBrush, buttonPath);
                }

                //Draw button border
                Color buttonBorderColor1 = ButtonNormalBorderColor1;
                Color buttonBorderColor2 = ButtonNormalBorderColor2;

                if (WebtroySwitch.IsButtonPressed)
                {
                    buttonBorderColor1 = ButtonPressedBorderColor1;
                    buttonBorderColor2 = ButtonPressedBorderColor2;
                }
                else if (WebtroySwitch.IsButtonHovered)
                {
                    buttonBorderColor1 = ButtonHoverBorderColor1;
                    buttonBorderColor2 = ButtonHoverBorderColor2;
                }

                if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                {
                    buttonBorderColor1 = buttonBorderColor1;
                    buttonBorderColor2 = buttonBorderColor2;
                }

                using (Brush buttonBorderBrush = new LinearGradientBrush(buttonRectangle, buttonBorderColor1, buttonBorderColor2, LinearGradientMode.Vertical))
                {
                    using (Pen buttonBorderPen = new Pen(buttonBorderBrush))
                    {
                        g.DrawPath(buttonBorderPen, buttonPath);
                    }
                }

                g.ResetClip();

                //Draw button image
                Image buttonImage = WebtroySwitch.ButtonImage ?? (WebtroySwitch.Checked ? WebtroySwitch.OnButtonImage : WebtroySwitch.OffButtonImage);

                if (buttonImage != null)
                {
                    g.SetClip(buttonPath);

                    WebtroySwitch.WebtroySwitchButtonAlignment alignment = WebtroySwitch.ButtonImage != null ? WebtroySwitch.ButtonAlignment : (WebtroySwitch.Checked ? WebtroySwitch.OnButtonAlignment : WebtroySwitch.OffButtonAlignment);

                    Size imageSize = buttonImage.Size;

                    Rectangle imageRectangle;

                    int imageXPos = buttonRectangle.X;

                    bool scaleImage = WebtroySwitch.ButtonImage != null ? WebtroySwitch.ButtonScaleImageToFit : (WebtroySwitch.Checked ? WebtroySwitch.OnButtonScaleImageToFit : WebtroySwitch.OffButtonScaleImageToFit);

                    if (scaleImage)
                    {
                        Size canvasSize = buttonRectangle.Size;
                        Size resizedImageSize = ImageHelper.RescaleImageToFit(imageSize, canvasSize);

                        if (alignment == WebtroySwitch.WebtroySwitchButtonAlignment.Center)
                        {
                            imageXPos = (int)((float)buttonRectangle.X + (((float)buttonRectangle.Width - (float)resizedImageSize.Width) / 2));
                        }
                        else if (alignment == WebtroySwitch.WebtroySwitchButtonAlignment.Right)
                        {
                            imageXPos = (int)((float)buttonRectangle.X + (float)buttonRectangle.Width - (float)resizedImageSize.Width);
                        }

                        imageRectangle = new Rectangle(imageXPos, (int)((float)buttonRectangle.Y + (((float)buttonRectangle.Height - (float)resizedImageSize.Height) / 2)), resizedImageSize.Width, resizedImageSize.Height);

                        if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                            g.DrawImage(buttonImage, imageRectangle, 0, 0, buttonImage.Width, buttonImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                        else
                            g.DrawImage(buttonImage, imageRectangle);
                    }
                    else
                    {
                        if (alignment == WebtroySwitch.WebtroySwitchButtonAlignment.Center)
                        {
                            imageXPos = (int)((float)buttonRectangle.X + (((float)buttonRectangle.Width - (float)imageSize.Width) / 2));
                        }
                        else if (alignment == WebtroySwitch.WebtroySwitchButtonAlignment.Right)
                        {
                            imageXPos = (int)((float)buttonRectangle.X + (float)buttonRectangle.Width - (float)imageSize.Width);
                        }

                        imageRectangle = new Rectangle(imageXPos, (int)((float)buttonRectangle.Y + (((float)buttonRectangle.Height - (float)imageSize.Height) / 2)), imageSize.Width, imageSize.Height);

                        if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                            g.DrawImage(buttonImage, imageRectangle, 0, 0, buttonImage.Width, buttonImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                        else
                            g.DrawImageUnscaled(buttonImage, imageRectangle);
                    }

                    g.ResetClip();
                }
            }
        }

        #endregion Render Method Implementations

        #region Helper Method Implementations

        public GraphicsPath GetRoundedRectanglePath(Rectangle rectangle, int radius)
        {
            GraphicsPath gp = new GraphicsPath();
            int diameter = 2*radius;

            if (diameter > WebtroySwitch.Height)
                diameter = WebtroySwitch.Height;

            if (diameter > WebtroySwitch.Width)
                diameter = WebtroySwitch.Width;

            gp.AddArc(rectangle.X, rectangle.Y, diameter, diameter, 180, 90);
            gp.AddArc(rectangle.X + rectangle.Width - diameter, rectangle.Y, diameter, diameter, 270, 90);
            gp.AddArc(rectangle.X + rectangle.Width - diameter, rectangle.Y + rectangle.Height - diameter, diameter, diameter, 0, 90);
            gp.AddArc(rectangle.X, rectangle.Y + rectangle.Height - diameter, diameter, diameter, 90, 90);
            gp.CloseFigure();

            return gp;
        }

        public override int GetButtonWidth()
        {
            float buttonWidth = 1.57f*WebtroySwitch.Height;
            return (int) buttonWidth;
        }

        public override Rectangle GetButtonRectangle()
        {
            int buttonWidth = GetButtonWidth();
            return GetButtonRectangle(buttonWidth);
        }

        public override Rectangle GetButtonRectangle(int buttonWidth)
        {
            Rectangle buttonRect = new Rectangle(WebtroySwitch.ButtonValue, 0, buttonWidth, WebtroySwitch.Height);
            return buttonRect;
        }

        #endregion Helper Method Implementations
    }
}
