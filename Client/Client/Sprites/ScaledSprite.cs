using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
                return new Rectangle((int)(Position.X + SpriteDrawingOffset.X), (int)(Position.Y + SpriteDrawingOffset.Y), width, height);
            }
        }

        public ScaledSprite(Texture2D texture, Vector2 position, int width, int height, Vector2 spriteDrawingOffset = default) 
            : base(texture, position, spriteDrawingOffset)
        {
            this.width = width;
            this.height = height;
        }

    }
}
