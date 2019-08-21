using System.Drawing;
using Haltroyify;

namespace Haltroyify
{
    public class WebtroySwitchMetroRenderer : WebtroySwitchRendererBase
    {
        #region Constructor

        public WebtroySwitchMetroRenderer()
        {
            BackColor = Color.White;
            LeftSideColor = Color.FromArgb(255, 23, 153, 0);
            LeftSideColorHovered = Color.FromArgb(255, 36, 182, 9);
            LeftSideColorPressed = Color.FromArgb(255, 121, 245, 100);
            RightSideColor = Color.FromArgb(255, 166, 166, 166);
            RightSideColorHovered = Color.FromArgb(255, 181, 181, 181);
            RightSideColorPressed = Color.FromArgb(255, 189, 189, 189);
            BorderColor = Color.FromArgb(255, 166, 166, 166);
            ButtonColor = Color.Black;
            ButtonColorHovered = Color.Black;
            ButtonColorPressed = Color.Black;
        }

        #endregion Constructor

        #region Public Properties

        public Color BackColor { get; set; }
        public Color LeftSideColor { get; set; }
        public Color LeftSideColorHovered { get; set; }
        public Color LeftSideColorPressed { get; set; }
        public Color RightSideColor { get; set; }
        public Color RightSideColorHovered { get; set; }
        public Color RightSideColorPressed { get; set; }
        public Color BorderColor { get; set; }
        public Color ButtonColor { get; set; }
        public Color ButtonColorHovered { get; set; }
        public Color ButtonColorPressed { get; set; }

        #endregion Public Properties

        #region Render Method Implementations

        public override void RenderBorder(Graphics g, Rectangle borderRectangle)
        {
            Color borderColor = !WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled ? BorderColor : BorderColor;

            g.SetClip(borderRectangle);

            using (Pen borderPen = new Pen(borderColor))
            {
                g.DrawRectangle(borderPen, borderRectangle.X, borderRectangle.Y, borderRectangle.Width - 1, borderRectangle.Height - 1);
            }

            g.ResetClip();
        }

        public override void RenderLeftToggleField(Graphics g, Rectangle leftRectangle, int totalToggleFieldWidth)
        {
            Rectangle adjustedLeftRect = new Rectangle(leftRectangle.X + 2, 2, leftRectangle.Width - 2, leftRectangle.Height - 4);

            if (adjustedLeftRect.Width > 0)
            {
                Color leftColor = LeftSideColor;

                if (WebtroySwitch.IsLeftSidePressed)
                    leftColor = LeftSideColorPressed;
                else if (WebtroySwitch.IsLeftSideHovered)
                    leftColor = LeftSideColorHovered;

                if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                    leftColor = leftColor;

                g.SetClip(adjustedLeftRect);

                using (Brush leftBrush = new SolidBrush(leftColor))
                {
                    g.FillRectangle(leftBrush, adjustedLeftRect);
                }

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
                            textXPos = (float) fullRectangle.X + (((float) fullRectangle.Width - (float) textSize.Width)/2);
                        }
                        else if (WebtroySwitch.OnSideAlignment == WebtroySwitch.WebtroySwitchAlignment.Near)
                        {
                            textXPos = (float) fullRectangle.X + (float)fullRectangle.Width - (float) textSize.Width;
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
        }

        public override void RenderRightToggleField(Graphics g, Rectangle rightRectangle, int totalToggleFieldWidth)
        {
            Rectangle adjustedRightRect = new Rectangle(rightRectangle.X, 2, rightRectangle.Width - 2, rightRectangle.Height - 4);

            if (adjustedRightRect.Width > 0)
            {
                Color rightColor = RightSideColor;

                if (WebtroySwitch.IsRightSidePressed)
                    rightColor = RightSideColorPressed;
                else if (WebtroySwitch.IsRightSideHovered)
                    rightColor = RightSideColorHovered;

                if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                    rightColor = rightColor;

                g.SetClip(adjustedRightRect);

                using (Brush rightBrush = new SolidBrush(rightColor))
                {
                    g.FillRectangle(rightBrush, adjustedRightRect);
                }

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
            }
        }

        public override void RenderButton(Graphics g, Rectangle buttonRectangle)
        {
            Color buttonColor = ButtonColor;

            if (WebtroySwitch.IsButtonPressed)
            {
                buttonColor = ButtonColorPressed;
            }
            else if (WebtroySwitch.IsButtonHovered)
            {
                buttonColor = ButtonColorHovered;
            }

            if (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled)
                buttonColor = buttonColor;

            g.SetClip(buttonRectangle);

            using (Brush backBrush = new SolidBrush(buttonColor))
            {
                g.FillRectangle(backBrush, buttonRectangle);
            }

            g.ResetClip();
        }

        #endregion Render Method Implementations

        #region Helper Method Implementations

        public override int GetButtonWidth()
        {
            return (int)((double)WebtroySwitch.Height / 3 * 2);
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