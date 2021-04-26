using System;
using System.Collections.Generic;
using System.Text;

namespace SysProgUI.IView
{
    public interface IViewAnalyse
    {
        string analyseLbl { set; }

        string toAnalyseTB { get; }

        public void ShowMessageBoxAnalyse(string message);

        public event Action AsmResultRequest;
    }
}
