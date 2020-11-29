using QA.API_WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService;

namespace AutoUpdateDLL
{
    public class AutoUpdateDLL : IAutoUpdateDLL
    {
        private string ConnectString;
        private string Dir;

        public AutoUpdateDLL(string _connectstring, string dir)
        {
            ConnectString = _connectstring;
            if (String.IsNullOrEmpty(dir))
                Dir = dir;
            else
                Dir = Directory.GetCurrentDirectory();
        }

        public void GetDLLs()
        {
            var query = DataQuery.Create("Application", "ws_GetApplicationDLL_List");
            var ds = Database.ProcessRequest(ConnectString, query);

            if (ds != null)
            {
                DataTable result = new DataTable();
                foreach (DataRow item in result.Rows)
                {
                    DecodeStringBase64ToFile(item["DATA"].ToString(), Dir);
                }
            }

        }


        public void DecodeStringBase64ToFile(string b64Str, string path)
        {
            Byte[] bytes = Convert.FromBase64String(b64Str);
            File.WriteAllBytes(path, bytes);
        }



    }
}
