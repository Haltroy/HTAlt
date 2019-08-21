using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Haltroyify;

namespace Haltroyify
{
    public class WebtroySwitchFancyRenderer :  WebtroySwitchRendererBase, IDisposable
    {
        #region Constructor

        private GraphicsPath _innerControlPath = null;

        public WebtroySwitchFancyRenderer()
        {
            OuterBorderColor1 = Color.FromArgb(255, 197, 199, 201);
            OuterBorderColor2 = Color.FromArgb(255, 207, 209, 212);
            InnerBorderColor1 = Color.FromArgb(200, 205, 208, 207);
            InnerBorderColor2 = Color.FromArgb(255, 207, 209, 212);
            LeftSideBackColor1 = Color.FromArgb(255, 61, 110, 6);
            LeftSideBackColor2 = Color.FromArgb(255, 93, 170, 9);
            RightSideBackColor1 = Color.FromArgb(255, 149, 0, 0);
            RightSideBackColor2 = Color.FromArgb(255, 198, 0, 0);
            ButtonNormalBorderColor1 = Color.FromArgb(255, 212, 209, 211);
            ButtonNormalBorderColor2 = Color.FromArgb(255, 197, 199, 201);
            ButtonNormalUpperSurfaceColor1 = Color.FromArgb(255, 252, 251, 252);
            ButtonNormalUpperSurfaceColor2 = Color.FromArgb(255, 247, 247, 247);
            ButtonNormalLowerSurfaceColor1 = Color.FromArgb(255, 233, 233, 233);
            ButtonNormalLowerSurfaceColor2 = Color.FromArgb(255, 225, 225, 225);
            ButtonHoverBorderColor1 = Color.FromArgb(255, 212, 207, 209);
            ButtonHoverBorderColor2 = Color.FromArgb(255, 223, 223, 223);
            ButtonHoverUpperSurfaceColor1 = Color.FromArgb(255, 240, 239, 240);
            ButtonHoverUpperSurfaceColor2 = Color.FromArgb(255, 235, 235, 235);
            ButtonHoverLowerSurfaceColor1 = Color.FromArgb(255, 221, 221, 221);
            ButtonHoverLowerSurfaceColor2 = Color.FromArgb(255, 214, 214, 214);
            ButtonPressedBorderColor1 = Color.FromArgb(255, 176, 176, 176);
            ButtonPressedBorderColor2 = Color.FromArgb(255, 176, 176, 176);
            ButtonPressedUpperSurfaceColor1 = Color.FromArgb(255, 189, 188, 189);
            ButtonPressedUpperSurfaceColor2 = Color.FromArgb(255, 185, 185, 185);
            ButtonPressedLowerSurfaceColor1 = Color.FromArgb(255, 175, 175, 175);
            ButtonPressedLowerSurfaceColor2 = Color.FromArgb(255, 169, 169, 169);
            ButtonShadowColor1 = Color.FromArgb(50, 0, 0, 0);
            ButtonShadowColor2 = Color.FromArgb(0, 0, 0, 0);

            ButtonShadowWidth = 7;
            CornerRadius = 6;
        }

        public void Dispose()
        {
            if (_innerControlPath != null)
                _innerControlPath.Dispose();
        }

        #endregion Constructor

        #region Public Properties

        public Color OuterBorderColor1 { get; set; }
        public Color OuterBorderColor2 { get; set; }
        public Color InnerBorderColor1 { get; set; }
        public Color InnerBorderColor2 { get; set; }
        public Color LeftSideBackColor1 { get; set; }
        public Color LeftSideBackColor2 { get; set; }
        public Color RightSideBackColor1 { get; set; }
        public Color RightSideBackColor2 { get; set; }
        public Color ButtonNormalBorderColor1 { get; set; }
        public Color ButtonNormalBorderColor2 { get; set; }
        public Color ButtonNormalUpperSurfaceColor1 { get; set; }
        public Color ButtonNormalUpperSurfaceColor2 { get; set; }
        public Color ButtonNormalLowerSurfaceColor1 { get; set; }
        public Color ButtonNormalLowerSurfaceColor2 { get; set; }
        public Color ButtonHoverBorderColor1 { get; set; }
        public Color ButtonHoverBorderColor2 { get; set; }
        public Color ButtonHoverUpperSurfaceColor1 { get; set; }
        public Color ButtonHoverUpperSurfaceColor2 { get; set; }
        public Color ButtonHoverLowerSurfaceColor1 { get; set; }
        public Color ButtonHoverLowerSurfaceColor2 { get; set; }
        public Color ButtonPressedBorderColor1 { get; set; }
        public Color ButtonPressedBorderColor2 { get; set; }
        public Color ButtonPressedUpperSurfaceColor1 { get; set; }
        public Color ButtonPressedUpperSurfaceColor2 { get; set; }
        public Color ButtonPressedLowerSurfaceColor1 { get; set; }
        public Color ButtonPressedLowerSurfaceColor2 { get; set; }
        public Color ButtonShadowColor1 { get; set; }
        public Color ButtonShadowColor2 { get; set; }
        
