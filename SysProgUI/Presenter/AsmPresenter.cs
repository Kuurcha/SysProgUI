using System;
using System.Collections.Generic;
using System.Text;
using SysProgUI.IView;
using Logic.Model;
namespace SysProgUI.Presenter
{
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
        private void OnResultRequest()
        {
            string[] tempResult = asmModel.DoOperation(viewAsm.aValue, viewAsm.bValue, viewAsm.AsmMode);
            if (tempResult[1] == null)
                viewAsm.asmResult = tempResult[0];
            else
                viewAsm.ShowMessageBoxAsm(tempResult[1]);
            
        }
    }
}
