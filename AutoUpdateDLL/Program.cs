using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateDLL
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            string connectString = @"Server=nguyentampc\sqlexpress;Database=Application;User Id=sa;Password=19042015;";
            string dir = @"D:\test";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(connectString, dir));

        }
    }
}
