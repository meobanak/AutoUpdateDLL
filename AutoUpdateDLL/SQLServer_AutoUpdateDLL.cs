using QA.API_WebService;
using System;
using System.Collections.Generic;
using System.CRM;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService;

namespace AutoUpdateDLL
{
    public class SQLServer_AutoUpdateDLL : IAutoUpdateDLL
    {
        private string ConnectString;
        private string Dir;

        public SQLServer_AutoUpdateDLL(string _connectstring, string dir)
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

            string[] myFiles = Directory.GetFiles(Dir);

            if (ds != null)
            {
                DataTable result = new DataTable();

                foreach (string myFile in myFiles)
                {
                    var info = myFile.GetInfoDLL();
                    
                    if(result
                        .AsEnumerable()
                        .Any(a => a["OriginalFileName"].ToString() == info["OriginalFileName"].ToString() 
                         && a["Version"].ToString().ConvertAssemblyVersionToInt() == info["OriginalFileName"].ToString().ConvertAssemblyVersionToInt()))
                    {
                        var item = result
                        .AsEnumerable().Where(a => a["OriginalFileName"].ToString() == info["OriginalFileName"].ToString()
                         && a["Version"].ToString().ConvertAssemblyVersionToInt() == info["OriginalFileName"].ToString().ConvertAssemblyVersionToInt()).FirstOrDefault();

                        DecodeStringBase64ToFile(item["DATA"].ToString(), Dir);
                        Console.WriteLine(myFile);
                    }
                }
            }
        }


        private void DecodeStringBase64ToFile(string b64Str, string path)
        {
            Byte[] bytes = Convert.FromBase64String(b64Str);
            File.WriteAllBytes(path, bytes);
        }



    }
}
