using System;
using System.Collections.Generic;

using System.Text;

namespace MapGen
{
    public class TreasureRoom : Room
    {
        public TreasureRoom(Point p):base(p)
        {
            Location = p;
        }

        public TreasureRoom(Room baseRoom):base(baseRoom.Location)
        {
            Location = baseRoom.Location;
            NorthExit = baseRoom.NorthExit;
            SouthExit = baseRoom.SouthExit;
            EastExit = baseRoom.EastExit;
            WestExit = baseRoom.WestExit;
        }
    }
}
