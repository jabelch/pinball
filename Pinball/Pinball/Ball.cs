using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Project03
{
	public class Ball : RoundBumper
	{
		SoundEffect rebound;
		float lFlipVel, rFlipVel;
		public float LFlipVel { set { lFlipVel = value; } }
		public float RFlipVel { set { rFlipVel = value; } }

        Matrix transform;
        bool newGame = true;
        public bool NewGame { get { return newGame; } set { newGame = value; } }

		public Ball(Texture2D texture, String name, Vector2 center, Vector2 origin, float rotation, float radius, SoundEffect rebound)
			: base(texture, name, center, origin, rotation, radius)
		{
			this.rebound = rebound;
		}

		public void Update(ArrayList spriteArray, float seconds)
		{
            foreach (Sprite s in spriteArray)
            {
                transform = Matrix.Identity;
                transform *= Matrix.CreateRotationZ(s.Rotation);
                transform *= Matrix.CreateTranslation(s.Location.X, s.Location.Y, 0);
                s.Transform = transform;
                
                foreach (Boundary b in s.GetSpriteBoundaries)
                {
                    bool collides;
					if (b.Type == 0) // Type 0 is Line
					{
						//collides = Collide((Line)b, transform);
						Line l = new Line((Line)b, transform);
						// If ball is hitting line AND moving toward it, 
						if (l.getDistance(this.Location) < this.Radius && Vector2.Dot(l.N, this.Velocity) < 0)
						{
							Vector2 collisionPoint = l.Start + l.N * Vector2.Dot(this.Location - l.Start, l.N);
							Vector2 reflect = collisionPoint - s.Location;

							Vector2 spriteVel = new Vector2(-reflect.Y, reflect.X);
							spriteVel.Normalize();
							float reflectLength = reflect.Length();
							if (s.Name == "LFLIP")
								spriteVel *= reflectLength * lFlipVel;
							else if (s.Name == "RFLIP")
								spriteVel *= reflect.Length() * rFlipVel;
							else
								spriteVel = Vector2.Zero;
							this.Velocity = l.Reflect(this.Velocity - spriteVel);
							collides = true;
							if (s.Name != "START" && s.Name != "LFLIP" && s.Name != "RFLIP")
							{
								rebound.Play();
							}

						}
						collides = false;

					}
					else //if (b.Type == 1) // Type 1 is Circle
						collides = Collide((Circle)b, transform);

                    if (collides && (s.Name != "START" && s.Name != "LFLIP" && s.Name != "RFLIP" ))
                    {
						rebound.Play();
                    }

                }
            }
			this.Location += this.Velocity * seconds;
			if (newGame)
				;
			else
				this.Velocity += this.GRAVITY * seconds;
            this.Velocity = this.Velocity * .9997f; // Drag
		}

		private bool Collide(Line line, Matrix m)
		{
            Line l = new Line(line, m);
			// If ball is hitting line AND moving toward it, 
			if (l.getDistance(this.Location) < this.Radius && Vector2.Dot(l.N, this.Velocity) < 0)
			{
				Vector2 collisionPoint = l.Start + l.N * Vector2.Dot(this.Location - l.Start, l.N);
				Vector2 vect = 
				this.Velocity = l.Reflect(this.Velocity);
                return true;
			}
            return false;
		}

		private bool Collide(Circle circle, Matrix m)
		{
            Circle c = new Circle(circle, m);
			Vector2 n = this.Location - c.Center;
			n.Normalize();
			if (c.getDistance(this.Location) < c.Radius + this.Radius && Vector2.Dot(n, this.Velocity) < 0)
			{
				this.Velocity = c.reflect(this.Velocity, this.Location);
                return true;
			}
            return false;
		}

        public void Tilt(String direction)
        {
			Vector2 change;
            if (direction == "a")
                change = new Vector2(-500, 0); // Change Direction Left
            else if (direction == "d")
				change = new Vector2(500, 0);  // Change Direction Right
            else
				change = new Vector2(0, -500);  // Change Direction Up
			this.Velocity += change;
        }
	}
}
