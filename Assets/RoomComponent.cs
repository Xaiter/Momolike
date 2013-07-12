using System;
using UnityEngine;
using System.Collections.Generic;
using MapGen;

namespace Momolike
{
    public class RoomComponent : MonoBehaviour
    {
        // Constants & Statics
        private static int ROOM_COUNT = 0;

        // Protected Properties
        protected GameObject Floor { get; set; }
        protected GameObject Ceiling { get; set; }
        protected Wall[] Walls { get; set; }

        // Public Fields
        public int RoomNumber;
        public RoomComponent NorthExit;
        public RoomComponent SouthExit;
        public RoomComponent EastExit;
        public RoomComponent WestExit;
        public float Width;
        public float Length;
        public float Height;



        // Constructors & Startup
        public RoomComponent()
        {

        }

        public void Start()
        {
            RoomNumber = ROOM_COUNT;
            ROOM_COUNT++;

            this.name = "Room " + RoomNumber;
            this.gameObject.name = this.name;
        }




        // Public Methods
        public virtual void CreateRoom()
        {
            CreateFloorAndCeiling();

            float xAdjust = (Length / 2) - (Wall.THICKNESS / 2);
            float yAdjust = (Height / 2) + (Wall.THICKNESS / 2);
            float zAdjust = (Width / 2) - (Wall.THICKNESS / 2);

            List<Wall> walls = new List<Wall>();

            walls.AddRange(CreateWall(NorthExit, this.Length, new Vector3(0, yAdjust, zAdjust), 90));
            walls.AddRange(CreateWall(SouthExit, this.Length, new Vector3(0, yAdjust, -zAdjust), 90));
            walls.AddRange(CreateWall(WestExit, this.Width, new Vector3(-xAdjust, yAdjust, 0), 0));
            walls.AddRange(CreateWall(EastExit, this.Width, new Vector3(xAdjust, yAdjust, 0), 0));

            Walls = walls.ToArray();
            TakeOwnershipOfChildren();


        }

        protected Wall[] CreateWall(RoomComponent exitRoom, float wallWidth, Vector3 wallAdjust, float rotation)
        {
            if (exitRoom != null)
                return CreateConnectingWall(exitRoom as HallwayComponent, wallAdjust);
            else
            {
                var wall = new Wall(this, wallWidth, Height, rotation);
                wall.Instance.transform.parent = this.transform;
                wall.Instance.transform.localPosition = wallAdjust;
                return new Wall[] { wall };
            }
        }

        public void SetExit(Directions direction, RoomComponent exitRoom)
        {
            switch (direction)
            {
                case Directions.North:
                    this.NorthExit = exitRoom;
                    exitRoom.SouthExit = this;
                    break;

                case Directions.South:
                    this.SouthExit = exitRoom;
                    exitRoom.NorthExit = this;
                    break;

                case Directions.East:
                    this.EastExit = exitRoom;
                    exitRoom.WestExit = this;
                    break;

                case Directions.West:
                    this.WestExit = exitRoom;
                    exitRoom.EastExit = this;
                    break;
            }
        }

        public void SetSize(float width, float length, float height)
        {
            this.Width = width;
            this.Length = length;
            this.Height = height;
        }



        // Protected Methods
        protected void CreateFloorAndCeiling()
        {
            Floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Floor.transform.parent = this.transform;
            Floor.transform.localScale = new Vector3(Length, Wall.THICKNESS, Width);
            Floor.transform.localPosition = new Vector3(0, 0, 0);
            Floor.name = GetRoomNamePrefix() + " Floor";


            Ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Ceiling.transform.parent = this.transform;
            Ceiling.transform.localScale = new Vector3(Length, Wall.THICKNESS, Width);
            Ceiling.transform.localPosition = new Vector3(0, Height + Wall.THICKNESS, 0);
            Ceiling.name = GetRoomNamePrefix() + " Ceiling";
        }

