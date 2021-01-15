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
using System.Windows.Forms;
using WebService;

namespace AutoUpdateDLL
{
    public class SQLServer_AutoUpdateDLL : IAutoUpdateDLL
    {
        private string ConnectString;
        private string Dir;

        public SQLServer_AutoUpdateDLL(string _connectstring, string dir = null)
        {
            ConnectString = _connectstring;
            if (!String.IsNullOrEmpty(dir))
                Dir = dir;
            else
                Dir = Directory.GetCurrentDirectory();
        }

        public void GetDLLs(ProgressBar progbar , RichTextBox content)
        {
            var query = DataQuery.Create("Application", "ws_GetApplicationDLL_List");
            var ds = Database.ProcessRequest(ConnectString, query);

            if (ds != null)
            {
                DataTable result = ds.Tables[0];
                progbar.Maximum = result.Rows.Count;
                int i = 0;
                foreach (DataRow item in result.Rows)
                {
                    if (IsUpdate(item))
                    {
                        Dictionary<string, object> param = new Dictionary<string, object>();
                        param["DATA"] = item["DATA"];
                        param["OriginalFileName"] = item["OriginalFileName"];
                        DecodeStringBase64ToFile(param, Dir);
                        content.Text += item["OriginalFileName"].ToString() + ": updated" +"\n" ; 


                    }
                    else
                    {
                        content.Text += item["OriginalFileName"].ToString() + ": same version" + "\n";
                    }    
                    progbar.Increment(i);
                    i++;
                }

            }
        }

        

        private bool IsUpdate(DataRow item)
        {
            string[] myFiles = Directory.GetFiles(Dir);
            bool IsExist = false;
            bool IsEqual = false;

            IsExist = myFiles.Any(a => Path.GetExtension(a) == ".dll" && a.GetInfoDLL()["OriginalFileName"].ToString() == item["OriginalFileName"].ToString());
            if (IsExist)
                IsEqual = myFiles.Any(a => Path.GetExtension(a) == ".dll" && a.GetInfoDLL()["OriginalFileName"].ToString() == item["OriginalFileName"].ToString()
                                 && a.GetInfoDLL()["Version"].ToString().ConvertAssemblyVersionToInt() == item["Version"].ToString().ConvertAssemblyVersionToInt());

            if ( !IsExist || ( !IsEqual && IsExist) )
                return true;

            return false;
        }


        private void DecodeStringBase64ToFile(Dictionary<string, object> param, string path)
        {
            string save_path = Path.Combine(path, param["OriginalFileName"].ToString());
            byte[] bytes = Convert.FromBase64String(param["DATA"].ToString());

            ByteArrayToFile(Path.Combine(path, param["OriginalFileName"].ToString()), bytes);

        }


        private bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }



    }
}
