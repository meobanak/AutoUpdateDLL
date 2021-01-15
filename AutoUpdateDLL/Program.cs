using System;
using System.Collections.Generic;
using System.Configuration;
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
            string connectString = ConfigurationManager.AppSettings["CONNECTSTRINGSQLSERVER"].ToString();
            string dir = ConfigurationManager.AppSettings["DIRECTORY"].ToString();
            string execappdir = ConfigurationManager.AppSettings["APPLICATION"].ToString();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(connectString, dir , execappdir));

        }
    }
}
