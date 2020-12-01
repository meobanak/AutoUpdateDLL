using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace System.CRM
{
    public static class StringExtensions
    {
        public static string SafeSQL(this string value)
        {
            return value
                .Replace("\\", "\\\\")
                // This simply replaces the silly 'smart quotes' with normal quotes.
                .Replace('\u2018', '\'').Replace('\u2019', '\'').Replace('\u201c', '\"').Replace('\u201d', '\"')
                .Replace("'", "''");
        }


        public static bool IsEmpty(this string value)
        {
            if (value == null)
                return true;

            return value.Trim().Length == 0;
        }


        public static bool IsNotEmpty(this string value)
        {
            return !value.IsEmpty();
        }


        public static bool IsValidEmail(this string value)
        {
            // source: http://thedailywtf.com/Articles/Validating_Email_Addresses.aspx
            string pattern = @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$";
            return Regex.IsMatch(value, pattern);
        }


        public static int ToInt32(this string value)
        {
            int result;

            if (int.TryParse(value, out result))
            {
                return result;
            }

            return 0;
        }


        public static string FormatWith(this string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            return string.Format(format, args);
        }


        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            return string.Format(provider, format, args);
        }

        public static DataTable StringToTable(this string collector)
        {
            if (collector == null)
                return null;

            var _string = collector.Split(',');
            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            foreach (string item in _string)
            {
                table.Rows.Add(item);
            }
            return table;
        }

        public static string ArrayToString(this object lst)
        {
            string text = "";
            //foreach (string item in  )
            //{
            //    text += item + ",";
            //}

            return text;
        }

        public static string EncodeFileToStringBase64(this string path)
        {
            Byte[] bytes = File.ReadAllBytes("path");
            String file = Convert.ToBase64String(bytes);
            return file;
        }


        public static Dictionary<string, object> GetInfoDLL(this string path)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            Assembly assembly = Assembly.LoadFile(path);
            result["Version"] = assembly.GetName().Version.ToString();
            result["ProductName"] = assembly.GetName().Name.ToString();
            result["OriginalFileName"] = assembly.ManifestModule.ToString();
            result["CreatedDate"] = DateTime.Today;
            return result;
        }


        public static int ConvertAssemblyVersionToInt(this string param)
        {
            int result = 0;
            if (!String.IsNullOrEmpty(param))
            {
                string s = param.Trim().Replace('.', ' ').RemoveSpecialCharacters();
                result = Convert.ToInt32(s);
            }

            return result;
        }


        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }




    }
}
