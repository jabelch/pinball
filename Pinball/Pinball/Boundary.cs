using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Project03
{

    public class TransformHelper
    {
        public static Vector2 transform(Matrix m, Vector2 p)
        {
            Vector3 v = new Vector3(p.X, p.Y, 0);
            v = Vector3.Transform(v, m);
            return new Vector2(v.X, v.Y);
        }
    }


    public class Boundary
    {
        int type;
        float rebound;

        //public float Rebound {get{return rebound;}}

        public Boundary(float rebound, int type)
        {
            this.type = type;
            this.rebound = rebound;
        }

        public int Type { get { return type; } }
        public float Rebound { get { return rebound; } }


    }

	public class Line : Boundary
	{
		Vector2 start, end, vect, n;
        float length;

		public Vector2 Start { get { return start; } }
		public Vector2 End { get { return end; } }
		public Vector2 Vector { get { return vect; } }
		public Vector2 N { get { return n; } }

		public Line(Vector2 start, Vector2 end, float rebound) : base(rebound, 0)
		{
			this.start = start;
			this.end = end;
			this.vect = end - start;
			this.n = new Vector2(vect.Y, -vect.X);
            this.n.Normalize();
			this.length = vect.Length();
		}

        public Line(Line l, Matrix m) : base(l.Rebound, 0)
        {
            
            this.start = TransformHelper.transform(m, l.start);
            this.end = TransformHelper.transform(m, l.end);
            this.vect = end - start;
            this.n = new Vector2(vect.Y, -vect.X);
            this.n.Normalize();
            this.length = vect.Length();
        }



		public Vector2 Reflect(Vector2 v)
		{
			return v - 2 * n * Vector2.Dot(v, n) * this.Rebound;
		}
		public float getDistance(Vector2 point)
		{
			Vector2 ballToStart = this.Start - point;
			Vector2 ballToEnd = this.End - point;
			float distFromLine;

			if (Vector2.Dot(ballToStart, this.Vector) > 0)
				distFromLine = ballToStart.Length();
			else if (Vector2.Dot(ballToEnd, this.Vector) < 0)
				distFromLine = ballToEnd.Length();
			else
				distFromLine = Math.Abs(Vector2.Dot(ballToStart, this.N));

			return distFromLine;
		}
			
	}

	public class Circle : Boundary
	{
		Vector2 center;
		float radius;

		public Vector2 Center { get { return center; } }
		public float Radius { get { return radius; } }

		public Circle(Vector2 center, float radius, float rebound) : base(rebound, 1)
		{
			this.center = center;
			this.radius = radius;
		}

        public Circle(Circle c, Matrix m) : base (c.Rebound, 1)
        {
            this.center = TransformHelper.transform(m, c.center);
            this.radius = c.radius;
        }


		public Vector2 reflect(Vector2 velocity, Vector2 location)
		{
			Vector2 n = location - this.Center;
			n.Normalize();
			return velocity - 2 * n * Vector2.Dot(velocity, n) * this.Rebound;
		}

		public float getDistance(Vector2 point)
		{
			return (this.Center - point).Length();
		}
	}
}
