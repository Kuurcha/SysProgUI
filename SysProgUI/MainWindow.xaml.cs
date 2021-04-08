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

namespace SysProgUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //InitializeComponent();
         
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
    }
}
