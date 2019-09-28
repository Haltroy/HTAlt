using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HaltroyFramework
{
    public partial class HaltroyInputBox : Form
    {
        Color BackgroundColor;
        Color OverlayColor;
        static int LinesCountIndexOf(string s)
        {
            int count = 0;
            int position = 0;
            while ((position = s.IndexOf('\n', position)) != -1)
            {
                count++;
                position++;         // Skip this occurrence!
            }
            return count;
        }
        public HaltroyInputBox(string title,
                               string description,
                               Icon icon,
                               string defaultValue = "",
                               Color? _BackColor = null,
                               Color? _OverlayColor = null,
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
            if (windowHeight != 150 ) { if (!(windowHeight < this.MinimumSize.Height)) { this.Height = windowHeight; }} else { this.Height = (15 * LinesCountIndexOf(description)) + 123; }
            this.MaximumSize = new Size(Screen.FromHandle(this.Handle).WorkingArea.Width, Screen.FromHandle(this.Handle).WorkingArea.Height);
            textBox1.Text = defaultValue;
            BackgroundColor = _BackColor ?? Color.White;
            OverlayColor = _OverlayColor ?? Color.DodgerBlue;
            button1.Text = OKText;
            button2.Text = CancelText;
        }
        public string TextValue()
        {
            return textBox1.Text;
        }
        private static int Brightness(Color c)
        {
            return (int)Math.Sqrt(
               c.R * c.R * .241 +
               c.G * c.G * .691 +
               c.B * c.B * .068);
        }
        private void CustomInputBox_Load(object sender, EventArgs e)
        {
            this.ForeColor = Brightness(BackgroundColor) < 130 ? Color.White : Color.Black;
            this.BackColor = BackgroundColor;
            button1.ForeColor = OverlayColor;
            button2.ForeColor = OverlayColor;
            button1.FlatAppearance.BorderColor = OverlayColor;
            button2.FlatAppearance.BorderColor = OverlayColor;
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
