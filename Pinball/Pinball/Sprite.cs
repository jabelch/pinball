using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project03
{
	public class Sprite
	{
		Texture2D texture;
		protected Vector2 GRAVITY = new Vector2(0, 700f);
		Vector2 location, origin, velocity;
		float width, height, rotation, scale;
		protected ArrayList boundaries;
        Matrix transform;
        String name;

        public Matrix Transform
        {
            get { return transform; }
            set { transform = value; }
        }

		public Texture2D Texture 
		{ 
			get { return texture; }
			set { texture = value; }
		}
		public Boundary Boundaries { set { boundaries.Add(value); } }
		public ArrayList GetSpriteBoundaries { get { return boundaries; } }
		public float Width { get { return width; } set { width = value; } }
		public float Height { get { return height; } set { height = value; } }
		public float Rotation { get { return rotation; } set { rotation = value; } }
        public String Name { get { return name; } set { name = value; } }
		public Vector2 Location
		{
			get { return location; }
			set { location = value; }
		}
		public Vector2 Origin
		{
			get { return origin; }
			set { origin = value; }
		}
		public float Scale
		{
			get { return scale; }
			set { scale = value; }
		}
		public Vector2 Velocity 
		{ 
			get { return velocity; } 
			set { velocity = value; } 
		}

		public Sprite(Texture2D texture, String name, Vector2 spriteLocation, Vector2 origin, float rotation)
        {
            this.texture = texture;
			this.rotation = rotation;
            this.name = name;
            this.location = spriteLocation;
            this.origin = origin;
            this.scale = 1;
            this.velocity = Vector2.Zero;
            this.width = texture.Width;
            this.height = texture.Height;
            this.boundaries = new ArrayList();
        }
	}
}
