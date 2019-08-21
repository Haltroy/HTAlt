using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Haltroyify;

namespace Haltroyify
{
    public class WebtroySwitchIOS5Renderer : WebtroySwitchRendererBase
    {
        #region Constructor

        public WebtroySwitchIOS5Renderer()
        {
            BorderColor = Color.FromArgb(255, 202, 202, 202);
            LeftSideUpperColor1 = Color.FromArgb(255, 17, 123, 220);
            LeftSideUpperColor2 = Color.FromArgb(255, 17, 123, 220);
            LeftSideLowerColor1 = Color.FromArgb(255, 65, 143, 218);
            LeftSideLowerColor2 = Color.FromArgb(255, 65, 143, 218);
            LeftSideUpperBorderColor = Color.FromArgb(150, 123, 157, 196);
            LeftSideLowerBorderColor = Color.FromArgb(150, 123, 157, 196);
            RightSideUpperColor1 = Color.FromArgb(255, 229, 229, 229);
            RightSideUpperColor2 = Color.FromArgb(255, 229, 229, 229);
            RightSideLowerColor1 = Color.FromArgb(255, 232, 232, 232);
            RightSideLowerColor2 = Color.FromArgb(255, 232, 232, 232);
            RightSideUpperBorderColor = Color.FromArgb(150, 175, 175, 175);
            RightSideLowerBorderColor = Color.FromArgb(150, 175, 175, 175);
            ButtonShadowColor = Color.Transparent;
            ButtonNormalOuterBorderColor = Color.FromArgb(255, 149, 172, 194);
            ButtonNormalInnerBorderColor = Color.FromArgb(255, 235, 235, 235);
            ButtonNormalSurfaceColor1 = Color.FromArgb(255, 251, 250, 251);
            ButtonNormalSurfaceColor2 = Color.FromArgb(255, 251, 250, 251);
            ButtonHoverOuterBorderColor = Color.FromArgb(255, 223, 223, 223);
            ButtonHoverInnerBorderColor = Color.FromArgb(255, 223, 223, 223);
            ButtonHoverSurfaceColor1 = Color.FromArgb(255, 239, 238, 239);
            ButtonHoverSurfaceColor2 = Color.FromArgb(255, 239, 238, 239);
            ButtonPressedOuterBorderColor = Color.FromArgb(255, 176, 176, 176);
            ButtonPressedInnerBorderColor = Color.FromArgb(255, 176, 176, 176);
            ButtonPressedSurfaceColor1 = Color.FromArgb(255, 187, 187, 187);
            ButtonPressedSurfaceColor2 = Color.FromArgb(255, 187, 187, 187);
        }

        #endregion Constructor

        #region Public Properties

        public Color BorderColor { get; set; }
        public Color LeftSideUpperColor1 { get; set; }
        public Color LeftSideUpperColor2 { get; set; }
        public Color LeftSideLowerColor1 { get; set; }
        public Color LeftSideLowerColor2 { get; set; }
        public Color LeftSideUpperBorderColor { get; set; }
        public Color LeftSideLowerBorderColor { get; set; }
        public Color RightSideUpperColor1 { get; set; }
        public Color RightSideUpperColor2 { get; set; }
        public Color RightSideLowerColor1 { get; set; }
        public Color RightSideLowerColor2 { get; set; }
        public Color RightSideUpperBorderColor { get; set; }
        public Color RightSideLowerBorderColor { get; set; }
        public Color ButtonShadowColor { get; set; }
        public Color ButtonNormalOuterBorderColor { get; set; }
        public Color ButtonNormalInnerBorderColor { get; set; }
        public Color ButtonNormalSurfaceColor1 { get; set; }
        public Color ButtonNormalSurfaceColor2 { get; set; }
        public Color ButtonHoverOuterBorderColor { get; set; }
        public Color ButtonHoverInnerBorderColor { get; set; }
        public Color ButtonHoverSurfaceColor1 { get; set; }
        public Color ButtonHoverSurfaceColor2 { get; set; }
        public Color ButtonPressedOuterBorderColor { get; set; }
        public Color ButtonPressedInnerBorderColor { get; set; }
        public Color ButtonPressedSurfaceColor1 { get; set; }
        public Color ButtonPressedSurfaceColor2 { get; set; }

        #endregion Public Properties

        #region Render Method Implementations

        public override void RenderBorder(Graphics g, Rectangle borderRectangle)
        {
            //Draw this one AFTER the button is drawn in the RenderButton method
        }

