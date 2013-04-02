using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project03
{
    class Flipper : Sprite
    {
        public Flipper(Texture2D texture, String name, Vector2 location, Vector2 origin, float rotation)
            : base(texture, name, location, origin, rotation)
        {

        }
    }
}
