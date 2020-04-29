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
    public partial class HaltroyMsgBox : Form
    {
        public Color BackgroundColor;
        private bool useOK = false;
        private readonly MessageBoxButtons msgbutton = MessageBoxButtons.OK;
        public string YesButtonText = "Yes";
        public string NoButtonText = "No";
        public string OKBUttonText = "OK";
        public string CancelButtonText = "Cancel";

        private static int LinesCountIndexOf(string s)
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
        public HaltroyMsgBox(string title,
                      string message,
                      MessageBoxButtons messageBoxButtons)
        {
            msgbutton = messageBoxButtons;
            InitializeComponent();
            Startup startup = new Startup();
            Text = title;
            label1.Text = message;
            Height = (15 * LinesCountIndexOf(message)) + 123;
            MaximumSize = new Size(Screen.FromHandle(Handle).WorkingArea.Width, Screen.FromHandle(Handle).WorkingArea.Height);
        }

        private void btYes_Click(object sender, EventArgs e)
        {
            DialogResult = useOK ? DialogResult.OK : DialogResult.Yes;
            Close();
        }

        private void btNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
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
        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void msgkts_Load(object sender, EventArgs e)
        {
            if (msgbutton == MessageBoxButtons.OK)
            {
                btYes.Visible = true;
                btNo.Visible = false;
                btCancel.Visible = false;
                btYes.Enabled = true;
                btNo.Enabled = false;
                btCancel.Enabled = false;
                useOK = true;
            }
            else if (msgbutton == MessageBoxButtons.OKCancel)
            {
                btYes.Visible = true;
                btNo.Visible = false;
                btCancel.Visible = true;
                btYes.Enabled = true;
                btNo.Enabled = false;
                btCancel.Enabled = true;
                useOK = true;
            }
            else if (msgbutton == MessageBoxButtons.YesNo)
            {
                btYes.Visible = true;
                btNo.Visible = true;
                btCancel.Visible = false;
                btYes.Enabled = true;
                btNo.Enabled = true;
                btCancel.Enabled = false;
            }
            else if (msgbutton == MessageBoxButtons.YesNoCancel)
            {
                btYes.Visible = true;
                btNo.Visible = true;
                btCancel.Visible = true;
                btYes.Enabled = true;
                btNo.Enabled = true;
                btCancel.Enabled = true;
            }
            else
            {
                btYes.Visible = false;
                btNo.Visible = false;
                btCancel.Visible = false;
                btYes.Enabled = false;
                btNo.Enabled = false;
                btCancel.Enabled = false;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btYes.Text = useOK ? OKBUttonText : YesButtonText;
            btNo.Text = NoButtonText;
            btCancel.Text = CancelButtonText;
            ForeColor = isBright(BackgroundColor) ? Color.Black : Color.White;
            BackColor = BackgroundColor;
            btCancel.BackColor = ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btCancel.ForeColor = isBright(BackgroundColor) ? Color.Black : Color.White;
            btYes.BackColor = ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btYes.ForeColor = isBright(BackgroundColor) ? Color.Black : Color.White;
            btNo.BackColor = ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            btNo.ForeColor = isBright(BackgroundColor) ? Color.Black : Color.White;
            flowLayoutPanel1.BackColor = BackgroundColor;
        }
    }
}
