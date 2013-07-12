using Momolike;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapGen
{
    public class Room
    {
        // Public Fields && Properties
        public RoomComponent UnityRoomComponent
        { 
            get; set; 
        }

        public Point Location
        {
            get;
            protected set;
        }

        public Exit NorthExit
        {
            get;
            set;
        }

        public Exit SouthExit
        {
            get;
            set;
        }

        public Exit EastExit
        {
            get;
            set;
        }

        public Exit WestExit
        {
            get;
            set;
        }

        public Room(Point p)
        {
            Location = p;
        }

        public Exit GetExit(Directions direction)
        {
            switch (direction)
            {
                case Directions.North:
                    return NorthExit;
                case Directions.East:
                    return EastExit;
                case Directions.West:
                    return WestExit;
                case Directions.South:
                    return SouthExit;
            }
            throw new ArgumentException("invalid enum val in argument");
        }

        public Point GetNeighborCoordinates(Directions direction)
        {
            return Room.GetNeighborCoordinates(this.Location, direction);
        }

        public virtual void AddExit(Directions direction)
        {
            switch (direction)
            {
                case Directions.North:
                    NorthExit = new Exit(direction);
                    break;
                case Directions.South:
                    SouthExit = new Exit(direction);
                    break;
                case Directions.East:
                    EastExit = new Exit(direction);
                    break;
                case Directions.West:
                    WestExit = new Exit(direction);
                    break;
                default:
                    throw new ArgumentException("invalid val in argument" + direction.ToString());
            }
        }

        public int GetNumberOfExits()
        {
            int returnValue = 0;

            if (NorthExit != null)
                returnValue++;

            if (SouthExit != null)
                returnValue++;

            if (EastExit != null)
                returnValue++;

            if (WestExit != null)
                returnValue++;

            return returnValue;
        }

        public static Point GetNeighborCoordinates(Point point, Directions direction)
        {
            switch (direction)
            {
                case Directions.North:
                    return new Point(point.X, point.Y + 1);
                case Directions.South:
                    return new Point(point.X, point.Y - 1);
                case Directions.East:
                    return new Point(point.X + 1, point.Y);
                case Directions.West:
                    return new Point(point.X - 1, point.Y);
            }

            throw new ArgumentException("invalid enum val in argument");
        }
    }
}
