using HTAlt.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HTAlt.WinForms.Example
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
            Application.Run(new Form1());
        }
    }
}
