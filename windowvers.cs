using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading; 
using System.Windows;
using System.Windows.Forms;

namespace SDKSample
{
    public class AppCode : Application
    {
        // Entry point method
        [STAThread]
        public static void Main()
        {
            AppCode app = new AppCode();
            app.Run();
        }
    }
}