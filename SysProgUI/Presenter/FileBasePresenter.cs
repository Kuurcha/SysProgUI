using System;
using System.Collections.Generic;
using System.Text;
using SysProgUI.IView;
using System.Linq;
using Logic.Model;
using System.Collections.ObjectModel;
using Logic;
namespace SysProgUI.Presenter
{
    class FileBasePresenter
    {
        private IViewDatabaseFramework viewDatabaseFramework;
        private IDataBaseModel dataBaseModel;
        public FileBasePresenter() { }
        
        public FileBasePresenter(IViewDatabaseFramework viewDatabaseFramework, IDataBaseModel dataBaseModel)
        {
            this.dataBaseModel = dataBaseModel;
            this.viewDatabaseFramework = viewDatabaseFramework;
            viewDatabaseFramework.DataBaseResultRequest += OnResultRequest;            
        }
        
        private void OnResultRequest(string sender)
        {

            bool mode = true;
            dataBaseModel.mode = "bin";
            if (viewDatabaseFramework.dbMode.Contains(".json"))
            {
                mode = false;
                dataBaseModel.mode = "json";
            }

                
            switch (sender)
            {             
                case "Добавить":
                    IEnumerable<DataBaseObject> resultAdd = dataBaseModel.Add(viewDatabaseFramework.ObjectForOperation);
                    if (mode)
                        viewDatabaseFramework.AccessInfoList_DB = resultAdd.Where(x => x is AccessInfo).Cast<AccessInfo>();
                    else
                        viewDatabaseFramework.DllFileInfoList_DB = resultAdd.Where(x => x is DllFileInfo).Cast<DllFileInfo>();
                    break;
                case "Удалить":
                    IEnumerable<DataBaseObject> resultDelete = dataBaseModel.Delete(viewDatabaseFramework.ObjectForOperation);
                    if (mode)
                        viewDatabaseFramework.AccessInfoList_DB = resultDelete.Where(x => x is AccessInfo).Cast<AccessInfo>();
                    else
                        viewDatabaseFramework.DllFileInfoList_DB = resultDelete.Where(x => x is DllFileInfo).Cast<DllFileInfo>();
                    break;
                case "Изменить":
                    IEnumerable<DataBaseObject> resultUpdate = dataBaseModel.Update(viewDatabaseFramework.ObjectForOperation);
                    if (mode)
                        viewDatabaseFramework.AccessInfoList_DB = resultUpdate.Where(x => x is AccessInfo).Cast<AccessInfo>();
                    else
                        viewDatabaseFramework.DllFileInfoList_DB = resultUpdate.Where(x => x is DllFileInfo).Cast<DllFileInfo>();
                    
                    break;
                default:
                    break;

            }
        }
    }
}
