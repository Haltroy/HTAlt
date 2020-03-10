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
using System.Windows.Forms;

namespace HaltroyFramework
{
    public partial class HaltroyInputBox : Form
    {
        Color BackgroundColor;
        static int LinesCountIndexOf(string s)
        {
            int count = 0;
            int position = 0;
            while ((position = s.IndexOf('\n', position)) != -1)
            {
                count++;
                position++;
            }
            return count;
        }
        public HaltroyInputBox(string title,
                               string description,
                               Icon icon,
                               string defaultValue = "",
                               Color? _BackColor = null,
                               string OKText = "OK",
                               string CancelText = "Cancel",
                               int windowWidth = 400,
                               int windowHeight = 150)
        {
            Startup start = new Startup();
            InitializeComponent();
            this.Text = title;
            label1.Text = description;
            this.Icon = icon;
            if (windowWidth != 400) { if (!(windowWidth < this.MinimumSize.Width)) { this.Width = windowWidth; } }
            if (windowHeight != 150) { if (!(windowHeight < this.MinimumSize.Height)) { this.Height = windowHeight; } } else { this.Height = (15 * LinesCountIndexOf(description)) + 70; }
            this.MaximumSize = new Size(Screen.FromHandle(this.Handle).WorkingArea.Width, Screen.FromHandle(this.Handle).WorkingArea.Height);
            textBox1.Text = defaultValue;
            BackgroundColor = _BackColor ?? Color.White;
            button1.Text = OKText;
            button2.Text = CancelText;
        }
        public string TextValue()
        {
            return textBox1.Text;
        }
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
        public static int GerekiyorsaArttır(int defaultint, int arttırma, int sınır)
        {
            return defaultint + arttırma > sınır ? defaultint : defaultint + arttırma;
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
                return Color.FromArgb(shiftAlpha ? GerekiyorsaArttır(baseColor.A, value, 255) : baseColor.A,
                      GerekiyorsaArttır(baseColor.R, value, 255),
                      GerekiyorsaArttır(baseColor.G, value, 255),
                      GerekiyorsaArttır(baseColor.B, value, 255));
            }
        }
        #endregion
        private void CustomInputBox_Load(object sender, EventArgs e)
        {
            this.ForeColor = isBright(BackgroundColor) ? Color.White : Color.Black;
            this.BackColor = BackgroundColor;
            button1.ForeColor = isBright(BackgroundColor) ? Color.White : Color.Black;
            button2.ForeColor = isBright(BackgroundColor) ? Color.White : Color.Black;
            textBox1.ForeColor = isBright(BackgroundColor) ? Color.White : Color.Black;
            textBox1.BackColor = ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
