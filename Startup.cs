using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace HaltroyFramework
{
    class Startup
    {
        string hfVersion;
        public Startup()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            hfVersion = version.ToString();
            PrintInfoToConsole();
        }
        public void PrintInfoToConsole()
        {
            Console.WriteLine(PrintInfo()
                + Environment.NewLine
                + "Please check for latest version of this product in our GitHub page for more stability."
                + Environment.NewLine
                + "If you are not the developer,then ignore this message."
                + Environment.NewLine
                + "------------------");
        }
        public string PrintInfo()
        {
            return ProductName()
                + Environment.NewLine
                + "version "
                + HFVersion().ToString()
                + Environment.NewLine
                + "by " + Developer()
                + Environment.NewLine
                + HFWebsite()
                + Environment.NewLine
                + "------------------";
        }
        public string ProductName()
        {
            return "Haltroy Framework";
        }
        public Version HFVersion()
        {
            return new Version(hfVersion);
        }
        public string Developer()
        { return "Haltroy"; }
        public Uri HFWebsite()
        {
            return new Uri("https://github.com/haltroy/haltroyframework");
        }
    }
}
