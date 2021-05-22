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
    public partial class BattleZone : Window
    {
        List<Ship>ships1=new List<Ship>();
        Zone fight1;
        List<Ship> ships2 = new List<Ship>();
        Zone fight2;
        List<Ship> ships = new List<Ship>();
        Zone fight;
        Button[,] buttonsLeft = new Button[10, 10];
        Button[,] buttonsRight = new Button[10, 10];
        public string name1;
        public string name2;
        int step = 1;
      
        public BattleZone(List<Ship> ships1, Zone fight1, List<Ship> ships2, Zone fight2, string name1, string name2)
        {
            
            InitializeComponent();
            this.ships1 = ships1;
            this.fight1 = fight1;
            this.ships2 = ships2;
            this.fight2 = fight2;
            this.name1 = name1;
            this.name2 = name2;
            labelPlayer1.Content = name1;
            labelPlayer2.Content = name2;
            labelMove.Content = name1;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    buttonsLeft[i, j] = new Button();
                    Grid.SetRow(buttonsLeft[i, j], i + 1);
                    Grid.SetColumn(buttonsLeft[i, j], j + 1);
                    buttonsLeft[i, j].Click += new RoutedEventHandler(this.Button_Click);
                    GridZoneLeft.Children.Add(buttonsLeft[i, j]);
                }
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    buttonsRight[i, j] = new Button();
                    Grid.SetRow(buttonsRight[i, j], i + 1);
                    Grid.SetColumn(buttonsRight[i, j], j + 1);
                    buttonsRight[i, j].Click += new RoutedEventHandler(this.Button_Click);
                    GridZoneRight.Children.Add(buttonsRight[i, j]);
                }
          
        }
        private void moveRight()//пересчет матрицы после очередного выстрела
        {
            int i, j;
            for (i = 0; i < 10; i++)
                for (j = 0; j < 10; j++)
                {
                    if (fight.moveMatrix[i, j] == 1 && fight.matrixShips[i, j] == 1)
                        MyControl1_Click_Red(buttonsRight[i, j]);
                    else
                        if (fight.moveMatrix[i, j] == 1)
                        MyControl1_Click_Gray(buttonsRight[i, j]);
                }
        }
        private void moveLeft()//пересчет матрицы после очередного выстрела
        {
            int i, j;
            for (i = 0; i < 10; i++)
                for (j = 0; j < 10; j++)
                {
                    if (fight.moveMatrix[i, j] == 1 && fight.matrixShips[i, j] == 1)
                        MyControl1_Click_Red(buttonsLeft[i, j]);
                    else
                        if (fight.moveMatrix[i, j] == 1)
                        MyControl1_Click_Gray(buttonsLeft[i, j]);
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
            int i;
            if (sender != null)
            {
                if (step % 2 == 1 && GridZoneLeft.Children.Contains(sender as Button) || step % 2 == 0 && GridZoneRight.Children.Contains(sender as Button))
                {
                    MessageBox.Show("Вы не можете убивать свои корабли!");
                    return;
                }
                if (step % 2 == 0) //step - четное -> ходит 2 игрок
                {
                    fight = fight1;
                    ships = ships1;
                }
                else //step - нечетное -> ходит 1 игрок
                {
                    fight = fight2;
                    ships = ships2;
                }
                int _row = Grid.GetRow((Button)sender);
                int _column = Grid.GetColumn((Button)sender);
                if (fight.moveMatrix[_row - 1, _column - 1] == 1)
                    return;
                fight.moveMatrix[_row - 1, _column - 1] = 1;
                if (fight.matrixShips[_row - 1, _column - 1] == 1)
                {
                    int j = 0;
                    while (!ships[j].searchShip(_row - 1, _column - 1))
                        j++;
                    if (ships[j].checkDestroyedShip()) //если корабль уничтожен, то обрисовываем вокруг него
                    {
                        for (i = 0; i < ships[j].Length; i++)
                            coloringAfterKillingTheShip(ships[j].Coordinates[i].X, ships[j].Coordinates[i].Y);
                    }
                }
                if (step % 2 == 0)
                    moveLeft();
                else
                    moveRight();

                for (i = 0; i < ships.Count; i++)
                    if (ships[i].checkDestroyedShip() != true)
                        break;
                if (i == ships.Count)
                    if (step % 2 == 0)
                    {
                        MessageBox.Show("Победа игрока 2!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Победа игрока 1!");
                        this.Close();
                    }
                if (fight.matrixShips[_row - 1, _column - 1] != 1) //ход другого играка, при условии, что попадания не было
                {
                    step++;
                    if (step % 2 != 0)
                        labelMove.Content = name1;
                    else
                        labelMove.Content = name2;
                }
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
