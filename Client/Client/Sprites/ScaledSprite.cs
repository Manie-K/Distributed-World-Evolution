using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public class ScaledSprite : Sprite
    {
        protected int width;
        protected int height;

        public ScaledSprite(Texture2D texture, Vector2 position, int width, int height, Vector2 spriteDrawingOffset = default) 
            : base(texture, position, spriteDrawingOffset)
        {
            this.width = width;
            this.height = height;
        }

    }
}
