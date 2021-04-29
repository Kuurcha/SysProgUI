using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
namespace SysProgUI.IView
{
    public interface IViewAnalyse: MyIView
    {
        string analyseLblOutput { set; }

        string toAnalyseTB { get; }
        bool analyseMode { get; }

        Label currentLbl { get; }

        void SetColorAnalyse(Control textbox, SolidColorBrush color);

        public void ShowMessageBoxAnalyse(string message);

        public event Action AnalyseResultRequest;
    }
}
