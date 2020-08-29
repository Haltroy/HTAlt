using System;
using System.Collections.Generic;
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

        protected HTTitleTab ParentTab
        {
            get
            {
                List<int> tabIndexes = new List<int>();
                foreach (HTTitleTab x in ParentTabs.Tabs)
                {
                    if (x.Content == this) { tabIndexes.Add(ParentTabs.Tabs.IndexOf(x)); }
                }
                return (tabIndexes.Count > 0 ? ParentTabs.Tabs[tabIndexes[0]] : null);
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

        private void button1_Click(object sender, EventArgs e)
        {
            ParentTab.BackColor = Tools.RandomColor(255, false);
            ParentTab.OverlayBackColor = Tools.RandomColor(255, false);
            ParentTab.OverlayColor = Tools.RandomColor(255, false);
            ParentTab.ForeColor = Tools.RandomColor(255, false);
            ParentTab.UseDefaultColor = false;
            ParentTabs.RedrawTabs();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!(ParentTabs.TabRenderer is HTTabRenderer)) { return; }
            var htTabRenderer = (ParentTabs.TabRenderer as HTTabRenderer);
            htTabRenderer.ApplyColors(
                Tools.RandomColor(255, false),
                Tools.RandomColor(255, false),
                Tools.RandomColor(255, false),
                Tools.RandomColor(255, false));
        }
    }
}