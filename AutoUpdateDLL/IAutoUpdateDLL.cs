using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateDLL
{
    public interface IAutoUpdateDLL
    {
        void GetDLLs(ProgressBar progbar, RichTextBox content);
    }
}
