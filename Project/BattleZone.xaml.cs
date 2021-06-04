using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using seaBattle_Library;

namespace Project //для хранения массива кораблей использ коллекцию
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class BattleZone : Window
    {
        private List<Ship> ships1 = new List<Ship>();
        private Zone fight1;
        private List<Ship> ships2 = new List<Ship>();
        private Zone fight2;
        private List<Ship> ships = new List<Ship>();
        private Zone fight; //зона, на которую происходит текущая атака
        private Button[,] buttonsLeft = new Button[10, 10];
        private Button[,] buttonsRight = new Button[10, 10];
        private List<MyVector> coordinates = new List<MyVector>();
        public string name1;
        public string name2;
        public string nameWin;
        private int step = 1;
        private bool bot;
        private bool part = false;
        private bool flagBotHit = false; //флаг - подбит ли корабль
        private int rowBotHit = -1; //строка попадания бота
        private int columnBotHit = -1; //столбец попадания бота
        private int indexShipBotHit = -1; //индекс подбитого ботом корабля
        private int orientationShipBotHit = -1; //1-корабль расположен вертикально, 2-горизонтально, -1 - иначе
        private bool watch = false;
        private bool flag=false;

        public BattleZone(List<Ship> ships1, Zone fight1, List<Ship> ships2, Zone fight2, string name1, string name2, bool bot, int step)
        {
            InitializeComponent();
            ImageBrush imBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../images/battle.jpg"))
            };
            this.Background = imBrush;
            this.ships1 = ships1;
            this.fight1 = fight1;
            this.ships2 = ships2;
            this.fight2 = fight2;
            this.name1 = name1;
            this.name2 = name2;
            this.bot = bot;
            this.step = step;
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
     
            if (bot) //заполнение координат матрицы для первого круга ударов бота
            {
                int i = 0, j = 0; ;

                for (i = 0; i < 10; i++)
                    for (j = 0; j < 10; j++)
                    {
                        if (fight1.matrixShips[i, j] == 1)
                            MyControl1_Click_Salmon(buttonsLeft[i, j]);
                    }
                i = 0;
                for (i = 3; i >= 0; i--)
                    coordinates.Add(new MyVector(i, 3 - i));
                for (i = 7; i >= 0; i--)
                    coordinates.Add(new MyVector(i, 7 - i));
                for (i = 9; i >= 6; i--)
                    coordinates.Add(new MyVector(i, 6 + 9 - i));
                for (i = 9; i >= 2; i--)
                    coordinates.Add(new MyVector(i, 2 + 9 - i));
            }
            moveLeft();
            moveRight();
            if (step % 2 != 0)
                labelMove.Content = name1;
            else
                labelMove.Content = name2;
            this.Closing += BattleZone_Closing;
        }
        private void BattleZone_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (flag)
                return;
            MessageBoxResult message= MessageBox.Show("Сохранить игру перед закрытием?", "",MessageBoxButton.YesNoCancel,MessageBoxImage.Question);
            if(message==MessageBoxResult.Yes)
                save_Click(null, null);
            if(message == MessageBoxResult.Cancel)
                e.Cancel=true;
        }       
        public int randGeneration(int i, int j)
        {
            Random rnd = new Random();
            return rnd.Next(i, j);
        }
        public void secondFightBot()
        {
            int i;
            coordinates.Add(new MyVector(0, 1));
            coordinates.Add(new MyVector(1, 0));
            coordinates.Add(new MyVector(9, 8));
            coordinates.Add(new MyVector(8, 9));
            for (i = 5; i >= 0; i--)
                coordinates.Add(new MyVector(i, 5 - i));
            for (i = 9; i >= 4; i--)
                coordinates.Add(new MyVector(i, 13 - i));
            for (i = 9; i >= 0; i--)
                coordinates.Add(new MyVector(i, 9 - i));
        }
        public void lastFightBot()
        {
            int i,j;
            for (i = 0; i <= 9; i++)
            {
                if (i % 2 == 0)
                    j = 0;
                else
                    j = 1;
                while (checkingCellsNearby(i, j))
                {
                    coordinates.Add(new MyVector(i, j));
                    j += 2;
                }
            }
        }
        public bool hitBot(int row, int column)
        {
            fight1.moveMatrix[row, column] = 1;
            if (fight1.matrixShips[row, column] == 0)
                return false;

            return true;
        }
        public bool checkBotWin()
        {
            int i;
            for (i = 0; i < ships1.Count; i++)
                if (ships1[i].checkDestroyedShip() != true)
                    break;
            if (i == ships1.Count)
            {
                nameWin = name2;
                return true;
            }
            return false;
        }
        public void showNotKilledShips()
        {
            for (int i = 0; i < ships1.Count; i++)
                if (!ships1[i].checkDestroyedShip())
                    for (int j = 0; j < ships1[i].Length; j++)
                        MyControl1_Click_Salmon(buttonsLeft[ships1[i].Coordinates[j].X, ships1[i].Coordinates[j].Y]);
            for (int i = 0; i < ships2.Count; i++)
                if (!ships2[i].checkDestroyedShip())
                    for (int j = 0; j < ships2[i].Length; j++)
                        MyControl1_Click_Salmon(buttonsRight[ships2[i].Coordinates[j].X, ships2[i].Coordinates[j].Y]);
            fightLabel.Content = "Бой завершился!";
            result.Content = "Победу одержал";
            labelMove.Content = nameWin;
            congrat.Content = "Поздравляем!";
        }
        public void showWinner()
        {
            Win win = new Win();
            win.winLabel.Content = "Вы проиграли!";
            win.winner.Content = " ";
            this.Hide();
            win.ShowDialog();
            this.Show();
            if (win.closeWin || win.restart)
            {
                flag = true;
                this.Close();
            }
            restart.Visibility = Visibility.Visible;
            showNotKilledShips();
            watch = true;
        }
        public int afterBotHit() //добивание корабля, когда бот попал по нему
        {
            if (watch)
                return 0;
            int column = columnBotHit, row = rowBotHit;
            if (orientationShipBotHit == -1) //если не знаем как расположен корабль
            {
                if (checkingCellsNearby(row, column + 1) && fight1.moveMatrix[row, column + 1] == 0)
                    column++;
                else
                    if (checkingCellsNearby(row, column - 1) && fight1.moveMatrix[row, column - 1] == 0)
                    column--;
                else
                    if (checkingCellsNearby(row + 1, column) && fight1.moveMatrix[row + 1, column] == 0)
                    row++;
                else
                    if (checkingCellsNearby(row - 1, column) && fight1.moveMatrix[row - 1, column] == 0)
                    row--;
            }
            else
            {
                int i;
                if (orientationShipBotHit == 1) //если корабль расположен вертикально
                {
                    for (i = 1; ships1[indexShipBotHit].checkDestroyedShip() != true; i++)
                        if (!checkingCellsNearby(row + i, column) || (fight1.moveMatrix[row + i, column] == 1 && fight1.matrixShips[row + i, column] == 0))
                            break;
                        else
                            if (hitBot(row + i, column) == false)
                            return 1;
                        else
                        {
                            ships1[indexShipBotHit].searchShip(row + i, column);
                            waitBotMove();
                            moveLeft();
                        }
                    for (i = 1; ships1[indexShipBotHit].checkDestroyedShip() != true; i++)
                        if (!checkingCellsNearby(row - i, column) || (fight1.moveMatrix[row - i, column] == 1 && fight1.matrixShips[row - i, column] == 0))
                            break;
                        else
                             if (hitBot(row - i, column) == false)
                            return 1;
                        else
                        {
                            ships1[indexShipBotHit].searchShip(row - i, column);
                            waitBotMove();
                            moveLeft();
                        }
                }
                if (orientationShipBotHit == 2) //если корабль расположен горизонтально
                {
                    for (i = 1; ships1[indexShipBotHit].checkDestroyedShip() != true; i++)
                        if (!checkingCellsNearby(row, column + i) || (fight1.moveMatrix[row, column + i] == 1 && fight1.matrixShips[row, column + i] == 0))
                            break;
                        else
                            if (hitBot(row, column + i) == false)
                            return 1;
                        else
                        {
                            ships1[indexShipBotHit].searchShip(row, column + i);
                            waitBotMove();
                            moveLeft();
                        }
                    for (i = 1; ships1[indexShipBotHit].checkDestroyedShip() != true; i++)
                        if (!checkingCellsNearby(row, column - i) || (fight1.moveMatrix[row, column - i] == 1 && fight1.matrixShips[row, column - i] == 0))
                            break;
                        else
                             if (hitBot(row, column - i) == false)
                            return 1;
                        else
                        {
                            ships1[indexShipBotHit].searchShip(row, column - i);
                            waitBotMove();
                            moveLeft();
                        }
                }
                waitBotMove();
                for (i = 0; i < ships1[indexShipBotHit].Length; i++)
                    coloringAfterKillingTheShipBot(ships1[indexShipBotHit].Coordinates[i].X, ships1[indexShipBotHit].Coordinates[i].Y);
                moveLeft();
                flagBotHit = false;
                rowBotHit = -1;
                columnBotHit = -1;
                indexShipBotHit = -1;
                orientationShipBotHit = -1;
                if (checkBotWin())
                    showWinner();
                return 0;
            }
            waitBotMove();
            fight1.moveMatrix[row, column] = 1;
            if (fight1.matrixShips[row, column] == 0) //если бот не попал, то ход переходит к другому игроку
            {
                moveLeft();
                return 1;
            }
            ships1[indexShipBotHit].searchShip(row, column);
            if (ships1[indexShipBotHit].checkDestroyedShip() == true)
            {
                int i = 0;
                for (i = 0; i < ships1[indexShipBotHit].Length; i++)
                    coloringAfterKillingTheShipBot(ships1[indexShipBotHit].Coordinates[i].X, ships1[indexShipBotHit].Coordinates[i].Y);
                moveLeft();
                flagBotHit = false;
                rowBotHit = -1;
                columnBotHit = -1;
                indexShipBotHit = -1;
                orientationShipBotHit = -1;
                if (checkBotWin())
                {
                    showWinner();
                    return 0;
                }
                return 0;
            }
            if (orientationShipBotHit == -1)//определение ориентации корабля
                if (columnBotHit == column) //если попадание в том же столбце, то вертикально
                    orientationShipBotHit = 1;
                else
                    if (rowBotHit == row)
                    orientationShipBotHit = 2;
            waitBotMove();
            afterBotHit();
            return 0;
        }
        public async void waitBotMove()
        {
            await Task.Run(() => System.Threading.Thread.Sleep(800));
        }
        private int botMove()
        {
            if (watch)
                return 0;
            int i;
            if (checkBotWin())
            {
                showWinner();
                return 0;
            }
            if (coordinates.Count == 0) //если лист клеток текущего этапа пуст, то задаем лист с новыми клетками
            {
                if (!part)
                {
                    secondFightBot();
                    part = true;
                }
                else
                    lastFightBot();
            }
            if (coordinates.Count == 1)
                i = 0;
            else
                i = randGeneration(0, coordinates.Count);
            int _row = coordinates[i].X;
            int _column = coordinates[i].Y;
            while (fight1.moveMatrix[_row, _column] == 1) //пропускам клетки, по которым уже были удары
            {
                coordinates.RemoveAt(i);
                botMove();
                return 0;
            }
            waitBotMove();
            fight1.moveMatrix[_row, _column] = 1; //фиксируем удар бота
            coordinates.RemoveAt(i);
            if (fight1.matrixShips[_row, _column] == 1)//если бот попал
            {
                moveLeft();
                flagBotHit = true;
                int j = 0;
                rowBotHit = _row; //фиксируем место попадания
                columnBotHit = _column;
                while (!ships1[j].searchShip(_row, _column)) //ищем корабль, по которому бот попал
                    j++;
                indexShipBotHit = j;
                if (ships1[j].checkDestroyedShip())
                {
                    for (i = 0; i < ships1[indexShipBotHit].Length; i++)
                        coloringAfterKillingTheShipBot(ships1[indexShipBotHit].Coordinates[i].X, ships1[indexShipBotHit].Coordinates[i].Y);
                    flagBotHit = false;
                    rowBotHit = -1;
                    columnBotHit = -1;
                    indexShipBotHit = -1;
                    orientationShipBotHit = -1;
                    waitBotMove();
                    moveLeft();
                    botMove();
                    return 0;
                }
                int k = afterBotHit();//добиваем корабль
                if (k == 1) //если бот не попал, то ходит другой игрок
                {
                    step++;
                    waitBotMove();
                    moveLeft();
                    return 0;
                }
                botMove();
                return 0;
            }
            moveLeft(); //пересчет матрицы после очередного выстрела            
            if (checkBotWin())
            {
                showWinner();
                return 0;
            }
            step++;
            return 0;
        }
        private void moveRight()//пересчет матрицы после очередного выстрела
        {
            int i, j;
            for (i = 0; i < 10; i++)
                for (j = 0; j < 10; j++)
                {
                    if (fight2.moveMatrix[i, j] == 1 && fight2.matrixShips[i, j] == 1)
                        MyControl1_Click_Red(buttonsRight[i, j]);
                    else
                        if (fight2.moveMatrix[i, j] == 1)
                        MyControl1_Click_Gray(buttonsRight[i, j]);
                }
        }
        private void moveLeft()//пересчет матрицы после очередного выстрела
        {
            int i, j;
            for (i = 0; i < 10; i++)
                for (j = 0; j < 10; j++)
                {
                    if (fight1.moveMatrix[i, j] == 1 && fight1.matrixShips[i, j] == 1)
                        MyControl1_Click_Red(buttonsLeft[i, j]);
                    else
                        if (fight1.moveMatrix[i, j] == 1)
                        MyControl1_Click_Gray(buttonsLeft[i, j]);
                }
        }
        public void coloringAfterKillingTheShip(int x, int y) //покраска после того, как целый корабль был потоплен
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (checkingCellsNearby(x + i, y + j))
                        fight.moveMatrix[x + i, y + j] = 1;
        }
        public void coloringAfterKillingTheShipBot(int x, int y) //покраска после того, как целый корабль был потоплен
        {
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (checkingCellsNearby(x + i, y + j))
                        fight1.moveMatrix[x + i, y + j] = 1;
        }
        private bool checkingCellsNearby(int x, int y) //проверка выхода за пределы поля
        {
            if (x < 0 || x > 9 || y < 0 || y > 9)
                return false;
            else
                return true;
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            int i;
            if (sender != null)
            {
                if (watch)
                    return;
                int _row = 0, _column = 0;
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
                _row = Grid.GetRow((Button)sender) - 1;
                _column = Grid.GetColumn((Button)sender) - 1;
                if (fight.moveMatrix[_row, _column] == 1)
                    return;
                fight.moveMatrix[_row, _column] = 1;
                if (fight.matrixShips[_row, _column] == 1)
                {
                    int j = 0;
                    while (!ships[j].searchShip(_row, _column))
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
                {
                    Win win = new Win();
                    if (step % 2 == 0)
                    {
                        win.winner.Content = name2;
                        nameWin = name2;
                    }
                    else
                        if (bot)
                        {
                            win.winLabel.Content = "Вы победили!";
                            win.winner.Content = " ";
                        }
                        else
                        {
                            win.winner.Content = name1;
                            nameWin = name1;
                        }
                    this.Hide();
                    win.ShowDialog();
                    this.Show();
                    if (win.restart || win.closeWin)
                    {
                        flag = true;
                        this.Close();
                    }
                    watch = true;
                    restart.Visibility = Visibility.Visible;
                    showNotKilledShips();
                    return;
                }
                if (fight.matrixShips[_row, _column] != 1) //ход другого игрока, при условии, что попадания не было
                {
                    step++;
                    if (step % 2 != 0)
                        labelMove.Content = name1;
                    else
                        labelMove.Content = name2;
                }
                if (step % 2 == 0 && bot) //ход бота
                {
                    if (!watch)
                        labelMove.Content = name2;
                    await Task.Run(() => System.Threading.Thread.Sleep(200));
                    fight = fight1;
                    if (flagBotHit) //если есть  корабль, который подбит, но не уничтожен
                    {
                        if (afterBotHit() == 1)//добивание корабля
                            step++;
                        else
                            botMove();
                    }
                    else
                        botMove(); //рандомный удар бота
                    if (!watch)
                        labelMove.Content = name1;
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
            b.Background = Brushes.CadetBlue;
            int row = Grid.GetColumn(b);
        }
        private void MyControl1_Click_Salmon(object sender)
        {
            Button b = (Button)sender;
            b.Background = Brushes.Salmon;
            int row = Grid.GetColumn(b);
        }
        private void restert_Click(object sender, RoutedEventArgs e)
        {
            flag = true;
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text documents (.txt)|*.txt";
            string CombinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\saves");
            saveFileDialog1.InitialDirectory = System.IO.Path.GetFullPath(CombinedPath);
            if (saveFileDialog1.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, System.Text.Encoding.Default))
                {
                    if (bot)
                        sw.WriteLine(1);
                    else
                        sw.WriteLine(0);
                    sw.WriteLine(name1);
                    for(int i=0; i<10; i++)
                    {
                        sw.WriteLine(ships1[i].Length + " " + ships1[i].Coordinates[0].X + " " + ships1[i].Coordinates[0].Y + " " + ships1[i].orientation);
                    }
                    for(int i=0; i<10; i++)
                    {
                        for(int j=0; j<10; j++)
                            sw.Write(fight1.moveMatrix[i, j] + " ");
                        sw.WriteLine();
                    }
                    sw.WriteLine(name2);
                    for (int i = 0; i < 10; i++)
                    {
                        sw.WriteLine(ships2[i].Length + " " + ships2[i].Coordinates[0].X + " " + ships2[i].Coordinates[0].Y + " " + ships2[i].orientation);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                            sw.Write(fight2.moveMatrix[i, j] + " ");
                        sw.WriteLine();
                    }
                    sw.WriteLine(step);

                }
            }
        }

        private void goToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Process.Start(path);
            Process.GetCurrentProcess().Kill();
        }
    }
}