        public override void RenderLeftToggleField(Graphics g, Rectangle leftRectangle, int totalToggleFieldWidth)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int buttonWidth = GetButtonWidth();

            //Draw upper gradient field
            int gradientRectWidth = leftRectangle.Width + buttonWidth / 2;
            int upperGradientRectHeight = (int)((double)0.8*(double)(leftRectangle.Height - 2));

            Rectangle controlRectangle = new Rectangle(0, 0, WebtroySwitch.Width, WebtroySwitch.Height);
            GraphicsPath controlClipPath = GetControlClipPath(controlRectangle);

            Rectangle upperGradientRectangle = new Rectangle(leftRectangle.X, leftRectangle.Y + 1, gradientRectWidth, upperGradientRectHeight - 1);

            g.SetClip(controlClipPath);
            g.IntersectClip(upperGradientRectangle);

            using (GraphicsPath upperGradientPath = new GraphicsPath())
            {
                upperGradientPath.AddArc(upperGradientRectangle.X, upperGradientRectangle.Y, WebtroySwitch.Height, WebtroySwitch.Height, 135, 135);
                upperGradientPath.AddLine(upperGradientRectangle.X, upperGradientRectangle.Y, upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y);
                upperGradientPath.AddLine(upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y, upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y + upperGradientRectangle.Height);
                upperGradientPath.AddLine(upperGradientRectangle.X, upperGradientRectangle.Y + upperGradientRectangle.Height, upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y + upperGradientRectangle.Height);

                Color upperColor1 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? LeftSideUpperColor1 : LeftSideUpperColor1;
                Color upperColor2 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? LeftSideUpperColor2 : LeftSideUpperColor2;

                using (Brush upperGradientBrush = new LinearGradientBrush(upperGradientRectangle, upperColor1, upperColor2, LinearGradientMode.Vertical))
                {
                    g.FillPath(upperGradientBrush, upperGradientPath);
                }
            }

            g.ResetClip();

            //Draw lower gradient field
            int lowerGradientRectHeight = (int)Math.Ceiling((double)0.5 * (double)(leftRectangle.Height - 2));

            Rectangle lowerGradientRectangle = new Rectangle(leftRectangle.X, leftRectangle.Y + (leftRectangle.Height / 2), gradientRectWidth, lowerGradientRectHeight);

            g.SetClip(controlClipPath);
            g.IntersectClip(lowerGradientRectangle);

            using (GraphicsPath lowerGradientPath = new GraphicsPath())
            {
                lowerGradientPath.AddArc(1, lowerGradientRectangle.Y, (int) (0.75*(WebtroySwitch.Height - 1)), WebtroySwitch.Height - 1, 215, 45); //Arc from side to top
                lowerGradientPath.AddLine(lowerGradientRectangle.X + buttonWidth/2, lowerGradientRectangle.Y, lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y);
                lowerGradientPath.AddLine(lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y, lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y + lowerGradientRectangle.Height);
                lowerGradientPath.AddLine(lowerGradientRectangle.X + buttonWidth/4, lowerGradientRectangle.Y + lowerGradientRectangle.Height, lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y + lowerGradientRectangle.Height);
                lowerGradientPath.AddArc(1, 1, WebtroySwitch.Height - 1, WebtroySwitch.Height - 1, 90, 70); //Arc from side to bottom

                Color lowerColor1 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? LeftSideLowerColor1 : LeftSideLowerColor1;
                Color lowerColor2 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? LeftSideLowerColor2 : LeftSideLowerColor2;

                using (Brush lowerGradientBrush = new LinearGradientBrush(lowerGradientRectangle, lowerColor1, lowerColor2, LinearGradientMode.Vertical))
                {
                    g.FillPath(lowerGradientBrush, lowerGradientPath);
                }
            }

            g.ResetClip();

            controlRectangle = new Rectangle(0, 0, WebtroySwitch.Width, WebtroySwitch.Height);
            controlClipPath = GetControlClipPath(controlRectangle);

            g.SetClip(controlClipPath);

            //Draw upper inside border
            Color upperBordercolor = LeftSideUpperBorderColor;

            if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                upperBordercolor = upperBordercolor;

            using (Pen upperBorderPen = new Pen(upperBordercolor))
            {
                g.DrawLine(upperBorderPen, leftRectangle.X, leftRectangle.Y + 1, leftRectangle.X + leftRectangle.Width + (buttonWidth / 2), leftRectangle.Y + 1);
            }

