using System;
using System.Collections.Generic;
using System.Text;
using Logic.Model;
using SysProgUI.IView;
using System.Media;
using System.Windows.Media;

namespace SysProgUI.Presenter
{
    /// <summary>
    /// Презентер анализатора
    /// </summary>
    public class AnalysePresenter
    {
        private IAnalyseModel analyseModel;
        private IViewAnalyse viewAnalyse;


        public AnalysePresenter(IViewAnalyse viewAnalyse, IAnalyseModel analyseModel) {
            this.analyseModel = analyseModel;
            this.viewAnalyse = viewAnalyse;
            viewAnalyse.AnalyseResultRequest += OnResultRequest;
        }
        /// <summary>
        /// Метод связываемый с событием, отвечающий за исполнение методов модели и получения результировающего сообщения
        /// </summary>
        public void OnResultRequest()
        {
              string result =  analyseModel.getResult(viewAnalyse.toAnalyseTB, !viewAnalyse.analyseMode);

            if (result.Contains("Ошибка!"))
            {
                viewAnalyse.SetColorAnalyse(viewAnalyse.currentLbl, new SolidColorBrush(Color.FromRgb(255, 0, 0)));
                viewAnalyse.LogToTextbox(Logic.LogManager.type.ERROR, result);
            }
               
            else
            {
                viewAnalyse.SetColorAnalyse(viewAnalyse.currentLbl, new SolidColorBrush(Color.FromRgb(0, 255, 0)));
                viewAnalyse.LogToTextbox(Logic.LogManager.type.INFO, "Успешная обработка: " + result);
            }
                
            viewAnalyse.analyseLblOutput = result;
        }
    }
}
