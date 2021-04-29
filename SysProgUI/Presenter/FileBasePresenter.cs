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
        
        public void changeModel(IDataBaseModel dataBaseModel)
        {
            this.dataBaseModel = dataBaseModel;
        }
        public FileBasePresenter(IViewDatabaseFramework viewDatabaseFramework, IDataBaseModel dataBaseModel)
        {
            this.dataBaseModel = dataBaseModel;
            this.viewDatabaseFramework = viewDatabaseFramework;
            viewDatabaseFramework.DataBaseResultRequest += OnResultRequest;   
            
        }

     
        private void OnResultRequest(string sender)
        {
            viewDatabaseFramework.LogToTextbox(LogManager.type.WARN, "On_Result_Request_Fired");
            bool mode = true;
            dataBaseModel.mode = "bin";
            if (viewDatabaseFramework.dbMode.Contains(".json"))
            {
                mode = false;
                dataBaseModel.mode = "json";
            }
            IEnumerable<DataBaseObject> getAllObjects = dataBaseModel.GetAll();

            switch (sender)
            {             
                case "Добавить":
                   
                    if (viewDatabaseFramework.ObjectForOperation != null)
                    {
                        foreach (DataBaseObject obj in getAllObjects)
                            if (obj.SystemId == viewDatabaseFramework.ObjectForOperation.SystemId)
                                throw new NotUniqueGuidException("Совпадение primary key! " + obj.SystemId);
                    }
                    IEnumerable<DataBaseObject> resultAdd = dataBaseModel.Add(viewDatabaseFramework.ObjectForOperation);
                   
                    if (mode)
                    {
                        viewDatabaseFramework.AccessInfoList_DB = resultAdd.Where(x => x is AccessInfo).Cast<AccessInfo>();
                        viewDatabaseFramework.LogToTextbox(LogManager.type.INFO, "Элемент " + ((AccessInfo)viewDatabaseFramework.ObjectForOperation).ToString() + "успешно добавлен.");
                    }

                    else
                    {
                        viewDatabaseFramework.LogToTextbox(LogManager.type.INFO, "Элемент " + ((DllFileInfo)viewDatabaseFramework.ObjectForOperation).ToString() + "успешно добавлен.");
                        viewDatabaseFramework.DllFileInfoList_DB = resultAdd.Where(x => x is DllFileInfo).Cast<DllFileInfo>();
                    }
                    break;
                case "Удалить":
                    IEnumerable<DataBaseObject> resultDelete = dataBaseModel.Delete(viewDatabaseFramework.ObjectForOperation);
                    if (mode)
                    {
                        viewDatabaseFramework.AccessInfoList_DB = resultDelete.Where(x => x is AccessInfo).Cast<AccessInfo>();
                        viewDatabaseFramework.LogToTextbox(LogManager.type.INFO, "Элемент " + ((AccessInfo)viewDatabaseFramework.ObjectForOperation).ToString() + "успешно удален.");
                    }
                    else
                    {
                        viewDatabaseFramework.DllFileInfoList_DB = resultDelete.Where(x => x is DllFileInfo).Cast<DllFileInfo>();
                        viewDatabaseFramework.LogToTextbox(LogManager.type.INFO, "Элемент " + ((DllFileInfo)viewDatabaseFramework.ObjectForOperation).ToString() + "успешно удален.");
                    }
                    break;
                case "Изменить":
                    IEnumerable<DataBaseObject> resultUpdate = dataBaseModel.Update(viewDatabaseFramework.ObjectForOperation);
                    if (mode)
                    {
                        viewDatabaseFramework.AccessInfoList_DB = resultUpdate.Where(x => x is AccessInfo).Cast<AccessInfo>();
                        viewDatabaseFramework.LogToTextbox(LogManager.type.INFO, "Успешно заменено на " + ((AccessInfo)viewDatabaseFramework.ObjectForOperation).ToString());

                    }
                    else
                    {
                        viewDatabaseFramework.DllFileInfoList_DB = resultUpdate.Where(x => x is DllFileInfo).Cast<DllFileInfo>();
                        viewDatabaseFramework.LogToTextbox(LogManager.type.INFO, "Успешно заменено на " + ((DllFileInfo)viewDatabaseFramework.ObjectForOperation).ToString());

                    }
                    break;
                case "Загрузить":
                    IEnumerable<DataBaseObject> resultLoad = dataBaseModel.Load();
                    if (mode)
                    {
                        viewDatabaseFramework.AccessInfoList_DB = resultLoad.Where(x => x is AccessInfo).Cast<AccessInfo>();
                        viewDatabaseFramework.LogToTextbox(LogManager.type.INFO, "Успешно загружена база AccsesInfo");

                    }
                    else
                    {
                        viewDatabaseFramework.DllFileInfoList_DB = resultLoad.Where(x => x is DllFileInfo).Cast<DllFileInfo>();
                        viewDatabaseFramework.LogToTextbox(LogManager.type.INFO, "Успешно загружено база DllFileInfoL");

                    }
                    
                    if (viewDatabaseFramework.ObjectForOperation != null)
                    {
                        foreach (DataBaseObject obj in getAllObjects)
                            foreach (DataBaseObject obj2 in getAllObjects)
                                if (obj.SystemId == viewDatabaseFramework.ObjectForOperation.SystemId)
                                    throw new NotUniqueGuidException("Совпадение primary key! " + obj.SystemId);
                    }
                    break;
                case "Сохранить":
                    dataBaseModel.Save(viewDatabaseFramework.pathForDB);
                    break;
                default:
                    break;

            }
        }
    }
}
