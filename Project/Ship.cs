using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Ship
    {
        private int length;
        private Vector[] coordinates;
        private bool[] shot;

        internal Vector[] Coordinates { get => coordinates; set => coordinates = value; }
        public int Length { get => length; set => length = value; }
        public bool[] Shot { get => shot; set => shot = value; }

        public Ship(int Lenght, int x, int y, bool direction, Zone fight) //direction = false, если расположен вертикально. true - горизонтально
        {
            Length = Lenght;
            Coordinates = new Vector[Length];
            if (direction)
                for (int i = 0; i < Length; i++)
                {
                    Coordinates[i] = new Vector(x, y + i);
                    fight.matrixShips[x, y + i] = 1;
                }
            else
                for (int i = 0; i < Length; i++)
                {
                    Coordinates[i] = new Vector(x + i, y);
                    fight.matrixShips[x + i, y] = 1;
                }
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
    }
}
