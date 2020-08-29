using System.Drawing;

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