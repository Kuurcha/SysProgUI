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
using Logic; 

namespace SysProgUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TabItem[] tabArray;
        public MainWindow()
        {
            InitializeComponent();

            //Button btn = new Button();
            //btn.FontWeight = FontWeights.Bold;

            //WrapPanel pnl = new WrapPanel();

            //TextBlock txt = new TextBlock();
            //txt.Text = "Multi";
            //txt.Foreground = Brushes.Blue;
            //pnl.Children.Add(txt);

            //txt = new TextBlock();
            //txt.Text = "Color";
            //txt.Foreground = Brushes.Red;
            //pnl.Children.Add(txt);

            //txt = new TextBlock();
            //txt.Text = "Button";
            //pnl.Children.Add(txt);

            //btn.Content = pnl;
            //btn.Margin = new Thickness(200, 200, 0, 0);
            //btn.Background = Brushes.Magenta;
            //canvas.Children.Add(btn);
            //var fileDialog = new OpenFileDialog();
            //int length = this.MainTabControl.Items.Count;
            var itemTabs = this.MainTabControl.Items;
            List<TabItem> tabList = new List<TabItem>();
            foreach (object item in itemTabs)
                tabList.Add((TabItem)item);
            tabArray = tabList.ToArray();
        }
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("You clicked me at " + e.GetPosition(this).ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            window.WindowState = WindowState.Minimized;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }




        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.SetCurrentValue(ForegroundProperty, new SolidColorBrush(Color.FromRgb(120, 120, 120)));
            foreach (TabItem item in tabArray)
                if (item.Header.Equals(rb.Content))
                        this.MainTabControl.SelectedItem = item;
      
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rb.SetCurrentValue(ForegroundProperty, new SolidColorBrush(Color.FromRgb(219, 220, 230)));

        }

        /// <summary>
        /// Метод отвечающий за изменение подсветки шрифта (свойства Forecolor), и проверяющий на случай если это нажатая Radiobutton цвет менять не надо
        /// </summary>
        /// <param name="sender"> Объект, в котором необходимо изменить</param>
        /// <param name="color"> Цвет который необходимо поставить Contor'у</param>
        public void SetCurrentForecolor (object sender, SolidColorBrush color)
        {
            Control obj = sender as Control;
            var rb = obj as RadioButton;
            if (rb != null)
            {
                if (!(bool)rb.IsChecked)
                    rb.SetCurrentValue(ForegroundProperty, color);
            }
            else
                obj.SetCurrentValue(ForegroundProperty, color);
        }
        private void Universal_MouseEnter(object sender, MouseEventArgs e)
        {
            SetCurrentForecolor(sender, new SolidColorBrush(Color.FromRgb(120, 120, 120)));
        }

        private void Universal_MouseLeave(object sender, MouseEventArgs e)
        {
            SetCurrentForecolor(sender, new SolidColorBrush(Color.FromRgb(219, 220, 230)));
            
        }
    }
}
