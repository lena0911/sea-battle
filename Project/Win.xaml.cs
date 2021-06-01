using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using seaBattle_Library;
namespace Project
{
    /// <summary>
    /// Логика взаимодействия для Win.xaml
    /// </summary>
    public partial class Win : Window
    {
        public bool restart = false;
        public bool wathcing = false;
        public bool closeWin = false;
        public Win()
        {           
            ImageBrush imBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../images/win.jpg"))
            };
            this.Background = imBrush;    
            InitializeComponent();
        }       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            restart = true;
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            wathcing = true;
            this.Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            closeWin = true;
            this.Close();
        }
    }
}
