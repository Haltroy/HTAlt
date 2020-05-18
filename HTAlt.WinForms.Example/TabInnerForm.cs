using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTAlt.Test
{
    public partial class TabInnerForm : Form
    {
        protected HTTitleBarTabs ParentTabs
        {
            get
            {
                return (ParentForm as HTTitleBarTabs);
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
