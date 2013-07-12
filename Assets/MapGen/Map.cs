using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MapGen
{
    public class Map
    {
        // Fields
        private Room[,] _internalArray = null;

        // Properties & Accessors
        public Room this[int X, int Y]
        {
            get { return _internalArray[X, Y]; }
        }
        public Room[,] Rooms
        {
            get { return _internalArray; }
            private set { _internalArray = value; }
        }



        // Constructors
        public Map(int roomCount)
        {
            MapGenerator gen = new SpreadRandomMapGenerator();
            _internalArray = gen.GenerateRooms(roomCount);
        }

        public Map(Room[,] rooms)
        {
            _internalArray = rooms;
        }



        // Methods
        public Room GetNeighboringRoom(Room currentRoom, Directions direction)
        {
            var p = currentRoom.GetNeighborCoordinates(direction);
            return this[p.X, p.Y];
        }

        public Room GetFirstRoom()
        {
            foreach (var room in Rooms)
                if (room != null)
                    return room;

            return null;
        }
    }
}
