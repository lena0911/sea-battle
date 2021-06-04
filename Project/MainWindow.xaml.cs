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
using Microsoft.Win32;
using seaBattle_Library;
namespace Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Ship> player1;
        public List<Ship> player2;
        public string name1;
        public string name2;
        public int step=1;
       int flag = 0;
        public MainWindow()
        {
            ImageBrush imBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../images/main.jpg"))
            };
            this.Background = imBrush;
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            players2rb.IsEnabled = false;
            vsPCrb.IsEnabled = false;
            saveButton.IsEnabled = false;
            if (players2rb.IsChecked == true) //если выбран режим "2 игрока"
            {
                if (flag == 0)
                {
                    Arrangement formArrangement1 = new Arrangement();
                    this.Hide();
                    formArrangement1.ShowDialog();
                    player1 = formArrangement1.ships;
                    name1 = formArrangement1.name.ToString();
                    Arrangement formArrangement2 = new Arrangement();
                    this.Hide();
                    formArrangement2.playerLabel.Content = "Игрок 2";
                    formArrangement2.ShowDialog();
                    arrangement.Content = "Старт!";
                    if (formArrangement2.closeWind)
                        this.Close();
                    this.Show();                 
                    player2 = formArrangement2.ships;
                    name2 = formArrangement2.name.ToString();
                }
                if (flag == 1)
                {
                    BattleZone fightZone = new BattleZone(player1, new Zone(player1), player2, new Zone(player2), name1, name2, false, step);
                    fightZone.Show();
                    this.Close();
                }
                flag++;
            }
            if (vsPCrb.IsChecked == true) //если выбран режим против компьютера
            {
                Arrangement formArrangement1 = new Arrangement();
                formArrangement1.ShowDialog();
                player1 = formArrangement1.ships;
                name1 = formArrangement1.name.ToString();
                List<Ship> shipsBot = new List<Ship>();
                AutomaticLocation auto = new AutomaticLocation(shipsBot);
                auto.shipsGeneration();
                shipsBot = auto.ships;
                name2 = "Бот Максим";
                BattleZone fightZone = new BattleZone(player1, new Zone(player1), shipsBot, new Zone(shipsBot), name1, name2, true, step);
                fightZone.Show();
                this.Close();
            }
            if(saveButton.IsChecked==true)
            {
                bool bot;               
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true) //добавить проверку контрольной суммы
                {
                    int[,] mas1 = new int[10, 10];
                    int[,] mas2 = new int[10, 10];
                    player1 = new List<Ship>();
                    player2 = new List<Ship>();
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName, Encoding.Default))
                    {
                        if (sr.ReadLine() == "1")
                            bot = true;
                        else
                            bot = false;
                        name1 = sr.ReadLine();
                        checkArena(sr, player1, mas1);
                        name2 = sr.ReadLine();
                        checkArena(sr, player2, mas2);
                        step = Convert.ToInt32(sr.ReadLine());
                        BattleZone fightZone = new BattleZone(player1, new Zone(player1, mas1), player2, new Zone(player2, mas2), name1, name2, bot, step);
                        fightZone.Show();
                        this.Close();
                    }
                }
            }
        }
        private void checkArena(StreamReader sr, List<Ship> player, int [,] mas)
        {
            bool or;
            int l, x, y;
            for (int i = 0; i < 10; i++)
            {
                string[] tmp = sr.ReadLine().Split(' ');
                l = Convert.ToInt32(tmp[0]);
                x = Convert.ToInt32(tmp[1]);
                y = Convert.ToInt32(tmp[2]);
                if (Convert.ToInt32(tmp[3]) == 1)
                    or = true;
                else
                    or = false;
                player.Add(new Ship(l, x, y, or));
            }
            for (int i = 0; i < 10; i++)
            {
                string[] tmp = sr.ReadLine().Split(' ');
                for (int j = 0; j < 10; j++)
                    mas[i, j] = Convert.ToInt32(tmp[j]);
            }
            for (int i = 0; i < player.Count; i++)
                for (int j = 0; j < player[i].Length; j++)
                    if (mas[player[i].Coordinates[j].X, player[i].Coordinates[j].Y] == 1)
                        player[i].searchShip(player[i].Coordinates[j].X, player[i].Coordinates[j].Y);
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