        public int ButtonShadowWidth { get; set; }
        public int CornerRadius { get; set; }

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

                Color outerBorderColor1 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? OuterBorderColor1 : OuterBorderColor1;
                Color outerBorderColor2 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? OuterBorderColor2 : OuterBorderColor2;

                using (Brush outerBorderBrush = new LinearGradientBrush(borderRectangle, outerBorderColor1, outerBorderColor2, LinearGradientMode.Vertical))
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

                Color innerBorderColor1 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? InnerBorderColor1 : InnerBorderColor1;
                Color innerBorderColor2 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? InnerBorderColor2 : InnerBorderColor2;

                using (Brush borderBrush = new LinearGradientBrush(borderRectangle, innerBorderColor1, innerBorderColor2, LinearGradientMode.Vertical))
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

            //Draw button surface
            Color buttonUpperSurfaceColor1 = ButtonNormalUpperSurfaceColor1;
            Color buttonUpperSurfaceColor2 = ButtonNormalUpperSurfaceColor2;
            Color buttonLowerSurfaceColor1 = ButtonNormalLowerSurfaceColor1;
            Color buttonLowerSurfaceColor2 = ButtonNormalLowerSurfaceColor2;

            if (WebtroySwitch.IsButtonPressed)
            {
                buttonUpperSurfaceColor1 = ButtonPressedUpperSurfaceColor1;
                buttonUpperSurfaceColor2 = ButtonPressedUpperSurfaceColor2;
                buttonLowerSurfaceColor1 = ButtonPressedLowerSurfaceColor1;
                buttonLowerSurfaceColor2 = ButtonPressedLowerSurfaceColor2;
            }
            else if (WebtroySwitch.IsButtonHovered)
            {
                buttonUpperSurfaceColor1 = ButtonHoverUpperSurfaceColor1;
                buttonUpperSurfaceColor2 = ButtonHoverUpperSurfaceColor2;
                buttonLowerSurfaceColor1 = ButtonHoverLowerSurfaceColor1;
                buttonLowerSurfaceColor2 = ButtonHoverLowerSurfaceColor2;
            }

            if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
            {
                buttonUpperSurfaceColor1 = buttonUpperSurfaceColor1;
                buttonUpperSurfaceColor2 = buttonUpperSurfaceColor2;
                buttonLowerSurfaceColor1 = buttonLowerSurfaceColor1;
                buttonLowerSurfaceColor2 = buttonLowerSurfaceColor2;
            }

            buttonRectangle.Inflate(-1, -1);

            int upperHeight = buttonRectangle.Height/2;

            Rectangle upperGradientRect = new Rectangle(buttonRectangle.X, buttonRectangle.Y, buttonRectangle.Width, upperHeight);
            Rectangle lowerGradientRect = new Rectangle(buttonRectangle.X, buttonRectangle.Y + upperHeight, buttonRectangle.Width, buttonRectangle.Height - upperHeight);

            using (GraphicsPath buttonPath = GetRoundedRectanglePath(buttonRectangle, CornerRadius))
            {
                g.SetClip(buttonPath);
                g.IntersectClip(upperGradientRect);

                //Draw upper button surface gradient
                using (Brush buttonUpperSurfaceBrush = new LinearGradientBrush(buttonRectangle, buttonUpperSurfaceColor1, buttonUpperSurfaceColor2, LinearGradientMode.Vertical))
                {
                    g.FillPath(buttonUpperSurfaceBrush, buttonPath);
                }

                g.ResetClip();

                g.SetClip(buttonPath);
                g.IntersectClip(lowerGradientRect);

                //Draw lower button surface gradient
                using (Brush buttonLowerSurfaceBrush = new LinearGradientBrush(buttonRectangle, buttonLowerSurfaceColor1, buttonLowerSurfaceColor2, LinearGradientMode.Vertical))
                {
                    g.FillPath(buttonLowerSurfaceBrush, buttonPath);
                }

                g.ResetClip();

                g.SetClip(buttonPath);

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
            float buttonWidth = 1.61f*WebtroySwitch.Height;
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
