using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seaBattle_Library
{
    public class AutomaticLocation
    {
        public List<Ship> ships = new List<Ship>();
        int pal1 = 4, pal2 = 3, pal3 = 2, pal4 = 1;
        const int n = 10;
        public int[,] matrixShips = new int[n, n];

        public int Pal1 { get => pal1; set => pal1 = value; }
        public int Pal2 { get => pal2; set => pal2 = value; }
        public int Pal3 { get => pal3; set => pal3 = value; }
        public int Pal4 { get => pal4; set => pal4 = value; }

        public AutomaticLocation(List<Ship> ships)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrixShips[i, j] = 0;
            this.ships = ships;
        }
        public int randGeneration(int i, int j)
        {
            Random rnd = new Random();
            return rnd.Next(i, j);
        }
        private bool CheckingCellsNearby(int x, int y) //проверка выхода за пределы поля
        {
            if (x < 0 || x > 9 || y < 0 || y > 9)
                return false;
            else
                return true;
        }
        public void shipsCount(int x) //уменьшаем кол-во доступных кораблей
        {
            if (x == 1)
                pal1--;
            if (x == 2)
                pal2--;
            if (x == 3)
                pal3--;
            if (x == 4)
                pal4--;
        }
        public int shipCheckBan(int x) //проверка можно ли еще генерировать корабль такого размера
        {
            if (x == 1 && pal1 == 0 || x == 2 && pal2 == 0 || x == 3 && pal3 == 0 || x == 4 && pal4 == 0)
                return 0;
            return 1;
        }
        public void block() //блокировка недоступных клеток
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (matrixShips[i, j] == 1)
                    {
                        for (int k = -1; k <= 1; k++)
                            for (int t = -1; t <= 1; t++)
                                if (CheckingCellsNearby(i + k, j + t) && matrixShips[i + k, j + t] == 0)
                                    matrixShips[i + k, j + t] = 2;
                    }
        }
        public void fillingTheMatrix(int i, int j, int n, bool or) //заполнение матрицы после добавления корабля
        {   //or == true, когда корабль вертик. false - горизонт 
            for (int t = 0; t < n; t++)
                if (or)
                    matrixShips[i + t, j] = 1;
                else
                    matrixShips[i, j + t] = 1;
        }
        public bool checkLoc(int i, int j, int n, bool or) //проверка можно ли поставить корабль
        {   //or == true, когда корабль вертик. false - горизонт 
            int tv = 0;
            if (or)
                for (tv = 0; tv < n; tv++)
                    if (CheckingCellsNearby(i + tv, j))
                        if (matrixShips[i + tv, j] != 0)
                            break;
                        else;
                    else
                        break;
            if (!or)
                for (tv = 0; tv < n; tv++)
                    if (CheckingCellsNearby(i, j + tv))
                        if (matrixShips[i, j + tv] != 0)
                            break;
                        else;
                    else
                        break;
            if (tv == n)
                return true; //можно
            return false; //нельзя
        }
        public void positionGeneration(int n) //генерация корабля
        {
            int i, j;
            bool orientation;
            while (shipCheckBan(n) != 0) //можно ли ставить такие корабли
            {
                i = randGeneration(0, 10);
                j = randGeneration(0, 10);
                orientation = Convert.ToBoolean(randGeneration(0, 2)); //true-вертик, false-горизонт
                if (checkLoc(i, j, n, orientation)) //проверка можно ли поставить корабль
                {
                    fillingTheMatrix(i, j, n, orientation); //заполнение матрицы после добавления корабля
                    shipsCount(n); //уменьшаем кол-во доступных кораблей
                    ships.Add(new Ship(n, i, j, orientation));
                    block(); //блокируем недоступные клетки
                }
            }
        }
        public void shipsGeneration() //генерация кораблей
        {
            positionGeneration(4);
            positionGeneration(3);
            positionGeneration(2);
            positionGeneration(1);
        }
    }
}