        protected float GetWallWidth(Directions direction)
        {
            if (direction == Directions.North || direction == Directions.South)
                return this.Length;

            return this.Width;
        }

        protected virtual Wall[] CreateConnectingWall(HallwayComponent hallway, Vector3 wallCenter)
        {
            // Prep maths
			Vector3 diff = this.transform.position - hallway.transform.position;
            Directions relativeDirection = GetRelativeDirection(hallway, diff);
            Vector2 openingSize = hallway.GetExitSize(relativeDirection);
            float wallWidth = GetWallWidth(relativeDirection);

            float sideGapAdjustment = openingSize.x / 2 - Wall.THICKNESS;
            float sidePieceWidth = (wallWidth - openingSize.x) / 2 + Wall.THICKNESS;
            float doorPieceHeight = this.Height - openingSize.y;
            float centerPieceHeight = Height - hallway.Height;
			

			
			// Create piece and take ownership
            var leftPiece = new Wall(this, sidePieceWidth, Height);
            var rightPiece = new Wall(this, sidePieceWidth, Height);
            var centerPiece = new Wall(this, hallway.Width - Wall.THICKNESS * 2, centerPieceHeight);
			TakeOwnershipOfChildren(leftPiece, rightPiece, centerPiece);

            
			
            // Position pieces
            leftPiece.Instance.transform.Rotate(new Vector3(0, 90, 0));
            leftPiece.SetLocalPosition(-sideGapAdjustment - sidePieceWidth / 2, wallWidth / 2 - Wall.THICKNESS / 2);

            rightPiece.Instance.transform.Rotate(new Vector3(0, 90, 0));
            rightPiece.SetLocalPosition(sideGapAdjustment + sidePieceWidth / 2, wallWidth / 2 - Wall.THICKNESS / 2);

            centerPiece.SetLocalPosition(0, Height - Wall.THICKNESS, wallWidth / 2 - Wall.THICKNESS / 2, 90);

			

            // rotate this shit
            switch(relativeDirection)
            {
                case Directions.North:
                    leftPiece.Instance.transform.RotateAround(this.transform.position, Vector3.up, 180);
                    rightPiece.Instance.transform.RotateAround(this.transform.position, Vector3.up, 180);
                    centerPiece.Instance.transform.RotateAround(this.transform.position, Vector3.up, 180);
                    break;

                case Directions.East:
                    leftPiece.Instance.transform.RotateAround(this.transform.position, Vector3.up, -90);
                    rightPiece.Instance.transform.RotateAround(this.transform.position, Vector3.up, -90);
                    centerPiece.Instance.transform.RotateAround(this.transform.position, Vector3.up, -90);
                    break;

                case Directions.West:
                    leftPiece.Instance.transform.RotateAround(this.transform.position, Vector3.up, 90);
                    rightPiece.Instance.transform.RotateAround(this.transform.position, Vector3.up, 90);
                    centerPiece.Instance.transform.RotateAround(this.transform.position, Vector3.up, 90);
                    break;
            }

            return new Wall[] {
				leftPiece, rightPiece, centerPiece
			};
        }

        protected Directions GetRelativeDirection(HallwayComponent hallway, Vector3 diff)
        {
            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
                if (diff.x > 0)
                    return Directions.East;
                else
                    return Directions.West;
            else
                if (diff.z > 0)
                    return Directions.North;
                else
                    return Directions.South;
        }

        protected string GetRoomNamePrefix()
        {
            return "Room " + RoomNumber;
        }
		
        protected void TakeOwnershipOfChildren()
        {
            TakeOwnershipOfChildren(this.Walls);
        }
		
		protected void TakeOwnershipOfChildren(params Wall[] walls)
		{
			if(walls == null || walls.Length == 0)
				return;
			
			foreach (var wall in walls)
            {
                wall.Instance.name = GetRoomNamePrefix() + " Wall";
                wall.Instance.renderer.material.color = Color.green;
                wall.Instance.transform.parent = this.transform;
            }
		}
    }


}

