using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seaBattle_Library
{
    public class MyVector
    {
        public int x;
        public int y;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public MyVector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
