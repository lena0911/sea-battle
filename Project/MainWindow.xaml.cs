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
        Button[,] buttons = new Button[10, 10];
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    buttons[i, j] = new Button();
                    Grid.SetRow(buttons[i, j], i + 1);
                    Grid.SetColumn(buttons[i, j], j + 1);
                    buttons[i, j].Click += new RoutedEventHandler(this.Button_Click);
                    GridZone.Children.Add(buttons[i, j]);
                }
            fight.matrixShips[2, 2] = 1; //строка столбец
            fight.matrixShips[2, 1] = 1;
            //fight.matrixShips[2, 0] = 1;
            fight.matrixShips[3, 4] = 1; //строка столбец
            fight.matrixShips[4, 4] = 1;
        }
        private void move()
        {
            int i, j;
            for (i = 0; i < 10; i++)
                for (j = 0; j < 10; j++)
                {
                    if (fight.moveMatrix[i, j] == 1 && fight.moveMatrix[i, j] == fight.matrixShips[i, j])
                        MyControl1_Click_Red(buttons[i, j]);
                    else
                        if (fight.moveMatrix[i, j] == 1)
                        MyControl1_Click_Gray(buttons[i, j]);
                }
        }
        private void checkDestroyedShip()
        {
            int i, j, i1, j1;
            for (i = 0; i < 10; i++)
                for (j = 0; j < 10; j++)
                {
                    if (fight.moveMatrix[i, j] == 1 && fight.moveMatrix[i, j] == fight.matrixShips[i, j])
                    {
                        i1 = i;
                        j1 = j;
                        while (fight.moveMatrix[i1 + 1, j1] == 1 && fight.matrixShips[i1 + 1, j1] == 1)
                            i1++;
                        while (fight.moveMatrix[i1, j1 + 1] == 1 && fight.matrixShips[i1, j1 + 1] == 1)
                            j1++;
                        if (!(i == i1 && j == j1))
                            for (int k = i - 1; k <= i1 + 1; k++)
                                for (int t = j - 1; t <= j1 + 1; t++)
                                {
                                    if (fight.moveMatrix[k, t] == 0)
                                        fight.moveMatrix[k, t] = 1;
                                }
                    }
                }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                int _row = Grid.GetRow((Button)sender);
                int _column = Grid.GetColumn((Button)sender);
                fight.moveMatrix[_row - 1, _column - 1] = 1;
                checkDestroyedShip();
                move();
            }

        }

        private void MyControl1_Click_Red(object sender)
        {
            Button b = (Button)sender;
            b.Background = Brushes.Red;
            int row = Grid.GetColumn(b);
        }
        private void MyControl1_Click_Gray(object sender)
        {
            Button b = (Button)sender;
            b.Background = Brushes.Gray;
            int row = Grid.GetColumn(b);
        }
    }
}
