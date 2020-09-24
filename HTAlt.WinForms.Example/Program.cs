using System;
using System.Windows.Forms;
using System.Linq;

namespace HTAlt.WinForms.Example
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Contains("-about"))
            {
                Application.Run(UI.About());
            }
            else if (args.Contains("-test"))
            {
                Application.Run(new Form1());
            }else
            {
                Application.Run(new Form1());
            }
        }
    }
}