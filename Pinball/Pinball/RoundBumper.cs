using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project03
{
	public class RoundBumper : Sprite
	{
		//Vector2 center;
		float radius;

		public float Radius { get { return radius; } }

		public RoundBumper(Texture2D texture, 
            String name,
			Vector2 center, 
			Vector2 origin, 
			float rotation,  
			float radius) 
			: base (texture, name, center, origin, rotation)
		{
			//this.center = center;
			this.radius = radius;
			//this.boundaries.Add(new Circle(Vector2.Zero, radius, rebound));
		}

	}
}
