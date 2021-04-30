using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using Logic;
using Logic.Model;
using SysProgUI.Presenter;
using SysProgUI.IView;
using Newtonsoft.Json;
using RadioButton = System.Windows.Controls.RadioButton;
using Control = System.Windows.Controls.Control;
using Label = System.Windows.Controls.Label;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace SysProgUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewAsm, IViewAnalyse, IViewDatabaseFramework
    {

        private TabItem[] tabArray;
        //IViewAnalyse

        public string toAnalyseTB { get { return languageConstructTB.Text; } }

        public string analyseLblOutput { set { infoLabel.Content = value; } }
        public bool analyseMode { get { return (bool)ForeachRB.IsChecked; } }

        public Label currentLbl { get { return infoLabel; } }

        public event Action AnalyseResultRequest;

        public void SetColorAnalyse(Control control, SolidColorBrush color)
        {
            control.SetCurrentValue(ForegroundProperty, color);
        }
        public void ShowMessageBoxAnalyse(string message)
        {

        }

        public DataBaseObject ObjectForOperation { get; set; }

        //IviewAsm

        public string asmResult { set { ResultTBr.Text = value; } }
        public string aValue { get { return FirstOperatorTB.Text; } }
        public string bValue { get { return SecondOperatorTB.Text; } }
        public bool AsmMode { get { return (bool)mulRb.IsChecked; } }

        public event Action AsmResultRequest;

        public event Action<string> DataBaseResultRequest;

        /// <summary>
        /// Метод показывающий пользователю сообщение об ошибке при работе с Ассемблером
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public void ShowMessageBoxAsm(string message)
        {
            System.Windows.MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        bool mode = false;


        /// <summary>
        /// Список AccessInfo который связан с таблицей DataGrid
        /// </summary>
        public IEnumerable<AccessInfo> AccessInfoList_DB
        {
            get { return accessInfosList_DB;  }
            set {
                ObservableCollection<AccessInfo> temp = new ObservableCollection<AccessInfo>();
                foreach (AccessInfo var in value)
                {
                    temp.Add(var);
                }
                accessInfosList_DB = temp;
                databaseBin.Items.Refresh();
                databaseBin.ItemsSource = accessInfosList_DB;
            }
        }
        /// <summary>
        /// Список DllFileInfo который связан с таблицей DataGrid
        /// </summary>
        public IEnumerable<DllFileInfo> DllFileInfoList_DB
        {
            get { return DllFileInfoList_DB; }
            set {
                ObservableCollection<DllFileInfo> temp = new ObservableCollection<DllFileInfo>();
                var test = value;
                for (int i = 0; i < value.Count(); i++)
                {
                    object tempObj = value.ElementAt(i);
                    if (tempObj is DllFileInfo)
                        temp.Add((DllFileInfo)tempObj);
                }
            
                dllFileInfoList_DB = temp;
                databaseJson.Items.Refresh();
                databaseJson.ItemsSource = dllFileInfoList_DB;
            }
        }
        public ObservableCollection<AccessInfo> accessInfosList_DB { set; get; }
        public ObservableCollection<DllFileInfo> dllFileInfoList_DB { set; get; }

        private ObservableCollection<AccessInfo> backupCollectionAccessInfo;
        private ObservableCollection<DllFileInfo> backupCollectionDllFileInfo;
           
        public string pathForDB { get; set; }
        /// <summary>
        /// Метод, Проверяющий какая конкретная таблица выбрана: Json или Bin 
        /// </summary>
        /// <returns>True - .bin, False => .json</returns>
        private bool checkState()
        {
            switch (Tableswitch.SelectedIndex)
            {
                case 0:
                    return true;
                case 1:
                    return false;
                default:
                    return false;
            }
        }
        /// <summary>
        /// Метод отвечающий за передачу выбранного режима в презентер
        /// </summary>
        public string dbMode { 
            get { return checkState() ? ".dll" : ".json"; }
        }

        private FileBasePresenter fbpresenter;
        private DataBaseMainModel DBmodel;
        private DataBaseFromFile  DBfromfileModel;

        LogManager logManager;

        /// <summary>
        /// Метод для логирования действий прогграммы
        /// </summary>
        /// <param name="type">Тип ошибки</param>
        /// <param name="logmessage">Сообщение об ошибке</param>
        public void LogToTextbox(LogManager.type type, string logmessage)
        {
            logManager.log(type, logmessage);
            loggerTB.Text = logManager.getLog();
        }
        /// <summary>
        /// Метод инициализации окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            var itemtabs = this.MainTabControl.Items;
            List<TabItem> tablist = new List<TabItem>();
            foreach (object item in itemtabs)
                tablist.Add((TabItem)item);
            tabArray = tablist.ToArray();

            AsmPresenter asmpr = new AsmPresenter(this, new AsmModel());
            AnalysePresenter analysepr = new AnalysePresenter(this, new AnalyseModel());
      
            //FileBasePresenter fbpresenter = new FileBasePresenter(this, DataBaseMainModel);
             accessInfosList_DB = new ObservableCollection<AccessInfo>();
            databaseBin.ItemsSource = accessInfosList_DB;
            databaseJson.ItemsSource = dllFileInfoList_DB;
            logManager = new LogManager();

            //DataBaseObject temp = new AccessInfo("Kurcha", "Let's pretend this is a hashcode", "password", "yhy@mail.ru");
            DataBaseObject temp = new DllFileInfo("Kurcha", "3.0", DateTime.Now) ;

            //using (StreamWriter outputFile = new StreamWriter("D:\\Programming\\testDll.json"))
            //{
            //    string result = JsonConvert.SerializeObject(temp);
            //    outputFile.WriteLine(result);
            //}
            LogToTextbox(LogManager.type.INFO, "Инциализация окна.");
        }



        /// <summary>
        /// Вызов события базы данных, если на него кто-либо подписан
        /// </summary>
        /// <param name="mode">Конкретная операция из события</param>
        public void CallEventDB(string mode)
        {
            DataBaseResultRequest?.Invoke(mode);
        }
        /// <summary>
        /// Вызов события подсчета ассемблерных функций, если на него кто-либо подписан
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Calc(object sender, RoutedEventArgs e)
        {
            AsmResultRequest?.Invoke();
            
        }
        /// <summary>
        /// Вызов события анализатора, если на него кто-либо подписан
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Analyse(object sender, RoutedEventArgs e)
        {
            AnalyseResultRequest?.Invoke();
            
        }

        /// <summary>
        /// Переключение объектов баз данных в зависимости от выбора режима работы с .dbf или же с файлами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataBase_Checked(object sender, RoutedEventArgs e)
        {

            if(databaseBin != null)
            {

                ObservableCollection<AccessInfo> tempAI = accessInfosList_DB; //!= null ? SupportClass.DeepClone<ObservableCollection<AccessInfo>>(accessInfosList_DB) : null;
                ObservableCollection<DllFileInfo> tempFI = dllFileInfoList_DB; //!= null ? SupportClass.DeepClone<ObservableCollection<DllFileInfo>>(dllFileInfoList_DB) : null;
                accessInfosList_DB = backupCollectionAccessInfo;
                dllFileInfoList_DB = backupCollectionDllFileInfo;
                backupCollectionDllFileInfo = tempFI;
                backupCollectionAccessInfo = tempAI;
                databaseBin.ItemsSource = accessInfosList_DB;
                databaseJson.ItemsSource = dllFileInfoList_DB;
                databaseBin.Items.Refresh();
                databaseJson.Items.Refresh();
                if (!(bool)DllRb.IsChecked)
                {
                    if (DBmodel != null)
                        fbpresenter = new FileBasePresenter(this, DBmodel);
                    LogToTextbox(LogManager.type.INFO,"Режим изменен на работу с базами данных");
                }
                else
                {
                    if (DBfromfileModel != null)
                        fbpresenter = new FileBasePresenter(this, DBfromfileModel);
                    LogToTextbox(LogManager.type.INFO, "Режим изменен на работу с базой файлов");
                }
            }
    }


        /// <summary>
        /// Метод отвечающий за смену вкладки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.SetCurrentValue(ForegroundProperty, new SolidColorBrush(Color.FromRgb(120, 120, 120)));
            if (tabArray != null)
                foreach (TabItem item in tabArray)
                    if (item.Header.Equals(rb.Content))
                        this.MainTabControl.SelectedItem = item;
            
        }
        /// <summary>
        /// Метод отвечающий за смену вкладки и возвращение цвета после прекращение его выделения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.SetCurrentValue(ForegroundProperty, new SolidColorBrush(Color.FromRgb(219, 220, 230)));

        }
        /// <summary>
        /// Визуальная смена операции в зависимости от выбранного пункта в Radiobutton
        /// </summary>
        /// <param name="rb"></param>
        public void setOperation(RadioButton rb)
        {
            if (rb.Content != null)
                if (rb.Content.Equals("Умножение")) OpLbl.Content = "X";
                else OpLbl.Content = "\\";
        }
        /// <summary>
        /// Событие отвечающее за нажатие на Radiobutton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked_Asm(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            setOperation(rb);


        }
        /// <summary>
        /// Событие отвечающее за разжатие RadioButton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Unchecked_Asm(object sender, RoutedEventArgs e)
    {
        RadioButton rb = sender as RadioButton;
        setOperation(rb);

    }


        /// <summary>
        /// Установка цвета на элемент
        /// </summary>
        /// <param name="element">Элемент для установки</param>
        /// <param name="color">Цвет для установки</param>
    public void setControlColor(System.Windows.Controls.Control element, SolidColorBrush color)
    {
        element.SetCurrentValue(ForegroundProperty, color);
    }

        /// <summary>
        /// Событие добавления в базу данных при нажатии на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            if (fbpresenter == null)
                ShowNoConnectedDBError();
            else
            {

                if (checkState())
                {
                    {
                        var dialogueWindow = new EditDialogue(this, checkState(), ((Button)sender).Content.ToString());
                        dialogueWindow.Show();
                    }
                }
                else
                {

                    var dialogue = new Microsoft.Win32.OpenFileDialog() { Filter = "json Files (*.json)|*.json" };
                    var result = dialogue.ShowDialog();
                    if (result == false) return;
                    string path = dialogue.FileName;
                    if (File.Exists(path))
                    {
                        string newFileNameJson = path;
                        if (File.Exists(path) || Directory.Exists(path))
                        {
                            BinaryReader binaryReaderJson = new BinaryReader(File.OpenRead(newFileNameJson));
                            LinkedList<DllFileInfo> listForWriting = new LinkedList<DllFileInfo>();
                            string jsonString = binaryReaderJson.ReadString();
                            while (binaryReaderJson.PeekChar() > -1)
                                jsonString = jsonString + binaryReaderJson.ReadString();
                            if (jsonString.Length > 2)
                            {
                                listForWriting = JsonConvert.DeserializeObject<LinkedList<DllFileInfo>>(jsonString);
                                binaryReaderJson.Close();
                            }
                            while (listForWriting.Count != 0)
                            {
                                ObjectForOperation = listForWriting.Last();
                                listForWriting.RemoveLast();
                                try
                                {
                                    CallEventDB("Добавить");
                                }
                                catch (NotUniqueGuidException ex)
                                {
                                    MessageBox.Show("Нарушается целостность данных! " + ex.Message);
                                    LogToTextbox(LogManager.type.ERROR, ex.Message);
                                }


                            }
                        }
                        //else
                        //    System.Windows.MessageBox.Show("Неудалось обработать Json файл", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error)
                    }
                    else MessageBox.Show("Файла не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
    }

    /// <summary>
    /// Получение текущего выделенного пользователем объекта
    /// </summary>
    public void SetCurrentlySelectedObject()
    {
        if (checkState())
            ObjectForOperation = (AccessInfo)databaseBin.SelectedItem;
        else
            ObjectForOperation = (DllFileInfo)databaseJson.SelectedItem;
    }
        /// <summary>
        /// Метод логирующий и отвечающий за показыванию пользователя конкретного сообщения об ошибке (об отсуствии подключенной бд)
        /// </summary>
        public void ShowNoConnectedDBError()
        {
            string errorMes = "Не подключена база данных";
            MessageBox.Show("Не подключена база данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            LogToTextbox(LogManager.type.ERROR, errorMes);
        }
        /// <summary>
        /// Метод удаления из базы данных при нажатии на кнопку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Delete(object sender, RoutedEventArgs e)
     {
            if (fbpresenter == null)
                ShowNoConnectedDBError();
            else
            {
                SetCurrentlySelectedObject();
                if (ObjectForOperation != null)
                    DataBaseResultRequest?.Invoke(((System.Windows.Controls.Button)sender).Content.ToString());
            }
          
    }
        public EditDialogue dialogueWindow = null;
    /// <summary>
    /// Метод модификации элемента базы данных
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        private void Button_Click_Modify(object sender, RoutedEventArgs e)
    {
            if (fbpresenter == null)
                ShowNoConnectedDBError();
            else
            {
                if (dialogueWindow == null)
                {
                    SetCurrentlySelectedObject();
                    if (ObjectForOperation != null)
                    {
                        dialogueWindow = new EditDialogue(this, checkState(), ((Button)sender).Content.ToString());
                        dialogueWindow.Show();
                    }
                }
            }
        }
    /// <summary>
    /// Метод сохранения базы данных по нажатию кнопки
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click_Save(object sender, RoutedEventArgs e)
    {
            if (fbpresenter == null)
                ShowNoConnectedDBError();
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Database file (*.mbf)|*.mdf" };
                //var result = saveFileDialog.ShowDialog();
                //string path = result.Value();
                if (saveFileDialog.ShowDialog() == true)
                {
                    pathForDB = saveFileDialog.FileName;
                    DataBaseResultRequest?.Invoke(((Button)sender).Content.ToString());

                }

            }
  
            fbpresenter = null;

    }

     
    /// <summary>
    /// Метод загрузки базы данных по нажатию кнопки
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click_Load(object sender, RoutedEventArgs e)
    {

            

            if ((bool)DllRb.IsChecked)
            {
                LogToTextbox(LogManager.type.WARN, "Button_Click_Load_Fired");
                var dialogue = new Microsoft.Win32.OpenFileDialog();
                dialogue.Filter = "Database file (*.mbf)|*.mdf";
                var result = dialogue.ShowDialog();
                if (result == false) return;
                string path = dialogue.FileName;
                if (File.Exists(path))
                {
                    DBmodel = new DataBaseMainModel();
                    DBmodel.path = path;
                    if (fbpresenter == null) fbpresenter = new FileBasePresenter(this, DBmodel);
                    else
                        fbpresenter.changeModel(DBmodel);
                }
              

            }
            else
            {
                System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog();
                var result = browse.ShowDialog();
                string path = browse.SelectedPath;
                if (Directory.Exists(path))
                {
                    DBfromfileModel = new DataBaseFromFile();
                    DBfromfileModel.path = path;

                    if (fbpresenter == null) fbpresenter = new FileBasePresenter(this, DBfromfileModel);
                    else
                        fbpresenter.changeModel(DBfromfileModel);
                }
              
            }



            try
            {
                CallEventDB(((Button)sender).Content.ToString());
            }
            catch (NotUniqueGuidException ex)
            {
                MessageBox.Show("Коллизия ключей, используйте другую базу данных/файлов");
                LogToTextbox(LogManager.type.ERROR, ex.Message);
            }
           
            databaseJson.Items.Refresh();
            databaseBin.Items.Refresh();
            

        }



        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
    {
        RadioButton rb = sender as RadioButton;
        if (rb.Name.Equals("Foreach"))
            mode = false;
        else
            mode = true;
    }



}
}
