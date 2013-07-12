using UnityEngine;
using System;
using System.Collections.Generic;
using MapGen;

namespace Momolike
{
	public class HallwayComponent : RoomComponent
	{
		public HallwayComponent() : base()
		{
			
		}
		
		public void CreateConnector(float width, float height, RoomComponent targetRoom)
		{
			// do absolutely nothing
		}
		
		public Vector2 GetExitSize(Directions direction)
		{
            return new Vector2(this.Width, this.Height);

            //switch(direction)
            //{
            //    case Directions.North:
            //        return new Vector2(this.Length, this.Height);
				
            //    case Directions.South:
            //        return new Vector2(this.Length, this.Height);
				
            //    case Directions.East:
            //        return new Vector2(this.Width, this.Height);
				
            //    default:
            //        return new Vector2(this.Width, this.Height);
            //}
		}
		
		protected override Wall[] CreateConnectingWall (HallwayComponent hallway, Vector3 wallCenter)
		{
			return new Wall[0];
		}
	}
}

