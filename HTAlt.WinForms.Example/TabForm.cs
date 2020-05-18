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
    public partial class TabForm : HTTitleBarTabs
    {
        public TabForm()
        {
            InitializeComponent();
            AeroPeekEnabled = true;
            TabRenderer = new HTTabRenderer(this, Color.White, Color.Black, Color.DodgerBlue, null, false);
        }
        public override HTTitleBarTab CreateTab()
        {
            return new HTTitleBarTab(this) { BackColor = Color.White, useDefaultBackColor = true, Content = new TabInnerForm() };
        }
    }
}
