using System;
using System.Windows.Forms;

namespace HTAlt.WinForms.Core.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void htButton1_Click(object sender, EventArgs e)
        {
            HTAlt.WinForms.UI.ShowAbout();
        }
    }
}