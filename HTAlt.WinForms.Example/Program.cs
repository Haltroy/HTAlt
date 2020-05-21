using HTAlt.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HTAlt.Test
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 frm1 = new Form1();
            frm1.Show();
            TabForm tabForm = new TabForm();
            tabForm.Tabs.Add(tabForm.CreateTab());
            HTTitleTabsApplicationContext appContext = new HTTitleTabsApplicationContext();
            appContext.Start(tabForm);
            Application.Run(appContext);
        }
    }
}
