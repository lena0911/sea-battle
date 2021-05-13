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

namespace Project //для хранения массива кораблей использ коллекцию
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Ship[] ships = new Ship[4];
        Zone fight = new Zone("Lena");
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
            ships[0] = new Ship(1, 0, 0, false, fight);
            ships[1] = new Ship(2, 8, 1, true, fight);
            ships[2] = new Ship(3, 6, 5, false, fight);
            ships[3] = new Ship(4, 2, 2, true, fight);
        }
        private void move()//пересчет матрицы после очередного выстрела
        {
            int i, j;
            for (i = 0; i < 10; i++)
                for (j = 0; j < 10; j++)
                {
                    if (fight.moveMatrix[i, j] == 1 && fight.matrixShips[i, j] == 1)
                        MyControl1_Click_Red(buttons[i, j]);
                    else
                        if (fight.moveMatrix[i, j] == 1)
                        MyControl1_Click_Gray(buttons[i, j]);
                }
        }
        public void coloringAfterKillingTheShip(int x, int y) //покраска после того, как целый корабль был потоплен
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (checkingCellsNearby(x+i, y+j))
                        fight.moveMatrix[x + i, y + j] = 1;
        }
        private bool checkingCellsNearby(int x, int y) //проверка выхода за пределы поля
        {
            if (x < 0 || x > 9 || y < 0 || y > 9)
                return false;
            else
                return true;
        }
    
        private void Button_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                int _row = Grid.GetRow((Button)sender);
                int _column = Grid.GetColumn((Button)sender);
                fight.moveMatrix[_row - 1, _column - 1] = 1;
                if (fight.matrixShips[_row - 1, _column - 1] == 1)
                {
                    int j = 0;
                    while (!ships[j].searchShip(_row - 1, _column - 1))
                        j++;
                    if (ships[j].checkDestroyedShip()) //если корабль уничтожен, то обрисовываем вокруг него
                    {
                        for(int i=0; i<ships[j].Length; i++)
                            coloringAfterKillingTheShip(ships[j].Coordinates[i].X, ships[j].Coordinates[i].Y);
                    }
                }
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
