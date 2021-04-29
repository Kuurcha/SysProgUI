using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;

namespace SysProgUI.IView
{
    public interface MyIView
    {
        void LogToTextbox(LogManager.type type, string logmessage);
    }
}
