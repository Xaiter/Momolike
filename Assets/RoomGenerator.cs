using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Momolike;
using MapGen;

namespace Momolike
{

    public class RoomGenerator : MonoBehaviour
    {
        // Room Constants
        private static readonly float ROOM_SIZE = 60;
        private static readonly float ROOM_HEIGHT = 6;
        private static readonly float HALLWAY_LENGTH = 25;
        private static readonly float HALLWAY_WIDTH = 10;
        private static readonly float HALLWAY_HEIGHT = 3f;

        private static readonly float ROOM_OFFSET_FACTOR = ROOM_SIZE + HALLWAY_LENGTH;
        private static readonly float HALLWAY_OFFSET_AMOUNT = ROOM_SIZE / 2 + HALLWAY_LENGTH / 2;


        void Start()
        {
            GenerateMap();
            return;

            var room = CreateRoom<RoomComponent>(30, 30, 6, Vector3.zero);
            var northRoom = CreateRoom<RoomComponent>(30, 30, 6, new Vector3(0, 0, 40));
            var eastRoom = CreateRoom<RoomComponent>(30, 30, 6, new Vector3(40, 0, 0));

            var northHallway = CreateRoom<HallwayComponent>(10, 10, 3, new Vector3(0, 0, 20));
            northHallway.SetExit(Directions.South, room);
            northHallway.SetExit(Directions.North, northRoom);
            northHallway.CreateRoom();

            var southHallway = CreateRoom<HallwayComponent>(10, 10, 3, new Vector3(0, 0, -20));
            southHallway.SetExit(Directions.North, room);
            southHallway.CreateRoom();

            var eastHallway = CreateRoom<HallwayComponent>(10, 10, 3, new Vector3(20, 0, 0));
            eastHallway.SetExit(Directions.West, room);
            eastHallway.SetExit(Directions.East, eastRoom);
            eastHallway.CreateRoom();

            var westHallway = CreateRoom<HallwayComponent>(10, 10, 3, new Vector3(-20, 0, 0));
            westHallway.SetExit(Directions.East, room);
            westHallway.CreateRoom();

            room.CreateRoom();
            northRoom.CreateRoom();
            eastRoom.CreateRoom();
        }

        private static void GenerateMap()
        {
            //var rooms = new Room[1, 2];
            //rooms[0, 0] = new Room(new Point(0, 0));
            //rooms[0, 1] = new Room(new Point(0, 1));

            //rooms[0, 0].AddExit(Directions.North);
            //rooms[0, 1].AddExit(Directions.South);
            //var map = new Map(rooms);

            var mapGenerator = new IsaacMapGenerator();
            var map = mapGenerator.GenerateMap(12);

            var roomComponents = new List<RoomComponent>();
            foreach (var room in map.Rooms)
            {
                if (room == null)
                    continue;

                var component = CreateRoom<RoomComponent>(ROOM_SIZE, ROOM_SIZE, ROOM_HEIGHT, new Vector3(room.Location.X * ROOM_OFFSET_FACTOR, 0, room.Location.Y * ROOM_OFFSET_FACTOR));
                room.UnityRoomComponent = component;
                roomComponents.Add(component);
            }

            roomComponents.AddRange(CreateHallways(map));

            foreach (var room in roomComponents)
                room.CreateRoom();
        }

        private static T CreateRoom<T>(float width, float length, float height, Vector3 roomCenterPosition) where T : RoomComponent
        {
            var obj = new GameObject();
            obj.transform.position = roomCenterPosition;
            obj.AddComponent<T>();

            var comp = obj.GetComponent<T>();
            comp.SetSize(width, length, height);

            return comp;
        }

        private static List<RoomComponent> CreateHallways(Map map)
        {
            var rooms = new List<RoomComponent>();
            CreateHallwaysRecursive(map, map.GetFirstRoom(), null, null, rooms);
            return rooms;
        }

        private static void CreateHallwaysRecursive(Map map, Room currentRoom, Room previousRoom, Directions? direction, List<RoomComponent> hallwayComponents)
        {
            if (direction.HasValue)
            {
                float xPos = currentRoom.Location.X * ROOM_OFFSET_FACTOR;
                float zPos = currentRoom.Location.Y * ROOM_OFFSET_FACTOR;

                if (direction.Value == Directions.West)
                    xPos += HALLWAY_OFFSET_AMOUNT;
                else if (direction.Value == Directions.East)
                    xPos -= HALLWAY_OFFSET_AMOUNT;
                else if (direction.Value == Directions.South)
                    zPos += HALLWAY_OFFSET_AMOUNT;
                else
                    zPos -= HALLWAY_OFFSET_AMOUNT;

                var hallwayComp = CreateRoom<HallwayComponent>(HALLWAY_WIDTH, HALLWAY_LENGTH, HALLWAY_HEIGHT, new Vector3(xPos, 0, zPos));
                hallwayComp.SetExit(direction.Value, currentRoom.UnityRoomComponent);
                hallwayComponents.Add(hallwayComp);

                previousRoom.UnityRoomComponent.SetExit(direction.Value, hallwayComp);
            }
            
            if (direction != Directions.South && currentRoom.NorthExit != null)
                CreateHallwaysRecursive(map, map.GetNeighboringRoom(currentRoom, Directions.North), currentRoom, Directions.North, hallwayComponents);
            
            if (direction != Directions.North && currentRoom.SouthExit != null)
                CreateHallwaysRecursive(map, map.GetNeighboringRoom(currentRoom, Directions.South), currentRoom, Directions.South, hallwayComponents);
            
            if (direction != Directions.East && currentRoom.WestExit != null)
                CreateHallwaysRecursive(map, map.GetNeighboringRoom(currentRoom, Directions.West), currentRoom, Directions.West, hallwayComponents);
            
            if (direction != Directions.West && currentRoom.EastExit != null)
                CreateHallwaysRecursive(map, map.GetNeighboringRoom(currentRoom, Directions.East), currentRoom, Directions.East, hallwayComponents);
        }
    }
}





