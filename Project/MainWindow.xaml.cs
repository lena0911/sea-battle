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

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Zone fight = new Zone();
        public MainWindow()
        {
            InitializeComponent();
            
            for (int i = 1; i < 11; i++)
                for (int j = 1; j < 11; j++)
                {
                    Button button = new Button();
                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);
                    button.Click += new RoutedEventHandler(this.Button_Click);
                    GridZone.Children.Add(button);
                }
            fight.matrixShips[3, 3] = 1;
            fight.matrixShips[3, 2] = 1;
        }
       private void Button_Click(object sender, EventArgs e)
        {
            int i, j;
            if (sender != null)
            {
                int _row = Grid.GetRow((Button)sender);
                int _column =  Grid.GetColumn((Button)sender);
                fight.moveMatrix[_row, _column] = 1;
                for(i=1; i<11; i++)
                    for(j=1; j<11; j++)
                    {
                       
                        Button button = new Button();
                       // Grid.Children.Add(button);
                        Grid.SetColumn(button, i);
                        Grid.SetRow(button, j);
                        if (fight.moveMatrix[i, j] == fight.matrixShips[i, j])                          
                            MyControl1_Click_Red(button, e);
                        else
                            MyControl1_Click_Gray(button, e);
                       
                        button = null;
                      //  Grid.Children.Remove(button);
                    }
            }

        }

        private void MyControl1_Click_Red(object sender, EventArgs e)
        {       
            Button b = (Button)sender;
            b.Background = Brushes.Red;
            int row = Grid.GetColumn(b);
        }
        private void MyControl1_Click_Gray(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.Background = Brushes.Gray;
            int row = Grid.GetColumn(b);
        }
    }
}
