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
        int flag = 1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (flag == 1)
            {
                Arrangement formArrangement1 = new Arrangement();
                formArrangement1.ShowDialog();
                player1 = formArrangement1.ships;
                
            }
            if(flag==2)
            {
                Arrangement formArrangement2 = new Arrangement();
                formArrangement2.ShowDialog();
                player2 = formArrangement2.ships;
            }
            if (flag == 3)
            {
                BattleZone fightZone = new BattleZone(player1, new Zone(player1), player2, new Zone(player2));
                fightZone.Show();
            }
            flag++;

           

        }
    }
}
