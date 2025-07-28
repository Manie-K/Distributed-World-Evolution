using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ColoredSprite : ScaledSprite
    {
        public Color color;
        public ColoredSprite(Texture2D texture, Vector2 position, int width, int height, Color color) : base(texture, position, width, height)
        {
            this.color = color;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, Rect, Color.White);
        }
    }
}