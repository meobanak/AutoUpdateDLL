using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateDLL
{
    public interface IAutoUpdateDLL
    {
        void GetDLLs();
        void DecodeStringBase64ToFile(string b64Str, string path);
    }
}
