//MIT License
//
//Copyright (c) 2020 Eren "Haltroy" Kanat
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using System;

namespace HTAlt
{
    internal class Startup
    {
        private readonly string htaltVersion;
        public Startup()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            htaltVersion = version.ToString();
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
                + HTAltVersion().ToString()
                + " by Haltroy"
                + Environment.NewLine
                + HFWebsite()
                + Environment.NewLine
                + "------------------";
        }
        public string ProductName()
        {
            return "HT-Alt";
        }
        public Version HTAltVersion()
        {
            return new Version(htaltVersion);
        }
        public string Developer()
        { return "Haltroy"; }
        public Uri HFWebsite()
        {
            return new Uri("https://github.com/haltroy/HTAlt");
        }
    }
}
