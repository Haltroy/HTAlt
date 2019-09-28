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
            if (Properties.Settings.Default.isInfoGiven == false)
            {
                Properties.Settings.Default.isInfoGiven = true;
                Console.WriteLine(PrintInfo());
            }
        }
        public string PrintInfo()
        {
            return "------------------" 
                + Environment.NewLine
                + ProductName()
                + " v"
                + HFVersion().ToString()
                + " by Haltroy"
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
