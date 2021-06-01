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
    /// Логика взаимодействия для Arrangement.xaml
    /// </summary>
    public partial class Arrangement : Window
    {
        public string name;
        public List<Ship> ships = new List<Ship>();
        private Button[,] buttons = new Button[10, 10];
        private int pal1 = 0, pal2 = 0, pal3 = 0, pal4 = 0;
        public bool closeWind = false;
        public Arrangement()
        {
            InitializeComponent();
            ImageBrush imBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../images/arrang.jpg"))
            };
            this.Background = imBrush;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    buttons[i, j] = new Button();
                    Grid.SetRow(buttons[i, j], i + 1);
                    Grid.SetColumn(buttons[i, j], j + 1);
                    buttons[i, j].Click += new RoutedEventHandler(this.Button_Click);
                    GridZone.Children.Add(buttons[i, j]);
                }
        }
        private bool CheckingCellsNearby(int x, int y) //проверка выхода за пределы поля
        {
            if (x < 0 || x > 9 || y < 0 || y > 9)
                return false;
            else
                return true;
        }

        private void CheckVisibleRadioButtons() //проверка допустимого кол-ва нажатий на радиокнопку
        {
            if (pal4 == 1)
                pal_4.IsEnabled = false;
            else
                pal_4.IsEnabled = true;
            if (pal1 == 4)
                pal_1.IsEnabled = false;
            else
                pal_1.IsEnabled = true;
            if (pal2 == 3)
                pal_2.IsEnabled = false;
            else
                pal_2.IsEnabled = true;
            if (pal3 == 2)
                pal_3.IsEnabled = false;
            else
                pal_3.IsEnabled = true;
            if (pal_1.IsChecked == false && pal_1.IsEnabled == true)
                pal_1.IsChecked = true;
            if (pal_2.IsChecked == false && pal_2.IsEnabled == true)
                pal_2.IsChecked = true;
            if (pal_3.IsChecked == false && pal_3.IsEnabled == true)
                pal_3.IsChecked = true;
            if (pal_4.IsChecked == false && pal_4.IsEnabled == true)
                pal_4.IsChecked = true;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (ships.Count < 10)
            {
                MessageBox.Show("Расставьте все корабли!");
                return;
            }
            if (Name.Text == "")
            {
                MessageBox.Show("Введите свое имя!");
                return;
            }
            name = Name.Text;
            this.Close();
        }

        public void blok(Ship ship)
        {
            for (int t = 0; t < ship.Length; t++)
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        if (CheckingCellsNearby(ship.Coordinates[t].X + i, ship.Coordinates[t].Y + j))
                            buttons[ship.Coordinates[t].X + i, ship.Coordinates[t].Y + j].IsEnabled = false;
            for (int t = 0; t < ship.Length; t++)
                buttons[ship.Coordinates[t].X, ship.Coordinates[t].Y].IsEnabled = true;
        }
        public int positionCheck0(int position)
        {
            if (position == 0)
            {
                MessageBox.Show("Вы не можете разместить корабль в этом месте!");
                return 1;
            }
            return 0;
        }
        public void unblok(Ship ship)
        {
            for (int t = 0; t < ship.Length; t++)
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        if (CheckingCellsNearby(ship.Coordinates[t].X + i, ship.Coordinates[t].Y + j))
                            buttons[ship.Coordinates[t].X + i, ship.Coordinates[t].Y + j].IsEnabled = true;
            for (int t = 0; t < ship.Length; t++)
                MyControl1_Click_White(buttons[ship.Coordinates[t].X, ship.Coordinates[t].Y]);
        }
        private int checkLoc(int i, int j, int lenght, bool orientation)//orientation=false, если по горизонтали
        {
            int t1 = 0, t2 = 0;
            if (!orientation)
            {
                for (t1 = 0; t1 < lenght; t1++)
                {
                    if (!CheckingCellsNearby(i, j + t1))
                    {
                        t1--;
                        break;
                    }
                    if (buttons[i, j + t1].IsEnabled == false)
                    {
                        t1--;
                        break;
                    }
                }
                for (t2 = 0; t2 < lenght; t2++)
                {
                    if (!CheckingCellsNearby(i, j - t2))
                    {
                        t2--;
                        break;
                    }
                    if (buttons[i, j - t2].IsEnabled == false)
                    {
                        t2--;
                        break;
                    }
                }
            }
            else
            {
                for (t1 = 0; t1 < lenght; t1++)
                {
                    if (!CheckingCellsNearby(i + t1, j))
                    {
                        t1--;
                        break;
                    }
                    if (buttons[i + t1, j].IsEnabled == false)
                    {
                        t1--;
                        break;
                    }
                }
                for (t2 = 0; t2 < lenght; t2++)
                {
                    if (!CheckingCellsNearby(i - t2, j))
                    {
                        t2--;
                        break;
                    }
                    if (buttons[i - t2, j].IsEnabled == false)
                    {
                        t2--;
                        break;
                    }
                }
            }
            if (t1 == lenght)
                return 1; //направо или вниз
            if (t2 == lenght)
                return 2; //налево или вверх
            if (t1 + t2 + 1 >= lenght)
                return 3; //посередине 
            return 0; //нельзя поставить
        }
        private void Button_Click_Random(object sender, RoutedEventArgs e)
        {
            Button_Click_Clear(sender, e);
            AutomaticLocation auto = new AutomaticLocation(ships);
            auto.shipsGeneration();
            pal1 = 4;
            pal2 = 3;
            pal3 = 2;
            pal4 = 1;
            CheckVisibleRadioButtons();
            for (int i = 0; i < ships.Count; i++)
            {
                blok(ships[i]);
                for (int j = 0; j < ships[i].Length; j++)
                    MyControl1_Click_Salmon(buttons[ships[i].Coordinates[j].X, ships[i].Coordinates[j].Y]);
            }
        }

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            pal1 = 0;
            pal2 = 0;
            pal3 = 0;
            pal4 = 0;
            for (int i = 0; i < ships.Count; i++)
                unblok(ships[i]);
            ships.Clear();
            CheckVisibleRadioButtons();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            closeWind = true;
            this.Close();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            int position = 0;
            if (sender != null)
            {
                int _row = Grid.GetRow((Button)sender);
                int _column = Grid.GetColumn((Button)sender);
                if (ships.Count > 0)
                    for (int i = 0; i < ships.Count; i++)
                        if (ships[i].searchShipForDelete(_row - 1, _column - 1) == true)
                        {
                            if (ships[i].Length == 4)
                                pal4--;
                            if (ships[i].Length == 3)
                                pal3--;
                            if (ships[i].Length == 2)
                                pal2--;
                            if (ships[i].Length == 1)
                                pal1--;
                            unblok(ships[i]);
                            ships.RemoveAt(i);
                            CheckVisibleRadioButtons();
                            for (int j = 0; j < ships.Count; j++)
                                blok(ships[j]);
                            return;
                        }
                if (ships.Count == 10)
                    return;
                if (pal_1.IsChecked == true)
                {
                    pal1++;
                    CheckVisibleRadioButtons();
                    ships.Add(new Ship(1, _row - 1, _column - 1, false));
                    MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                    blok(ships.Last());
                    return;
                }
                if (pal_2.IsChecked == true)
                {
                    if (vertical.IsChecked == true)
                    {
                        position = checkLoc(_row - 1, _column - 1, 2, true);
                        if (positionCheck0(position) == 1)
                            return;
                        pal2++;
                        CheckVisibleRadioButtons();
                        ships.Add(new Ship(2, _row - position, _column - 1, true));
                        MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                        if (position == 1)
                            MyControl1_Click_Salmon(buttons[_row, _column - 1]);
                        if (position == 2)
                            MyControl1_Click_Salmon(buttons[_row - 2, _column - 1]);
                        blok(ships.Last());
                    }
                    else
                    {
                        position = checkLoc(_row - 1, _column - 1, 2, false);
                        if (positionCheck0(position) == 1)
                            return;
                        pal2++;
                        CheckVisibleRadioButtons();
                        ships.Add(new Ship(2, _row - 1, _column - position, false));
                        MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                        if (position == 1)
                            MyControl1_Click_Salmon(buttons[_row - 1, _column]);
                        if (position == 2)
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 2]);
                        blok(ships.Last());
                    }
                    return;
                }
                if (pal_3.IsChecked == true)
                {
                    if (vertical.IsChecked == true)
                    {
                        position = checkLoc(_row - 1, _column - 1, 3, true);
                        if (positionCheck0(position) == 1)
                            return;
                        pal3++;
                        CheckVisibleRadioButtons();
                        if (position == 3)
                        {
                            ships.Add(new Ship(3, _row - 2, _column - 1, true));
                            MyControl1_Click_Salmon(buttons[_row - 2, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row, _column - 1]);
                            blok(ships.Last());
                            return;
                        }
                        if (position == 2)
                            position++;
                        ships.Add(new Ship(3, _row - position, _column - 1, true));
                        MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                        if (position == 1)
                        {
                            MyControl1_Click_Salmon(buttons[_row, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row + 1, _column - 1]);
                        }
                        else
                        {
                            MyControl1_Click_Salmon(buttons[_row - 2, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row - 3, _column - 1]);
                        }
                        blok(ships.Last());
                    }
                    else
                    {
                        position = checkLoc(_row - 1, _column - 1, 3, false);
                        if (positionCheck0(position) == 1)
                            return;
                        pal3++;
                        CheckVisibleRadioButtons();
                        if (position == 3)
                        {
                            ships.Add(new Ship(3, _row - 1, _column - 2, false));
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 2]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column]);
                            blok(ships.Last());
                            return;
                        }
                        if (position == 2)
                            position++;
                        ships.Add(new Ship(3, _row - 1, _column - position, false));
                        MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                        if (position == 1)
                        {
                            MyControl1_Click_Salmon(buttons[_row - 1, _column]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column + 1]);
                        }
                        else
                        {
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 2]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 3]);
                        }
                        blok(ships.Last());
                    }
                    return;
                }
                if (pal_4.IsChecked == true)
                {
                    if (vertical.IsChecked == true)
                    {
                        position = checkLoc(_row - 1, _column - 1, 4, true);
                        if (positionCheck0(position) == 1)
                            return;
                        pal4++;
                        CheckVisibleRadioButtons();
                        if (position == 3)
                        {
                            if (CheckingCellsNearby(_row - 3, _column - 1) && buttons[_row - 3, _column - 1].IsEnabled == true)
                            {
                                ships.Add(new Ship(4, _row - 3, _column - 1, true));
                                MyControl1_Click_Salmon(buttons[_row - 3, _column - 1]);
                            }
                            else
                            {
                                ships.Add(new Ship(4, _row - 2, _column - 1, true));
                                MyControl1_Click_Salmon(buttons[_row + 1, _column - 1]);
                            }
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row - 2, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row, _column - 1]);
                            blok(ships.Last());
                            return;
                        }
                        if (position == 1)
                        {
                            ships.Add(new Ship(4, _row - 1, _column - 1, true));
                            MyControl1_Click_Salmon(buttons[_row, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row + 1, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row + 2, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                            blok(ships.Last());
                        }
                        if (position == 2)
                        {
                            ships.Add(new Ship(4, _row - 4, _column - 1, true));
                            MyControl1_Click_Salmon(buttons[_row - 4, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row - 3, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row - 2, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                            blok(ships.Last());
                        }
                    }
                    else
                    {
                        position = checkLoc(_row - 1, _column - 1, 4, false);
                        if (positionCheck0(position) == 1)
                            return;
                        pal4++;
                        CheckVisibleRadioButtons();
                        if (position == 3)
                        {
                            if (CheckingCellsNearby(_row - 1, _column - 3) && buttons[_row - 1, _column - 3].IsEnabled == true)
                            {
                                ships.Add(new Ship(4, _row - 1, _column - 3, false));
                                MyControl1_Click_Salmon(buttons[_row - 1, _column - 3]);
                            }
                            else
                            {
                                ships.Add(new Ship(4, _row - 1, _column - 2, false));
                                MyControl1_Click_Salmon(buttons[_row - 1, _column + 1]);
                            }
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 2]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column]);
                            blok(ships.Last());
                            return;
                        }
                        if (position == 1)
                        {
                            ships.Add(new Ship(4, _row - 1, _column - 1, false));
                            MyControl1_Click_Salmon(buttons[_row - 1, _column]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column + 1]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column + 2]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                            blok(ships.Last());
                        }
                        if (position == 2)
                        {
                            ships.Add(new Ship(4, _row - 1, _column - 4, false));
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 4]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 3]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 2]);
                            MyControl1_Click_Salmon(buttons[_row - 1, _column - 1]);
                            blok(ships.Last());
                        }
                    }
                    return;
                }
            }
        }
        private void MyControl1_Click_Salmon(object sender)
        {
            Button b = (Button)sender;
            b.Background = Brushes.Salmon;
            int row = Grid.GetColumn(b);
        }
        private void MyControl1_Click_White(object sender)
        {
            Button b = (Button)sender;
            b.Background = Brushes.LightGray;
            int row = Grid.GetColumn(b);
        }

    }
}
