using System;
using System.Collections.Generic;
using System.Text;
using Logic;
using System.Collections.ObjectModel;
using System.Windows;
namespace SysProgUI.IView
{
    interface IViewDatabaseFramework
    {
        // false - .bin, true - .json
        IEnumerable<AccessInfo> AccessInfoList_DB { set; get; }
        IEnumerable<DllFileInfo> DllFileInfoList_DB { set; get; }

        public string pathForDB { get; set; }
        
        public string dbMode { get; }
        DataBaseObject ObjectForOperation { get; }

        public event Action<string> DataBaseResultRequest;


    }
}
