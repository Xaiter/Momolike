using System;
using System.Collections.Generic;

using System.Text;

namespace MapGen
{
    public class Exit
    {
        public Directions Direction
        {
            get; private set;
        }

        public Exit(Directions dir)
        {
            Direction = dir;
        }
    }

    public enum Directions {North, East, West, South};
}
