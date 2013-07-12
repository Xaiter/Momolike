using System;
using System.Collections.Generic;

using System.Text;

namespace MapGen
{
    public class Point
    {

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Concat("(", this.X, ",", this.Y, ")");
        }
    }
}
