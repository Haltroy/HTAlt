using System.Drawing;
using System.Windows.Forms;

namespace Haltroyify
{
    public abstract class WebtroySwitchRendererBase
    {
        #region Private Members

        private WebtroySwitch _WebtroySwitch;

        #endregion Private Members

        #region Constructor

        protected WebtroySwitchRendererBase()
        {}

        internal void SetWebtroySwitch(WebtroySwitch WebtroySwitch)
        {
            _WebtroySwitch = WebtroySwitch;
        }

        internal WebtroySwitch WebtroySwitch
        {
            get { return _WebtroySwitch; }
        }

        #endregion Constructor

        #region Render Methods

        public void RenderBackground(PaintEventArgs e)
        {
            if (_WebtroySwitch == null)
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle controlRectangle = new Rectangle(0, 0, _WebtroySwitch.Width, _WebtroySwitch.Height);

            FillBackground(e.Graphics, controlRectangle);
            RenderBorder(e.Graphics, controlRectangle);
        }

        public void RenderControl(PaintEventArgs e)
        {
            if (_WebtroySwitch == null)
                return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle buttonRectangle = GetButtonRectangle();
            int totalToggleFieldWidth = WebtroySwitch.Width - buttonRectangle.Width;

            if (buttonRectangle.X > 0)
            {
                Rectangle leftRectangle = new Rectangle(0, 0, buttonRectangle.X, WebtroySwitch.Height);

                if (leftRectangle.Width > 0)
                    RenderLeftToggleField(e.Graphics, leftRectangle, totalToggleFieldWidth);
            }

            if (buttonRectangle.X + buttonRectangle.Width < e.ClipRectangle.Width)
            {
                Rectangle rightRectangle = new Rectangle(buttonRectangle.X + buttonRectangle.Width, 0, WebtroySwitch.Width - buttonRectangle.X - buttonRectangle.Width, WebtroySwitch.Height);

                if (rightRectangle.Width > 0)
                    RenderRightToggleField(e.Graphics, rightRectangle, totalToggleFieldWidth);
            }

            RenderButton(e.Graphics, buttonRectangle);
        }

        public void FillBackground(Graphics g, Rectangle controlRectangle)
        {
            Color backColor = (!WebtroySwitch.Enabled && WebtroySwitch.GrayWhenDisabled) ? WebtroySwitch.BackColor : WebtroySwitch.BackColor;

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
