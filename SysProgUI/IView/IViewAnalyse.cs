using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
namespace SysProgUI.IView
{
   /// <summary>
   /// IView для Анализатора.
   /// </summary>
    public interface IViewAnalyse: MyIView
    {
        string analyseLblOutput { set; }

        string toAnalyseTB { get; }
        bool analyseMode { get; }

        Label currentLbl { get; }


        /// <summary>
        /// Установка цвета объекта типа Control
        /// </summary>
        /// <param name="textbox">Оьъект типа Control</param>
        /// <param name="color">Цвет</param>
        void SetColorAnalyse(Control textbox, SolidColorBrush color);

        /// <summary>
        /// Метод показания сообщения об ошибке
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public void ShowMessageBoxAnalyse(string message);

        public event Action AnalyseResultRequest;
    }
}
