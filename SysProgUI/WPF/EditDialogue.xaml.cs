using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Linq;
using Logic.Model;
using Logic;
namespace SysProgUI
{
    /// <summary>
    /// Interaction logic for EditDialogue.xaml
    /// </summary>
    public partial class EditDialogue : Window
    {
        MainWindow mainWindow;
        bool mode = false;
        string operation = null;
        /// <summary>
        /// Метод инициализации диалогового окна
        /// </summary>
        /// <param name="mainWindow">Экземпляр основного окна</param>
        /// <param name="mode">true = AccessInfo/False = DllFileInfo</param>
        public EditDialogue(MainWindow mainWindow, bool mode, string operation)
        {

            InitializeComponent();
            this.mode = mode;
            this.mainWindow = mainWindow;
            this.operation = operation;
            if (mode)
            {
                FirstLbl.Content = "Логин";
                SecondLbl.Content = "Хеш-код";
                ThirdLbl.Content = "Пароль";
                FourthLbl.Content = "Email";
                FourthLbl.Visibility = Visibility.Visible;
                FourthTB.Visibility = Visibility.Visible;
                if (operation == "Изменить")
                {
                    var tempObject = ((AccessInfo)(mainWindow.ObjectForOperation));
                    FirstTB.Text = tempObject.Login;
                    SecondTB.Text = tempObject.Hashcode;
                    ThirdTB.Text = tempObject.Password;
                    FourthTB.Text = tempObject.Email;
                }
            }
            else
            {
                FirstLbl.Content = "Имя файла";
                SecondLbl.Content = "Версия файла";
                ThirdLbl.Content = "Дата последнего редактирования";
                FourthLbl.Visibility = Visibility.Hidden;
                FourthTB.Visibility = Visibility.Hidden;
                if (operation == "Изменить")
                {
                    var tempObject = ((DllFileInfo)(mainWindow.ObjectForOperation));
                    FirstTB.Text = tempObject.FileName;
                    SecondTB.Text = tempObject.FileVersion;
                    ThirdTB.Text = tempObject.DateOfLastEdit.ToString();
                }
            }
    
     
           
        }
        /// <summary>
        /// Метод стирающий экземпляр dialogueWindow из основного окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            mainWindow.dialogueWindow = null;
        }
        /// <summary>
        /// Метод корректного закрытия окна на кнопку отмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            mainWindow.ObjectForOperation = null;
            this.Close();
        }
        /// <summary>
        /// Метод кнопки потверждения и заполнение полей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Confirm(object sender, RoutedEventArgs e)
        {
            if (mode)
            {
                string login = FirstTB.Text;
                string Hash = SecondTB.Text;
                string Password = ThirdTB.Text;
                string Email = FourthTB.Text;
                bool testEmail = (Email.Contains('@') && Email.Contains('.')) && (Email.ToCharArray().Count(x => x == '@') == 1) && (Email.ToCharArray().Count(x => x == '.') == 1);
                if (!testEmail)
                {
                    string errMes = "Неверный формат Email!";
                    MessageBox.Show(errMes, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    mainWindow.LogToTextbox(LogManager.type.ERROR,errMes);
                    mainWindow.ObjectForOperation = null;
                    
                }
                   
                else
                {
                    if (operation == "Добавить")
                        mainWindow.ObjectForOperation = new AccessInfo(login, Hash, Password, Email);
                    else
                    {
                        var tempObject = ((AccessInfo)(mainWindow.ObjectForOperation));
                        tempObject.Email = Email;
                        tempObject.Hashcode = Hash;
                        tempObject.Password = Password;
                        tempObject.Login = login;
                        mainWindow.ObjectForOperation = tempObject;
                    }
                    string sucMes = "Объект с полями "  + "Логин: " + login +  " Хеш: " + Hash +  " Пароль: " + Password + " Email: " + Email;
                    MessageBox.Show(sucMes, "Успешно!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    mainWindow.CallEventDB(operation);
                    mainWindow.LogToTextbox(LogManager.type.INFO, sucMes );
                    this.Close();
                }    
            }
            else
            {
                string FileName = FirstTB.Text;
                string FileVersion = SecondTB.Text;
                string EditDate = ThirdTB.Text;
                var tempObject = ((DllFileInfo)(mainWindow.ObjectForOperation));
                tempObject.FileName = FileName;
                tempObject.FileVersion = FileVersion;
                tempObject.DateOfLastEdit = DateTime.Parse(EditDate);
                mainWindow.ObjectForOperation = tempObject;
                string sucMes = "Объект " + tempObject.ToString();
                MessageBox.Show(sucMes, "Успешно!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                mainWindow.CallEventDB(operation);
                mainWindow.LogToTextbox(LogManager.type.INFO, sucMes);
                this.Close();
            }
        }
    }
}