            //Draw lower inside border
            Color lowerBordercolor = LeftSideLowerBorderColor;

            if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                lowerBordercolor = lowerBordercolor;

            using (Pen lowerBorderPen = new Pen(lowerBordercolor))
            {
                g.DrawLine(lowerBorderPen, leftRectangle.X, leftRectangle.Y + leftRectangle.Height - 1, leftRectangle.X + leftRectangle.Width + (buttonWidth / 2), leftRectangle.Y + leftRectangle.Height - 1);
            }

            //Draw image or text
            if (WebtroySwitch.OnSideImage != null || !string.IsNullOrEmpty(WebtroySwitch.OnText))
            {
                RectangleF fullRectangle = new RectangleF(leftRectangle.X + 2 - (totalToggleFieldWidth - leftRectangle.Width), 2, totalToggleFieldWidth - 2, WebtroySwitch.Height - 4);

                g.IntersectClip(fullRectangle);

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
            }

            g.ResetClip();
        }

        public override void RenderRightToggleField(Graphics g, Rectangle rightRectangle, int totalToggleFieldWidth)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle buttonRectangle = GetButtonRectangle();

            Rectangle controlRectangle = new Rectangle(0, 0, WebtroySwitch.Width, WebtroySwitch.Height);
            GraphicsPath controlClipPath = GetControlClipPath(controlRectangle);

            //Draw upper gradient field
            int gradientRectWidth = rightRectangle.Width + buttonRectangle.Width / 2;
            int upperGradientRectHeight = (int)((double)0.8 * (double)(rightRectangle.Height - 2));
            
            Rectangle upperGradientRectangle = new Rectangle(rightRectangle.X - buttonRectangle.Width / 2, rightRectangle.Y + 1, gradientRectWidth - 1, upperGradientRectHeight - 1);

            g.SetClip(controlClipPath);
            g.IntersectClip(upperGradientRectangle);

            using (GraphicsPath upperGradientPath = new GraphicsPath())
            {
                upperGradientPath.AddLine(upperGradientRectangle.X, upperGradientRectangle.Y, upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y);
                upperGradientPath.AddArc(upperGradientRectangle.X + upperGradientRectangle.Width - WebtroySwitch.Height + 1, upperGradientRectangle.Y - 1, WebtroySwitch.Height, WebtroySwitch.Height, 270, 115);
                upperGradientPath.AddLine(upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y + upperGradientRectangle.Height, upperGradientRectangle.X, upperGradientRectangle.Y + upperGradientRectangle.Height);
                upperGradientPath.AddLine(upperGradientRectangle.X, upperGradientRectangle.Y + upperGradientRectangle.Height, upperGradientRectangle.X, upperGradientRectangle.Y);

                Color upperColor1 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? RightSideUpperColor1 : RightSideUpperColor1;
                Color upperColor2 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? RightSideUpperColor2 : RightSideUpperColor2;

                using (Brush upperGradientBrush = new LinearGradientBrush(upperGradientRectangle, upperColor1, upperColor2, LinearGradientMode.Vertical))
                {
                    g.FillPath(upperGradientBrush, upperGradientPath);
                }
            }

            g.ResetClip();

            //Draw lower gradient field
            int lowerGradientRectHeight = (int)Math.Ceiling((double)0.5 * (double)(rightRectangle.Height - 2));
            
            Rectangle lowerGradientRectangle = new Rectangle(rightRectangle.X - buttonRectangle.Width / 2, rightRectangle.Y + (rightRectangle.Height / 2), gradientRectWidth - 1, lowerGradientRectHeight);

            g.SetClip(controlClipPath);
            g.IntersectClip(lowerGradientRectangle);

