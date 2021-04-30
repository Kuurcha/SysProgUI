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
using Microsoft.Win32;



namespace SysProgUI.WPF.Resource
{
    partial class Dictionary : ResourceDictionary
    {
        /// <summary>
        /// Метод отвечающий за изменение подсветки шрифта (свойства Forecolor), и проверяющий на случай если это нажатая Radiobutton цвет менять не надо
        /// </summary>
        /// <param name="sender"> Объект, в котором необходимо изменить</param>
        /// <param name="color"> Цвет который необходимо поставить Contor'у</param>
        public void SetCurrentForecolor(object sender, SolidColorBrush color)
        {
            Control obj = sender as Control;
            var rb = obj as RadioButton;
            if (rb != null)
            {
                if (!(bool)rb.IsChecked)
                    rb.SetCurrentValue(Control.ForegroundProperty, color);
            }
            else
                obj.SetCurrentValue(Control.ForegroundProperty, color);
        }
        /// <summary>
        /// Метод отвечающий за правильный цвет кнопок при начале выделения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Universal_MouseEnter(object sender, MouseEventArgs e)
        {
            SetCurrentForecolor(sender, new SolidColorBrush(Color.FromRgb(120, 120, 120)));
        }
        /// <summary>
        /// Метод отвечающий за правильный цвет кнопок при конце выделения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Universal_MouseLeave(object sender, MouseEventArgs e)
        {
            SetCurrentForecolor(sender, new SolidColorBrush(Color.FromRgb(219, 220, 230)));

        }
        /// <summary>
        /// Метод кнопки закрытия
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
           Window.GetWindow((Control)sender).Close();
         
        }

        /// <summary>
        /// Метод кнопки сворачивания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window.GetWindow((Control)sender).WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Метод отвечающий за корректный перенос интерфейса мышкой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window temp = Window.GetWindow((UIElement)sender);
            if (e.ChangedButton == MouseButton.Left)
                temp.DragMove();
        }
    }
}
