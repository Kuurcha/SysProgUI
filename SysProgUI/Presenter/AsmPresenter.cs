using System;
using System.Collections.Generic;
using System.Text;
using SysProgUI.IView;
using Logic.Model;
namespace SysProgUI.Presenter
{
    /// <summary>
    /// Презентер для исполнения ассемблерных вставок
    /// </summary>
    class AsmPresenter
    {
        private IViewAsm viewAsm;

        private IAsmModel asmModel;
        public AsmPresenter(IViewAsm viewAsm, IAsmModel asmModel)
        {
            this.asmModel = asmModel;
            this.viewAsm = viewAsm;
            viewAsm.AsmResultRequest += OnResultRequest;
        }
        /// <summary>
        /// Метод, связаываемый с событием возвращающий результат операции или сообщение об ошибке.
        /// </summary>
        private void OnResultRequest()
        {
            string[] tempResult = asmModel.DoOperation(viewAsm.aValue, viewAsm.bValue, viewAsm.AsmMode);
            if (tempResult[1] == null)
            {
                viewAsm.asmResult = tempResult[0];
                viewAsm.LogToTextbox(Logic.LogManager.type.INFO, "Успешно, результат операции: " + tempResult[0]);
            }
            else
            {
                viewAsm.ShowMessageBoxAsm(tempResult[1]);
                viewAsm.LogToTextbox(Logic.LogManager.type.ERROR, tempResult[1]);
            }
               
            
        }
    }
}
