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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace HaltroyFramework
{
    public class HaltroySwitchRenderer : HaltroySwitchRendererBase
    {
        #region Constructor

        public HaltroySwitchRenderer()
        {
        }

        #endregion Constructor

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
            int upperGradientRectHeight = (int)(0.8 * (leftRectangle.Height - 2));

            Rectangle controlRectangle = new Rectangle(0, 0, HaltroySwitch.Width, HaltroySwitch.Height);
            GraphicsPath controlClipPath = GetControlClipPath(controlRectangle);

            Rectangle upperGradientRectangle = new Rectangle(leftRectangle.X, leftRectangle.Y + 1, gradientRectWidth, upperGradientRectHeight - 1);

            g.SetClip(controlClipPath);
            g.IntersectClip(upperGradientRectangle);

            using (GraphicsPath upperGradientPath = new GraphicsPath())
            {
                upperGradientPath.AddArc(upperGradientRectangle.X, upperGradientRectangle.Y, HaltroySwitch.Height, HaltroySwitch.Height, 135, 135);
                upperGradientPath.AddLine(upperGradientRectangle.X, upperGradientRectangle.Y, upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y);
                upperGradientPath.AddLine(upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y, upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y + upperGradientRectangle.Height);
                upperGradientPath.AddLine(upperGradientRectangle.X, upperGradientRectangle.Y + upperGradientRectangle.Height, upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y + upperGradientRectangle.Height);

                Color upperColor1 = (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled) ? HaltroySwitch.OverlayColor : HaltroySwitch.OverlayColor;

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
                lowerGradientPath.AddArc(1, lowerGradientRectangle.Y, (int)(0.75 * (HaltroySwitch.Height - 1)), HaltroySwitch.Height - 1, 215, 45); //Arc from side to top
                lowerGradientPath.AddLine(lowerGradientRectangle.X + buttonWidth / 2, lowerGradientRectangle.Y, lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y);
                lowerGradientPath.AddLine(lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y, lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y + lowerGradientRectangle.Height);
                lowerGradientPath.AddLine(lowerGradientRectangle.X + buttonWidth / 4, lowerGradientRectangle.Y + lowerGradientRectangle.Height, lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y + lowerGradientRectangle.Height);
                lowerGradientPath.AddArc(1, 1, HaltroySwitch.Height - 1, HaltroySwitch.Height - 1, 90, 70); //Arc from side to bottom

                Color lowerColor1 = (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled) ? HaltroySwitch.OverlayColor : HaltroySwitch.OverlayColor;

                using (Brush lowerGradientBrush = new LinearGradientBrush(lowerGradientRectangle, lowerColor1, lowerColor1, LinearGradientMode.Vertical))
                {
                    g.FillPath(lowerGradientBrush, lowerGradientPath);
                }
            }

            g.ResetClip();

            controlRectangle = new Rectangle(0, 0, HaltroySwitch.Width, HaltroySwitch.Height);
            controlClipPath = GetControlClipPath(controlRectangle);

            g.SetClip(controlClipPath);

            //Draw upper inside border
            Color upperBorderColor = HaltroySwitch.BorderColor;

            //  if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
            //    upperHaltroySwitch.BorderColor = upperHaltroySwitch.BorderColor;

            using (Pen upperBorderPen = new Pen(upperBorderColor))
            {
                g.DrawLine(upperBorderPen, leftRectangle.X, leftRectangle.Y + 1, leftRectangle.X + leftRectangle.Width + (buttonWidth / 2), leftRectangle.Y + 1);
            }

            //Draw lower inside border
            Color lowerBorderColor = HaltroySwitch.BorderColor;

            //    if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
            //      lowerHaltroySwitch.BorderColor = lowerHaltroySwitch.BorderColor;

            using (Pen lowerBorderPen = new Pen(lowerBorderColor))
            {
                g.DrawLine(lowerBorderPen, leftRectangle.X, leftRectangle.Y + leftRectangle.Height - 1, leftRectangle.X + leftRectangle.Width + (buttonWidth / 2), leftRectangle.Y + leftRectangle.Height - 1);
            }

            //Draw image or text
            if (HaltroySwitch.OnSideImage != null || !string.IsNullOrEmpty(HaltroySwitch.OnText))
            {
                RectangleF fullRectangle = new RectangleF(leftRectangle.X + 2 - (totalToggleFieldWidth - leftRectangle.Width), 2, totalToggleFieldWidth - 2, HaltroySwitch.Height - 4);

                g.IntersectClip(fullRectangle);

                if (HaltroySwitch.OnSideImage != null)
                {
                    Size imageSize = HaltroySwitch.OnSideImage.Size;
                    Rectangle imageRectangle;

                    int imageXPos = (int)fullRectangle.X;

                    if (HaltroySwitch.OnSideScaleImageToFit)
                    {
                        Size canvasSize = new Size((int)fullRectangle.Width, (int)fullRectangle.Height);
                        Size resizedImageSize = ImageHelper.RescaleImageToFit(imageSize, canvasSize);

                        if (HaltroySwitch.OnSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Center)
                        {
                            imageXPos = (int)(fullRectangle.X + ((fullRectangle.Width - resizedImageSize.Width) / 2));
                        }
                        else if (HaltroySwitch.OnSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Near)
                        {
                            imageXPos = (int)(fullRectangle.X + fullRectangle.Width - resizedImageSize.Width);
                        }

                        imageRectangle = new Rectangle(imageXPos, (int)(fullRectangle.Y + ((fullRectangle.Height - resizedImageSize.Height) / 2)), resizedImageSize.Width, resizedImageSize.Height);

                        if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
                        {
                            g.DrawImage(HaltroySwitch.OnSideImage, imageRectangle, 0, 0, HaltroySwitch.OnSideImage.Width, HaltroySwitch.OnSideImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                        }
                        else
                        {
                            g.DrawImage(HaltroySwitch.OnSideImage, imageRectangle);
                        }
                    }
                    else
                    {
                        if (HaltroySwitch.OnSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Center)
                        {
                            imageXPos = (int)(fullRectangle.X + ((fullRectangle.Width - imageSize.Width) / 2));
                        }
                        else if (HaltroySwitch.OnSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Near)
                        {
                            imageXPos = (int)(fullRectangle.X + fullRectangle.Width - imageSize.Width);
                        }

                        imageRectangle = new Rectangle(imageXPos, (int)(fullRectangle.Y + ((fullRectangle.Height - imageSize.Height) / 2)), imageSize.Width, imageSize.Height);

                        if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
                        {
                            g.DrawImage(HaltroySwitch.OnSideImage, imageRectangle, 0, 0, HaltroySwitch.OnSideImage.Width, HaltroySwitch.OnSideImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                        }
                        else
                        {
                            g.DrawImageUnscaled(HaltroySwitch.OnSideImage, imageRectangle);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(HaltroySwitch.OnText))
                {
                    SizeF textSize = g.MeasureString(HaltroySwitch.OnText, HaltroySwitch.OnFont);

                    float textXPos = fullRectangle.X;

                    if (HaltroySwitch.OnSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Center)
                    {
                        textXPos = fullRectangle.X + ((fullRectangle.Width - textSize.Width) / 2);
                    }
                    else if (HaltroySwitch.OnSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Near)
                    {
                        textXPos = fullRectangle.X + fullRectangle.Width - textSize.Width;
                    }

                    RectangleF textRectangle = new RectangleF(textXPos, fullRectangle.Y + ((fullRectangle.Height - textSize.Height) / 2), textSize.Width, textSize.Height);

                    Color textForeColor = HaltroySwitch.OnForeColor;

                    //if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
                    //  textForeColor = textForeColor;

                    using (Brush textBrush = new SolidBrush(textForeColor))
                    {
                        g.DrawString(HaltroySwitch.OnText, HaltroySwitch.OnFont, textBrush, textRectangle);
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

            Rectangle controlRectangle = new Rectangle(0, 0, HaltroySwitch.Width, HaltroySwitch.Height);
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
                upperGradientPath.AddArc(upperGradientRectangle.X + upperGradientRectangle.Width - HaltroySwitch.Height + 1, upperGradientRectangle.Y - 1, HaltroySwitch.Height, HaltroySwitch.Height, 270, 115);
                upperGradientPath.AddLine(upperGradientRectangle.X + upperGradientRectangle.Width, upperGradientRectangle.Y + upperGradientRectangle.Height, upperGradientRectangle.X, upperGradientRectangle.Y + upperGradientRectangle.Height);
                upperGradientPath.AddLine(upperGradientRectangle.X, upperGradientRectangle.Y + upperGradientRectangle.Height, upperGradientRectangle.X, upperGradientRectangle.Y);

                Color upperColor1 = (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled) ? HaltroySwitch.BackColor : HaltroySwitch.BackColor;

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
                lowerGradientPath.AddArc(lowerGradientRectangle.X + lowerGradientRectangle.Width - (int)(0.75 * (HaltroySwitch.Height - 1)), lowerGradientRectangle.Y, (int)(0.75 * (HaltroySwitch.Height - 1)), HaltroySwitch.Height - 1, 270, 45);  //Arc from top to side
                lowerGradientPath.AddArc(HaltroySwitch.Width - HaltroySwitch.Height, 0, HaltroySwitch.Height, HaltroySwitch.Height, 20, 70); //Arc from side to bottom
                lowerGradientPath.AddLine(lowerGradientRectangle.X + lowerGradientRectangle.Width, lowerGradientRectangle.Y + lowerGradientRectangle.Height, lowerGradientRectangle.X, lowerGradientRectangle.Y + lowerGradientRectangle.Height);
                lowerGradientPath.AddLine(lowerGradientRectangle.X, lowerGradientRectangle.Y + lowerGradientRectangle.Height, lowerGradientRectangle.X, lowerGradientRectangle.Y);

                Color lowerColor1 = (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled) ? HaltroySwitch.BackColor : HaltroySwitch.BackColor;

                using (Brush lowerGradientBrush = new LinearGradientBrush(lowerGradientRectangle, lowerColor1, lowerColor1, LinearGradientMode.Vertical))
                {
                    g.FillPath(lowerGradientBrush, lowerGradientPath);
                }
            }

            g.ResetClip();

            controlRectangle = new Rectangle(0, 0, HaltroySwitch.Width, HaltroySwitch.Height);
            controlClipPath = GetControlClipPath(controlRectangle);

            g.SetClip(controlClipPath);

            //Draw upper inside border
            Color upperBorderColor = HaltroySwitch.BorderColor;

            // if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
            //upperHaltroySwitch.BorderColor = upperHaltroySwitch.BorderColor;

            using (Pen upperBorderPen = new Pen(upperBorderColor))
            {
                g.DrawLine(upperBorderPen, rightRectangle.X - (buttonRectangle.Width / 2), rightRectangle.Y + 1, rightRectangle.X + rightRectangle.Width, rightRectangle.Y + 1);
            }

            //Draw lower inside border
            Color lowerBorderColor = HaltroySwitch.BorderColor;

            //  if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
            //    lowerHaltroySwitch.BorderColor = lowerHaltroySwitch.BorderColor;

            using (Pen lowerBorderPen = new Pen(lowerBorderColor))
            {
                g.DrawLine(lowerBorderPen, rightRectangle.X - (buttonRectangle.Width / 2), rightRectangle.Y + rightRectangle.Height - 1, rightRectangle.X + rightRectangle.Width, rightRectangle.Y + rightRectangle.Height - 1);
            }

            //Draw image or text
            if (HaltroySwitch.OffSideImage != null || !string.IsNullOrEmpty(HaltroySwitch.OffText))
            {
                RectangleF fullRectangle = new RectangleF(rightRectangle.X, 2, totalToggleFieldWidth - 2, HaltroySwitch.Height - 4);

                g.IntersectClip(fullRectangle);

                if (HaltroySwitch.OffSideImage != null)
                {
                    Size imageSize = HaltroySwitch.OffSideImage.Size;
                    Rectangle imageRectangle;

                    int imageXPos = (int)fullRectangle.X;

                    if (HaltroySwitch.OffSideScaleImageToFit)
                    {
                        Size canvasSize = new Size((int)fullRectangle.Width, (int)fullRectangle.Height);
                        Size resizedImageSize = ImageHelper.RescaleImageToFit(imageSize, canvasSize);

                        if (HaltroySwitch.OffSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Center)
                        {
                            imageXPos = (int)(fullRectangle.X + ((fullRectangle.Width - resizedImageSize.Width) / 2));
                        }
                        else if (HaltroySwitch.OffSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Far)
                        {
                            imageXPos = (int)(fullRectangle.X + fullRectangle.Width - resizedImageSize.Width);
                        }

                        imageRectangle = new Rectangle(imageXPos, (int)(fullRectangle.Y + ((fullRectangle.Height - resizedImageSize.Height) / 2)), resizedImageSize.Width, resizedImageSize.Height);

                        if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
                        {
                            g.DrawImage(HaltroySwitch.OnSideImage, imageRectangle, 0, 0, HaltroySwitch.OnSideImage.Width, HaltroySwitch.OnSideImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                        }
                        else
                        {
                            g.DrawImage(HaltroySwitch.OnSideImage, imageRectangle);
                        }
                    }
                    else
                    {
                        if (HaltroySwitch.OffSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Center)
                        {
                            imageXPos = (int)(fullRectangle.X + ((fullRectangle.Width - imageSize.Width) / 2));
                        }
                        else if (HaltroySwitch.OffSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Far)
                        {
                            imageXPos = (int)(fullRectangle.X + fullRectangle.Width - imageSize.Width);
                        }

                        imageRectangle = new Rectangle(imageXPos, (int)(fullRectangle.Y + ((fullRectangle.Height - imageSize.Height) / 2)), imageSize.Width, imageSize.Height);

                        if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
                        {
                            g.DrawImage(HaltroySwitch.OnSideImage, imageRectangle, 0, 0, HaltroySwitch.OnSideImage.Width, HaltroySwitch.OnSideImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                        }
                        else
                        {
                            g.DrawImageUnscaled(HaltroySwitch.OffSideImage, imageRectangle);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(HaltroySwitch.OffText))
                {
                    SizeF textSize = g.MeasureString(HaltroySwitch.OffText, HaltroySwitch.OffFont);

                    float textXPos = fullRectangle.X;

                    if (HaltroySwitch.OffSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Center)
                    {
                        textXPos = fullRectangle.X + ((fullRectangle.Width - textSize.Width) / 2);
                    }
                    else if (HaltroySwitch.OffSideAlignment == HaltroySwitch.HaltroySwitchAlignment.Far)
                    {
                        textXPos = fullRectangle.X + fullRectangle.Width - textSize.Width;
                    }

                    RectangleF textRectangle = new RectangleF(textXPos, fullRectangle.Y + ((fullRectangle.Height - textSize.Height) / 2), textSize.Width, textSize.Height);

                    Color textForeColor = HaltroySwitch.OffForeColor;

                    //    if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
                    //      textForeColor = textForeColor;

                    using (Brush textBrush = new SolidBrush(textForeColor))
                    {
                        g.DrawString(HaltroySwitch.OffText, HaltroySwitch.OffFont, textBrush, textRectangle);
                    }
                }
            }

            g.ResetClip();
        }

        public override void RenderButton(Graphics g, Rectangle buttonRectangle)
        {
            if (HaltroySwitch.IsButtonOnLeftSide)
            {
                buttonRectangle.X += 1;
            }
            else if (HaltroySwitch.IsButtonOnRightSide)
            {
                buttonRectangle.X -= 1;
            }

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //Draw button shadow
            buttonRectangle.Inflate(1, 1);

            Rectangle shadowClipRectangle = new Rectangle(buttonRectangle.Location, buttonRectangle.Size);
            shadowClipRectangle.Inflate(0, -1);

            if (HaltroySwitch.IsButtonOnLeftSide)
            {
                shadowClipRectangle.X += shadowClipRectangle.Width / 2;
                shadowClipRectangle.Width = shadowClipRectangle.Width / 2;
            }
            else if (HaltroySwitch.IsButtonOnRightSide)
            {
                shadowClipRectangle.Width = shadowClipRectangle.Width / 2;
            }

            g.SetClip(shadowClipRectangle);

            g.ResetClip();

            buttonRectangle.Inflate(-1, -1);

            //Draw outer button border
            Color buttonOuterBorderColor = HaltroySwitch.BorderColor;

            if (HaltroySwitch.IsButtonPressed)
            {
                buttonOuterBorderColor = HaltroySwitch.BorderColor;
            }
            else if (HaltroySwitch.IsButtonHovered)
            {
                buttonOuterBorderColor = HaltroySwitch.BorderColor;
            }

            // if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
            //    buttonOuterHaltroySwitch.BorderColor = buttonOuterHaltroySwitch.BorderColor;

            using (Brush outerBorderBrush = new SolidBrush(buttonOuterBorderColor))
            {
                g.FillEllipse(outerBorderBrush, buttonRectangle);
            }

            //Draw inner button border
            buttonRectangle.Inflate(-1, -1);

            Color buttonInnerBorderColor = HaltroySwitch.BorderColor;

            if (HaltroySwitch.IsButtonPressed)
            {
                buttonInnerBorderColor = HaltroySwitch.BorderColor;
            }
            else if (HaltroySwitch.IsButtonHovered)
            {
                buttonInnerBorderColor = HaltroySwitch.BorderColor;
            }

            //   if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
            //     buttonInnerHaltroySwitch.BorderColor = buttonInnerHaltroySwitch.BorderColor;

            using (Brush innerBorderBrush = new SolidBrush(buttonInnerBorderColor))
            {
                g.FillEllipse(innerBorderBrush, buttonRectangle);
            }

            //Draw button surface
            buttonRectangle.Inflate(-1, -1);

            Color buttonUpperSurfaceColor = HaltroySwitch.ButtonColor;

            if (HaltroySwitch.IsButtonPressed)
            {
                buttonUpperSurfaceColor = HaltroySwitch.ButtonPressedColor;
            }
            else if (HaltroySwitch.IsButtonHovered)
            {
                buttonUpperSurfaceColor = HaltroySwitch.ButtonHoverColor;
            }

            //      if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
            //        buttonUpperSurfaceColor = buttonUpperSurfaceColor;

            Color buttonLowerSurfaceColor = HaltroySwitch.ButtonColor;

            if (HaltroySwitch.IsButtonPressed)
            {
                buttonLowerSurfaceColor = HaltroySwitch.ButtonPressedColor;
            }
            else if (HaltroySwitch.IsButtonHovered)
            {
                buttonLowerSurfaceColor = HaltroySwitch.ButtonHoverColor;
            }

            //     if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
            //       buttonLowerSurfaceColor = buttonLowerSurfaceColor;

            using (Brush buttonSurfaceBrush = new LinearGradientBrush(buttonRectangle, buttonUpperSurfaceColor, buttonLowerSurfaceColor, LinearGradientMode.Vertical))
            {
                g.FillEllipse(buttonSurfaceBrush, buttonRectangle);
            }

            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;

            //Draw outer control border
            Rectangle controlRectangle = new Rectangle(0, 0, HaltroySwitch.Width, HaltroySwitch.Height);

            using (GraphicsPath borderPath = GetControlClipPath(controlRectangle))
            {
                Color controlBorderColor = (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled) ? HaltroySwitch.BorderColor : HaltroySwitch.BorderColor;

                using (Pen borderPen = new Pen(controlBorderColor))
                {
                    g.DrawPath(borderPen, borderPath);
                }
            }

            g.ResetClip();

            //Draw button image
            Image buttonImage = HaltroySwitch.ButtonImage ?? (HaltroySwitch.Checked ? HaltroySwitch.OnButtonImage : HaltroySwitch.OffButtonImage);

            if (buttonImage != null)
            {
                g.SetClip(GetButtonClipPath());

                HaltroySwitch.HaltroySwitchButtonAlignment alignment = HaltroySwitch.ButtonImage != null ? HaltroySwitch.ButtonAlignment : (HaltroySwitch.Checked ? HaltroySwitch.OnButtonAlignment : HaltroySwitch.OffButtonAlignment);

                Size imageSize = buttonImage.Size;

                Rectangle imageRectangle;

                int imageXPos = buttonRectangle.X;

                bool scaleImage = HaltroySwitch.ButtonImage != null ? HaltroySwitch.ButtonScaleImageToFit : (HaltroySwitch.Checked ? HaltroySwitch.OnButtonScaleImageToFit : HaltroySwitch.OffButtonScaleImageToFit);

                if (scaleImage)
                {
                    Size canvasSize = buttonRectangle.Size;
                    Size resizedImageSize = ImageHelper.RescaleImageToFit(imageSize, canvasSize);

                    if (alignment == HaltroySwitch.HaltroySwitchButtonAlignment.Center)
                    {
                        imageXPos = (int)(buttonRectangle.X + ((buttonRectangle.Width - (float)resizedImageSize.Width) / 2));
                    }
                    else if (alignment == HaltroySwitch.HaltroySwitchButtonAlignment.Right)
                    {
                        imageXPos = (int)(buttonRectangle.X + (float)buttonRectangle.Width - resizedImageSize.Width);
                    }

                    imageRectangle = new Rectangle(imageXPos, (int)(buttonRectangle.Y + ((buttonRectangle.Height - (float)resizedImageSize.Height) / 2)), resizedImageSize.Width, resizedImageSize.Height);

                    if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
                    {
                        g.DrawImage(buttonImage, imageRectangle, 0, 0, buttonImage.Width, buttonImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                    }
                    else
                    {
                        g.DrawImage(buttonImage, imageRectangle);
                    }
                }
                else
                {
                    if (alignment == HaltroySwitch.HaltroySwitchButtonAlignment.Center)
                    {
                        imageXPos = (int)(buttonRectangle.X + ((buttonRectangle.Width - (float)imageSize.Width) / 2));
                    }
                    else if (alignment == HaltroySwitch.HaltroySwitchButtonAlignment.Right)
                    {
                        imageXPos = (int)(buttonRectangle.X + (float)buttonRectangle.Width - imageSize.Width);
                    }

                    imageRectangle = new Rectangle(imageXPos, (int)(buttonRectangle.Y + ((buttonRectangle.Height - (float)imageSize.Height) / 2)), imageSize.Width, imageSize.Height);

                    if (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled)
                    {
                        g.DrawImage(buttonImage, imageRectangle, 0, 0, buttonImage.Width, buttonImage.Height, GraphicsUnit.Pixel, ImageHelper.GetGrayscaleAttributes());
                    }
                    else
                    {
                        g.DrawImageUnscaled(buttonImage, imageRectangle);
                    }
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
            return HaltroySwitch.Height - 2;
        }

        public override Rectangle GetButtonRectangle()
        {
            int buttonWidth = GetButtonWidth();
            return GetButtonRectangle(buttonWidth);
        }

        public override Rectangle GetButtonRectangle(int buttonWidth)
        {
            Rectangle buttonRect = new Rectangle(HaltroySwitch.ButtonValue, 1, buttonWidth, buttonWidth);
            return buttonRect;
        }

        #endregion Helper Method Implementations
    }
}