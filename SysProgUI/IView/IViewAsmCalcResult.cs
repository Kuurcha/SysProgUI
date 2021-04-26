using System;
using System.Text;
using System.Windows;

using System.Windows.Media;

namespace SysProgUI.IView
{
    public interface IViewAsm
    {
        string asmResult { set; }
        
        string aValue { get; }
        string bValue { get; }
        bool AsmMode { get; }

        public void ShowMessageBoxAsm(string message);

        public event Action AsmResultRequest;
    }
}

