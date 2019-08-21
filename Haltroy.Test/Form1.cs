using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Haltroy.Test
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void HaltroySlider1_Scroll(object sender, ScrollEventArgs e)
        {
            this.Text = "OLD VALUE: " + e.OldValue + " | NEW VaLUE: " + e.NewValue;
        }
    }
}
