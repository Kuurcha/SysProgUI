using System;
using System.Text;
using System.Windows;

using System.Windows.Media;

namespace SysProgUI.IView
{
    /// <summary>
    /// IView для ассемблерных функций
    /// </summary>
    public interface IViewAsm: MyIView
    {
        string asmResult { set; }
        
        string aValue { get; }
        string bValue { get; }
        bool AsmMode { get; }

        /// <summary>
        /// Показывает сообщение об ошибке assembler
        /// </summary>
        /// <param name="message">сообщение об ошибке</param>
        public void ShowMessageBoxAsm(string message);

        public event Action AsmResultRequest;
    }
}

