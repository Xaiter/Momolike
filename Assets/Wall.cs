using System;
using UnityEngine;

namespace Momolike
{
	public class Wall
	{
		// Constants
		public const float THICKNESS = 1.0f;
        
        // Fields
        private RoomComponent _owner = null;
		
		// Properties
		public GameObject Instance { get; private set; }
		
        
		
		
		// Constructors
		public Wall(RoomComponent owner, float width, float height) : this(owner, width, height, 0)
		{
            _owner = owner;
		}
		
		public Wall(RoomComponent owner, float width, float height, float rotation)
		{
			GenerateObject(width, height, rotation);
		}
		
		
		
		// Methods
		public void SetLocalPosition(float x, float z)
		{
			Instance.transform.localPosition = new Vector3(x, Instance.transform.localPosition.y, z);
		}
		
		public void SetLocalPosition(float x, float y, float z, float rotation)
		{
			SetLocalPosition(new Vector3(x, y, z), rotation);
		}
		
		public void SetLocalPosition(Vector3 localPosition, float rotation)
		{
			Instance.transform.localPosition = localPosition;
			Instance.transform.localRotation = Quaternion.AngleAxis(rotation, Vector3.up);
		}
		
		private void GenerateObject(float width, float height, float rotation)
		{
			Instance = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Instance.transform.localScale = new Vector3(THICKNESS, height, width);
			Instance.transform.localPosition = new Vector3(0, height / 2 + THICKNESS / 2, 0);
			Instance.transform.localRotation = Quaternion.AngleAxis(rotation, Vector3.up);
			
			Instance.renderer.material.color = Color.blue;
		}
	}
}

