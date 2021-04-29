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
        /// 
        /// </summary>
        /// <param name="mainWindow"></param>
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
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            mainWindow.dialogueWindow = null;
        }
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            mainWindow.ObjectForOperation = null;
            this.Close();
        }
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
                    MessageBox.Show("Неверный формат Email!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show("Объект с полями" + Environment.NewLine + "Логин: " + login + Environment.NewLine + "Хеш: " + Hash + Environment.NewLine + "Пароль: " + Password + Environment.NewLine + "Email: " + Email, "Успешно!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    mainWindow.CallEventDB(operation);
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
                mainWindow.CallEventDB(operation);
                this.Close();
            }
        }
    }
}
