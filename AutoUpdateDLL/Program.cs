using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateDLL
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectString = @"Server=nguyentampc\sqlexpress;Database=Application;User Id=sa;Password=19042015;";
            string dir = "";
            IAutoUpdateDLL upd = new SQLServer_AutoUpdateDLL(connectString, dir);
            upd.GetDLLs();
        }
    }
}
