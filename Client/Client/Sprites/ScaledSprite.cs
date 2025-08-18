using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ScaledSprite : Sprite
    {
        private int width;
        private int height;

        public Rectangle Rect
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, width, height);
            }

        }

        public ScaledSprite(Texture2D texture, Vector2 position, int width, int height) : base(texture, position)
        {
            this.width = width;
            this.height = height;
        }

    }
}