            using (GraphicsPath lowerGradientPath = new GraphicsPath())
            {
                lowerGradientPath.AddLine(lowerGradientRectangle.X, lowerGradientRectangle.Y, lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y);
                lowerGradientPath.AddArc(lowerGradientRectangle.X + lowerGradientRectangle.Width - (int)(0.75 * (WebtroySwitch.Height - 1)), lowerGradientRectangle.Y, (int)(0.75 * (WebtroySwitch.Height - 1)), WebtroySwitch.Height - 1, 270, 45);  //Arc from top to side
                lowerGradientPath.AddArc(WebtroySwitch.Width - WebtroySwitch.Height, 0, WebtroySwitch.Height, WebtroySwitch.Height, 20, 70); //Arc from side to bottom
                lowerGradientPath.AddLine(lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y + lowerGradientRectangle.Height, lowerGradientRectangle.X, lowerGradientRectangle.Y + lowerGradientRectangle.Height);
                lowerGradientPath.AddLine(lowerGradientRectangle.X, lowerGradientRectangle.Y + lowerGradientRectangle.Height, lowerGradientRectangle.X, lowerGradientRectangle.Y);

                Color lowerColor1 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? RightSideLowerColor1 : RightSideLowerColor1;
                Color lowerColor2 = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? RightSideLowerColor2 : RightSideLowerColor2;

                using (Brush lowerGradientBrush = new LinearGradientBrush(lowerGradientRectangle, lowerColor1, lowerColor2, LinearGradientMode.Vertical))
                {
                    g.FillPath(lowerGradientBrush, lowerGradientPath);
                }
            }

            g.ResetClip();

            controlRectangle = new Rectangle(0, 0, WebtroySwitch.Width, WebtroySwitch.Height);
            controlClipPath = GetControlClipPath(controlRectangle);

            g.SetClip(controlClipPath);

            //Draw upper inside border
            Color upperBordercolor = RightSideUpperBorderColor;

            if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                upperBordercolor = upperBordercolor;

            using (Pen upperBorderPen = new Pen(upperBordercolor))
            {
                g.DrawLine(upperBorderPen, rightRectangle.X - (buttonRectangle.Width / 2), rightRectangle.Y + 1, rightRectangle.X + rightRectangle.Width, rightRectangle.Y + 1);
            }

            //Draw lower inside border
            Color lowerBordercolor = RightSideLowerBorderColor;

            if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                lowerBordercolor = lowerBordercolor;

            using (Pen lowerBorderPen = new Pen(lowerBordercolor))
            {
                g.DrawLine(lowerBorderPen, rightRectangle.X - (buttonRectangle.Width / 2), rightRectangle.Y + rightRectangle.Height - 1, rightRectangle.X + rightRectangle.Width, rightRectangle.Y + rightRectangle.Height - 1);
            }

            //Draw image or text
            if (WebtroySwitch.OffSideImage != null || !string.IsNullOrEmpty(WebtroySwitch.OffText))
            {
                RectangleF fullRectangle = new RectangleF(rightRectangle.X, 2, totalToggleFieldWidth - 2, WebtroySwitch.Height - 4);

                g.IntersectClip(fullRectangle);

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
            }

