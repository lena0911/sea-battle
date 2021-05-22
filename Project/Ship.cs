using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Ship
    {
        private int length; //количество палуб
        private Vector[] coordinates; //массив из вектор-координат
        private bool[] shot; //массив попаданий в каждую палубу корабля

        internal Vector[] Coordinates { get => coordinates; set => coordinates = value; }
        public int Length { get => length; set => length = value; }
        public bool[] Shot { get => shot; set => shot = value; }

        public Ship(int Lenght, int x, int y, bool direction) //direction = true, если расположен вертикально. false - горизонтально
        {
            Length = Lenght;
            Coordinates = new Vector[Length];
            if (direction)
                for (int i = 0; i < Length; i++)
                    Coordinates[i] = new Vector(x+i, y);
            else
                for (int i = 0; i < Length; i++)
                    Coordinates[i] = new Vector(x, y + i);
            shot = new bool[length];
            for (int i = 0; i < length; i++)
                shot[i] = false;
        }
        public bool checkDestroyedShip() //проверка уничтожен ли корабль полностью
        {
            for (int i = 0; i < length; i++)
                if (Shot[i] == false)
                    return false;
            return true;
        }

        public bool searchShip(int x, int y) //поиск коробля, по которому попали
        {
            for (int i = 0; i < length; i++)
                if (coordinates[i].X == x && coordinates[i].Y == y)
                {
                    shot[i] = true;
                    return true;
                }
            return false;
        }
        public bool searchShipForDelete(int x, int y) //поиск коробля, по которому попали
        {
            for (int i = 0; i < length; i++)
                if (coordinates[i].X == x && coordinates[i].Y == y)
                    return true;
            return false;
        }
    }
}
