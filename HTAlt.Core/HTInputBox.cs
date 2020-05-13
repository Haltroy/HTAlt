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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HTAlt
{
    /// <summary>
    /// Customizable Input Box.
    /// </summary>
    public partial class HTInputBox : Form
    {
        #region HTControls
        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://github.com/Haltroy/HTAlt/wiki/HTInputBox-Class");
        private readonly Version firstHTAltVersion = new Version("0.1.1.0");
        private readonly string description = "Customizable Input Box.";
        /// <summary>
        /// This control's wiki link.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's wiki link.")]
        public Uri WikiLink => wikiLink;
        /// <summary>
        /// This control's first appearance version for HTAlt.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's first appearance version for HTAlt.")]
        public Version FirstHTAltVersion => firstHTAltVersion;
        /// <summary>
        /// This control's description.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's description.")]
        public string Description => description;
        /// <summary>
        /// Information about this control's project.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("Information about this control's project.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public HTInfo ProjectInfo => info;
        #endregion
        /// <summary>
        /// Background color of HTInputBox. Foreground color is auto-selected to White or Black.
        /// </summary>
        public Color BackgroundColor;
        /// <summary>
        /// Text to display on "OK" button.
        /// </summary>
        public string OK = "OK";
        /// <summary>
        /// Text to display on "Cancel" button.
        /// </summary>
        public string Cancel = "Cancel";
        /// <summary>
        /// Text to display on "Set to default" button.
        /// </summary>
        public string SetToDefault = "Set to default";
        private readonly string defaultString = "";

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
        /// <summary>
        /// Creates a new Input Box.
        /// </summary>
        /// <param name="title">Title of the input box.</param>
        /// <param name="description">Description of the input box.</param>
        /// <param name="defaultValue">Default value of the input box.</param>
        public HTInputBox(string title,
                               string description,
                               string defaultValue = "")
        {
            Tools.PrintInfoToConsole();
            InitializeComponent();
            defaultString = defaultValue;
            Text = title;
            label1.Text = description;
            Height = (15 * LinesCountIndexOf(description)) + 95;
            MaximumSize = new Size(Screen.FromHandle(Handle).WorkingArea.Width, Screen.FromHandle(Handle).WorkingArea.Height);
            textBox1.Text = defaultString;
            BackgroundColor = Color.White;
            button1.Text = OK;
            button2.Text = Cancel;
        }
        /// <summary>
        /// Value inside the textbox in this input box.
        /// </summary>
        public string TextValue => textBox1.Text;


        private void Button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1.ButtonText = OK;
            button2.ButtonText = Cancel;
            haltroyButton1.ButtonText = SetToDefault;
            ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            BackColor = BackgroundColor;
            button1.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            button1.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            button2.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            haltroyButton1.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
            button2.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            haltroyButton1.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            textBox1.ForeColor = Tools.AutoWhiteBlack(BackgroundColor); ;
            textBox1.BackColor = Tools.ShiftBrightnessIfNeeded(BackgroundColor, 20, false);
        }

        private void haltroyButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text = defaultString;
        }
    }
}
