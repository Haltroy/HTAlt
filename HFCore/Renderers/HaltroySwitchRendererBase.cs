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
using System.Drawing;
using System.Windows.Forms;

namespace HaltroyFramework
{
    public abstract class HaltroySwitchRendererBase
    {
        #region Private Members

        private HaltroySwitch _HaltroySwitch;

        #endregion Private Members

        #region Constructor

        protected HaltroySwitchRendererBase()
        { }

        internal void SetHaltroySwitch(HaltroySwitch HaltroySwitch)
        {
            _HaltroySwitch = HaltroySwitch;
        }

        internal HaltroySwitch HaltroySwitch => _HaltroySwitch;

        #endregion Constructor

        #region Render Methods

        public void RenderBackground(PaintEventArgs e)
        {
            if (_HaltroySwitch == null)
            {
                return;
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle controlRectangle = new Rectangle(0, 0, _HaltroySwitch.Width, _HaltroySwitch.Height);

            FillBackground(e.Graphics, controlRectangle);
            RenderBorder(e.Graphics, controlRectangle);
        }

        public void RenderControl(PaintEventArgs e)
        {
            if (_HaltroySwitch == null)
            {
                return;
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle buttonRectangle = GetButtonRectangle();
            int totalToggleFieldWidth = HaltroySwitch.Width - buttonRectangle.Width;

            if (buttonRectangle.X > 0)
            {
                Rectangle leftRectangle = new Rectangle(0, 0, buttonRectangle.X, HaltroySwitch.Height);

                if (leftRectangle.Width > 0)
                {
                    RenderLeftToggleField(e.Graphics, leftRectangle, totalToggleFieldWidth);
                }
            }

            if (buttonRectangle.X + buttonRectangle.Width < e.ClipRectangle.Width)
            {
                Rectangle rightRectangle = new Rectangle(buttonRectangle.X + buttonRectangle.Width, 0, HaltroySwitch.Width - buttonRectangle.X - buttonRectangle.Width, HaltroySwitch.Height);

                if (rightRectangle.Width > 0)
                {
                    RenderRightToggleField(e.Graphics, rightRectangle, totalToggleFieldWidth);
                }
            }

            RenderButton(e.Graphics, buttonRectangle);
        }

        public void FillBackground(Graphics g, Rectangle controlRectangle)
        {
            Color backColor = (!HaltroySwitch.Enabled && HaltroySwitch.GrayWhenDisabled) ? HaltroySwitch.BackColor : HaltroySwitch.BackColor;

            using (Brush backBrush = new SolidBrush(backColor))
            {
                g.FillRectangle(backBrush, controlRectangle);
            }
        }

        public abstract void RenderBorder(Graphics g, Rectangle borderRectangle);
        public abstract void RenderLeftToggleField(Graphics g, Rectangle leftRectangle, int totalToggleFieldWidth);
        public abstract void RenderRightToggleField(Graphics g, Rectangle rightRectangle, int totalToggleFieldWidth);
        public abstract void RenderButton(Graphics g, Rectangle buttonRectangle);

        #endregion Render Methods

        #region Helper Methods

        public abstract int GetButtonWidth();
        public abstract Rectangle GetButtonRectangle();
        public abstract Rectangle GetButtonRectangle(int buttonWidth);

        #endregion Helper Methods
    }
}