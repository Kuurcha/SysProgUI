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
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using Logic;
using Logic.Model;
using SysProgUI.Presenter;
using SysProgUI.IView;
using Newtonsoft.Json;
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

        public void ShowMessageBoxAsm(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        bool mode = false;


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
        public string pathForDB { get; set; }
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
        public string dbMode { 
            get { return checkState() ? ".dll" : ".json"; }
        }

        private FileBasePresenter fbpresenter;
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


            //DataBaseObject temp = new AccessInfo("Kurcha", "Let's pretend this is a hashcode", "password", "yhy@mail.ru");
            DataBaseObject temp = new DllFileInfo("Kurcha", "3.0", DateTime.Now) ;
            using (StreamWriter outputFile = new StreamWriter("D:\\Programming\\testDll.json"))
            {
                string result = JsonConvert.SerializeObject(temp);
                outputFile.WriteLine(result);
            }
        }




        public void CallEventDB(string mode)
        {
            DataBaseResultRequest?.Invoke(mode);
        }

        private void Button_Click_Calc(object sender, RoutedEventArgs e)
        {
            AsmResultRequest?.Invoke();
            
        }
        private void Button_Click_Analyse(object sender, RoutedEventArgs e)
        {
            AnalyseResultRequest?.Invoke();
            
        }






        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.SetCurrentValue(ForegroundProperty, new SolidColorBrush(Color.FromRgb(120, 120, 120)));
            if (tabArray != null)
                foreach (TabItem item in tabArray)
                    if (item.Header.Equals(rb.Content))
                        this.MainTabControl.SelectedItem = item;

        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.SetCurrentValue(ForegroundProperty, new SolidColorBrush(Color.FromRgb(219, 220, 230)));

        }

        public void setOperation(RadioButton rb)
        {
            if (rb.Content != null)
                if (rb.Content.Equals("Умножение")) OpLbl.Content = "X";
                else OpLbl.Content = "\\";
        }
        private void RadioButton_Checked_Asm(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            setOperation(rb);


        }

        private void RadioButton_Unchecked_Asm(object sender, RoutedEventArgs e)
    {
        RadioButton rb = sender as RadioButton;
        setOperation(rb);

    }



    public void setControlColor(Control element, SolidColorBrush color)
    {
        element.SetCurrentValue(ForegroundProperty, color);
    }

  
    private void Button_Click_Add(object sender, RoutedEventArgs e)
    {
            if (fbpresenter == null) 
                MessageBox.Show("Не подключена база данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (checkState())
                {
                    {
                        dialogueWindow = new EditDialogue(this, checkState(), ((Button)sender).Content.ToString());
                        dialogueWindow.Show();
                    }
                }
                else
                {

                    var dialogue = new Microsoft.Win32.OpenFileDialog() { Filter = "Json Files (*.Json)|*.json" };
                    var result = dialogue.ShowDialog();
                    if (result == false) return;
                    string path = dialogue.FileName;
                    if (File.Exists(path))
                    {
                        string resultJsonString = File.ReadAllText(path);
                        ObjectForOperation = JsonConvert.DeserializeObject<DllFileInfo>(resultJsonString);
                        CallEventDB("Добавить");
                        //else
                        //    MessageBox.Show("Неудалось обработать Json файл", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error)

                    }
                    else MessageBox.Show("Файла не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }   
    }

    public void SetCurrentlySelectedObject()
    {
        if (checkState())
            ObjectForOperation = (AccessInfo)databaseBin.SelectedItem;
        else
            ObjectForOperation = (DllFileInfo)databaseJson.SelectedItem;
    }
    private void Button_Click_Delete(object sender, RoutedEventArgs e)
     {
            if (fbpresenter == null)
                MessageBox.Show("Не подключена база данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                SetCurrentlySelectedObject();
                if (ObjectForOperation != null)
                    DataBaseResultRequest?.Invoke(((Button)sender).Content.ToString());
            }
          
    }
        public EditDialogue dialogueWindow = null;
    private void Button_Click_Modify(object sender, RoutedEventArgs e)
    {
            if (fbpresenter == null)
                MessageBox.Show("Не подключена база данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
    private void Button_Click_Save(object sender, RoutedEventArgs e)
    {
            if (fbpresenter == null)
                MessageBox.Show("Не подключена база данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

    private void Button_Click_Load(object sender, RoutedEventArgs e)
    {
            var dialogue = new Microsoft.Win32.OpenFileDialog() { Filter = "Database file (*.mbf)|*.mdf" };
            var result = dialogue.ShowDialog();
            if (result == false) return;
            string path = dialogue.FileName;
            DataBaseMainModel temp = new DataBaseMainModel();
            temp.path = path;
            fbpresenter = new FileBasePresenter(this, temp);
            CallEventDB(((Button)sender).Content.ToString());
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
