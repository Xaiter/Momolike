using System;
using System.Collections.Generic;

using System.Text;

namespace MapGen
{
    public abstract class MapGenerator
    {
        protected static readonly Directions[] AVAILABLE_DIRECTIONS = Enum.GetValues(typeof(Directions)) as Directions[];

        public abstract Room[,] GenerateRooms(int maxRooms);

        public virtual Map GenerateMap(int maxRooms)
        {
            Map map = new Map(GenerateRooms(maxRooms));
            return map;
        }

        public T GetRoomAtPoint<T>(Point location, IEnumerable<T> rooms) where T : Room
        {
            foreach (var room in rooms)
                if (room.Location.X == location.X && room.Location.Y == location.Y)
                    return room;

            return null;
        }

        public Directions ReverseDirection(Directions direction)
        {
            switch (direction)
            {
                case Directions.North:
                    return Directions.South;
                case Directions.South:
                    return Directions.North;
                case Directions.West:
                    return Directions.East;
                case Directions.East:
                    return Directions.West;
            }
            throw new FormatException("bad enum arg");
        }

        public Directions? GetEmptyNeighborDirection<T>(IList<T> rooms, Room room) where T : Room
        {
            foreach (var direction in AVAILABLE_DIRECTIONS)
            {
                var neighborLocation = room.GetNeighborCoordinates(direction);
                if (!RoomExistsAtLocation<T>(neighborLocation, rooms))
                    return direction;
            }

            return null;
        }

        protected T[,] ConvertListToMap<T>(IList<T> rooms) where T : Room
        {
            int minX = 0;
            int maxX = 0;
            int minY = 0;
            int maxY = 0;

            foreach (var newRoom in rooms)
            {
                if (newRoom.Location.X < minX)
                    minX = newRoom.Location.X;
                if (newRoom.Location.X > maxX)
                    maxX = newRoom.Location.X;
                if (newRoom.Location.Y < minY)
                    minY = newRoom.Location.Y;
                if (newRoom.Location.Y > maxY)
                    maxY = newRoom.Location.Y;
            }

            T[,] roomArray = new T[maxX + -minX + 1, maxY + -minY + 1];

            foreach (var room in rooms)
            {
                room.Location.X += -minX;
                room.Location.Y += -minY;
                roomArray[room.Location.X, room.Location.Y] = room;
            }
            return roomArray;
        }

        protected bool RoomExistsAtLocation<T>(Point point, IList<T> rooms) where T : Room
        {
            return GetRoomAtPoint<T>(point, rooms) != null;
        }

        protected IList<Room> MarkSpecialRooms<T>(IList<T> rooms) where T : Room
        {
            return MarkBranchEndRooms<T>(rooms);
        }

        protected IList<Room> MarkBranchEndRooms<T>(IList<T> rooms) where T : Room
        {
            List<Room> markedRooms = new List<Room>();
            for (int i = 0; i < rooms.Count; i++)
            {
                var room = rooms[i];
                if (room.GetNumberOfExits() == 1)
                {
                    markedRooms.Add(new TreasureRoom(room));
                }
                else
                    markedRooms.Add(room);
            }
            return markedRooms;
        }

        protected T[] FindLeafRooms<T>(IEnumerable<T> rooms) where T : Room
        {
            var items = new List<T>();

            foreach (var room in rooms)
                if (room.GetNumberOfExits() == 1)
                    items.Add(room);

            return items.ToArray();
        }


    }
}
