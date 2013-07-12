using System;
using System.Collections.Generic;
using System.Text;

namespace MapGen
{
    public class WeightedMapGenerator : MapGenerator
    {
        public override Room[,] GenerateRooms(int maxRooms)
        {
            int minX = 0;
            int maxX = 0;
            int minY = 0;
            int maxY = 0;

            Directions[] directions = (Directions[])Enum.GetValues(typeof(Directions));
            List<WeightedRoom> rooms = new List<WeightedRoom>();
            rooms.Add(new WeightedRoom(new Point(0, 0)));

            while (rooms.Count < maxRooms)
            {
                WeightedRoom targetRoom = (WeightedRoom)rooms[Randomizer.GetRandomNumber(rooms.Count)];

                if (!ShouldCreateNextRoom(targetRoom))
                    continue;
                

                Directions direction = Randomizer.GetRandomDirection();
                Exit targetExit = targetRoom.GetExit(direction);

                if (targetExit != null)
                    continue;

                targetExit = new Exit(direction);
                targetRoom.AddExit(direction);
                WeightedRoom neighbor = GetRoomAtPoint(targetRoom.GetNeighborCoordinates(direction), rooms);
                Directions neighborDirection = ReverseDirection(direction);

                if (neighbor != null)
                {
                    neighbor.AddExit(neighborDirection);
                }
                else
                {
                    WeightedRoom newRoom = new WeightedRoom(targetRoom.GetNeighborCoordinates(direction));
                    newRoom.AddExit(neighborDirection);
                    rooms.Add(newRoom);
                }
            }
            Room[,] roomArray = ConvertListToMap(MarkSpecialRooms(rooms));

            return roomArray;
        }

        protected bool ShouldCreateNextRoom(WeightedRoom targetRoom)
        {
            float probability = 1;
            switch (targetRoom.GetNumberOfExits())
            {
                case 4:
                    return false;

                case 3:
                    probability = 0.001F;
                    break;

                case 2:
                    probability = 0.01F;
                    break;

                case 1:
                    probability = 0.2F;
                    break;
                default:
                    return true;
            }

            return Randomizer.GetRandomBool(probability);
        }

    }
}
