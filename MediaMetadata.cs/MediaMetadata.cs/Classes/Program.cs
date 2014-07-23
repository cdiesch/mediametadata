using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaMetadata.cs
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Kyle was here. TEST. Will be removed with next push
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fMetadata());     
        }
    }
}