            g.ResetClip();
        }

        public override void RenderButton(Graphics g, Rectangle buttonRectangle)
        {
            if (WebtroySwitch.IsButtonOnLeftSide)
                buttonRectangle.X += 1;
            else if (WebtroySwitch.IsButtonOnRightSide)
                buttonRectangle.X -= 1;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //Draw button shadow
            buttonRectangle.Inflate(1, 1);

            Rectangle shadowClipRectangle = new Rectangle(buttonRectangle.Location, buttonRectangle.Size);
            shadowClipRectangle.Inflate(0, -1);

            if (WebtroySwitch.IsButtonOnLeftSide)
            {
                shadowClipRectangle.X += shadowClipRectangle.Width / 2;
                shadowClipRectangle.Width = shadowClipRectangle.Width / 2;
            }
            else if (WebtroySwitch.IsButtonOnRightSide)
            {
                shadowClipRectangle.Width = shadowClipRectangle.Width / 2;
            }

            g.SetClip(shadowClipRectangle);

            Color buttonShadowColor = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? ButtonShadowColor : ButtonShadowColor;

            using (Pen buttonShadowPen = new Pen(buttonShadowColor))
            {
                g.DrawEllipse(buttonShadowPen, buttonRectangle);
            }

            g.ResetClip();

            buttonRectangle.Inflate(-1, -1);

            //Draw outer button border
            Color buttonOuterBorderColor = ButtonNormalOuterBorderColor;

            if (WebtroySwitch.IsButtonPressed)
                buttonOuterBorderColor = ButtonPressedOuterBorderColor;
            else if (WebtroySwitch.IsButtonHovered)
                buttonOuterBorderColor = ButtonHoverOuterBorderColor;

            if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                buttonOuterBorderColor = buttonOuterBorderColor;

            using (Brush outerBorderBrush = new SolidBrush(buttonOuterBorderColor))
            {
                g.FillEllipse(outerBorderBrush, buttonRectangle);
            }

            //Draw inner button border
            buttonRectangle.Inflate(-1, -1);

            Color buttonInnerBorderColor = ButtonNormalInnerBorderColor;

            if (WebtroySwitch.IsButtonPressed)
                buttonInnerBorderColor = ButtonPressedInnerBorderColor;
            else if (WebtroySwitch.IsButtonHovered)
                buttonInnerBorderColor = ButtonHoverInnerBorderColor;

            if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                buttonInnerBorderColor = buttonInnerBorderColor;

            using (Brush innerBorderBrush = new SolidBrush(buttonInnerBorderColor))
            {
                g.FillEllipse(innerBorderBrush, buttonRectangle);
            }

            //Draw button surface
            buttonRectangle.Inflate(-1, -1);

            Color buttonUpperSurfaceColor = ButtonNormalSurfaceColor1;

            if (WebtroySwitch.IsButtonPressed)
                buttonUpperSurfaceColor = ButtonPressedSurfaceColor1;
            else if (WebtroySwitch.IsButtonHovered)
                buttonUpperSurfaceColor = ButtonHoverSurfaceColor1;

            if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                buttonUpperSurfaceColor = buttonUpperSurfaceColor;

            Color buttonLowerSurfaceColor = ButtonNormalSurfaceColor2;

            if (WebtroySwitch.IsButtonPressed)
                buttonLowerSurfaceColor = ButtonPressedSurfaceColor2;
            else if (WebtroySwitch.IsButtonHovered)
                buttonLowerSurfaceColor = ButtonHoverSurfaceColor2;

            if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                buttonLowerSurfaceColor = buttonLowerSurfaceColor;

            using (Brush buttonSurfaceBrush = new LinearGradientBrush(buttonRectangle, buttonUpperSurfaceColor, buttonLowerSurfaceColor, LinearGradientMode.Vertical))
            {
                g.FillEllipse(buttonSurfaceBrush, buttonRectangle);
            }

            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;

            //Draw outer control border
            Rectangle controlRectangle = new Rectangle(0, 0, WebtroySwitch.Width, WebtroySwitch.Height);

            using (GraphicsPath borderPath = GetControlClipPath(controlRectangle))
            {
                Color controlBorderColor = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? BorderColor : BorderColor;

                using (Pen borderPen = new Pen(controlBorderColor))
                {
                    g.DrawPath(borderPen, borderPath);
                }
            }

            g.ResetClip();

            //Draw button image
            Image buttonImage = WebtroySwitch.ButtonImage ?? (WebtroySwitch.Checked ? WebtroySwitch.OnButtonImage : WebtroySwitch.OffButtonImage);

            if (buttonImage != null)
            {
                g.SetClip(GetButtonClipPath());

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
                        g.DrawImage(buttonImage, imageRectangle, 0,0, buttonImage.Width, buttonImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
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

        #endregion Render Method Implementations

        #region Helper Method Implementations

        public GraphicsPath GetControlClipPath(Rectangle controlRectangle)
        {
            GraphicsPath borderPath = new GraphicsPath();
            borderPath.AddArc(controlRectangle.X, controlRectangle.Y, controlRectangle.Height, controlRectangle.Height, 90, 180);
            borderPath.AddArc(controlRectangle.Width - controlRectangle.Height, controlRectangle.Y, controlRectangle.Height, controlRectangle.Height, 270, 180);
            borderPath.CloseFigure();

            return borderPath;
        }

        public GraphicsPath GetButtonClipPath()
        {
            Rectangle buttonRectangle = GetButtonRectangle();

            GraphicsPath buttonPath = new GraphicsPath();

            buttonPath.AddArc(buttonRectangle.X, buttonRectangle.Y, buttonRectangle.Height, buttonRectangle.Height, 0, 360);

            return buttonPath;
        }

        public override int GetButtonWidth()
        {
            return WebtroySwitch.Height - 2;
        }

        public override Rectangle GetButtonRectangle()
        {
            int buttonWidth = GetButtonWidth();
            return GetButtonRectangle(buttonWidth);
        }

        public override Rectangle GetButtonRectangle(int buttonWidth)
        {
            Rectangle buttonRect = new Rectangle(WebtroySwitch.ButtonValue, 1, buttonWidth, buttonWidth);
            return buttonRect;
        }
        
        #endregion Helper Method Implementations
    }
}
