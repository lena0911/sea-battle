
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Zone
    {
        public static int n=10;
        public int [,] matrixShips= new int[n, n];
        public int[,] moveMatrix = new int[n, n];
        
        public int pal4=1;
        public Zone()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrixShips[i, j] = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    moveMatrix[i, j] = 0;

        }
        public bool attack(int x, int y)
        {
            if (matrixShips[x, y] == 0)
            {
                //окрашивание в цвет промоха
                return false;
            }
            if (matrixShips[x, y] == 1)
            {
                //окрашивание в цвет попадания
                return true;
            }
            return false;
        }
    }

}
