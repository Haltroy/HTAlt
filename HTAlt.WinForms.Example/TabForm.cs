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
    public partial class TabForm : HTTitleTabs
    {
        public TabForm()
        {
            InitializeComponent();
            AeroPeekEnabled = true;
            TabRenderer = new HTTabRenderer(this);
        }
        public override HTTitleTab CreateTab()
        {
            return new HTTitleTab(this) 
            { 
                RightImage = Properties.Resources.logo,
                BackColor = Color.White, 
                UseDefaultBackColor = true, 
                Content = new TabInnerForm() 
            };
        }
    }
}
