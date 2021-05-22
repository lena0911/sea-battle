
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Zone
    {
        public static int n=10;
        public int [,] matrixShips= new int[n, n];
        public int[,] moveMatrix = new int[n, n];
        public string name { get; set; }

        public Zone(List<Ship> ships)
        { 
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrixShips[i, j] = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    moveMatrix[i, j] = 0;
            for (int i = 0; i < ships.Count; i++)
                for (int j = 0; j < ships[i].Length; j++)
                    matrixShips[ships[i].Coordinates[j].X, ships[i].Coordinates[j].Y] = 1;
        }
        public bool attack(int x, int y)
        {
            if (matrixShips[x, y] == 0)       
                return false;
            if (matrixShips[x, y] == 1)
                return true;
            return false;
        }
        
    }

}
