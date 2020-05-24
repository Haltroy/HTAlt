using HTAlt.Standart;
using HTAlt.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTAlt.WinForms.Example
{
    public partial class TabInnerForm : Form
    {
        protected HTTitleTabs ParentTabs
        {
            get
            {
                return (ParentForm as HTTitleTabs);
            }
        }
        public TabInnerForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            BackColor = ParentTabs.BackColor;
            ForeColor = Tools.AutoWhiteBlack(ParentTabs.BackColor);
            Text = "Demo";
        }
    }
}
