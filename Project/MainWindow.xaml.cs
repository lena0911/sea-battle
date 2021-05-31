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
using System.Windows.Shapes;

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
        int flag = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            players2rb.IsEnabled = false;
            vsPCrb.IsEnabled = false;
            if (players2rb.IsChecked == true) //если выбран режим "2 игрока"
            {
                if (flag == 0)
                {

                    Arrangement formArrangement1 = new Arrangement();
                    formArrangement1.ShowDialog();
                    player1 = formArrangement1.ships;
                    name1 = formArrangement1.name.ToString();

                    Arrangement formArrangement2 = new Arrangement();
                    formArrangement2.ShowDialog();
                    player2 = formArrangement2.ships;
                    name2 = formArrangement2.name.ToString();
                }
                if (flag == 1)
                {
                    BattleZone fightZone = new BattleZone(player1, new Zone(player1), player2, new Zone(player2), name1, name2, false);
                    fightZone.Show();
                    this.Close();
                }
                flag++;
            }
            else //если выбран режим против компьютера
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
                BattleZone fightZone = new BattleZone(player1, new Zone(player1), shipsBot, new Zone(shipsBot), name1, name2, true);
                fightZone.Show();
                this.Close();
            }
        }
    }
}
